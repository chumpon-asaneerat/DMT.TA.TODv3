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
    /// Interaction logic for HeaderShift.xaml
    /// </summary>
    public partial class HeaderShift : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public HeaderShift()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private HeaderBarService service = HeaderBarService.Instance;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();

            if (null != service) service.Register(this.UpdateUI);

            RuntimeManager.Instance.TSBChanged += Instance_TSBChanged;
            RuntimeManager.Instance.TSBShiftChanged += Instance_TSBShiftChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            RuntimeManager.Instance.TSBShiftChanged -= Instance_TSBShiftChanged;
            RuntimeManager.Instance.TSBChanged -= Instance_TSBChanged;

            if (null != service) service.Unregister(this.UpdateUI);
        }

        #endregion

        #region RuntimeManager Handlers

        private void Instance_TSBChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void Instance_TSBShiftChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        #endregion

        private void UpdateUI()
        {
            var shift = TSBShift.GetTSBShift().Value();
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                if (null != shift)
                {
                    txtShiftDate.Text = shift.BeginDateString;
                    txtShiftTime.Text = shift.BeginTimeString;
                    txtShiftId.Text = shift.ShiftNameTH;
                }
                else
                {
                    txtShiftDate.Text = string.Empty;
                    txtShiftTime.Text = string.Empty;
                    txtShiftId.Text = string.Empty;
                }
            }));
        }
    }
}
