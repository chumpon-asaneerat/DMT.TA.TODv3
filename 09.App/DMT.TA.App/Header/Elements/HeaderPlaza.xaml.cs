#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

//using NLib.Services;
using DMT.Configurations;
using DMT.Services;
using DMT.Models;

#endregion

namespace DMT.Controls.Header
{
    /// <summary>
    /// Interaction logic for HeaderPlaza.xaml
    /// </summary>
    public partial class HeaderPlaza : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public HeaderPlaza()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtPlazaId.Visibility = Visibility.Collapsed;

            UpdateUI();

            TAConfigManager.Instance.ConfigChanged += ConfigChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            TAConfigManager.Instance.ConfigChanged -= ConfigChanged;
        }

        #endregion

        #region Config Watcher Handlers

        private void ConfigChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        #endregion

        private PlazaInfoConfig Config
        {
            get
            {
                if (null == TAConfigManager.Instance.Value ||
                    null == TAConfigManager.Instance.Value.UIConfig ||
                    null == TAConfigManager.Instance.Value.UIConfig.StatusBars) return null;
                return TAConfigManager.Instance.Value.UIConfig.HeaderBars.PlazaInfo;
            }
        }

        private void UpdateUI()
        {
            var tsb = TSB.GetCurrent().Value();
            if (null != Config)
            {
                txtPlazaId.Text = "รหัสด่าน : " + tsb.TSBId;
                txtPlazaName.Text = "ชื่อด่าน : " + tsb.TSBNameTH;
            }
            else
            {
                txtPlazaId.Text = "รหัสด่าน : ";
                txtPlazaName.Text = "ชื่อด่าน : ";
            }
        }
    }
}
