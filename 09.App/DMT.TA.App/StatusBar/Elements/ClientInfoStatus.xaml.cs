#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using NLib.Utils;

#endregion

namespace DMT.Controls.StatusBar
{
    /// <summary>
    /// Interaction logic for ClientInfoStatus.xaml
    /// </summary>
    public partial class ClientInfoStatus : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ClientInfoStatus()
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
            var ipaddr = NetworkUtils.GetLocalIPAddress();
            txtStatus.Text = (null != ipaddr) ? ipaddr.ToString() : "0.0.0.0";
        }
    }
}
