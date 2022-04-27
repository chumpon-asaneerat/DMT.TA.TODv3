#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

//using NLib.Services;
using DMT.Models;
using DMT.Services;

#endregion

namespace DMT
{
    /// <summary>
    /// The AccountApp class.
    /// </summary>
    public static class AccountApp
    {
        /// <summary>
        /// Gets or sets is SCW Service online.
        /// </summary>
        public static bool SCWOnline { get; set; }

        /// <summary>
        /// Permissions Static class.
        /// </summary>
        public static class Permissions
        {
            /// <summary>Gets or sets Role for account permission.</summary>
            public static string[] Account = new string[] 
            {
                "ADMINS",
                "ACCOUNT",
                /*"CTC_MGR", "CTC", "TC",*/
                "MT_ADMIN", "MT_TECH",
                "FINANCE", "SV",
                "RAD_MGR", "RAD_SUP"            
            };
        }

        /// <summary>
        /// Gets or sets Current Account User.
        /// </summary>
        public static class User
        {
            /// <summary>Gets or sets current User.</summary>
            public static Models.User Current { get; set; }
        }

        /// <summary>
        /// Pages Static class.
        /// </summary>
        public static class Pages
        {
            #region Main Menu

            private static Account.Pages.Menu.MainMenu _MainMenu;

            /// <summary>Gets MainMenu Page.</summary>
            public static Account.Pages.Menu.MainMenu  MainMenu
            {
                get
                {
                    if (null == _MainMenu)
                    {
                        lock (typeof(AccountApp))
                        {
                            _MainMenu = new Account.Pages.Menu.MainMenu();
                        }
                    }
                    return _MainMenu;
                }
            }

            #endregion

            #region SignIn

            private static DMT.Pages.SignInPage _SignIn;

            /// <summary>Gets SignIn Page.</summary>
            public static DMT.Pages.SignInPage SignIn
            {
                get
                {
                    if (null == _SignIn)
                    {
                        lock (typeof(AccountApp))
                        {
                            _SignIn = new DMT.Pages.SignInPage();
                        }
                    }
                    return _SignIn;
                }
            }

            #endregion

            #region TSB Balance Page

            private static Account.Pages.Balance.TSBCheckBalancePage _TSBCheckBalancePage;

            /// <summary>Gets TSB Check Balance Page.</summary>
            public static Account.Pages.Balance.TSBCheckBalancePage TSBCheckBalancePage
            {
                get
                {
                    if (null == _TSBCheckBalancePage)
                    {
                        lock (typeof(AccountApp))
                        {
                            _TSBCheckBalancePage = new Account.Pages.Balance.TSBCheckBalancePage();
                        }
                    }
                    return _TSBCheckBalancePage;
                }
            }

            #endregion

            #region TSB Request Exchange View Page

            private static Account.Pages.Exchange.TSBRequestExchangeViewPage _TSBRequestExchangeView;

            /// <summary>Gets TSB Request Exchange View Page.</summary>
            public static Account.Pages.Exchange.TSBRequestExchangeViewPage TSBRequestExchangeView
            {
                get
                {
                    if (null == _TSBRequestExchangeView)
                    {
                        lock (typeof(AccountApp))
                        {
                            _TSBRequestExchangeView = new Account.Pages.Exchange.TSBRequestExchangeViewPage();
                        }
                    }
                    return _TSBRequestExchangeView;
                }
            }

            #endregion

            #region Coupon History View

            private static Account.Pages.Coupon.CouponHistoryViewPage _CouponHistoryView;

            /// <summary>Gets Coupon History View Page.</summary>
            public static Account.Pages.Coupon.CouponHistoryViewPage CouponHistoryView
            {
                get
                {
                    if (null == _CouponHistoryView)
                    {
                        lock (typeof(AccountApp))
                        {
                            _CouponHistoryView = new Account.Pages.Coupon.CouponHistoryViewPage();
                        }
                    }
                    return _CouponHistoryView;
                }
            }

            #endregion

            #region SAP Send Coupoon Sold View

            private static Account.Pages.SAP.SAPSendCouponSoldPage _SAPSendCouponSoldView;

            /// <summary>Gets SAP Send Coupoon Sold View Page.</summary>
            public static Account.Pages.SAP.SAPSendCouponSoldPage SAPSendCouponSoldView
            {
                get
                {
                    if (null == _SAPSendCouponSoldView)
                    {
                        lock (typeof(AccountApp))
                        {
                            _SAPSendCouponSoldView = new Account.Pages.SAP.SAPSendCouponSoldPage();
                        }
                    }
                    return _SAPSendCouponSoldView;
                }
            }

            #endregion
        }

        /// <summary>
        /// Windows Static class.
        /// </summary>
        public static class Windows
        {
            #region User Search

            /// <summary>Gets User Search Window.</summary>
            public static DMT.Windows.UserSearchWindow UserSearch
            {
                get
                {
                    var ret = new DMT.Windows.UserSearchWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region User Check Balance Window

            /// <summary>Gets SUser Check Balance Window.</summary>
            public static DMT.Windows.UserCheckBalanceWindow UserCheckBalance
            {
                get
                {
                    var ret = new DMT.Windows.UserCheckBalanceWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region SAP Customer Search

            /// <summary>Gets SAP Customer Search Window.</summary>
            public static DMT.Windows.SAPCustomerWindow SAPCustomerSearch
            {
                get
                {
                    var ret = new DMT.Windows.SAPCustomerWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Gets Request Exchange Window

            /// <summary>Gets Request Exchange Window.</summary>
            public static DMT.Windows.TSBRequestExchangeWindow RequestExchangeWindow
            {
                get
                {
                    var ret = new DMT.Windows.TSBRequestExchangeWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region MessageBox(s)

            /// <summary>Gets MessageBox Window.</summary>
            public static DMT.Windows.MessageBoxWindow MessageBox
            {
                get
                {
                    var ret = new DMT.Windows.MessageBoxWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }
            /// <summary>Gets MessageBox Yes-No Window</summary>
            public static DMT.Windows.MessageBoxYesNoWindow MessageBoxYesNo
            {
                get
                {
                    var ret = new DMT.Windows.MessageBoxYesNoWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            /// <summary>Gets MessageBox Yes-No 1 Window</summary>
            public static DMT.Windows.MessageBoxYesNo1Window MessageBoxYesNo1
            {
                get
                {
                    var ret = new DMT.Windows.MessageBoxYesNo1Window();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            /// <summary>Gets MessageBox Yes-No 2 Window</summary>
            public static DMT.Windows.MessageBoxYesNo2Window MessageBoxYesNo2
            {
                get
                {
                    var ret = new DMT.Windows.MessageBoxYesNo2Window();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            /// <summary>Gets MessageBox Yes-No 3 Window</summary>
            public static DMT.Windows.MessageBoxYesNo3Window MessageBoxYesNo3
            {
                get
                {
                    var ret = new DMT.Windows.MessageBoxYesNo3Window();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            /// <summary>Gets Confirm Approve Window.</summary>
            public static DMT.Windows.ConfirmApproveWindow ConfirmApproveMessageBox
            {
                get
                {
                    var ret = new DMT.Windows.ConfirmApproveWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }
            /// <summary>Gets Confirm Reject Window.</summary>
            public static DMT.Windows.ConfirmRejectWindow ConfirmRejectMessageBox
            {
                get
                {
                    var ret = new DMT.Windows.ConfirmRejectWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion
        }
    }
}
