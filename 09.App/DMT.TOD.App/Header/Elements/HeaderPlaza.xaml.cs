#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;

#endregion

namespace DMT.Controls.Header
{
    using ops = Services.Operations.Plaza.Infrastructure; // reference to static class.

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
            RuntimeManager.Instance.TSBChanged += Instance_TSBChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            RuntimeManager.Instance.TSBChanged -= Instance_TSBChanged;
        }

        #endregion

        #region RuntimeManager Handlers

        private void Instance_TSBChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        #endregion

        private void UpdateUI()
        {
            var tsb = ops.TSB.Current().Value();
            if (null != tsb)
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
