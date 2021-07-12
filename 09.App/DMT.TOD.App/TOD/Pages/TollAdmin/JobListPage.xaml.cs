#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;

using DMT.Configurations;
using DMT.Models;
using DMT.Services;

using NLib;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.TOD.Pages.TollAdmin
{
    /// <summary>
    /// Interaction logic for JobListPage.xaml
    /// </summary>
    public partial class JobListPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public JobListPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _sup;
        private CurrentTSBManager manager;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshUserShifts();
        }

        private void cmdCloseShift_Click(object sender, RoutedEventArgs e)
        {
            CloseShift();
        }

        #endregion

        #region ListView Handlers

        private void lstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = lstUsers.SelectedItem as UserShift;
            RefreshJobList(item);
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TODApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void RefreshUserShifts()
        {
            lstUsers.ItemsSource = null; // Reset.
            lstUsers.ItemsSource = TODAPI.UnCloseUserShifts;
            lstUsers.SelectedIndex = -1; // Set SelectedIndex will refresh job list.
        }

        private void RefreshJobList(UserShift value)
        {
            lstLaneJobs.ItemsSource = null;
            if (null == value || !value.Begin.HasValue) return; // no selection.
            // Refresh jobs.
            if (null == manager || null == manager.Jobs) return;

            manager.Jobs.ViewMode = ViewModes.TSB; // View all.
            manager.Jobs.OnlyJobInShift = true;
            manager.Jobs.UserShift = value;// assign selected user shift.
            manager.Jobs.Refresh();

            // Bind to ListView
            lstLaneJobs.ItemsSource = manager.Jobs.AllJobs;
        }

        private void CloseShift()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            string msg;

            if (lstUsers.SelectedIndex == -1)
            {
                var win = TODApp.Windows.MessageBox;
                msg = "กรุณาเลือกกะพนักงาน ที่ต้องการปิดกะ";
                win.Setup(msg, "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            bool online = manager.Jobs.SCWOnline; // online status.
            msg = string.Format("SCW Status: {0}", online ? "Online" : "Offline");
            med.Info("JOB LIST - " + msg); // write log.

            if (null != manager && null != manager.Jobs && null != manager.Jobs.UserShift)
            {
                if (null != manager.Jobs.AllJobs && manager.Jobs.AllJobs.Count > 0)
                {
                    // has one or more jobs.
                    var win = TODApp.Windows.MessageBox;
                    msg = "ไม่สามารถปิดกะได้ " + Environment.NewLine + "เนื่องจาก พบข้อมูลกะพนักงานที่ลงตู้แล้ว";
                    med.Info("JOB LIST - " + msg); // write log.
                    win.Setup(msg, "DMT - Tour of Duty");
                    win.ShowDialog();
                }
                else
                {
                    // get current user shift.
                    var usrShf = manager.Jobs.UserShift;

                    // no job.
                    var win = TODApp.Windows.MessageBoxYesNo;
                    msg = "ไม่พบข้อมูลการลงตู้ของพนักงาน ต้องการปิดกะหรือไม่?";
                    med.Info("JOB LIST - " + msg); // write log.
                    win.Setup(msg, "DMT - Tour of Duty");
                    if (win.ShowDialog() == false)
                    {
                        return; // do nothing.
                    }

                    string chiefInfo = (null == _sup) ?
                        "ไม่พบข้อมูลหัวหน้าพนังงานเก็บเงิน" :
                        string.Format("Chief Id: {0}, Chief Name: {1}", _sup.UserId, _sup.FullNameTH);
                    string userInfo = string.Format("User Id: {0}, User Name: {1}", usrShf.UserId, usrShf.FullNameTH);
                    msg = string.Format("[ {0} ] ทำการปิดกะของ < {1} >",
                        chiefInfo, userInfo);
                    med.Info("JOB LIST - " + msg); // write log.

                    // end user shift.
                    var ret = UserShift.EndUserShift(usrShf).Value();
                    GenerateUserShiftFile(ret);

                    RefreshUserShifts(); // refresh all.
                }
            }
            else
            {
                var win = TODApp.Windows.MessageBox;
                msg = "ไม่สามารถปิดกะได้ เนื่องจากไม่พบข้อมูลกะพนักงาน";
                win.Setup(msg, "DMT - Tour of Duty");
                win.ShowDialog();
            }
        }

        private void GenerateUserShiftFile(UserShift value)
        {
            if (null == value)
                return;
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                // write to TAxTOD message queue.
                TAxTODMQService.Instance.WriteQueue(value);
                // Send Entry to TA Message Queue
                TAMQService.Instance.WriteQueue(value);
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user">The User instance.</param>
        public void Setup(User user)
        {
            _sup = user;
            if (null == manager)
            {
                manager = new CurrentTSBManager();
            }

            // Load User Shifts.
            RefreshUserShifts();

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                lstUsers.Focus();
            }));
        }

        #endregion
    }
}
