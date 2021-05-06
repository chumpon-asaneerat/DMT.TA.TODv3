#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Reflection;

using DMT.Models;
using DMT.Services;

using NLib;
using NLib.Services;
using NLib.Reflection;

#endregion

using ops = DMT.Services.Operations.TAxTOD.SAP;

namespace DMT.Account.Pages.SAP
{
    /// <summary>
    /// Interaction logic for SAPSendCouponSoldPage.xaml
    /// </summary>
    public partial class SAPSendCouponSoldPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SAPSendCouponSoldPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _chief = null;
        private int _tollwayId = 9;

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = AccountApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void Reset()
        {
            txtCustomerFilter.Text = string.Empty;
            dtSoldDate.Value = new DateTime?();
        }

        private void GetSAPTSBs()
        {
            cbTSBs.ItemsSource = null;
            var ret = ops.GetTSBs();
            if (null != ret && ret.Ok)
            {
                List<Models.SAPTSB> src = ret.Value();
                List<Models.SAPTSB> list;
                if (null != src)
                {
                    list = src.OrderBy(item => item.TSBId).ToList();
                }
                else list = src;


                cbTSBs.ItemsSource = list;
                if (list.Count > 0) cbTSBs.SelectedIndex = 0;
            }
        }

        private void Search()
        {
            DateTime? dt = (dtSoldDate.Value.HasValue) ? dtSoldDate.Value.Value : new DateTime?();
            var ret = ops.GetCouponSolds(Models.Search.TAxTOD.SAP.CouponSolds.Create(dt, _tollwayId));
            if (null != ret && ret.Ok)
            {
                var list = ret.Value();
                /*
                list.ForEach(item =>
                {
                    Console.WriteLine("SN: {0} - CouponType: {1}, SoldBy: {2}", item.SerialNo, item.CouponType, item.SoldBy);
                });
                */
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="chief">The Current User.</param>
        public void Setup(User chief)
        {
            _chief = chief;

            if (null != _chief)
            {

            }

            GetSAPTSBs();
            Reset();

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtCustomerFilter.SelectAll();
                txtCustomerFilter.Focus();
            }));
        }

        #endregion
    }
}
