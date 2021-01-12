#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using NLib;

#endregion

namespace DMT.Controls.StatusBar
{
    /// <summary>
    /// Interaction logic for AppInfoStatus.xaml
    /// </summary>
    public partial class AppInfoStatus : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public AppInfoStatus()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }

        #endregion

        private void UpdateUI()
        {
            txtAppInfo.Text = ApplicationManager.Instance.Environments.Options.AppInfo.DisplayText;
        }
    }
}
