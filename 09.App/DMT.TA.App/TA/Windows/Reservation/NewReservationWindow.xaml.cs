#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;
using System.Linq;

#endregion

namespace DMT.TA.Windows.Reservation
{
    using ops = DMT.Services.Operations.TAxTOD.SAP2;

    /// <summary>
    /// Interaction logic for NewReservationWindow.xaml
    /// </summary>
    public partial class NewReservationWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public NewReservationWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            SaveReservationToQueue();
            DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            // Add to list
            AddCurrrentItem();
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            // Clear list
            ClearItems();
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = (null != sender) ? sender as Button : null;
            var ctx = (null != button) ? button.DataContext : null;
            var item = (null != ctx) ? ctx as ReserveRequestItem : null;
            // delete item
            DeleteItem(item);
        }

        #endregion

        #region TextBox Handlers

        private void txtAmount_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) 
            {
                // Add to list
                AddCurrrentItem();

                e.Handled = true;
            }
        }

        #endregion

        #region Private Methods

        private TSB tsb;
        private List<CouponMaster> couponTypes;
        private Storagelocation storage;
        private ReserveRunningNo runningNo;
        private ReserveRequest request;
        private ReserveRequestItem reqItem;
        private string sYear = string.Empty;

        private void LoadMasterData()
        {
            tsb = TSB.GetCurrent().Value();
            sYear = DateTime.Now.Year.ToString();
            storage = ops.GetStorageLocations(tsb.TSBId).Value().FirstOrDefault();
            //Console.WriteLine(storage);
            runningNo = ops.GetReservationCurrentRunningNo(tsb.TSBId, sYear).Value().FirstOrDefault();
            //Console.WriteLine(runningNo);
        }

        private void PrepareRequest()
        {
            grid.DataContext = null;

            request = new ReserveRequest();
            request.basedate = DateTime.Now.Date.ToString("yyyyMMdd", 
                System.Globalization.DateTimeFormatInfo.InvariantInfo);
            request.goodsrecipient = (null != runningNo) ? string.Format("{0}{1}{2:D4}", 
                runningNo.PrefixTxt, sYear, runningNo.RunningNo + 1) : null;
            request.receivingstor = (null != storage) ? storage.Storage_location : null;
            request.userid = (null != TAApp.User.Current) ? TAApp.User.Current.UserId : null;
            if (null == request.items)
            {
                request.items = new List<ReserveRequestItem>();
            }

            grid.DataContext = request.items;

            PrepereItemInputs();
        }

        private void ClearItems()
        {
            grid.DataContext = null;

            if (null != request)
            {
                request.items.Clear();
            }

            grid.DataContext = request.items;

            PrepereItemInputs();
        }

        private void DeleteItem(ReserveRequestItem delItem)
        {
            if (null == delItem)
                return;
            if (null == request && null != request.items)
                return;

            int idx = request.items.FindIndex(item =>
            {
                return item.materialnum == delItem.materialnum;
            });

            if (idx != -1)
            {
                request.items.RemoveAt(idx);
            }

            grid.DataContext = null;

            grid.DataContext = request.items;
        }

        private void PrepereItemInputs()
        {
            cbCouponMasters.SelectedIndex = (null != couponTypes && couponTypes.Count > 0) ? 0 : -1;

            txtAmount.DataContext = null;
            reqItem = new ReserveRequestItem(); // for binding
            txtAmount.DataContext = reqItem;

            txtAmount.SelectAll();
            txtAmount.Focus();
        }

        private void AddCurrrentItem()
        {
            if (cbCouponMasters.SelectedIndex == -1 ||
                cbCouponMasters.SelectedIndex >= couponTypes.Count)
            {
                return;
            }
            if (null == request || null == reqItem)
            {
                return;
            }
            var couponType = couponTypes[cbCouponMasters.SelectedIndex];
            reqItem.description = couponType.Description;
            reqItem.materialnum = couponType.MATERIAL_NUM;

            // check exits in list
            int idx = request.items.FindIndex(item => 
            {
                return item.materialnum == reqItem.materialnum;
            });

            if (idx != -1 && null != request.items[idx])
            {
                // exist so update
                request.items[idx].description = reqItem.description;
                request.items[idx].quantity = reqItem.quantity;
            }
            else
            {
                // not exist so add new
                request.items.Add(reqItem);
            }

            grid.DataContext = null;

            grid.DataContext = request.items;

            PrepereItemInputs();
        }

        private void LoadComboboxs()
        {
            couponTypes = ops.GetCouponMasters().Value();
            cbCouponMasters.ItemsSource = couponTypes;
            cbCouponMasters.SelectedIndex = (null != couponTypes && couponTypes.Count > 0) ? 0 : -1;
        }

        private void SaveReservationToQueue()
        {

        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            LoadMasterData();
            LoadComboboxs();
            PrepareRequest();
        }

        #endregion
    }
}
