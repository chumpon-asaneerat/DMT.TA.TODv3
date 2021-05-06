#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using NLib.Services;

#endregion

using DMT.Services;
using DMT.Models.ExtensionMethods;
using ops = DMT.Services.Operations.TAxTOD.SAP;

namespace DMT.Account.Pages.Menu
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainMenu()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdCreditAndCouponSummary_Click(object sender, RoutedEventArgs e)
        {
            // TSB Credit and Coupon Summary
        }

        private void cndRequestExchangeHistory_Click(object sender, RoutedEventArgs e)
        {
            // TSB Request Exchange History
        }

        private void cmdTSBBalanceSummary_Click(object sender, RoutedEventArgs e)
        {
            // TSB Balance Summary
        }

        private void cndRequestExchangeManage_Click(object sender, RoutedEventArgs e)
        {
            // TSB Request Exchange Management
        }

        private void cndCouponSoldHistory_Click(object sender, RoutedEventArgs e)
        {
            // Coupon History
            var page = AccountApp.Pages.CouponHistoryView;
            page.Setup(AccountApp.User.Current);
            PageContentManager.Instance.Current = page;
        }


        private void cndSendDataToSAP_Click(object sender, RoutedEventArgs e)
        {
            TestGetCustomers();
            TestGetTSBs();
        }

        private void cndExit_Click(object sender, RoutedEventArgs e)
        {
            // Exit
            // When enter Sign In Screen reset current user.
            AccountApp.User.Current = null;

            var page = AccountApp.Pages.SignIn;
            page.Setup(AccountApp.Permissions.Account);
            PageContentManager.Instance.Current = page;
        }

        #endregion

        private void TestGetCustomers()
        {
            Console.WriteLine("ทดสอบ GetCustomers");
            var ret = ops.GetCustomers(Models.Search.TAxTOD.SAP.SearchCustomer.Create(""));
            if (null != ret && ret.Ok)
            {
                var list = ret.Value();
                list.ForEach(item =>
                {
                    Console.WriteLine("Code: {0} - Name: {1}", item.CardCode, item.CardName);
                });
            }
        }

        private void TestGetTSBs()
        {
            Console.WriteLine("ทดสอบ GetTSBs");
            var ret = ops.GetTSBs();
            if (null != ret && ret.Ok)
            {
                var list = ret.Value();
                list.ForEach(item =>
                {
                    Console.WriteLine("Id: {0} - NameTH: {1}, WHSCode: {2}", item.TSBId, item.TSB_Th_Name, item.SapWhsCode);
                });
            }
        }
    }
}
