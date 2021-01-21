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
    #region TODAPI

    /// <summary>
    /// The TODAPI class.
    /// </summary>
    public static class TODAPI
    {
        /// <summary>The NetWorkId for SCW.</summary>
        public static int NetworkId = TODConfigManager.Instance.DMT.networkId;
    }

    #endregion

    #region CurrentTSBManager

    /// <summary>
    /// The CurrentTSBManager Manager Class.
    /// </summary>
    public class CurrentTSBManager
    {
        #region Internal Variables

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
            // Init TSB 
            TSB = TSB.GetCurrent().Value();
            if (null != TSB)
            {
                // Load Plaza Groups and Plaza
                TSBPlazaGroups = PlazaGroup.GetTSBPlazaGroups(TSB).Value();
                TSBPlazas = Plaza.GetTSBPlazas(TSB).Value();

                // Get Current TSB Shift and Chief
                // Gets Supervisor
                Shift = TSBShift.GetTSBShift(TSB.TSBId).Value();
                Chief = (null != Shift) ? User.GetByUserId(Shift.UserId).Value() : null;
            }
            else
            {
                // TSB Not found.
                TSBPlazaGroups = new List<PlazaGroup>();
                TSBPlazas = new List<Plaza>();

                Shift = null;
                Chief = null;
            }
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
        /// Gets or set PlazaGroup.
        /// </summary>
        public PlazaGroup PlazaGroup
        {
            get { return PlazaGroup; }
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
                    PlazaGroupPlazas = new List<Plaza>();
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
        /// Gets Current TSB Shift.
        /// </summary>
        public TSBShift Shift { get; private set; }
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
        /// The PlazaGroupChanged Event Handler.
        /// </summary>
        public event System.EventHandler PlazaGroupChanged;

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

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {
            // Raise Event.
            PlazaGroupChanged.Call(sender, e);
        }

        #endregion

        private void LoadTSBJobs()
        {
            // Create new job list.
            if (null == AllJobs) AllJobs = new List<LaneJob>();
            AllJobs.Clear();
        }

        private void LoadPlazaGroupJobs()
        {
            LoadPlazaGroupJobs();
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

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Current TSB Manager.
        /// </summary>
        public CurrentTSBManager Current { get; private set; }
        /// <summary>
        /// Gets All Jobs for specificed user on current shift.
        /// </summary>
        public List<LaneJob> AllJobs { get; private set; }
        /// <summary>
        /// Gets Current Jobs for specificed user on current shift and plaza goup.
        /// </summary>
        public List<LaneJob> CurrentJobs { get; private set; }

        #endregion

        #region Public Events

        /// <summary>
        /// The UserChanged Event Handler.
        /// </summary>
        public event System.EventHandler UserChanged;
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

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {
            // Raise Event.
            PlazaGroupChanged.Call(sender, e);
        }

        #endregion

        #endregion

        #region Public Methods

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
        private UserShiftManager() : base() { }
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
                _userShift = UserShift.GetUserShift(Current.User.UserId).Value();
            }
            // Raise Event.
            UserChanged.Call(sender, e);
            UserShiftChanged.Call(this, EventArgs.Empty);
        }

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {
            // Raise Event.
            PlazaGroupChanged.Call(sender, e);
        }

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Current TSB Manager.
        /// </summary>
        public CurrentTSBManager Current { get; private set; }
        /// <summary>
        /// Gets Current User Shift.
        /// </summary>
        public UserShift Shift { get { return _userShift; } }

        #endregion

        #region Public Events

        /// <summary>
        /// The UserChanged Event Handler.
        /// </summary>
        public event System.EventHandler UserChanged;
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
            // Init date(s).
            _EntryDate = new DateTime?(_now);
            _RevenueDate = new DateTime?(_now);

            Current = new CurrentTSBManager();
            if (null != Current)
            {
                Current.UserChanged += Current_UserChanged;
                Current.PlazaGroupChanged += Current_PlazaGroupChanged;
            }

            Jobs = new JobManager(Current);

            Payments = new PaymentManager(Current);

            UserrShifts = new UserShiftManager(Current);
            if (null != UserrShifts)
            {
                UserrShifts.UserShiftChanged += UserrShifts_UserShiftChanged;
            }

            Clear();
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~RevenueEntryManager()
        {
            if (null != UserrShifts)
            {
                UserrShifts.UserShiftChanged -= UserrShifts_UserShiftChanged;
            }

            if (null != Current)
            {
                Current.PlazaGroupChanged -= Current_PlazaGroupChanged;
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

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {
            // Raise Event.
            PlazaGroupChanged.Call(sender, e);
        }

        #endregion

        #region UserShiftManager EventHandlers

        private void UserrShifts_UserShiftChanged(object sender, EventArgs e)
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
                    var shift = UserrShifts.Shift;
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

        public void Clear()
        {
            this.EntryDate = new DateTime?(_now);
            this.RevenueDate = new DateTime?(_now);
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
        public UserShiftManager UserrShifts { get; private set; }
        /// <summary>
        /// Checks Has User Shift.
        /// </summary>
        public bool HasUserShift
        {
            get { return (null != UserrShifts && null != UserrShifts.Shift); }
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
                /*
                if (_EntryDate != value)
                {
                    _EntryDate = value;
                    RaiseChanged("EntryDate");
                }
                */
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
                    RevenueDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss") : string.Empty;
            }
        }

        #endregion

        #region User/Chief (Current)

        /// <summary>
        /// Gets Current User.
        /// </summary>
        public User User 
        { 
            get { return this.Current.User; }
            set { this.Current.User = value; }
        }
        /// <summary>
        /// Gets Current Chief.
        /// </summary>
        public User Chief { get { return this.Current.Chief; } }

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
        /// The PlazaGroupChanged Event Handler.
        /// </summary>
        public event System.EventHandler PlazaGroupChanged;

        #endregion
    }

    #endregion

    #region HistoricalRevenueEntryManager

    /// <summary>
    /// The HistoricalRevenueEntryManager class.
    /// </summary>
    public class HistoricalRevenueEntryManager
    {
        #region Public Properties

        #endregion
    }

    #endregion
}
