#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
//using System.Windows.Forms;
//using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection;

using DMT.Configurations;
using DMT.Controls;
using DMT.Services;
using DMT.Models;
using DMT.Models.ExtensionMethods;

using NLib;
using NLib.IO;
using NLib.Reflection;

using RestSharp;

#endregion

namespace DMT.Services
{
    using scwOps = Services.Operations.SCW.TOD;


    #region TODAPI

    /// <summary>
    /// The TODAPI class.
    /// </summary>
    public static class TODAPI
    {
        #region Static Properties

        /// <summary>The NetWorkId for SCW.</summary>
        public static int NetworkId 
        { 
            get { return TODConfigManager.Instance.DMT.networkId; } 
        }

        #region TSB/PlazaGroups/Plazas/Lanes

        /// <summary>
        /// Gets Current TSB.
        /// </summary>
        public static TSB TSB 
        { 
            get { return TSB.GetCurrent().Value(); } 
        }
        /// <summary>
        /// Gets TSB PlazaGroups.
        /// </summary>
        public static List<PlazaGroup> TSBPlazaGroups 
        { 
            get { return PlazaGroup.GetTSBPlazaGroups(TODAPI.TSB).Value(); } 
        }
        /// <summary>
        /// Gets TSB Plazas.
        /// </summary>
        public static List<Plaza> TSBPlazas
        {
            get { return Plaza.GetTSBPlazas(TSB).Value(); }
        }
        /// <summary>
        /// Gets TSB Lanes.
        /// </summary>
        public static List<Lane> TSBLanes
        {
            get { return Lane.GetTSBLanes(TSB).Value(); }
        }

        #endregion

        #region Shifts

        /// <summary>
        /// Gets Shifts.
        /// </summary>
        public static List<Models.Shift> Shifts 
        { 
            get { return Models.Shift.GetShifts().Value(); } 
        }

        #endregion

        #region TSBShift

        /// <summary>
        /// Gets Current TSB Shift.
        /// </summary>
        public static TSBShift TSBShift 
        {
            get { return TSBShift.GetTSBShift(TSB.TSBId).Value(); }
        }

        #endregion

        #region UserShifts

        /// <summary>
        /// Gets Unclose User Shifts.
        /// </summary>
        public static List<UserShift> UnCloseUserShifts 
        { 
            get { return UserShift.GetUnCloseUserShifts().Value(); } 
        }

        #endregion

        #endregion

        #region Static Methods

        /// <summary>
        /// Search User By partial User Id.
        /// </summary>
        /// <param name="userId">The partial User Id.</param>
        /// <param name="permissions">The permission roles.</param>
        /// <param name="title">The Window Title (optional).</param>
        /// <returns>Returns UserSearchResult instance.</returns>
        public static UserSearchResult SearchUser(string userId, 
            string[] permissions,
            string title = "กรุณาเลือกพนักงานเก็บเงิน")
        {
            if (string.IsNullOrEmpty(userId)) 
                return new UserSearchResult() { User = null, IsCanceled = true };
            UserSearchManager.Instance.Title = title;
            return UserSearchManager.Instance.SelectUser(userId, permissions);
        }

        #endregion

        #region Extension Methods

        /// <summary>
        /// Gets Current Chief.
        /// </summary>
        /// <param name="value">The TSB Shift.</param>
        /// <returns>Returns Current User (Chief).</returns>
        public static User Chief(this TSBShift value)
        {
            if (null == value) return null;
            return User.GetByUserId(value.UserId).Value();
        }

        #endregion
    }

    #endregion

    #region CurrentTSBManager

    /// <summary>
    /// The CurrentTSBManager Manager Class.
    /// </summary>
    public class CurrentTSBManager
    {
        #region Internal Variables

