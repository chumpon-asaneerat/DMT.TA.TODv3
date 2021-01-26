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
    /// The TAApp class.
    /// </summary>
    public static class TAApp
    {
        /// <summary>
        /// Permissions Static class.
        /// </summary>
        public static class Permissions
        {
            /// <summary>Gets or sets Role for CTC permission.</summary>
            public static string[] CTC = new string[]
            {
                "ADMINS",
                "ACCOUNT",
                "CTC_MGR", "CTC", /*"TC",*/
                "MT_ADMIN", "MT_TECH",
                "FINANCE", "SV",
                "RAD_MGR", "RAD_SUP"
            };

            /// <summary>Gets or sets Role for TC permission.</summary>
            public static string[] TC = new string[]
            {
                "ADMINS",
                "ACCOUNT",
                "CTC_MGR", "CTC", "TC",
                "MT_ADMIN", "MT_TECH",
                "FINANCE", "SV",
                "RAD_MGR", "RAD_SUP"
            };
        }

        /// <summary>
        /// Gets or sets Current TA User.
        /// </summary>
        public static class User
        {
            public static Models.User Current { get; set; }
        }

        /// <summary>
        /// Pages Static class.
        /// </summary>
        public static class Pages
        {
            #region Main Menu

            private static TA.Pages.Menu.MainMenu _MainMenu;

            /// <summary>Gets MainMenu Page.</summary>
            public static TA.Pages.Menu.MainMenu MainMenu
            {
                get
                {
                    if (null == _MainMenu)
                    {
                        lock (typeof(TAApp))
                        {
                            _MainMenu = new TA.Pages.Menu.MainMenu();
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
                        lock (typeof(TAApp))
                        {
                            _SignIn = new DMT.Pages.SignInPage();
                        }
                    }
                    return _SignIn;
                }
            }

            #endregion

            #region Report Menu

            private static TA.Pages.Menu.ReportMenu _ReportMenu;

            /// <summary>Gets _ReportMenu Page.</summary>
            public static TA.Pages.Menu.ReportMenu ReportMenu
            {
                get
                {
                    if (null == _ReportMenu)
                    {
                        lock (typeof(TAApp))
                        {
                            _ReportMenu = new TA.Pages.Menu.ReportMenu();
                        }
                    }
                    return _ReportMenu;
                }
            }

            #endregion

            #region Configuration Menu

            private static TA.Pages.Menu.ConfigurationMenu _ConfigurationMenu;

            /// <summary>Gets ConfigurationMenu Page.</summary>
            public static TA.Pages.Menu.ConfigurationMenu ConfigurationMenu
            {
                get
                {
                    if (null == _ConfigurationMenu)
                    {
                        lock (typeof(TAApp))
                        {
                            _ConfigurationMenu = new TA.Pages.Menu.ConfigurationMenu();
                        }
                    }
                    return _ConfigurationMenu;
                }
            }

            #endregion

            #region Internal Exchange

            private static TA.Pages.Exchange.InternalExchangePage _InternalExchange;

            /// <summary>Gets Internal Exchange Page.</summary>
            public static TA.Pages.Exchange.InternalExchangePage InternalExchange
            {
                get
                {
                    if (null == _InternalExchange)
                    {
                        lock (typeof(TAApp))
                        {
                            _InternalExchange = new TA.Pages.Exchange.InternalExchangePage();
                        }
                    }
                    return _InternalExchange;
                }
            }

            #endregion

            #region Manage Exchange

            private static TA.Pages.Exchange.ManageExchangePage _ManageExchange;

            /// <summary>Gets Manage Exchange Page.</summary>
            public static TA.Pages.Exchange.ManageExchangePage ManageExchange
            {
                get
                {
                    if (null == _ManageExchange)
                    {
                        lock (typeof(TAApp))
                        {
                            _ManageExchange = new TA.Pages.Exchange.ManageExchangePage();
                        }
                    }
                    return _ManageExchange;
                }
            }

            #endregion

            #region Request Exchange

            private static TA.Pages.Exchange.RequestExchangePage _RequestExchange;

            /// <summary>Gets Request Exchange Page.</summary>
            public static TA.Pages.Exchange.RequestExchangePage RequestExchange
            {
                get
                {
                    if (null == _RequestExchange)
                    {
                        lock (typeof(TAApp))
                        {
                            _RequestExchange = new TA.Pages.Exchange.RequestExchangePage();
                        }
                    }
                    return _RequestExchange;
                }
            }

            #endregion

            #region Collector Credit Manage

            private static TA.Pages.Credit.CollectorCreditManagePage _CollectorCreditManage;

            /// <summary>Gets Collector Credit Manage Page.</summary>
            public static TA.Pages.Credit.CollectorCreditManagePage CollectorCreditManage
            {
                get
                {
                    if (null == _CollectorCreditManage)
                    {
                        lock (typeof(TAApp))
                        {
                            _CollectorCreditManage = new TA.Pages.Credit.CollectorCreditManagePage();
                        }
                    }
                    return _CollectorCreditManage;
                }
            }

            #endregion

            #region Credit History View

            private static TA.Pages.Credit.CreditHistoryViewPage _CreditHistoryView;

            /// <summary>Gets Credit History View Page.</summary>
            public static TA.Pages.Credit.CreditHistoryViewPage CreditHistoryView
            {
                get
                {
                    if (null == _CreditHistoryView)
                    {
                        lock (typeof(TAApp))
                        {
                            _CreditHistoryView = new TA.Pages.Credit.CreditHistoryViewPage();
                        }
                    }
                    return _CreditHistoryView;
                }
            }

            #endregion

            #region Coupon History View

            private static TA.Pages.Coupon.CouponHistoryViewPage _CouponHistoryView;

            /// <summary>Gets Coupon History View Page.</summary>
            public static TA.Pages.Coupon.CouponHistoryViewPage CouponHistoryView
            {
                get
                {
                    if (null == _CouponHistoryView)
                    {
                        lock (typeof(TAApp))
                        {
                            _CouponHistoryView = new TA.Pages.Coupon.CouponHistoryViewPage();
                        }
                    }
                    return _CouponHistoryView;
                }
            }

            #endregion

            #region Coupon TSB Sale

            private static TA.Pages.Coupon.CouponTSBSalePage _CouponTSBSale;

            /// <summary>Gets Coupon TSB Sale Page.</summary>
            public static TA.Pages.Coupon.CouponTSBSalePage CouponTSBSale
            {
                get
                {
                    if (null == _CouponTSBSale)
                    {
                        lock (typeof(TAApp))
                        {
                            _CouponTSBSale = new TA.Pages.Coupon.CouponTSBSalePage();
                        }
                    }
                    return _CouponTSBSale;
                }
            }

            #endregion

            #region Receive Coupon

            private static TA.Pages.Coupon.ReceiveCouponPage _ReceiveCoupon;

            /// <summary>Gets Receive Coupon Page.</summary>
            public static TA.Pages.Coupon.ReceiveCouponPage ReceiveCoupon
            {
                get
                {
                    if (null == _ReceiveCoupon)
                    {
                        lock (typeof(TAApp))
                        {
                            _ReceiveCoupon = new TA.Pages.Coupon.ReceiveCouponPage();
                        }
                    }
                    return _ReceiveCoupon;
                }
            }

            #endregion

            #region Return Coupon

            private static TA.Pages.Coupon.ReturnCouponPage _ReturnCoupon;

            /// <summary>Gets Return Coupon Page.</summary>
            public static TA.Pages.Coupon.ReturnCouponPage ReturnCoupon
            {
                get
                {
                    if (null == _ReturnCoupon)
                    {
                        lock (typeof(TAApp))
                        {
                            _ReturnCoupon = new TA.Pages.Coupon.ReturnCouponPage();
                        }
                    }
                    return _ReturnCoupon;
                }
            }

            #endregion
        }

        /// <summary>
        /// Windows Static class.
        /// </summary>
        public static class Windows
        {
            #region Credit

            /// <summary>Gets Collector Credit Borrow Window.</summary>
            public static TA.Windows.Credit.CollectorCreditBorrowWindow CollectorCreditBorrow
            {
                get
                {
                    var ret = new TA.Windows.Credit.CollectorCreditBorrowWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            /// <summary>Gets Collector Credit Return Window.</summary>
            public static TA.Windows.Credit.CollectorCreditReturnWindow CollectorCreditReturn
            {
                get
                {
                    var ret = new TA.Windows.Credit.CollectorCreditReturnWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            /// <summary>Gets Collector Received Bag Window.</summary>
            public static TA.Windows.Credit.CollectorReceivedBagWindow CollectorReceivedBag
            {
                get
                {
                    var ret = new TA.Windows.Credit.CollectorReceivedBagWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Internal Exchange

            /// <summary>Gets Internal Exchange Window.</summary>
            public static TA.Windows.Exchange.InternalExchangeWindow InternalExchange
            {
                get 
                {
                    var ret = new TA.Windows.Exchange.InternalExchangeWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret; 
                }
            }

            #endregion

            #region Receive Exchange

            /// <summary>Gets Receive Exchange Window.</summary>
            public static TA.Windows.Exchange.ReceiveExchangeWindow ReceiveExchange
            {
                get 
                {
                    var ret = new TA.Windows.Exchange.ReceiveExchangeWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Plaza Balance Summary

            /// <summary>Gets Plaza Balance Summary Window.</summary>
            public static TA.Windows.Plaza.PlazaBalanceSummaryWindow PlazaBalanceSummary
            {
                get 
                {
                    var ret = new TA.Windows.Plaza.PlazaBalanceSummaryWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

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

            #endregion
        }
    }
}
