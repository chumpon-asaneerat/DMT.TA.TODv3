#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;

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
                        DMT.Windows.MessageBoxWindow msg = new DMT.Windows.MessageBoxWindow();
                        msg.Owner = Application.Current.MainWindow;
                        msg.Setup("ไม่สามารถเปิดกะใหม่ได้ เนื่องจาก ยังมีกะที่ยังไม่ป้อนรายได้", "DMT - Tour of Duty");
                        msg.ShowDialog();
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
        }

        #endregion
    }
}
