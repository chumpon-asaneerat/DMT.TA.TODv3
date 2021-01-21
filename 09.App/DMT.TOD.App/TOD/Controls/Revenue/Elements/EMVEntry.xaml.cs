#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using DMT.Configurations;
using DMT.Controls;
using DMT.Models;
using DMT.Services;

#endregion

namespace DMT.TOD.Controls.Revenue.Elements
{
    using scwOps = Services.Operations.SCW.TOD;

    /// <summary>
    /// Interaction logic for EMVEntry.xaml
    /// </summary>
    public partial class EMVEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public EMVEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private TSB _tsb = null;
        private List<Plaza> _plazas = null;
        private Models.RevenueEntry entry = null;

        private int rowCnt = 0;
        private decimal amtVal = 0;

        #endregion

        #region DataContext Change Handler

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (null != entry)
            {
                LoadItems();
            }
            else
            {
                UpdateSummary();
            }
        }

        #endregion

        #region Private Methods

        private void RefreshEMV(DateTime dt1, DateTime dt2)
        {
            grid.ItemsSource = null;

            List<LaneEMV> results = new List<LaneEMV>();
            List<LaneEMV> items = new List<LaneEMV>();
            List<LaneEMV> sortList = new List<LaneEMV>();

            if (null != entry && null != _tsb && null != _plazas)
            {
                var userShift = UserShift.GetUserShift(entry.UserId).Value();
                int networkId = TODConfigManager.Instance.DMT.networkId;

                if (null != userShift && userShift.Begin.HasValue && null != _plazas && _plazas.Count > 0)
                {
                    _plazas.ForEach(plaza =>
                    {
                        int pzId = plaza.SCWPlazaId;
                        SCWEMVTransactionList param = new SCWEMVTransactionList();
                        param.networkId = networkId;
                        param.plazaId = pzId;
                        param.staffId = userShift.UserId;
                        param.startDateTime = dt1;
                        param.endDateTime = dt2;
                        var emvList = scwOps.emvTransactionList(param);
                        if (null != emvList && null != emvList.list)
                        {
                            emvList.list.ForEach(item =>
                            {
                                if (item.trxDateTime.HasValue &&
                                    userShift.Begin.Value < item.trxDateTime.Value)
                                {
                                    items.Add(new LaneEMV(item));
                                }
                            });
                        }
                    });

                    sortList = items.OrderBy(o => o.TrxDateTime).Distinct().ToList();
                }
                results.AddRange(sortList.ToArray());
            }
            // Calculate Summary.
            if (null != results && results.Count > 0)
            {
                rowCnt = results.Count;
                amtVal = decimal.Zero;
                sortList.ForEach(item =>
                {
                    amtVal += item.Amount;
                });
            }
            else
            {
                rowCnt = 0;
                amtVal = decimal.Zero;
            }

            UpdateSummary();

            grid.ItemsSource = results;
        }

        private void UpdateSummary()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load Items.
        /// </summary>
        public void LoadItems()
        {
            if (null == entry) return;

            DateTime dt1 = entry.ShiftBegin.Value;
            DateTime dt2 = entry.ShiftEnd.Value;

            RefreshEMV(dt1, dt2);
        }
        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The Revenue Entry.</param>
        /// <param name="tsb"></param>
        /// <param name="plazas"></param>
        public void Setup(Models.RevenueEntry value, TSB tsb, List<Plaza> plazas)
        {
            entry = value;
            _tsb = tsb;
            _plazas = plazas;
            this.DataContext = entry;
        }

        #endregion
    }
}
