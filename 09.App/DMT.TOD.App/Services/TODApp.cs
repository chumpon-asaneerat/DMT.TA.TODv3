#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

//using NLib.Services;
using DMT.Configurations;
using DMT.Models;
using DMT.Services;

#endregion

namespace DMT
{
    /// <summary>
    /// The TODApp class.
    /// </summary>
    public static class TODApp
    {
        /// <summary>
        /// Gets or sets Current TODAppWebServiceConfig.
        /// </summary>
        public static TODAppWebServiceConfig Config { get; set; }
        /// <summary>
        /// Gets or sets IsRegistered.
        /// </summary>
        public static bool IsRegistered { get; set; }
        /// <summary>
        /// Gets or sets is SCW Service online.
        /// </summary>
        public static bool SCWOnline { get; set; }

        /// <summary>
        /// Variables Static class.
        /// </summary>
        public static class Variables
        {
            /// <summary>Chief Revenue Entry Prerender DateTime.</summary>
            public static DateTime ChiefRevenueLastRenderTime = DateTime.MinValue;
            /// <summary>Collector Revenue Entry Prerender DateTime.</summary>
            public static DateTime CollectorRevenueLastRenderTime = DateTime.MinValue;
        }

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
        /// Pages Static class.
        /// </summary>
        public static class Pages
        {
            #region Main Menu

            private static TOD.Pages.Menu.MainMenu _MainMenu;

            /// <summary>Gets Main Menu Page.</summary>
            public static TOD.Pages.Menu.MainMenu MainMenu
            {
                get
                {
                    if (null == _MainMenu)
                    {
                        lock (typeof(TODApp))
                        {
                            _MainMenu = new TOD.Pages.Menu.MainMenu();
                        }
                    }
                    return _MainMenu;
                }
            }

            #endregion

            #region Report Menu

            private static TOD.Pages.Menu.ReportMenu _ReportMenu;

            /// <summary>Gets Report Menu Page.</summary>
            public static TOD.Pages.Menu.ReportMenu ReportMenu
            {
                get
                {
                    if (null == _ReportMenu)
                    {
                        lock (typeof(TODApp))
                        {
                            _ReportMenu = new TOD.Pages.Menu.ReportMenu();
                        }
                    }
                    return _ReportMenu;
                }
            }

            #endregion

            #region Configuration Menu

            private static TOD.Pages.Menu.ConfigurationMenu _ConfigurationMenu;

            /// <summary>Gets _Configuration Menu Page.</summary>
            public static TOD.Pages.Menu.ConfigurationMenu ConfigurationMenu
            {
                get
                {
                    if (null == _ConfigurationMenu)
                    {
                        lock (typeof(TODApp))
                        {
                            _ConfigurationMenu = new TOD.Pages.Menu.ConfigurationMenu();
                        }
                    }
                    return _ConfigurationMenu;
                }
            }

            #endregion

            #region Revenue (Chief)

            private static TOD.Pages.Revenue.ChiefRevenueEntryPage _ChiefRevenueEntry;

            /// <summary>Gets Revenue (Chief) Page.</summary>
            public static TOD.Pages.Revenue.ChiefRevenueEntryPage ChiefRevenueEntry
            {
                get
                {
                    if (null == _ChiefRevenueEntry)
                    {
                        lock (typeof(TODApp))
                        {
                            _ChiefRevenueEntry = new TOD.Pages.Revenue.ChiefRevenueEntryPage();
                        }
                    }
                    return _ChiefRevenueEntry;
                }
            }

            #endregion

            #region Revenue (Collector)

            private static TOD.Pages.Revenue.CollectorRevenueEntryPage _CollectorRevenueEntry;

            /// <summary>Gets Revenue (Collector) Page.</summary>
            public static TOD.Pages.Revenue.CollectorRevenueEntryPage CollectorRevenueEntry
            {
                get
                {
                    if (null == _CollectorRevenueEntry)
                    {
                        lock (typeof(TODApp))
                        {
                            _CollectorRevenueEntry = new TOD.Pages.Revenue.CollectorRevenueEntryPage();
                        }
                    }
                    return _CollectorRevenueEntry;
                }
            }

            #endregion

            #region Change Shift

            private static TOD.Pages.TollAdmin.ChangeShiftPage _ChangeShift;

            /// <summary>Gets Change Shift Page.</summary>
            public static TOD.Pages.TollAdmin.ChangeShiftPage ChangeShift
            {
                get
                {
                    if (null == _ChangeShift)
                    {
                        lock (typeof(TODApp))
                        {
                            _ChangeShift = new TOD.Pages.TollAdmin.ChangeShiftPage();
                        }
                    }
                    return _ChangeShift;
                }
            }

            #endregion

