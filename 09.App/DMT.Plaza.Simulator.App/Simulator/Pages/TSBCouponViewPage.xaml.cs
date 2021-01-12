#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DMT.Models;
using DMT.Services;
using System.Collections.ObjectModel;
using NLib.Reflection;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Runtime.InteropServices;

#endregion

namespace DMT.Simulator.Pages
{
    /// <summary>
    /// Interaction logic for TSBCouponViewPage.xaml
    /// </summary>
    public partial class TSBCouponViewPage : UserControl
    {
        #region Constructor

        public TSBCouponViewPage()
        {
            InitializeComponent();
        }

        #endregion

        private LocalOperations ops = LocalServiceOperations.Instance.Plaza;

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTSBs();

            if (cbCouponType.SelectedIndex == -1)
            {
                cbCouponType.SelectedIndex = 0; // auto select
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region ListView Handlers

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tsb = listView.SelectedItem as TSB;
            RefreshCoupon(tsb);
        }

        #endregion

        #region Button Handlers

        private void cmdAddCoupon_Click(object sender, RoutedEventArgs e)
        {
            string idRange = txtRange.Text;
            txtRange.Text = string.Empty;

            var tsb = listView.SelectedItem as TSB;
            if (null == tsb) return;

            var ids = idRange.ParseRange(0, 999999);
            if (null != ids)
            {
                // remove duplicate id.
                ids = ids.Distinct();
                foreach (var id in ids)
                {
                    TSBCouponTransaction item = new TSBCouponTransaction();
                    item.TSBId = tsb.TSBId;
                    if ((cbCouponType.SelectedIndex == 0))
                    {
                        item.CouponId = "ข" + id.ToString("D6");
                        item.CouponType = CouponType.BHT35;
                        item.Price = 665;
                    }
                    else
                    {
                        item.CouponId = "C" + id.ToString("D6");
                        item.CouponType = CouponType.BHT80;
                        item.Price = 1520;
                    }
                    ops.Coupons.SaveTransaction(item);
                }

                RefreshCoupon(tsb);
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            /*
            var item = pgrid.SelectedObject as TSBCouponTransaction;
            if (null == item) return;
            ops.Coupons.SaveTransaction(item);
            // clear
            pgrid.SelectedObject = null;
            */
        }

        #endregion

        private void RefreshTSBs()
        {
            listView.ItemsSource = null;

            var tsbs = ops.TSB.GetTSBs().Value();
            listView.ItemsSource = tsbs;
        }

        private void RefreshCoupon(TSB tsb)
        {
            lvCoupon.ItemsSource = null;

            if (null != tsb)
            {
                var coupons = ops.Coupons.GetTSBCouponTransactions(tsb).Value();
                lvCoupon.ItemsSource = coupons;
            }
        }
    }
}
