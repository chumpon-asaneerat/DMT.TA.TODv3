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

        // TODO: Refactor HeaderShift elements LocalOperations
        //private LocalOperations ops = LocalServiceOperations.Instance.Plaza;

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
            /*
            // TODO: Refactor HeaderShift element Update current shift
            var ret = ops.Shifts.GetCurrent();
            var shift = ret.Value();
            if (null != shift)
            {
                // TODO: Refactor
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
            */
        }
    }
}
