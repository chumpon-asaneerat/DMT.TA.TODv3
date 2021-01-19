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
        }

        /// <summary>
        /// Windows Static class.
        /// </summary>
        public static class Windows
        {
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
