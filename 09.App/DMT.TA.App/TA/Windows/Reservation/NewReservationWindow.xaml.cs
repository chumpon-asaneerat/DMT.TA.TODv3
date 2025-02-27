﻿#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;
using System.Linq;
using NLib.Reflection.Emit;
using NLib;

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
            if (!SaveReservationToQueue())
            {
                return;
            }
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
            MethodBase med = MethodBase.GetCurrentMethod();

            tsb = TSB.GetCurrent().Value();
            sYear = DateTime.Now.Year.ToString();
            var storages = ops.GetStorageLocations(tsb.TSBId).Value();
            storage = (null != storages) ? storages.FirstOrDefault() : null;

            var runningNos = ops.GetReservationCurrentRunningNo(tsb.TSBId, sYear).Value();
            runningNo = (null != runningNos) ? runningNos.FirstOrDefault() : null;

            if (null == storage || null == runningNo)
            {
                cbCouponMasters.IsEnabled = false;
                txtAmount.IsEnabled = false;
                cmdAdd.IsEnabled = false;
                cmdClear.IsEnabled = false;
                cmdOk.Visibility = Visibility.Hidden;

                med.Err("NEW COUPON RESERVATION - LOAD MASTER DATA:");
                med.Err("  - Cannot no requred data from server");
            }
            else
            {
                cbCouponMasters.IsEnabled = true;
                txtAmount.IsEnabled = true;
                cmdAdd.IsEnabled = true;
                cmdClear.IsEnabled = true;
                cmdOk.Visibility = Visibility.Visible;

                med.Info("NEW COUPON RESERVATION - LOAD MASTER DATA:");
                med.Info("  - Success load master data");
            }
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

        private bool HstQty()
        {
            return (null != reqItem && reqItem.quantity > 0);
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

            if  (!HstQty())
            {
                txtAmount.SelectAll();
                txtAmount.Focus();
                return;
            }

            var couponType = couponTypes[cbCouponMasters.SelectedIndex];
            reqItem.description = couponType.Description.Trim();
            reqItem.materialnum = couponType.MATERIAL_NUM.Trim();

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

        private bool CanSave()
        {
            bool ret = false;
            if (null != tsb && null != runningNo && null != request)
            {
                ret = true;
            }
            return ret;
        }

        private bool HasItems()
        {
            bool ret = false;
            if (null != tsb && null != runningNo && null != request &&
                null != request.items && request.items.Count > 0)
            {
                ret = true;
            }
            return ret;
        }

        private bool SaveReservationToQueue()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            bool ret = false;

            if (!CanSave())
            {
                var win = TAApp.Windows.MessageBox;
                string msg = "ไม่ข้อมูลมาสเตอร์ จาก TA server";
                win.Setup(msg, "Toll Admin");
                win.ShowDialog();
                return ret;
            }
            if (!HasItems())
            {
                var win = TAApp.Windows.MessageBox;
                string msg = "ไม่พบรายการคูปองที่ขอ ต้องมีอย่างน้อย 1 รายการ";
                win.Setup(msg, "Toll Admin");
                win.ShowDialog();
                return ret;
            }

            if (null != tsb && null != runningNo && null != request && null != request.items)
            {
                int itemNum = 1;
                foreach (var item in request.items) 
                {
                    item.itemnumber = itemNum.ToString();
                    item.goodsrecipient = request.goodsrecipient; // same as header
                    itemNum++;
                }

                // Update current running no
                ops.UpdateReservationCurrentRunningNo(tsb.TSBId, runningNo.RunningNo + 1);

                // Write to queue
                TAxTODMQService.Instance.WriteQueue(request);

                var win = TAApp.Windows.MessageBox;
                string msg = string.Format("สร้างใบเบิก เลขที่ : '{0}' สำเร็จ", request.goodsrecipient);
                win.Setup(msg, "Toll Admin");
                win.ShowDialog();

                ret = true;
            }
            else
            {
                med.Err("SaveReservationToQueue failed. Some parameter(s) is null.");
            }

            return ret;
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
