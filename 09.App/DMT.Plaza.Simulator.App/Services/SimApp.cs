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

            #region Bank Note Entry

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
