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
    /// The ConfigApp class.
    /// </summary>
    public static class ConfigApp
    {
        /// <summary>
        /// Pages Static class.
        /// </summary>
        public static class Pages
        {
            #region Shift View

            private static Config.Pages.ShiftViewPage _ShiftView;

            /// <summary>Gets Shift View Page.</summary>
            public static Config.Pages.ShiftViewPage ShiftView
            {
                get
                {
                    if (null == _ShiftView)
                    {
                        lock (typeof(ConfigApp))
                        {
                            _ShiftView = new Config.Pages.ShiftViewPage();
                        }
                    }
                    return _ShiftView;
                }
            }

            #endregion

            #region User View

            private static Config.Pages.UserViewPage _UserView;

            /// <summary>Gets User View Entry Page.</summary>
            public static Config.Pages.UserViewPage UserView
            {
                get
                {
                    if (null == _UserView)
                    {
                        lock (typeof(ConfigApp))
                        {
                            _UserView = new Config.Pages.UserViewPage();
                        }
                    }
                    return _UserView;
                }
            }

            #endregion

            #region TSB View

            private static Config.Pages.TSBViewPage _TSBView;

            /// <summary>Gets TSB View Page.</summary>
            public static Config.Pages.TSBViewPage TSBView
            {
                get
                {
                    if (null == _TSBView)
                    {
                        lock (typeof(ConfigApp))
                        {
                            _TSBView = new Config.Pages.TSBViewPage();
                        }
                    }
                    return _TSBView;
                }
            }

            #endregion
        }

        /// <summary>
        /// Windows Static class.
        /// </summary>
        public static class Windows
        {

        }
    }
}