        private Models.Shift _shift = null;
        private PlazaGroup _plazaGroup = null;
        private User _user = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CurrentTSBManager() : base()
        {
            Refresh();
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~CurrentTSBManager() { }

        #endregion

        #region Private Methods

        private void RaiseUserChanged()
        {
            UserChanged.Call(this, EventArgs.Empty);
        }

        private void RaiseShiftChanged()
        {
            ShiftChanged.Call(this, EventArgs.Empty);
        }

        private void RaisePlazaGroupChanged()
        {
            PlazaGroupChanged.Call(this, EventArgs.Empty);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Refresh.
        /// </summary>
        public void Refresh()
        {
            // Clear Master Objects.
            TSBPlazaGroups = null;
            TSBPlazas = null;
            TSBLanes = null;
            TSBShift = null;
            Chief = null;
            Shifts = null;
            // Clear Selections.
            Shift = null;
            PlazaGroup = null;
            PlazaGroupPlazas = null;
            User = null;

            // Init TSB 
            TSB = TODAPI.TSB;
            if (null != TSB)
            {
                // Load Plaza Groups, Plazas and Lanes
                TSBPlazaGroups = TODAPI.TSBPlazaGroups;
                TSBPlazas = TODAPI.TSBPlazas;
                TSBLanes = TODAPI.TSBLanes;
                // Get Current TSB Shift
                TSBShift = TODAPI.TSBShift;
                // Gets Chief
                Chief = TSBShift.Chief();
            }
            // Init Shifts
            Shifts = TODAPI.Shifts;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Current TSB.
        /// </summary>
        public TSB TSB { get; private set; }
        /// <summary>
        /// Gets TSB Plaza Groups.
        /// </summary>
        public List<PlazaGroup> TSBPlazaGroups { get; private set; }
        /// <summary>
        /// Gets TSB Plazas.
        /// </summary>
        public List<Plaza> TSBPlazas { get; private set; }
        /// <summary>
        /// Gets TSB Lanes.
        /// </summary>
        public List<Lane> TSBLanes { get; private set; }

        /// <summary>
        /// Gets or set PlazaGroup.
        /// </summary>
        public PlazaGroup PlazaGroup
        {
            get { return _plazaGroup; }
            set
            {
                if (null != _plazaGroup && null != value && (_plazaGroup.PlazaGroupId == value.PlazaGroupId))
                    return; // Same PlazaGroupId.

                _plazaGroup = value;

                if (null != _plazaGroup)
                {
                    PlazaGroupPlazas = Plaza.GetPlazaGroupPlazas(_plazaGroup).Value();
                }
                else
                {
                    PlazaGroupPlazas = null;
                }
                // Raise Event.
                RaisePlazaGroupChanged();
            }
        }
        /// <summary>
        /// Gets PlazaGroup Plazas.
        /// </summary>
        public List<Plaza> PlazaGroupPlazas { get; private set; }
        /// <summary>
        /// Gets Shifts.
        /// </summary>
        public List<Models.Shift> Shifts { get; private set; }
        /// <summary>
        /// Gets Current TSB Shift.
        /// </summary>
        public TSBShift TSBShift { get; private set; }
        /// <summary>
        /// Gets Current Shift
        /// </summary>
        public Models.Shift Shift
        {
            get { return _shift; }
            set
            {
                if (null != _shift && null != value && _shift.ShiftId == value.ShiftId)
                    return;

                _shift = value;

                // Raise Event.
                RaiseShiftChanged();
            }
        }
        /// <summary>
        /// Gets Current Chief
        /// </summary>
        public User Chief { get; private set; }
        /// <summary>
        /// Gets or set User.
        /// </summary>
        public User User 
        {
            get { return _user; }
            set
            {
                if (null != _user && null != value && _user.UserId == value.UserId)
                    return; // Same UserId

                _user = value;

                // Raise Event.
                RaiseUserChanged();
            }
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The UserChanged Event Handler.
        /// </summary>
        public event System.EventHandler UserChanged;
        /// <summary>
        /// The ShiftChanged Event Handler.
        /// </summary>
        public event System.EventHandler ShiftChanged;
        /// <summary>
        /// The PlazaGroupChanged Event Handler.
        /// </summary>
        public event System.EventHandler PlazaGroupChanged;

        #endregion
    }

    #endregion

    #region UserShiftManager

    /// <summary>
    /// The UserShiftManager class.
    /// </summary>
    public class UserShiftManager
    {
        #region Internal Variables

        private UserShift _userShift = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        private UserShiftManager() : base()
        {
            IsCustom = false;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="manager">The CurrentTSBManager instance.</param>
        public UserShiftManager(CurrentTSBManager manager) : this()
        {
            Current = manager;
            if (null != Current)
            {
                Current.UserChanged += Current_UserChanged;
                Current.ShiftChanged += Current_ShiftChanged;
                Current.PlazaGroupChanged += Current_PlazaGroupChanged;
            }
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~UserShiftManager()
        {
            if (null != Current)
            {
                Current.PlazaGroupChanged -= Current_PlazaGroupChanged;
                Current.ShiftChanged -= Current_ShiftChanged;
                Current.UserChanged -= Current_UserChanged;
            }
        }

        #endregion

        #region Private Methods

        #region CurrentTSBManager Event Handlers

        private void Current_UserChanged(object sender, EventArgs e)
        {
            if (null == Current || null == Current.User)
            {
                _userShift = null;
            }
            else
            {
                if (!IsCustom)
                {
                    _userShift = UserShift.GetUserShift(Current.User.UserId).Value();
                }
                else
                {
                    // Create new instance.
                    var inst = new UserShift();
                    // Assign properties
                    if (null != Current.TSB) Current.TSB.AssignTo(inst);
                    if (null != Current.Shift) Current.Shift.AssignTo(inst);
                    if (null != Current.User) Current.User.AssignTo(inst);

                    // Update UserShiftId from exists one.
                    if (null != _userShift) inst.UserShiftId = _userShift.UserShiftId;

                    // Update Begin and End Date from exists one.
                    inst.Begin = (null != _userShift) ? _userShift.Begin : new DateTime?();
                    inst.End = (null != _userShift) ? _userShift.End : new DateTime?();

                    // Update to current instance.
                    _userShift = inst;
                }
            }
            // Raise Event.
            UserChanged.Call(sender, e);
            UserShiftChanged.Call(this, EventArgs.Empty);
        }

        private void Current_ShiftChanged(object sender, EventArgs e)
        {
            // Raise Event.
            ShiftChanged.Call(sender, e);
        }

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {
            // Raise Event.
            PlazaGroupChanged.Call(sender, e);
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Refresh data.
        /// </summary>
        public void Refresh()
        {
            _userShift = null;
        }
        /// <summary>
        /// Create New User Shift.
        /// </summary>
        /// <returns></returns>
        public UserShift Create()
        {
            var inst = new UserShift();

            if (null != Current.TSB) Current.TSB.AssignTo(inst);
            if (null != Current.Shift) Current.Shift.AssignTo(inst);
            if (null != Current.User) Current.User.AssignTo(inst);
            inst.Begin = DateTime.Now;
            inst.End = inst.Begin;

            return inst;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Current TSB Manager.
        /// </summary>
        public CurrentTSBManager Current { get; private set; }
        /// <summary>
        /// Gets or sets is custom mode.
        /// </summary>
        public bool IsCustom { get; set; }
        /// <summary>
        /// Gets or sets Current User Shift.
        /// </summary>
        public UserShift Shift
        {
            get { return _userShift; }
            set
            {
                if (!IsCustom) return;
                _userShift = value;
            }
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The UserChanged Event Handler.
        /// </summary>
        public event System.EventHandler UserChanged;
        /// <summary>
        /// The ShiftChanged Event Handler.
        /// </summary>
        public event System.EventHandler ShiftChanged;
        /// <summary>
        /// The PlazaGroupChanged Event Handler.
        /// </summary>
        public event System.EventHandler PlazaGroupChanged;

        /// <summary>
        /// The UserShiftChanged Event Handler.
        /// </summary>
        public event System.EventHandler UserShiftChanged;

        #endregion
    }

    #endregion

    #region JobManager

    /// <summary>
    /// The JobManager class.
    /// </summary>
    public class JobManager
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private JobManager() : base() { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="manager">The CurrentTSBManager instance.</param>
        public JobManager(CurrentTSBManager manager) : this()
        {
            Current = manager;
            if (null != Current)
            {
                Current.UserChanged += Current_UserChanged;
                Current.ShiftChanged += Current_ShiftChanged;
                Current.PlazaGroupChanged += Current_PlazaGroupChanged;
            }
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~JobManager() 
        {
            if (null != Current)
            {
                Current.PlazaGroupChanged -= Current_PlazaGroupChanged;
                Current.ShiftChanged -= Current_ShiftChanged;
                Current.UserChanged -= Current_UserChanged;
            }
        }

        #endregion

        #region Private Methods

        #region CurrentTSBManager Event Handlers

        private void Current_UserChanged(object sender, EventArgs e)
        {
            // Raise Event.
            UserChanged.Call(sender, e);
        }

        private void Current_ShiftChanged(object sender, EventArgs e)
        {
            // Raise Event.
            ShiftChanged.Call(sender, e);
        }

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {
            // Raise Event.
            PlazaGroupChanged.Call(sender, e);
        }

        #endregion

        private void LoadTSBJobs()
        {
            UserShift usrShift = this.UserShift;

            // Create new job list.
            if (null == AllJobs) AllJobs = new List<LaneJob>();
            AllJobs.Clear();

            if (null == PlazaGroupJobs) PlazaGroupJobs = new List<LaneJob>();
            PlazaGroupJobs.Clear();

            if (null == usrShift) return; // No data assigned.

            if (OnlyJobInShift)
            {
                if (!usrShift.Begin.HasValue) return;
            }
            bool isOnline = false;
            var tsbPlazas = Current.TSBPlazas;
            if (null != tsbPlazas)
            {
                var jobs = new List<LaneJob>();

                tsbPlazas.ForEach(plaza =>
                {
                    // Load job for each user.
                    var param = new SCWJobList();
                    param.networkId = TODAPI.NetworkId;
                    param.plazaId = plaza.SCWPlazaId;
                    param.staffId = usrShift.UserId;

                    var ret = scwOps.jobList(param);
                    // Checks execute status.
                    isOnline = (null != ret && null != ret.status && ret.status.code == "S200");
                    if (isOnline && null != ret.list && ret.list.Count > 0)
                    {
                        // Loop to find match job.
                        ret.list.ForEach(job =>
                        {
                            if (OnlyJobInShift)
                            {
                                // Only job match match plaza id and 
                                // BOJ DateTime is greater thant UserShift Begin DateTime.
                                if (job.plazaId.Value == plaza.SCWPlazaId &&
                                    job.bojDateTime.HasValue &&
                                    usrShift.Begin.Value <= job.bojDateTime.Value)
                                {
                                    jobs.Add(new LaneJob(job, usrShift));
                                }
                            }
                            else
                            {
                                // All Job match plaza id.
                                if (job.plazaId.Value == plaza.SCWPlazaId &&
                                    job.bojDateTime.HasValue)
                                {
                                    jobs.Add(new LaneJob(job, usrShift));
                                }
                            }
                        });
                    }
                });

                // sort by BOJ DateTime and assigned to jobs list.
                AllJobs.AddRange(jobs.OrderBy(x => x.Begin).ToArray());

                LoadPlazaGroupJobs();
            }
        }

        private void LoadPlazaGroupJobs()
        {
            if (null == PlazaGroupJobs) PlazaGroupJobs = new List<LaneJob>();
            PlazaGroupJobs.Clear();

            if (null == PlazaGroup) return;

            var plazagroupPlazas = Plaza.GetPlazaGroupPlazas(PlazaGroup).Value();

            if (null == plazagroupPlazas || null == AllJobs || AllJobs.Count <= 0)
                return;

            if (null != plazagroupPlazas)
            {
                plazagroupPlazas.ForEach(plaza =>
                {
                    AllJobs.ForEach(job =>
                    {
                        if (job.PlazaGroupId == plaza.PlazaGroupId)
                        {
                            // Match Selected Plaza Group Id.
                            PlazaGroupJobs.Add(job);
                        }
                    });
                });
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Refresh Jobs.
        /// </summary>
        public void Refresh()
        {
            LoadTSBJobs();
        }
        /// <summary>
        /// Gets all jobs or seelcted jobs lane string i.e. 3,4,5.
        /// </summary>
        /// <param name="checkSelected"></param>
        /// <returns></returns>
        public string GetLaneString(bool checkSelected)
        {
            var Lanes = new List<int>();
            if (null != PlazaGroupJobs)
            {
                PlazaGroupJobs.ForEach(job =>
                {
                    if (checkSelected)
                    {
                        // Check if user not checked item in list view ignore it.
                        if (!job.Selected) return; 
                    }
                    if (!job.LaneNo.HasValue) return;
                    if (!Lanes.Contains(job.LaneNo.Value))
                    {
                        // add to list
                        Lanes.Add(job.LaneNo.Value);
                    }
                });
            }
            // Build Lane List String.
            int iCnt = 0;
            int iMax = Lanes.Count;
            string laneList = string.Empty;
            Lanes.ForEach(laneNo =>
            {
                laneList += laneNo.ToString();
                if (iCnt < iMax - 1) laneList += ", ";
                iCnt++;
            });
            return laneList;
        }

        /// <summary>
        /// Checks is user selection is continuous.
        /// </summary>
        public bool IsContinuous
        {
            get
            {
                bool isContinuous = true;
                if (OnlyJobInShift) return isContinuous;

                if (null != PlazaGroupJobs && PlazaGroupJobs.Count > 0)
                {
                    // Create indexes list.
                    int idx = 0;
                    List<int> indexs = new List<int>();
                    foreach (var job in PlazaGroupJobs)
                    {
                        if (job.Selected) indexs.Add(idx);
                        idx++;
                    }
                    // Check Continuous
                    if (null != indexs && indexs.Count > 0)
                    {
                        // 3, 4, 5, 7
                        int currIndex = indexs[0] - 1; // set init value to first minus 1 for check in loop.
                        foreach (int val in indexs)
                        {
                            if (val - 1 > currIndex)
                            {
                                isContinuous = false;
                                break;
                            }
                            currIndex = val; // update new current index.
                        }
                    }
                }

                return isContinuous;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Current TSB Manager.
        /// </summary>
        public CurrentTSBManager Current { get; private set; }
        /// <summary>
        /// Gets or sets UserShift (used for AllJobs and PlazaGroupJobs).
        /// </summary>
        public UserShift UserShift { get; set; }
        /// <summary>
        /// Gets All Jobs for specificed user on current shift.
        /// </summary>
        public List<LaneJob> AllJobs { get; private set; }
        /// <summary>
        /// Gets or sets PlazaGroup (used for PlazaGroupJobs).
        /// </summary>
        public PlazaGroup PlazaGroup { get; set; }
        /// <summary>
        /// Gets Current Jobs for specificed user on current shift and plaza group.
        /// </summary>
        public List<LaneJob> PlazaGroupJobs { get; private set; }
        /// <summary>
        /// Gets or sets show only job between User Shift Begin to End DateTime.
        /// </summary>
        public bool OnlyJobInShift { get; set; }

        #endregion

        #region Public Events

        /// <summary>
        /// The UserChanged Event Handler.
        /// </summary>
        public event System.EventHandler UserChanged;
        /// <summary>
        /// The ShiftChanged Event Handler.
        /// </summary>
        public event System.EventHandler ShiftChanged;
        /// <summary>
        /// The PlazaGroupChanged Event Handler.
        /// </summary>
        public event System.EventHandler PlazaGroupChanged;

        #endregion
    }

    #endregion

    #region PaymentManager

    /// <summary>
    /// The PaymentManager class.
    /// </summary>
    public class PaymentManager
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private PaymentManager() : base() { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="manager">The CurrentTSBManager instance.</param>
        public PaymentManager(CurrentTSBManager manager) : this()
        {
            Current = manager;
            if (null != Current)
            {
                Current.UserChanged += Current_UserChanged;
                Current.ShiftChanged += Current_ShiftChanged;
                Current.PlazaGroupChanged += Current_PlazaGroupChanged;
            }
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~PaymentManager()
        {
            if (null != Current)
            {
                Current.PlazaGroupChanged -= Current_PlazaGroupChanged;
                Current.ShiftChanged -= Current_ShiftChanged;
                Current.UserChanged -= Current_UserChanged;
            }
        }

        #endregion

        #region Private Methods

        #region CurrentTSBManager Event Handlers

        private void Current_UserChanged(object sender, EventArgs e)
        {
            // Raise Event.
            UserChanged.Call(sender, e);
        }

        private void Current_ShiftChanged(object sender, EventArgs e)
        {
            // Raise Event.
            ShiftChanged.Call(sender, e);
        }

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {
            // Raise Event.
            PlazaGroupChanged.Call(sender, e);
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Refresh all payments.
        /// </summary>
        public void Refresh()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Current TSB Manager.
        /// </summary>
        public CurrentTSBManager Current { get; private set; }

        #endregion

        #region Public Events

        /// <summary>
        /// The UserChanged Event Handler.
        /// </summary>
        public event System.EventHandler UserChanged;
        /// <summary>
        /// The ShiftChanged Event Handler.
        /// </summary>
        public event System.EventHandler ShiftChanged;
        /// <summary>
        /// The PlazaGroupChanged Event Handler.
        /// </summary>
        public event System.EventHandler PlazaGroupChanged;

        #endregion
    }

    #endregion

    #region RevenueEntryManager

    /// <summary>
    /// The RevenueEntryManager class.
    /// </summary>
    public class RevenueEntryManager : INotifyPropertyChanged
    {
        #region Internal Variables

        private DateTime _now = DateTime.Now;

        private bool _byChief = false;
        private DateTime? _RevenueDate = new DateTime?();
        private DateTime? _EntryDate = new DateTime?();

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RevenueEntryManager() : base()
        {
            Current = new CurrentTSBManager();
            if (null != Current)
            {
                Current.UserChanged += Current_UserChanged;
                Current.ShiftChanged += Current_ShiftChanged;
                Current.PlazaGroupChanged += Current_PlazaGroupChanged;
            }

            Jobs = new JobManager(Current);

            Payments = new PaymentManager(Current);

            UserShifts = new UserShiftManager(Current);
            if (null != UserShifts)
            {
                UserShifts.UserShiftChanged += UserShifts_UserShiftChanged;
            }

            Refresh();
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~RevenueEntryManager()
        {
            if (null != UserShifts)
            {
                UserShifts.UserShiftChanged -= UserShifts_UserShiftChanged;
            }

            if (null != Current)
            {
                Current.PlazaGroupChanged -= Current_PlazaGroupChanged;
                Current.ShiftChanged -= Current_ShiftChanged;
                Current.UserChanged -= Current_UserChanged;
            }
        }

        #endregion

        #region Private Methods

        #region CurrentTSBManager Event Handlers

        private void Current_UserChanged(object sender, EventArgs e)
        {
            // Raise Event.
            UserChanged.Call(sender, e);
        }

        private void Current_ShiftChanged(object sender, EventArgs e)
        {
            // Raise Event.
            ShiftChanged.Call(sender, e);
        }

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {
            // Raise Event.
            PlazaGroupChanged.Call(sender, e);
        }

        #endregion

        #region UserShiftManager EventHandlers

        private void UserShifts_UserShiftChanged(object sender, EventArgs e)
        {
            CheckUserShift();
        }

        #endregion

        #region Event Raisers

        /// <summary>
        /// Raise Property Changed Event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void RaiseChanged(string propertyName)
        {
            PropertyChanged.Call(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Revenue/Entry Date Check method(s)

        private void CheckRevenueDate()
        {
            if (!ByChief)
            {
                if (HasUserShift)
                {
                    var shift = UserShifts.Shift;
                    RevenueDate = (shift.Begin.HasValue) ? shift.Begin.Value.Date : new DateTime?(_now);
                }
                else
                {
                    RevenueDate = new DateTime?(_now);
                }
            }
            else
            {
                // By Chief
                RevenueDate = new DateTime?(_now);
            }
        }

        #endregion

        #region UserShift method(s)

        private void CheckUserShift()
        {
            CheckRevenueDate();
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Refresh.
        /// </summary>
        public void Refresh()
        {
            // Readonly field so need manual raise related events.
            _EntryDate = new DateTime?(_now);
            RaiseChanged("EntryDate");
            RaiseChanged("EntryDateString");
            RaiseChanged("EntryDateTimeString");

            this.RevenueDate = new DateTime?(_now);

            if (null != Current) Current.Refresh();
            if (null != UserShifts) UserShifts.Refresh();
            if (null != Jobs) Jobs.Refresh();
            if (null != Payments) Payments.Refresh();
        }

        #endregion

        #region Public Properties

        #region Managers

        /// <summary>
        /// Gets Current TSB Manager.
        /// </summary>
        public CurrentTSBManager Current { get; private set; }
        /// <summary>
        /// Gets Job Manager.
        /// </summary>
        public JobManager Jobs { get; private set; }
        /// <summary>
        /// Gets Payment Manager.
        /// </summary>
        public PaymentManager Payments { get; private set; }
        /// <summary>
        /// Gets User Shift Manager.
        /// </summary>
        public UserShiftManager UserShifts { get; private set; }

        #endregion

        #region PlazaGroup

        /// <summary>
        /// Gets or set PlazaGroup.
        /// </summary>
        public PlazaGroup PlazaGroup
        {
            get { return (null != Current) ? Current.PlazaGroup : null; }
            set
            {
                if (null != Current)
                {
                    Current.PlazaGroup = value;
                    RaiseChanged("PlazaGroupId");
                    RaiseChanged("PlazaGroupNameEN");
                    RaiseChanged("PlazaGroupNameTH");
                }
            }
        }
        /// <summary>
        /// Gets PlazaGroup Id.
        /// </summary>
        public string PlazaGroupId
        {
            get { return (null != Current && null != Current.PlazaGroup) ? Current.PlazaGroup.PlazaGroupId : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets PlazaGroup Name EN.
        /// </summary>
        public string PlazaGroupNameEN
        {
            get { return (null != Current && null != Current.PlazaGroup) ? Current.PlazaGroup.PlazaGroupNameEN : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets PlazaGroup TH.
        /// </summary>
        public string PlazaGroupNameTH
        {
            get { return (null != Current && null != Current.PlazaGroup) ? Current.PlazaGroup.PlazaGroupNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region User Shift

        /// <summary>
        /// Checks Has User Shift.
        /// </summary>
        public bool HasUserShift
        {
            get { return (null != UserShifts && null != UserShifts.Shift); }
        }
        /// <summary>
        /// Gets Collector UserShift.
        /// </summary>
        public UserShift UserShift
        {
            get { return (HasUserShift) ? UserShifts.Shift : null; }
        }
        /// <summary>
        /// Gets Shift Name EN.
        /// </summary>
        public string ShiftNameEN
        {
            get { return (HasUserShift) ? UserShifts.Shift.ShiftNameEN : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Shift Name TH.
        /// </summary>
        public string ShiftNameTH
        {
            get { return (HasUserShift) ? UserShifts.Shift.ShiftNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region ByChief

        /// <summary>Gets or sets is revenue by chief.</summary>
        public bool ByChief 
        {
            get { return _byChief; }
            set
            {
                if (_byChief != value)
                {
                    _byChief = value;
                    UserShifts.IsCustom = _byChief; // in chief mode user shift can custom.
                    CheckRevenueDate();
                    RaiseChanged("ByChief");
                }
            }
        }

        #endregion

        #region EntryDate

        /// <summary>
        /// Gets Entry Date.
        /// </summary>
        public DateTime? EntryDate 
        {
            get { return _EntryDate; }
            set { }
        }
        /// <summary>
        /// Gets Entry Date String.
        /// </summary>
        public string EntryDateString 
        {
            get 
            { 
                return (EntryDate.HasValue) ? 
                    EntryDate.Value.ToThaiDateTimeString("dd/MM/yyyy") : string.Empty; 
            }
        }
        /// <summary>
        /// Gets Entry DateTime String.
        /// </summary>
        public string EntryDateTimeString
        {
            get
            {
                return (EntryDate.HasValue) ?
                    EntryDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss") : string.Empty;
            }
        }

        #endregion

        #region RevenueDate

        /// <summary>
        /// Gets or sets Revenue Date.
        /// </summary>
        public DateTime? RevenueDate
        {
            get { return _RevenueDate; }
            set
            {
                if (_RevenueDate != value)
                {
                    _RevenueDate = value;
                    RaiseChanged("RevenueDate");
                    RaiseChanged("RevenueDateString");
                    RaiseChanged("RevenueDateTimeString");
                }
            }
        }
        /// <summary>
        /// Gets Revenue Date String.
        /// </summary>
        public string RevenueDateString
        {
            get
            {
                return (RevenueDate.HasValue) ?
                    RevenueDate.Value.ToThaiDateTimeString("dd/MM/yyyy") : string.Empty;
            }
        }
        /// <summary>
        /// Gets Revenue DateTime String.
        /// </summary>
        public string RevenueDateTimeString
        {
            get
            {
                return (RevenueDate.HasValue) ?
                    RevenueDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss") : string.Empty;
            }
        }

        #endregion

        #region User/Chief (Current)

        /// <summary>
        /// Gets or set User (Collector).
        /// </summary>
        public User User 
        { 
            get { return (null != Current) ? Current.User : null; }
            set 
            {
                if (null != Current)
                {
                    Current.User = value;
                    RaiseChanged("CollectorId");
                    RaiseChanged("CollectorNameEN");
                    RaiseChanged("CollectorNameTH");
                }
            }
        }

        /// <summary>
        /// Gets Collector Id.
        /// </summary>
        public string CollectorId
        {
            get { return (null != Current && null != Current.User) ? Current.User.UserId : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Collector Name EN.
        /// </summary>
        public string CollectorNameEN
        {
            get { return (null != Current && null != Current.User) ? Current.User.FullNameEN : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Collector Name TH.
        /// </summary>
        public string CollectorNameTH
        {
            get { return (null != Current && null != Current.User) ? Current.User.FullNameTH : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Current Chief.
        /// </summary>
        public User Chief { get { return this.Current.Chief; } }
        /// <summary>
        /// Gets Chief Id.
        /// </summary>
        public string ChiefId
        {
            get { return (null != Current && null != Current.Chief) ? Current.Chief.UserId : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Chief Name EN.
        /// </summary>
        public string ChiefNameEN
        {
            get { return (null != Current && null != Current.Chief) ? Current.Chief.FullNameEN : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Chief Name TH.
        /// </summary>
        public string ChiefNameTH
        {
            get { return (null != Current && null != Current.Chief) ? Current.Chief.FullNameTH : string.Empty; }
            set { }
        }

        #endregion

        #endregion

        #region Public Events

        /// <summary>
        /// The PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The UserChanged Event Handler.
        /// </summary>
        public event System.EventHandler UserChanged;
        /// <summary>
        /// The ShiftChanged Event Handler.
        /// </summary>
        public event System.EventHandler ShiftChanged;
        /// <summary>
        /// The PlazaGroupChanged Event Handler.
        /// </summary>
        public event System.EventHandler PlazaGroupChanged;

        #endregion
    }

    #endregion
}
