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
using NLib.Services;
using NLib.Reports.Rdlc;
using NLib.Reflection;

using RestSharp;

using WebSocketSharp;

#endregion

namespace DMT.Services
{
    using scwOps = Services.Operations.SCW.TOD;
    using taaOps = Services.Operations.TA;

    #region ViewModes Enum

    /// <summary>
    /// The ViewModes Enum
    /// </summary>
    public enum ViewModes
    {
        TSB,
        TOD
    }

    #endregion

    #region PaymentTypes Enum

    /// <summary>
    /// The PaymentTypes Enum
    /// </summary>
    public enum PaymentTypes
    {
        EMV,
        QRCode,
        Both
    }

    #endregion

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
            get
            {
                var obj = TSB.GetCurrent().Value();
                bool needSave = false;
                if (obj.MaxCredit <= decimal.Zero)
                {
                    obj.MaxCredit = 200000;
                    needSave = true;
                }
                if (obj.LowLimitST25 <= decimal.Zero)
                {
                    obj.LowLimitST25 = 100;
                    needSave = true;
                }
                if (obj.LowLimitST50 <= decimal.Zero)
                {
                    obj.LowLimitST50 = 100;
                    needSave = true;
                }
                if (obj.LowLimitBHT1 <= decimal.Zero)
                {
                    obj.LowLimitBHT1 = 1000;
                    needSave = true;
                }
                if (obj.LowLimitBHT2 <= decimal.Zero)
                {
                    obj.LowLimitBHT2 = 1000;
                    needSave = true;
                }
                if (obj.LowLimitBHT5 <= decimal.Zero)
                {
                    obj.LowLimitBHT5 = 1000;
                    needSave = true;
                }
                if (obj.LowLimitBHT10 <= decimal.Zero)
                {
                    obj.LowLimitBHT10 = 2000;
                    needSave = true;
                }
                if (obj.LowLimitBHT20 <= decimal.Zero)
                {
                    obj.LowLimitBHT20 = 2000;
                    needSave = true;
                }
                if (obj.LowLimitBHT50 <= decimal.Zero)
                {
                    obj.LowLimitBHT50 = 2000;
                    needSave = true;
                }
                if (obj.LowLimitBHT100 <= decimal.Zero)
                {
                    obj.LowLimitBHT100 = 2000;
                    needSave = true;
                }
                if (obj.LowLimitBHT500 <= decimal.Zero)
                {
                    obj.LowLimitBHT500 = 2000;
                    needSave = true;
                }
                if (obj.LowLimitBHT1000 <= decimal.Zero)
                {
                    obj.LowLimitBHT1000 = 2000;
                    needSave = true;
                }
                if (needSave) TSB.SaveTSB(obj);

                return obj;
            }
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

        #region TOD

        /// <summary>
        /// Gets TOD PlazaGroups.
        /// </summary>
        public static List<PlazaGroup> TODPlazaGroups
        {
            get { return GetTODPlazaGroups(); }
        }
        /// <summary>
        /// Gets TOD Plazas.
        /// </summary>
        public static List<Plaza> TODPlazas
        {
            get { return GetTODPlazas(); }
        }
        /// <summary>
        /// Gets TOD Lanes.
        /// </summary>
        public static List<Lane> TODLanes
        {
            get { return GetTODLanes(); }

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

        #region User

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

        #region TOD PlazaGroup/Plaza methods

        /// <summary>
        /// Get TOD's PlazaGroups.
        /// </summary>
        /// <returns>Returns list of PlazaGroup.</returns>
        public static List<PlazaGroup> GetTODPlazaGroups()
        {
            List<PlazaGroup> results = new List<PlazaGroup>();

            var cfg = TODConfigManager.Instance.Value;
            var plazas = (null != cfg && null != cfg.Plazas) ? cfg.Plazas : null;
            if (null != plazas && plazas.Count > 0)
            {
                plazas.ForEach(plaza =>
                {
                    if (null == plaza && plaza.PlazaId <= 0) return;
                    var match = Plaza.GetPlazaBySCWPlazaId(plaza.PlazaId).Value();
                    if (null != match && match.PlazaGroupId != string.Empty)
                    {
                        var exist = results.Find(plazagroup => 
                        { 
                            return plazagroup.PlazaGroupId == match.PlazaGroupId; 
                        });
                        if (null != exist) return; // already exist.

                        var group = PlazaGroup.GetPlazaGroup(match.PlazaGroupId).Value();
                        if (null != group) results.Add(group);
                    }
                });
            }

            return results;
        }
        /// <summary>
        /// Get TOD's Plazas
        /// </summary>
        /// <returns>Returns list of Plaza.</returns>
        public static List<Plaza> GetTODPlazas()
        {
            List<Plaza> results = new List<Plaza>();

            var cfg = TODConfigManager.Instance.Value;
            var plazas = (null != cfg && null != cfg.Plazas) ? cfg.Plazas : null;
            if (null != plazas && plazas.Count > 0)
            {
                plazas.ForEach(plaza =>
                {
                    if (null == plaza && plaza.PlazaId <= 0) return;
                    var match = Plaza.GetPlazaBySCWPlazaId(plaza.PlazaId).Value();
                    // Check match plaza group.
                    if (null != match)
                    {
                        results.Add(match);
                    }
                });
            }

            return results;
        }
        /// <summary>
        /// Get TOD's Lanes
        /// </summary>
        /// <returns>Returns list of Lane.</returns>
        public static List<Lane> GetTODLanes()
        {
            List<Lane> results = new List<Lane>();

            var cfg = TODConfigManager.Instance.Value;
            var plazas = (null != cfg && null != cfg.Plazas) ? cfg.Plazas : null;
            if (null != plazas && plazas.Count > 0)
            {
                plazas.ForEach(plaza =>
                {
                    if (null == plaza && plaza.PlazaId <= 0) return;
                    var match = Plaza.GetPlazaBySCWPlazaId(plaza.PlazaId).Value();
                    if (null == match) return;
                    var lanes = Lane.GetPlazaLanes(match).Value();
                    if (null != lanes && lanes.Count > 0)
                    {
                        lanes.ForEach(lane => 
                        {
                            var exist = results.Find(eachLan => 
                            {
                                return eachLan.LaneId == lane.LaneId;
                            });
                            if (null != exist) return; // Already exists.
                            results.Add(lane);
                        });
                    }
                });
            }

            return results;
        }
        /// <summary>
        /// Get TSB PlazaGroup's Plazas
        /// </summary>
        /// <param name="value">The PlazaGroup instance.</param>
        /// <returns>Returns list of Plaza.</returns>
        public static List<Plaza> GetTSBPlazaGroupPlazas(PlazaGroup value)
        {
            List<Plaza> results;
            if (null == value)
            {
                results = new List<Plaza>();
            }
            else
            {
                results = Plaza.GetPlazaGroupPlazas(value).Value();
            }
            return results;
        }
        /// <summary>
        /// Get TOD PlazaGroup's Plazas.
        /// </summary>
        /// <param name="value">The PlazaGroup instance.</param>
        /// <returns>Returns list of Plaza.</returns>
        public static List<Plaza> GetTODPlazaGroupPlazas(PlazaGroup value)
        {
            List<Plaza> results = new List<Plaza>();
            if (null == value) return results;

            var cfg = TODConfigManager.Instance.Value;
            var plazas = (null != cfg && null != cfg.Plazas) ? cfg.Plazas : null;
            if (null != plazas && plazas.Count > 0)
            {
                plazas.ForEach(plaza => 
                {
                    if (null == plaza && plaza.PlazaId <= 0) return;
                    var match = Plaza.GetPlazaBySCWPlazaId(plaza.PlazaId).Value();
                    // Check match plaza group.
                    if (null != match && match.PlazaGroupId == value.PlazaGroupId)
                    {
                        results.Add(match);
                    }
                });
            }

            return results;
        }

        #endregion

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
    /// The CurrentTSBManager Class.
    /// </summary>
    public class CurrentTSBManager : INotifyPropertyChanged
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
            TODConfigManager.Instance.ConfigChanged += Instance_ConfigChanged;

            this.Jobs = new JobManager(this);
            this.Payments = new PaymentManager(this);
            this.UserShifts = new UserShiftManager(this);
            Refresh();
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~CurrentTSBManager() 
        {
            TODConfigManager.Instance.ConfigChanged -= Instance_ConfigChanged;
            this.UserShifts = null;
            this.Payments = null;
            this.Jobs = null;
        }

        #endregion

        #region Private Methods

        private void Instance_ConfigChanged(object sender, EventArgs e)
        {
            Refresh();
        }
        /// <summary>
        /// Raise Property Changed Event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void RaiseChanged(string propertyName)
        {
            PropertyChanged.Call(this, new PropertyChangedEventArgs(propertyName));
        }

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
            TODPlazaGroups = null;
            TODPlazas = null;
            TODLanes = null;

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

                TODPlazaGroups = TODAPI.TODPlazaGroups;
                TODPlazas = TODAPI.TODPlazas;
                TODLanes = TODAPI.TODLanes;
                // Get Current TSB Shift
                TSBShift = TODAPI.TSBShift;
                // Gets Chief
                Chief = TSBShift.Chief();
            }
            // Init Shifts
            Shifts = TODAPI.Shifts;

