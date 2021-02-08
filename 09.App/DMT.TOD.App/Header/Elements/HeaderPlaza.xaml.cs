#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

//using NLib.Services;
using DMT.Configurations;
using DMT.Services;

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

            TODConfigManager.Instance.ConfigChanged += ConfigChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            TODConfigManager.Instance.ConfigChanged -= ConfigChanged;
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
                if (null == TODConfigManager.Instance.Value ||
                    null == TODConfigManager.Instance.Value.UIConfig ||
                    null == TODConfigManager.Instance.Value.UIConfig.StatusBars) return null;
                return TODConfigManager.Instance.Value.UIConfig.HeaderBars.PlazaInfo;
            }
        }

        private void UpdateUI()
        {
            if (null != Config)
            {
                txtPlazaId.Text = "รหัสด่าน : " + Config.PlazaId;
                txtPlazaName.Text = "ชื่อด่าน : " + Config.PlazaNameTH;
            }
            else
            {
                txtPlazaId.Text = "รหัสด่าน : ";
                txtPlazaName.Text = "ชื่อด่าน : ";
            }
        }
    }
}
