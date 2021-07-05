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
    /// The SimApp class.
    /// </summary>
    public static class SimApp
    {
        /// <summary>
        /// Pages Static class.
        /// </summary>
        public static class Pages
        {
            #region Lane Activity

            private static Simulator.Pages.LaneActivityPage _LaneActivity;

            /// <summary>Gets Lane Activity Page.</summary>
            public static Simulator.Pages.LaneActivityPage LaneActivity
            {
                get
                {
                    if (null == _LaneActivity)
                    {
                        lock (typeof(SimApp))
                        {
                            _LaneActivity = new Simulator.Pages.LaneActivityPage();
                        }
                    }
                    return _LaneActivity;
                }
            }

            #endregion

            #region Bank Note Entry

            private static Simulator.Pages.BankNoteEntryPage _BankNoteEntry;

            /// <summary>Gets Bank Note Entry Page.</summary>
            public static Simulator.Pages.BankNoteEntryPage BankNoteEntry
            {
                get
                {
                    if (null == _BankNoteEntry)
                    {
                        lock (typeof(SimApp))
                        {
                            _BankNoteEntry = new Simulator.Pages.BankNoteEntryPage();
                        }
                    }
                    return _BankNoteEntry;
                }
            }

            #endregion

            #region TA Server Get Coupon

            private static Simulator.Pages.TAServerGetCoupon _TAServerGetCoupon;

            /// <summary>Gets TAServer Get Coupon Page.</summary>
            public static Simulator.Pages.TAServerGetCoupon TAServerGetCoupon
            {
                get
                {
                    if (null == _TAServerGetCoupon)
                    {
                        lock (typeof(SimApp))
                        {
                            _TAServerGetCoupon = new Simulator.Pages.TAServerGetCoupon();
                        }
                    }
                    return _TAServerGetCoupon;
                }
            }

            #endregion

            #region TA Server Sync Service

            private static Simulator.Pages.TAServerCouponSyncPage _TAServerCouponSync;

            /// <summary>Gets TAS erver Sunc Coupon Page.</summary>
            public static Simulator.Pages.TAServerCouponSyncPage TAServerCouponSync
            {
                get
                {
                    if (null == _TAServerCouponSync)
                    {
                        lock (typeof(SimApp))
                        {
                            _TAServerCouponSync = new Simulator.Pages.TAServerCouponSyncPage();
                        }
                    }
                    return _TAServerCouponSync;
                }
            }

            #endregion

            #region Sup Adj Server

            private static Simulator.Pages.SubAdjServerPage _SubAdjServer;

            /// <summary>Gets Sup Adj Server Page.</summary>
            public static Simulator.Pages.SubAdjServerPage SubAdjServer
            {
                get
                {
                    if (null == _SubAdjServer)
                    {
                        lock (typeof(SimApp))
                        {
                            _SubAdjServer = new Simulator.Pages.SubAdjServerPage();
                        }
                    }
                    return _SubAdjServer;
                }
            }

            #endregion
        }

        /// <summary>
        /// Windows Static class.
        /// </summary>
        public static class Windows
        {
            #region User List

            /// <summary>Gets User List Window.</summary>
            public static Simulator.Windows.UserListWindow UserList
            {
                get 
                {
                    var ret = new Simulator.Windows.UserListWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret; 
                }
            }

            #endregion

            #region Payment

            /// <summary>Gets Payment Window.</summary>
            public static Simulator.Windows.PaymentWindow Payment
            {
                get 
                { 
                    var ret = new Simulator.Windows.PaymentWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion
        }
    }
}
