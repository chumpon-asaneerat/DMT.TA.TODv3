#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using DMT.Models;
using DMT.Services;

#endregion

namespace DMT.Controls.Header
{
    /// <summary>
    /// Interaction logic for HeaderChief.xaml
    /// </summary>
    public partial class HeaderChief : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public HeaderChief()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();
            RuntimeManager.Instance.TSBChanged += Instance_TSBChanged;
            RuntimeManager.Instance.ShiftChanged += Instance_ShiftChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            RuntimeManager.Instance.ShiftChanged -= Instance_ShiftChanged;
            RuntimeManager.Instance.TSBChanged -= Instance_TSBChanged;
        }

        #endregion

        #region RuntimeManager Handlers

        private void Instance_TSBChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void Instance_ShiftChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        #endregion

        private void UpdateUI()
        {
            var shift = TSBShift.GetTSBShift().Value();
            if (null == shift)
            {
                txtSupervisorId.Text = "รหัสหัวหน้าด่าน : ";
                txtSupervisorName.Text = "หัวหน้าด่าน : ";
            }
            else
            {
                txtSupervisorId.Text = "รหัสหัวหน้าด่าน : " + shift.UserId;
                txtSupervisorName.Text = "หัวหน้าด่าน : " + shift.FullNameTH;
            }
        }
    }
}
