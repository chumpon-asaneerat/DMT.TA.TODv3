#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using DMT.Configurations;
using DMT.Models;
using DMT.Services;

using NLib.Services;
using NLib.Reflection;
using System.Windows.Threading;

#endregion

namespace DMT.TOD.Pages.TollAdmin
{
    using scwOps = Services.Operations.SCW.TOD;
    using taOps = Services.Operations.TA.Notify;

    /// <summary>
    /// Interaction logic for ChangeShiftPage.xaml
    /// </summary>
    public partial class ChangeShiftPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChangeShiftPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _user = null;
        private List<UserShift> _usershifts = null;
        private TSB _tsb = null;
        private List<Plaza> _plazas = null;
        private List<Lane> _lanes = null;
        private List<LaneJob> _jobs = null;

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            if (cbShifts.SelectedIndex == -1)
            {
                // Show Message.
                var msg = TODApp.Windows.MessageBox;
                msg.Setup("กรุณาเลือกกะ ที่ต้องการเปลี่ยน", "DMT - Tour of Duty");
                msg.ShowDialog();

                cbShifts.Focus();
                return;
            }
            var shift = cbShifts.SelectedItem as Models.Shift;
            if (null != shift)
            {
                int networkId = TODConfigManager.Instance.DMT.networkId;

                TSBShift inst = TSBShift.Create(shift, _user).Value();
                // set date
                inst.Begin = DateTime.Now;

                // Update TSB Shift
                var ret = TSBShift.ChangeShift(inst);
                if (ret.Ok && null != _user && null != _plazas && _plazas.Count > 0)
                {
                    // Update TOD
                    RuntimeManager.Instance.RaiseTSBShiftChanged();

                    // write to TAxTOD message queue.
                    TAxTODMQService.Instance.WriteQueue(inst);
                    // write to TA App message queue.
                    TAMQService.Instance.WriteQueue(inst);

                    // send to SCW server
                    var scw = new SCWSaveChiefDuty();
                    scw.networkId = networkId;
                    scw.plazaId = Convert.ToInt32(_plazas[0].SCWPlazaId);
                    scw.staffId = _user.UserId;
                    scw.staffTypeId = 1;
                    scw.beginDateTime = inst.Begin;
                    // write to queue.
                    SCWMQService.Instance.WriteQueue(scw);
                }
            }

            GotoMainMenu();
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TODApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void RefreshLanes()
        {
            lvJobs.ItemsSource = null;
            if (null == _tsb || 
                null == _plazas || _plazas.Count <= 0 ||
                null == _lanes || _lanes.Count <= 0 ||
                null == _user) return;

            int networkId = TODConfigManager.Instance.DMT.networkId;
            
            if (null == _jobs)
            {
                // Create new job list.
                _jobs = new List<LaneJob>();
            }
            _jobs.Clear();

            var alljobs = new List<LaneJob>();
            // Gets jobs from each plaza.
            _plazas.ForEach(plaza => 
            {
                if (null != _usershifts && _usershifts.Count > 0)
                {
                    _usershifts.ForEach(userShift => 
                    {
                        // Load job for each user.
                        var param = new SCWJobList();
                        param.networkId = networkId;
                        param.plazaId = plaza.SCWPlazaId;
                        param.staffId = userShift.UserId;

                        var ret = scwOps.jobList(param);
                        if (null != ret && null != ret.list && ret.list.Count > 0)
                        {
                            ret.list.ForEach(job =>
                            {
                                // Maps Lanes to get access more info for binding.
                                // Note: SCW return only laneId so its cannot display more information so we need to map on 
                                // local lane data.
                                var matchLane = _lanes.Find(lane =>
                                {
                                    return job.plazaId == lane.SCWPlazaId && job.laneId == lane.LaneNo;
                                });
                                if (null != matchLane)
                                {
                                    alljobs.Add(new LaneJob(job, matchLane, userShift));
                                }
                            });
                        }
                    });
                }
            });

            // sort and assigned to jobs list.
            _jobs.AddRange(alljobs.OrderBy(x => x.Begin).ToArray());

            lvJobs.ItemsSource = _jobs;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user">The Chief user.</param>
        public void Setup(User user)
        {
            _user = user;
            if (null != _user)
            {
                // Load Shifts.
                DateTime dt = DateTime.Now;
                var shifts = Models.Shift.GetShifts().Value();
                cbShifts.ItemsSource = shifts;
                // Auto Select shift by time of day.
                int autoIdx = -1;
                if (null != shifts && shifts.Count > 0)
                    autoIdx = shifts.FindIndex(shift => { return shift.CheckIsCurrent(); });
                if (autoIdx != -1) cbShifts.SelectedIndex = autoIdx;

                // Get Current TSB and related plazas, lanes.
                _tsb = TSB.GetCurrent().Value();
                if (null == _tsb) return;
                _plazas = Plaza.GetTSBPlazas(_tsb.TSBId).Value();
                _lanes = Lane.GetTSBLanes(_tsb.TSBId).Value();

                // Gets User Shifts that not closed.
                _usershifts = UserShift.GetUnCloseUserShifts().Value();

                // Load related lane data.
                RefreshLanes();
            }

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                cbShifts.Focus();
            }));
        }

        #endregion
    }
}
