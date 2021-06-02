#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;
using System.Windows.Threading;

#endregion

namespace DMT.TOD.Windows.UserShifts
{
    /// <summary>
    /// Interaction logic for BOSWindow.xaml
    /// </summary>
    public partial class BOSWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public BOSWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _user = null;
        private TSB _tsb = null;

        #endregion

        #region Window Handlers

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                DialogResult = false;
            }
        }

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            if (cbShift.SelectedIndex == -1)
            {
                cbShift.Focus();
                return;
            }
            Models.Shift shift = cbShift.SelectedItem as Models.Shift;
            if (null != shift)
            {
                UserShift inst = UserShift.Create(shift, _user).Value();
                if (null != inst)
                {
                    var success = UserShift.BeginUserShift(inst).Ok;
                    if (!success)
                    {
                        // Show Message.
                        var msg = TODApp.Windows.MessageBox;
                        msg.Setup("ไม่สามารถเปิดกะใหม่ได้ เนื่องจาก ยังมีกะที่ยังไม่ป้อนรายได้", "DMT - Tour of Duty");
                        msg.ShowDialog();
                    }
                    else
                    {

                        // Send User Shift to TA Message Queue.
                        TAMQService.Instance.WriteQueue(inst);
                    }
                }
            }

            DialogResult = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user">The Collector User.</param>
        public void Setup(User user)
        {
            _user = user;
            if (null != _user)
            {
                DateTime dt = DateTime.Now;
                var shifts = Models.Shift.GetShifts().Value();
                cbShift.ItemsSource = shifts;
                // Auto Select shift by time of day.
                int autoIdx = -1;
                if (null != shifts && shifts.Count > 0)
                    autoIdx = shifts.FindIndex(shift => { return shift.CheckIsCurrent(); });
                if (autoIdx != -1) cbShift.SelectedIndex = autoIdx;

                _tsb = TSB.GetCurrent().Value();
                if (null != _tsb)
                {
                    txtPlaza.Text = _tsb.TSBNameTH;
                }
                txtDate.Text = dt.ToThaiDateString();
                txtTime.Text = dt.ToThaiTimeString();

                txtID.Text = _user.UserId;
                txtName.Text = _user.FullNameTH;
            }

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                cbShift.Focus();
            }));
        }

        #endregion
    }
}
