#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
//using System.Windows.Forms;
//using System.Runtime.InteropServices;
//using System.ComponentModel;
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
            if (null != UserChanged)
            {
                UserChanged.Call(this, EventArgs.Empty);
            }
        }

        private void RaisePlazaGroupChanged()
        {
            if (null != PlazaGroupChanged)
            {
                PlazaGroupChanged.Call(this, EventArgs.Empty);
            }
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

        }

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {

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

        }

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {

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
    }

    #endregion

    #region UserShiftManager and UserShift ExtensionMethods

    /// <summary>
    /// The UserShiftManager class.
    /// </summary>
    public class UserShiftManager
    {
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

        }

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {

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
        public UserShift Shift
        {
            get 
            {
                if (null == Current || null == Current.User) return null;
                return UserShift.GetUserShift(Current.User.UserId).Value();
            }
        }

        #endregion
    }

    #endregion

    #region RevenueEntryManager

    /// <summary>
    /// The RevenueEntryManager class.
    /// </summary>
    public class RevenueEntryManager
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RevenueEntryManager() : base()
        {
            this.ByChief = false;

            Current = new CurrentTSBManager();
            if (null != Current)
            {
                Current.UserChanged += Current_UserChanged;
                Current.PlazaGroupChanged += Current_PlazaGroupChanged;
            }

            Jobs = new JobManager(Current);
            Payments = new PaymentManager(Current);
            UserrShifts = new UserShiftManager(Current);

            Clear();
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~RevenueEntryManager()
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

        }

        private void Current_PlazaGroupChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #endregion

        #region Public Methods

        public void Clear()
        {
            this.EntryDate = DateTime.Now;
            this.RevenueDate = DateTime.Now;
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

        #endregion

        #region ByChief

        /// <summary>Gets or sets is revenue by chief.</summary>
        public bool ByChief { get; set; }

        #endregion

        #region EntryDate

        /// <summary>
        /// Gets or sets Entry Date.
        /// </summary>
        public DateTime? EntryDate { get; internal set; }
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
        public DateTime? RevenueDate { get; set; }
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