            if (null != UserShifts) UserShifts.Refresh();
            if (null != Jobs) Jobs.Refresh();
            if (null != Payments) Payments.Refresh();
        }

        #endregion

        #region Public Properties

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

        #region TSB/Plaza/PlazaGroups

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

        #endregion

        #region TOD

        /// <summary>
        /// Gets TOD Plaza Groups.
        /// </summary>
        public List<PlazaGroup> TODPlazaGroups { get; private set; }
        /// <summary>
        /// Gets TOD Plazas.
        /// </summary>
        public List<Plaza> TODPlazas { get; private set; }
        /// <summary>
        /// Gets TOD Lanes.
        /// </summary>
        public List<Lane> TODLanes { get; private set; }

        #endregion

        #region Shift/TSBShift

        /// <summary>
        /// Gets Shifts.
        /// </summary>
        public List<Models.Shift> Shifts { get; private set; }
        /// <summary>
        /// Gets Current TSB Shift.
        /// </summary>
        public TSBShift TSBShift { get; private set; }
        /// <summary>
        /// Gets Current Shift (Required to used later for create user shift).
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

        #endregion

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

                RaiseChanged("CollectorId");
                RaiseChanged("CollectorNameEN");
                RaiseChanged("CollectorNameTH");
            }
        }
        /// <summary>
        /// Gets Collector Id.
        /// </summary>
        public string CollectorId
        {
            get { return (null != User) ? User.UserId : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Collector Name EN.
        /// </summary>
        public string CollectorNameEN
        {
            get { return (null != User) ? User.FullNameEN : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Collector Name TH.
        /// </summary>
        public string CollectorNameTH
        {
            get { return (null != User) ? User.FullNameTH : string.Empty; }
            set { }
        }

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
            UpdateUserShift();
            // Raise Event.
            UserChanged.Call(sender, e);
            UserShiftChanged.Call(this, EventArgs.Empty);
        }

        private void Current_ShiftChanged(object sender, EventArgs e)
        {
            UpdateUserShift();
            // Raise Event.
            ShiftChanged.Call(sender, e);
        }

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {
            UpdateUserShift();
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
        /// <summary>
        /// Update User Shift. force update.
        /// </summary>
        public void UpdateUserShift()
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
            UserShiftChanged.Call(this, EventArgs.Empty);
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
    public class JobManager : INotifyPropertyChanged
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private JobManager() : base() 
        {
            ViewMode = ViewModes.TOD;
        }
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

            RaiseChanged("CollectorId");
            RaiseChanged("CollectorNameEN");
            RaiseChanged("CollectorNameTH");
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

        #region Job Methods

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
            var tsbPlazas =  (ViewMode == ViewModes.TSB) ? Current.TSBPlazas : Current.TODPlazas;
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
                                    job.jobNo.HasValue &&
                                    job.laneId.HasValue &&
                                    job.bojDateTime.HasValue &&
                                    usrShift.Begin.Value <= job.bojDateTime.Value)
                                {
                                    // Check duplicate.
                                    var exist = jobs.Find(item =>
                                    {
                                        var found = (item.JobNo.Value == job.jobNo.Value &&
                                            item.LaneNo.Value == job.laneId.Value &&
                                            item.Begin.Value == job.bojDateTime.Value);
                                        return found;
                                    });
                                    if (null == exist)
                                    {
                                        jobs.Add(new LaneJob(job, usrShift));
                                    }
                                    else
                                    {
                                        //Console.WriteLine("Detected duplicate job.");
                                    }
                                }
                            }
                            else
                            {
                                // All Job match plaza id.
                                if (job.plazaId.Value == plaza.SCWPlazaId &&
                                    job.jobNo.HasValue &&
                                    job.laneId.HasValue &&
                                    job.bojDateTime.HasValue)
                                {
                                    // Check duplicate.
                                    var exist = jobs.Find(item =>
                                    {
                                        var found = (item.JobNo.Value == job.jobNo.Value &&
                                            item.LaneNo.Value == job.laneId.Value &&
                                            item.Begin.Value == job.bojDateTime.Value);
                                        return found;
                                    });
                                    if (null == exist)
                                    {
                                        jobs.Add(new LaneJob(job, usrShift));
                                    }
                                    else
                                    {
                                        //Console.WriteLine("Detected duplicate job.");
                                    }
                                }
                            }
                        });
                    }
                });

                // sort by BOJ DateTime and assigned to jobs list.
                AllJobs.AddRange(jobs.OrderBy(x => x.Begin).ToArray());

                if (OnlyJobInShift)
                {
                    LoadPlazaGroupJobs();
                }
                else
                {
                    //PlazaGroupJobs.AddRange(jobs.OrderBy(x => x.Begin).ToArray());
                    LoadPlazaGroupJobs();
                }
            }

            SCWOnline = isOnline; // Update public property.
        }

        private void LoadPlazaGroupJobs()
        {
            if (null == PlazaGroupJobs) PlazaGroupJobs = new List<LaneJob>();
            PlazaGroupJobs.Clear();

            if (null == PlazaGroup) return;

            //var plazagroupPlazas = Plaza.GetPlazaGroupPlazas(PlazaGroup).Value();
            var plazagroupPlazas = (ViewMode == ViewModes.TSB) ? 
                TODAPI.GetTSBPlazaGroupPlazas(PlazaGroup) : TODAPI.GetTODPlazaGroupPlazas(PlazaGroup);

            if (null == plazagroupPlazas || null == AllJobs || AllJobs.Count <= 0)
                return;

            if (null != plazagroupPlazas)
            {
                plazagroupPlazas.ForEach(plaza =>
                {
                    AllJobs.ForEach(job =>
                    {
                        // Match Selected Plaza Group Id and all required data is not null.
                        if (job.PlazaGroupId == plaza.PlazaGroupId &&
                            job.JobNo.HasValue && job.LaneNo.HasValue && job.Begin.HasValue)
                        {
                            // Check Duplicate
                            var exist = PlazaGroupJobs.Find(item =>
                            {
                                var found = (item.JobNo.Value == job.JobNo.Value &&
                                    item.LaneNo.Value == job.LaneNo.Value &&
                                    item.Begin.Value == job.Begin.Value);
                                return found;
                            });

                            if (null == exist)
                            {
                                PlazaGroupJobs.Add(job);
                            }
                            else
                            {
                                //Console.WriteLine("Detected duplicate job.");
                            }
                        }
                    });
                });
            }
        }

        #endregion

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

        #endregion

        #region Public Properties

        #region Manager

        /// <summary>
        /// Gets Current TSB Manager.
        /// </summary>
        public CurrentTSBManager Current { get; private set; }

        #endregion

        #region UserShift and PlazaGroup

        /// <summary>
        /// Gets or sets UserShift (used for AllJobs and PlazaGroupJobs).
        /// </summary>
        public UserShift UserShift { get; set; }
        /// <summary>
        /// Gets or sets PlazaGroup (used for PlazaGroupJobs).
        /// </summary>
        public PlazaGroup PlazaGroup { get; set; }

        #endregion

        #region User

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
            get { return (null != Current) ? Current.CollectorId : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Collector Name EN.
        /// </summary>
        public string CollectorNameEN
        {
            get { return (null != Current) ? Current.CollectorNameEN : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Collector Name TH.
        /// </summary>
        public string CollectorNameTH
        {
            get { return (null != Current) ? Current.CollectorNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region Search Condition and Result

        /// <summary>
        /// Gets or set View Mode.
        /// </summary>
        public ViewModes ViewMode { get; set; }
        /// <summary>
        /// Gets All Jobs for specificed user on current shift.
        /// </summary>
        public List<LaneJob> AllJobs { get; private set; }
        /// <summary>
        /// Gets Current Jobs for specificed user on current shift and plaza group.
        /// </summary>
        public List<LaneJob> PlazaGroupJobs { get; private set; }
        /// <summary>
        /// Gets or sets show only job between User Shift Begin to End DateTime.
        /// </summary>
        public bool OnlyJobInShift { get; set; }
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
        /// <summary>
        /// Checks is SCW server is online.
        /// </summary>
        public bool SCWOnline { get; set; }

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

    #region PaymentManager

    /// <summary>
    /// The PaymentManager class.
    /// </summary>
    public class PaymentManager : INotifyPropertyChanged
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private PaymentManager() : base() 
        {
            EnableLaneFilter = false;
            ViewMode = ViewModes.TOD;
        }
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

            RaiseChanged("CollectorId");
            RaiseChanged("CollectorNameEN");
            RaiseChanged("CollectorNameTH");
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

        #region Payment Methods

        private int? GetLaneFilter()
        {
            int? ret = new int?();
            if (string.IsNullOrEmpty(Filter)) return ret;
            int num;
            if (int.TryParse(Filter.Trim(), out num))
            {
                ret = new int?(num);
            }
            return ret;
        }

        private void LoadEMVItems()
        {
            if (null == EMVItems) EMVItems = new List<LaneEMV>();
            EMVItems.Clear();

            List<LaneEMV> results = new List<LaneEMV>();
            List<LaneEMV> items = new List<LaneEMV>();
            List<LaneEMV> sortList = new List<LaneEMV>();

            if (null != User && null != Current && null !=
                Current.TSB)
            {

                List<Plaza> plazas;
                if (null == PlazaGroup)
                {
                    plazas = (ViewMode == ViewModes.TSB) ?
                        Current.TSBPlazas : Current.TODPlazas;
                }
                else
                {
                    plazas = (ViewMode == ViewModes.TSB) ?
                        TODAPI.GetTSBPlazaGroupPlazas(PlazaGroup) : TODAPI.GetTODPlazaGroupPlazas(PlazaGroup);
                }

                int networkId = TODAPI.NetworkId;
                var userShift = Current.UserShifts.Shift;

                if (Begin.HasValue && End.HasValue && null != plazas && plazas.Count > 0)
                {
                    plazas.ForEach(plaza =>
                    {
                        int pzId = plaza.SCWPlazaId;
                        SCWEMVTransactionList param = new SCWEMVTransactionList();
                        param.networkId = networkId;
                        param.plazaId = pzId;
                        param.staffId = User.UserId;
                        if (Begin.HasValue)
                        {
                            var dt = Begin.Value;
                            param.startDateTime = new DateTime?(new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, DateTimeKind.Local));
                        }
                        else
                        {
                            param.startDateTime = new DateTime?();
                        }
                        if (End.HasValue)
                        {
                            var dt = End.Value;
                            param.endDateTime = new DateTime?(new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, DateTimeKind.Local));
                        }
                        else
                        {
                            param.endDateTime = new DateTime?();
                        }
                        var emvList = scwOps.emvTransactionList(param);
                        if (null != emvList && null != emvList.list)
                        {
                            emvList.list.ForEach(item =>
                            {
                                if (item.trxDateTime.HasValue &&
                                    Begin.Value <= item.trxDateTime.Value)
                                {
                                    items.Add(new LaneEMV(item));
                                }
                            });
                        }
                    });

                    sortList = items.OrderBy(o => o.TrxDateTime).Distinct().ToList();
                }

                if (EnableLaneFilter)
                {
                    // Filter By Lane
                    var filter = GetLaneFilter();
                    if (filter.HasValue)
                    {
                        // Filter only specificed lane no.
                        results = sortList.Where(o => o.LaneNo == filter.Value).ToList();
                    }
                    else
                    {
                        results.AddRange(sortList.ToArray());
                    }
                }
                else
                {
                    results.AddRange(sortList.ToArray());
                }
            }

            EMVItems = results;
        }

        private void LoadQRcodeItems()
        {
            if (null == QRCodeItems) QRCodeItems = new List<LaneQRCode>();
            QRCodeItems.Clear();

            List<LaneQRCode> results = new List<LaneQRCode>();
            List<LaneQRCode> items = new List<LaneQRCode>();
            List<LaneQRCode> sortList = new List<LaneQRCode>();

            if (null != User && null != Current && 
                null != Current.TSB)
            {
                List<Plaza> plazas;
                if (null == PlazaGroup)
                {
                    plazas = (ViewMode == ViewModes.TSB) ?
                        Current.TSBPlazas : Current.TODPlazas;
                }
                else
                {
                    plazas = (ViewMode == ViewModes.TSB) ?
                        TODAPI.GetTSBPlazaGroupPlazas(PlazaGroup) : TODAPI.GetTODPlazaGroupPlazas(PlazaGroup);
                }

                int networkId = TODAPI.NetworkId;

                if (null != User && Begin.HasValue && End.HasValue && null != plazas && plazas.Count > 0)
                {
                    plazas.ForEach(plaza =>
                    {
                        int pzId = plaza.SCWPlazaId;
                        SCWQRCodeTransactionList param = new SCWQRCodeTransactionList();
                        param.networkId = networkId;
                        param.plazaId = pzId;
                        param.staffId = User.UserId;
                        if (Begin.HasValue)
                        {
                            var dt = Begin.Value;
                            param.startDateTime = new DateTime?(new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, DateTimeKind.Local));
                        }
                        else
                        {
                            param.startDateTime = new DateTime?();
                        }
                        if (End.HasValue)
                        {
                            var dt = End.Value;
                            param.endDateTime = new DateTime?(new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, DateTimeKind.Local));
                        }
                        else
                        {
                            param.endDateTime = new DateTime?();
                        }
                        var qrcodeList = scwOps.qrcodeTransactionList(param);
                        if (null != qrcodeList && null != qrcodeList.list)
                        {
                            qrcodeList.list.ForEach(item =>
                            {
                                if (item.trxDateTime.HasValue &&
                                    Begin.Value <= item.trxDateTime.Value)
                                {
                                    items.Add(new LaneQRCode(item));
                                }
                            });
                        }
                    });

                    sortList = items.OrderBy(o => o.TrxDateTime).Distinct().ToList();
                }

                if (EnableLaneFilter)
                {
                    // Filter By Lane
                    var filter = GetLaneFilter();
                    if (filter.HasValue)
                    {
                        // Filter only specificed lane no.
                        results = sortList.Where(o => o.LaneNo == filter.Value).ToList();
                    }
                    else
                    {
                        results.AddRange(sortList.ToArray());
                    }
                }
                else
                {
                    results.AddRange(sortList.ToArray());
                }
            }

            QRCodeItems = results;
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Refresh all payments.
        /// </summary>
        public void Refresh()
        {
            if (null == EMVItems) EMVItems = new List<LaneEMV>();
            EMVItems.Clear();
            if (null == QRCodeItems) QRCodeItems = new List<LaneQRCode>();
            QRCodeItems.Clear();

            if (PaymentType == PaymentTypes.EMV)
            {
                // EMV
                LoadEMVItems();
            }
            else if (PaymentType == PaymentTypes.QRCode)
            {
                // QRCODE
                LoadQRcodeItems();
            }
            else
            {
                // BOTH
                LoadEMVItems();
                LoadQRcodeItems();
            }
        }

        #endregion

        #region Public Properties

        #region Managers

        /// <summary>
        /// Gets Current TSB Manager.
        /// </summary>
        public CurrentTSBManager Current { get; private set; }
        
        #endregion

        #region User

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
            get { return (null != Current) ? Current.CollectorId : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Collector Name EN.
        /// </summary>
        public string CollectorNameEN
        {
            get { return (null != Current) ? Current.CollectorNameEN : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Collector Name TH.
        /// </summary>
        public string CollectorNameTH
        {
            get { return (null != Current ) ? Current.CollectorNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region Search Condition

        /// <summary>
        /// Gets or sets Payment type.
        /// </summary>
        public PaymentTypes PaymentType { get; set; }
        /// <summary>
        /// Gets or set View Mode.
        /// </summary>
        public ViewModes ViewMode { get; set; }
        /// <summary>
        /// Gets or sets Begin DateTime.
        /// </summary>
        public DateTime? Begin { get; set; }
        /// <summary>
        /// Gets or sets End DateTime.
        /// </summary>
        public DateTime? End { get; set; }
        /// <summary>
        /// Gets or sets has lane filter.
        /// </summary>
        public bool EnableLaneFilter { get; set; }
        /// <summary>
        /// Gets or sets filter.
        /// </summary>
        public string Filter { get; set; }
        /// <summary>
        /// Gets or sets plaza gorup filter.
        /// </summary>
        public PlazaGroup PlazaGroup { get; set; }

        #endregion

        #region Payment Items

        /// <summary>
        /// Gets EMV List.
        /// </summary>
        public List<LaneEMV> EMVItems { get; private set; }
        /// <summary>
        /// Gets QRCode List.
        /// </summary>
        public List<LaneQRCode> QRCodeItems { get; private set; }

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
                if (null != Current.UserShifts)
                {
                    Current.UserShifts.UserShiftChanged += UserShifts_UserShiftChanged;
                }
            }

            Refresh();
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~RevenueEntryManager()
        {
            if (null != Current)
            {
                if (null != Current.UserShifts)
                {
                    Current.UserShifts.UserShiftChanged -= UserShifts_UserShiftChanged;
                }

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

            RaiseChanged("CollectorId");
            RaiseChanged("CollectorNameEN");
            RaiseChanged("CollectorNameTH");
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
            CheckRevenueDate();
            
            RaiseChanged("ShiftId");
            RaiseChanged("ShiftNameEN");
            RaiseChanged("ShiftNameTH");
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

        #region User Credit Methods

        private UserCreditBalance CheckUserCredit(bool isNew)
        {
            UserCreditBalance usrCredit;
            if (!ByChief)
            {
                var search = Models.Search.Credit.User.Completed.Create(User, PlazaGroup);
                usrCredit = taaOps.Credit.User.Completed(search).Value();

                //TODO: UserCredit offline need some model and logic.
                /*
                var ret = taaOps.Credit.User.Completed(search);
                usrCredit = (null != ret && ret.Ok) ? ret.Value() : null;
                */
            }
            else
            {
                // By chief create empty balance - update from Revenue Entry and save.
                usrCredit = new UserCreditBalance();
                usrCredit.State = UserCreditBalance.StateTypes.Completed; // set completed state.

                usrCredit.TSBId = (null != UserShift) ? UserShift.TSBId : string.Empty;
                usrCredit.TSBNameEN = (null != UserShift) ? UserShift.TSBNameEN : string.Empty;
                usrCredit.TSBNameTH = (null != UserShift) ? UserShift.TSBNameTH : string.Empty;
                usrCredit.UserId = (null != UserShift) ? UserShift.UserId : string.Empty;
                usrCredit.FullNameEN = (null != UserShift) ? UserShift.FullNameEN : string.Empty;
                usrCredit.FullNameTH = (null != UserShift) ? UserShift.FullNameTH : string.Empty;

                if (!isNew)
                {
                    usrCredit.BagNo = (null != Entry) ? Entry.BagNo : string.Empty;
                    usrCredit.BeltNo = (null != Entry) ? Entry.BeltNo : string.Empty;
                    usrCredit.RevenueId = (null != Entry) ? Entry.RevenueId : string.Empty;
                }
                else
                {
                    usrCredit.BagNo = string.Empty;
                    usrCredit.BeltNo = string.Empty;
                    usrCredit.RevenueId = string.Empty;
                }
            }
            return usrCredit;
        }

        #endregion

        #region Coupon Sold Methods

        private UserCouponSoldSummary CheckSoldCoupon()
        {
            UserCouponSoldSummary usrCouponSold = null;
            if (!ByChief)
            {
                var dt1 = UserShift.Begin.Value;
                var dt2 = (UserShift.End.HasValue) ? UserShift.End.Value : DateTime.Now;
                var search = Search.Coupon.User.Sold.Create(PlazaGroup, User, dt1, dt2);
                var ret = taaOps.Coupon.User.Sold(search);
                if (null != ret && ret.Ok)
                {
                    usrCouponSold = ret.Value();
                }
            }
            return usrCouponSold;
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

        #endregion

        #region Public Methods

        #region Refresh

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
        }

        #endregion

        #region RevenueShift method(s)

        /// <summary>
        /// Check Revenue Shift. Call before create NewRevenueEntry to check UserShiftRevenue.
        /// </summary>
        public void CheckRevenueShift()
        {
            #region Check User Revenue Shift

            MethodBase med = MethodBase.GetCurrentMethod();

            IsNewRevenueShift = false;

            UserShiftRevenue refShf;
            if (!ByChief)
            {
                // Gets User Shift from Self.
                refShf = UserShiftRevenue.GetPlazaRevenue(UserShift, PlazaGroup).Value();
                if (null == refShf)
                {
                    string msg = "User Revenue Shift not found. Create New!!.";
                    med.Info(msg);

                    // Create new if not found.
                    refShf = UserShiftRevenue.CreatePlazaRevenue(UserShift, PlazaGroup).Value();
                    this.IsNewRevenueShift = true;
                }
                else
                {
                    string msg = "User Revenue Shift found.";
                    med.Info(msg);
                }
            }
            else
            {
                string msg = "User Shift is New so User Revenue Shift not found. Create New!!.";
                med.Info(msg);

                // Gets User Shift from Job Manager (In this case no UserShift so UserRevenueShift is new one).
                refShf = UserShiftRevenue.CreatePlazaRevenue(Jobs.UserShift, PlazaGroup).Value();
                this.IsNewRevenueShift = true;
            }

            RevenueShift = refShf; // Assign to public property.

            #endregion
        }

        #endregion

        #region Revenue Entry methods.

        /// <summary>
        /// Create New Revenue Entry.
        /// </summary>
        public bool NewRevenueEntry()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            #region Check Null objects

            if (null == Current || null == UserShifts || null == Jobs || null == Payments)
            {
                string msg = "Cannot create RevenueEntry Some of Revenue Managers is null.";
                med.Info(msg);
                return false;
            }

            if (!ByChief && null == this.UserShift)
            {
                string msg = "User Shift (Revenue Entry Manager) Not found.";
                med.Info(msg);
                return false;
            }

            if (ByChief && null == Jobs.UserShift)
            {
                string msg = "User Shift (Job Manager) Not found.";
                med.Info(msg);
                return false;
            }

            #endregion

            #region Check User Credit Balance

            var usrCredit = CheckUserCredit(true);

            #endregion

            #region Revenue Entry

            Entry = new RevenueEntry();

            if (!ByChief)
            {
                // Check User Coupon Sold Balance
                var usrSold = CheckSoldCoupon();
                if (null != usrSold)
                {
                    //Entry.CouponSoldEnable = false;
                    Entry.CouponSoldEnable = true;
                    Entry.CouponSoldBHT35 = usrSold.CouponBHT35;
                    Entry.CouponSoldBHT80 = usrSold.CouponBHT80;
                    Entry.CouponSoldBHT35Total = usrSold.CouponBHT35Total;
                    Entry.CouponSoldBHT80Total = usrSold.CouponBHT80Total;
                }
                else
                {
                    Entry.CouponSoldEnable = true;
                }
            }
            else
            {
                Entry.CouponSoldEnable = true;
            }

            bool success = UpdateRevenueEntry();

            if (null != usrCredit)
            {
                string msg = string.Format("User Credit found. BagNo: {0}, BeltNo: {1}",
                    usrCredit.BagNo, usrCredit.BeltNo);
                med.Info(msg);

                Entry.BagNo = usrCredit.BagNo;
                Entry.BeltNo = usrCredit.BeltNo;
            }
            else
            {
                string msg = "User Credit not found.";
                med.Info(msg);

                Entry.BagNo = string.Empty;
                Entry.BeltNo = string.Empty;
            }

            #endregion

            return success;
        }

        private bool UpdateRevenueEntry()
        {
            if (null == Entry || null == PlazaGroup) return false;
            if (!ByChief)
            {
                if (null == UserShift) return false;
            }
            else
            {
                if (null == Jobs || null == Jobs.UserShift) return false;
            }

            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("UpdateRevenueEntry information");

            // Check is historical
            Entry.IsHistorical = ByChief;
            // assigned plaza group.
            Entry.PlazaGroupId = PlazaGroup.PlazaGroupId;
            // update object properties.
            PlazaGroup.AssignTo(Entry); // assigned plaza group name (EN/TH)

            var usrshf = (!ByChief) ? UserShift : Jobs.UserShift;

            usrshf.AssignTo(Entry); // assigned user shift

            // assigned date after sync object(s) to RevenueEntry.

            // assigned Entry date.
            if (EntryDate.HasValue)
            {
                var dt = EntryDate.Value;
                Entry.EntryDate = new DateTime?(new DateTime(
                    dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, DateTimeKind.Local));
            }
            else
            {
                Entry.EntryDate = new DateTime?();
            }

            var dtNow = DateTime.Now;
            if (!ByChief)
            {
                Entry.RevenueDate = new DateTime?(new DateTime(
                    RevenueDate.Value.Year, RevenueDate.Value.Month, RevenueDate.Value.Day,
                    dtNow.Hour, dtNow.Minute, dtNow.Second, dtNow.Millisecond, DateTimeKind.Local));
            }
            else
            {
                if (!Entry.RevenueDate.HasValue)
                {
                    Entry.RevenueDate = new DateTime?(new DateTime(
                        RevenueDate.Value.Year, RevenueDate.Value.Month, RevenueDate.Value.Day,
                        dtNow.Hour, dtNow.Minute, dtNow.Second, dtNow.Millisecond, DateTimeKind.Local));
                }
                med.Info("Revenue Date - By Chief : {0:dd/MM/yyyy}", Entry.RevenueDate);
            }

            // Generate lane string.
            Entry.Lanes = Jobs.GetLaneString(ByChief);

            // Find begin/end of revenue.
            DateTime begin = usrshf.Begin.Value; // Begin time used start of shift.
            DateTime end = DateTime.Now; // End time used printed date

            if (!Entry.ShiftBegin.HasValue || Entry.ShiftBegin.Value == DateTime.MinValue)
            {
                Entry.ShiftBegin = begin;
            }
            if (!Entry.ShiftEnd.HasValue || Entry.ShiftEnd == DateTime.MinValue)
            {
                Entry.ShiftEnd = end;
            }

            // Update Colllector data.
            if (null != User)
            {
                Entry.CollectorNameEN = User.FullNameEN;
                Entry.CollectorNameTH = User.FullNameTH;
            }
            // Update Chief data.
            if (!ByChief)
            {
                // By Normal operation.
                if (null != Chief)
                {
                    Entry.SupervisorId = Chief.UserId;
                    Entry.SupervisorNameEN = Chief.FullNameEN;
                    Entry.SupervisorNameTH = Chief.FullNameTH;
                }
            }
            else
            {
                // By Chief.
                if (null != TODApp.Chief.Current)
                {
                    Entry.SupervisorId = TODApp.Chief.Current.UserId;
                    Entry.SupervisorNameEN = TODApp.Chief.Current.FullNameEN;
                    Entry.SupervisorNameTH = TODApp.Chief.Current.FullNameTH;
                }
            }

            return true;
        }
        /// <summary>
        /// Save Revenue Entry.
        /// </summary>
        /// <returns></returns>
        public bool SaveRevenueEntry()
        {
            if (null == Entry ||
                !Entry.RevenueDate.HasValue ||
                Entry.RevenueDate.Value == DateTime.MinValue ||
                !Entry.EntryDate.HasValue ||
                Entry.EntryDate.Value == DateTime.MinValue)
            {
                return false;
            }

            if (null == PlazaGroup || null == UserShift) return false;

            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("SaveRevenueEntry information");

            if (Entry.RevenueId == string.Empty)
            {
                // Set Unique ID.
                var unique = UniqueCode.GetUniqueId("RevenueEntry").Value();
                if (string.IsNullOrWhiteSpace(Entry.RevenueId))
                {
                    string yr = DateTime.Now.ToThaiDateTimeString("yy");
                    string autoId = (null != unique) ? yr + unique.LastNumber.ToString("D5") : string.Empty; // auto generate.
                    Entry.RevenueId = autoId;
                    UniqueCode.IncreaseUniqueId("RevenueEntry");
                }
            }
            med.Info("RevenueId : {0}", Entry.RevenueId);

            // Reload usrCredit for update Revenue Id.
            var usrCredit = CheckUserCredit(false);
            if (null != usrCredit)
            {
                usrCredit.RevenueId = Entry.RevenueId;
                taaOps.Credit.User.Save(usrCredit);
            }

            // Save Revenue Entry.
            var revInst = RevenueEntry.Save(Entry).Value();
            string revId = (null != revInst) ? revInst.RevenueId : string.Empty;
            if (null != RevenueShift)
            {
                // save revenue shift (for plaza)
                UserShiftRevenue.SavePlazaRevenue(RevenueShift, Entry.RevenueDate.Value, revId);
            }

            List<LaneJob> allJobs;
            List<LaneJob> currJobs;
            bool bCloseUserShift;

            if (!ByChief)
            {
                // Collector
                allJobs = (null != Jobs) ? Jobs.AllJobs : null;
                currJobs = (null != Jobs) ? Jobs.PlazaGroupJobs : null;
                // get all lanes information.
                bCloseUserShift = (
                    (null == allJobs && null == currJobs) ||
                    (null != allJobs && null != currJobs && allJobs.Count == currJobs.Count));
            }
            else
            {
                // Chief
                allJobs = (null != Jobs) ? Jobs.AllJobs : null;

                currJobs = new List<LaneJob>();
                if (null != Jobs && null != Jobs.PlazaGroupJobs)
                {
                    Jobs.PlazaGroupJobs.ForEach(job => 
                    {
                        if (!job.Selected) return;
                        currJobs.Add(job);
                    });
                }
                // get all lanes information.
                bCloseUserShift = (
                    (null == allJobs && null == currJobs) ||
                    (null != allJobs && null != currJobs && allJobs.Count == currJobs.Count));
            }


            if (bCloseUserShift)
            {
                med.Info("No more jobs. Auto close user shift.", Entry.RevenueId);
                // no lane activitie in user shift.
                var ret = UserShift.EndUserShift(UserShift).Value();

                GenerateUserShiftFile(ret);
            }
            else
            {
                med.Info("Has more jobs. user shift is not closed.", Entry.RevenueId);
            }

            // Generte Revenue (declare) File and mark sync status.
            GenerateRevnueFile();

            return !bCloseUserShift;
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

        private void GenerateRevnueFile()
        {
            if (null == Entry) return;

            MethodBase med = MethodBase.GetCurrentMethod();

            // Generate File.
            try
            {
                if (null == Entry || null == PlazaGroup) return;

                int networkId = TODConfigManager.Instance.DMT.networkId;

                // Need to sync currency and coupon master!!
                var currencies = MCurrency.GetCurrencies().Value();
                var coupons = MCoupon.GetCoupons().Value();
                var cardAllows = MCardAllow.GetCardAllows().Value();

                var emv = new List<SCWEMVTransaction>();
                if (null != Payments && null != Payments.EMVItems)
                {
                    Payments.EMVItems.ForEach(item => 
                    {
                        if (null == item.Transaction) return;
                        emv.Add(item.Transaction);
                    });
                }
                var qrCode = new List<SCWQRCodeTransaction>();                 
                if (null != Payments && null != Payments.QRCodeItems)
                {
                    Payments.QRCodeItems.ForEach(item =>
                    {
                        if (null == item.Transaction) return;
                        qrCode.Add(item.Transaction);
                    });
                }

                // find lane attendances.
                var jobs = new List<SCWJob>();
                if (null != Jobs && null != Jobs.PlazaGroupJobs)
                {
                    Jobs.PlazaGroupJobs.ForEach(job =>
                    {
                        if (null == job.Job) return;
                        if (ByChief && !job.Selected) return; // By Chief but not selected ignore it.
                        jobs.Add(job.Job);
                    });
                }

                var plazas = Plaza.GetPlazaGroupPlazas(PlazaGroup).Value();

                // TODO: REVENUE ENTRY to find plazaid may be need to loop all job and check if has only one plazaid use that value instead use the first one.
                int plazaId = (null != plazas && plazas.Count > 0) ? plazas[0].SCWPlazaId : -1;

                if (plazaId == -1)
                {
                    med.Info("declare error: Cannot search plaza id.");
                    return;
                }

                // write to TAxTOD message queue.
                TAxTODMQService.Instance.WriteQueue(Entry);
                // Send Entry to TA Message Queue
                TAMQService.Instance.WriteQueue(Entry);

                // Create declare json file.
                // send to server
                SCWDeclare declare = Entry.ToServer(networkId, currencies, coupons, cardAllows,
                    jobs, emv, qrCode, plazaId);

                med.Info("DECLARE INFORMATION");
                int jobCnt = (null != declare.jobList) ? declare.jobList.Count : 0;
                med.Info("Job List Count: {0}", jobCnt);
                int cashCnt = (null != declare.cashList) ? declare.cashList.Count : 0;
                med.Info("Cash List Count: {0}", cashCnt);
                int couponBookCnt = (null != declare.couponBookList) ? declare.couponBookList.Count : 0;
                med.Info("Coupon Book (sold) List Count: {0}", couponBookCnt);
                int couponCnt = (null != declare.couponList) ? declare.couponList.Count : 0;
                med.Info("Coupon (usage) List Count: {0}", couponCnt);
                int cardAllowCnt = (null != declare.cardAllowList) ? declare.cardAllowList.Count : 0;
                med.Info("Card Allow List Count: {0}", cardAllowCnt);
                int emvCnt = (null != declare.emvList) ? declare.emvList.Count : 0;
                med.Info("EMV List Count: {0}", emvCnt);
                int qrCodeCnt = (null != declare.qrcodeList) ? declare.qrcodeList.Count : 0;
                med.Info("QR Code List Count: {0}", qrCodeCnt);

                // send.
                SCWMQService.Instance.WriteQueue(declare);

                // Update local database status.
                Entry.Status = 1; // generated json file OK.
                Entry.LastUpdate = DateTime.Now;
                Models.RevenueEntry.Save(Entry);
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }
        }

        /// <summary>
        /// Load Exists Revenue Entry.
        /// </summary>
        /// <param name="entry"></param>
        public void LoadRevenueEntry(RevenueEntry entry)
        {
            if (null == entry) return;
            Entry = entry;
        }
        /// <summary>
        /// Checks is return Bag.
        /// </summary>
        /// <returns></returns>
        public bool IsReturnBag()
        {
            bool ret = false;

            // TODO: Need check state 1 (received bag) and 2 (completed).
            // and when check each state need difference dialog.
            var usrCredit = CheckUserCredit(true);
            
            //TODO: UserCredit offline need some model and logic.
            //if (null == usrCredit) return true; // Assume cannot connect to TA.
            
            if (null != usrCredit && usrCredit.State == UserCreditBalance.StateTypes.Completed)
            {
                ret = true;
            }

            return ret;
        }

        #endregion

        #region Report Medels

        /// <summary>
        /// Checks all information to build Revenue Slip report is loaded.
        /// </summary>
        public bool CanBuildRevenueSlipReport
        {
            get
            {
                return (null != UserShift &&
                    null != PlazaGroup &&
                    null != RevenueShift &&
                    null != Entry);
            }
        }
        /// <summary>
        /// Gets RevenueSlip ReportModel.
        /// </summary>
        /// <returns></returns>
        public RdlcReportModel GetRevenueSlipReportModel()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("   - Load Report [ RevenueSlip ] rdlc file."); // write log
            Assembly assembly = this.GetType().Assembly;
            RdlcReportModel inst = new RdlcReportModel();
            inst.Definition.EmbededReportName = "DMT.TOD.Reports.RevenueSlip.rdlc";
            inst.Definition.RdlcInstance = RdlcReportUtils.GetEmbededReport(assembly,
                inst.Definition.EmbededReportName);

            med.Info("   - Clear data source."); // write log
            // clear reprot datasource.
            inst.DataSources.Clear();

            List<RevenueEntry> items = new List<RevenueEntry>();
            if (null != Entry)
            {
                items.Add(Entry);
            }

            // assign new data source
            med.Info("   - Assign new data source in Report Model."); // write log
            RdlcReportDataSource mainDS = new RdlcReportDataSource();
            mainDS.Name = "main"; // the datasource name in the rdlc report.
            mainDS.Items = items; // setup data source
            // Add to datasources
            inst.DataSources.Add(mainDS);

            med.Info("   - Assign parameter(s) in Report Model."); // write log
            // Add parameters (if required).
            DateTime today = DateTime.Now;
            string printDate = today.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
            inst.Parameters.Add(RdlcReportParameter.Create("PrintDate", printDate));
            string histText = (null != Entry && Entry.IsHistorical) ?
                "(นำส่งย้อนหลัง)" : "";
            inst.Parameters.Add(RdlcReportParameter.Create("HistoryText", histText));

            return inst;
        }

        #endregion

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
        public JobManager Jobs { get { return (null != Current) ? Current.Jobs : null; } }
        /// <summary>
        /// Gets Payment Manager.
        /// </summary>
        public PaymentManager Payments { get { return (null != Current) ? Current.Payments : null; } }
        /// <summary>
        /// Gets User Shift Manager.
        /// </summary>
        public UserShiftManager UserShifts { get { return (null != Current) ? Current.UserShifts : null; } }

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
            get 
            { 
                return (null != UserShifts && null != UserShifts.Shift); 
            }
        }
        /// <summary>
        /// Gets Collector UserShift.
        /// </summary>
        public UserShift UserShift
        {
            get 
            { 
                return (null != UserShifts) ? UserShifts.Shift : null; 
            }
        }
        /// <summary>
        /// Gets User Shift Name EN.
        /// </summary>
        public string ShiftNameEN
        {
            get 
            {
                var ret = (HasUserShift) ? UserShifts.Shift.ShiftNameEN : string.Empty;
                //Console.WriteLine("Shift Name EN: {0}", ret);
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets User Shift Name TH.
        /// </summary>
        public string ShiftNameTH
        {
            get 
            {
                var ret = (HasUserShift) ? UserShifts.Shift.ShiftNameTH : string.Empty;
                //Console.WriteLine("Shift Name TH: {0}", ret);
                return ret; 
            }
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
            set 
            {
                if (_EntryDate != value)
                {
                    _EntryDate = value;
                    RaiseChanged("EntryDate");
                    RaiseChanged("EntryDateString");
                    RaiseChanged("EntryDateTimeString");
                }
            }
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
            get { return (null != Current) ? Current.CollectorId : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Collector Name EN.
        /// </summary>
        public string CollectorNameEN
        {
            get { return (null != Current) ? Current.CollectorNameEN : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Collector Name TH.
        /// </summary>
        public string CollectorNameTH
        {
            get { return (null != Current) ? Current.CollectorNameTH : string.Empty; }
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

        #region Revenu Entry/User Revenue Shift

        /// <summary>
        /// Gets Revenue Entry.
        /// </summary>
        public RevenueEntry Entry { get; private set; }
        /// <summary>
        /// Checks is New Revenue Entry.
        /// </summary>
        public bool IsNewRevenueEntry
        {
            get
            {
                return (null == Entry ||
                    Entry.RevenueId == string.Empty ||
                    !Entry.EntryDate.HasValue ||
                    Entry.EntryDate.Value == DateTime.MinValue ||
                    !Entry.RevenueDate.HasValue ||
                    Entry.RevenueDate.Value == DateTime.MinValue);
            }
        }

        /// <summary>
        /// Gets is new User Revenue Shift.
        /// </summary>
        public bool IsNewRevenueShift { get; private set; }
        /// <summary>
        /// Gets User Revenue Shift.
        /// </summary>
        public UserShiftRevenue RevenueShift { get; private set; }

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

    #region SupAdjManager

    /// <summary>
    /// The SupAdj Manager.
    /// </summary>
    public class SupAdjManager
    {
        #region Internal Variables

        private WebSocket ws = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SupAdjManager() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~SupAdjManager() { }

        #endregion

        #region Private Methods

        #region WS Handlers

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
        }

        private void Ws_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            // Cross thread wrapper.
            /*
            Invoke(new MethodInvoker(delegate () {
                string message = e.Data;
                Log(string.Format("WS OnMessage: {0}", message));
                txtRecv.Text = message;
            }));
            */
        }

        private void Ws_OnClose(object sender, WebSocketSharp.CloseEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            //Reconnect(e.Code, e.Reason);
            //Log(string.Format("WS OnClose : Code:{0}, Reason:{1}", e.Code, e.Reason));
        }

        private void Ws_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            //Log(string.Format("WS OnError: {0}", e.Message));
        }

        #endregion

        #endregion

        #region Public Method

        /// <summary>
        /// Checks has adjust event.
        /// </summary>
        /// <returns>Returns true if has one or more adjust events.</returns>
        public bool HasAdjustEvents()
        {
            bool ret = false;

            return ret;
        }

        #endregion
    }

    #endregion
}
