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

#endregion

namespace DMT.Services
{
    using scwOps = Services.Operations.SCW.TOD;

    #region TAAPI

    /// <summary>
    /// The TAAPI class.
    /// </summary>
    public static class TAAPI
    {
        #region Static Properties

        /// <summary>The NetWorkId for SCW.</summary>
        public static int NetworkId
        {
            get { return TAConfigManager.Instance.DMT.networkId; }
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
            get { return PlazaGroup.GetTSBPlazaGroups(TAAPI.TSB).Value(); }
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
            Refresh();
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~CurrentTSBManager()
        {

        }

        #endregion

        #region Private Methods

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
            TSB = TAAPI.TSB;
            if (null != TSB)
            {
                // Load Plaza Groups, Plazas and Lanes
                TSBPlazaGroups = TAAPI.TSBPlazaGroups;
                TSBPlazas = TAAPI.TSBPlazas;
                TSBLanes = TAAPI.TSBLanes;
                // Get Current TSB Shift
                TSBShift = TAAPI.TSBShift;
                // Gets Chief
                Chief = TSBShift.Chief();
            }
            // Init Shifts
            Shifts = TAAPI.Shifts;
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
}