            #region EMV/QRCode

            private static TOD.Pages.TollAdmin.EMVQRCodeListPage _EMVQRCode;

            /// <summary>Gets EMV/QRCode Page.</summary>
            public static TOD.Pages.TollAdmin.EMVQRCodeListPage EMVQRCode
            {
                get
                {
                    if (null == _EMVQRCode)
                    {
                        lock (typeof(TODApp))
                        {
                            _EMVQRCode = new TOD.Pages.TollAdmin.EMVQRCodeListPage();
                        }
                    }
                    return _EMVQRCode;
                }
            }

            #endregion

            #region Job List

            private static TOD.Pages.TollAdmin.JobListPage _JobList;

            /// <summary>Gets Job List Page.</summary>
            public static TOD.Pages.TollAdmin.JobListPage JobList
            {
                get
                {
                    if (null == _JobList)
                    {
                        lock (typeof(TODApp))
                        {
                            _JobList = new TOD.Pages.TollAdmin.JobListPage();
                        }
                    }
                    return _JobList;
                }
            }

            #endregion

            #region Report Revenue Slip (Empty)

            private static TOD.Pages.Reports.EmpytRevenueSlipPage _EmptyRevenueSlip;

            /// <summary>Gets Report Revenue Slip (Empty) Page.</summary>
            public static TOD.Pages.Reports.EmpytRevenueSlipPage EmptyRevenueSlip
            {
                get
                {
                    if (null == _EmptyRevenueSlip)
                    {
                        lock (typeof(TODApp))
                        {
                            _EmptyRevenueSlip = new TOD.Pages.Reports.EmpytRevenueSlipPage();
                        }
                    }
                    return _EmptyRevenueSlip;
                }
            }


            private static TOD.Pages.Reports.RevenueSlipPreviewPage _RevenueSlipPreview;

            /// <summary>Gets Report Revenue Slip Page.</summary>
            public static TOD.Pages.Reports.RevenueSlipPreviewPage RevenueSlipPreview
            {
                get
                {
                    if (null == _RevenueSlipPreview)
                    {
                        lock (typeof(TODApp))
                        {
                            _RevenueSlipPreview = new TOD.Pages.Reports.RevenueSlipPreviewPage();
                        }
                    }
                    return _RevenueSlipPreview;
                }
            }

            private static TOD.Pages.Reports.DailyRevenueSummaryPreviewPage _DailyRevenueSummaryPreview;

            /// <summary>Gets Report Daily Revenue Summary Page.</summary>
            public static TOD.Pages.Reports.DailyRevenueSummaryPreviewPage DailyRevenueSummaryPreview
            {
                get
                {
                    if (null == _DailyRevenueSummaryPreview)
                    {
                        lock (typeof(TODApp))
                        {
                            _DailyRevenueSummaryPreview = new TOD.Pages.Reports.DailyRevenueSummaryPreviewPage();
                        }
                    }
                    return _DailyRevenueSummaryPreview;
                }
            }

            #endregion
        }

        /// <summary>
        /// Windows Static class.
        /// </summary>
        public static class Windows
        {
            #region SignIn

            /// <summary>Gets SignIn Window.</summary>
            public static DMT.Windows.SignInWindow SignIn
            {
                get 
                {
                    var ret = new DMT.Windows.SignInWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret; 
                }
            }

            #endregion

            #region BOS (Begin Of Shift)

            /// <summary>Gets BOS (Begin Of Shift Window.</summary>
            public static TOD.Windows.UserShifts.BOSWindow BOS
            {
                get 
                {
                    var ret = new TOD.Windows.UserShifts.BOSWindow();
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

            #region SupAdjStatusWindow

            /// <summary>Gets SupAdjStatus Window.</summary>
            public static DMT.Windows.SupAdjStatusWindow SupAdjStatus
            {
                get
                {
                    var ret = new DMT.Windows.SupAdjStatusWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion


            #region Reports

            /// <summary>Gets Revenue Slip Search Window.</summary>
            public static DMT.TOD.Windows.Reports.RevenueSlipSearchWindow RevenueSlipSearch
            {
                get
                {
                    var ret = new DMT.TOD.Windows.Reports.RevenueSlipSearchWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            /// <summary>Gets Revenue Summary Search Window.</summary>
            public static DMT.TOD.Windows.Reports.RevenueSummarySearchWindow RevenueSummarySearch
            {
                get
                {
                    var ret = new DMT.TOD.Windows.Reports.RevenueSummarySearchWindow();
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

        /// <summary>
        /// Chief Static class.
        /// </summary>
        public static class Chief
        {
            /// <summary>
            /// Gets or sets current login chief.
            /// </summary>
            public static User Current { get; set; }
        }
    }
}
