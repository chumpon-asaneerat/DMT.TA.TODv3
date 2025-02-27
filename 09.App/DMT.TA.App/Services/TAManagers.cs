﻿#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media;
using System.Threading.Tasks;
using DMT.Controls.StatusBar;

#endregion

namespace DMT.Services
{
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

        #region TA

        /// <summary>
        /// Gets TA PlazaGroups.
        /// </summary>
        public static List<PlazaGroup> TAPlazaGroups
        {
            get { return GetTAPlazaGroups(); }
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

        #endregion

        #region Static Methods

        #region TA PlazaGroup/Plaza methods

        /// <summary>
        /// Get TA's PlazaGroups.
        /// </summary>
        /// <returns>Returns list of PlazaGroup.</returns>
        public static List<PlazaGroup> GetTAPlazaGroups()
        {
            List<PlazaGroup> results = new List<PlazaGroup>();

            var cfg = TAConfigManager.Instance.Value;
            var plazaGroups = (null != cfg && null != cfg.PlazaGroups) ? cfg.PlazaGroups : null;
            if (null != plazaGroups && plazaGroups.Count > 0)
            {
                plazaGroups.ForEach(plazaGroup =>
                {
                    if (null == plazaGroup) return;
                    if (string.IsNullOrWhiteSpace(plazaGroup.PlazaGroupId)) return;
                    var match = PlazaGroup.GetPlazaGroup(plazaGroup.PlazaGroupId).Value();
                    if (null != match && match.PlazaGroupId != string.Empty)
                    {
                        var exist = results.Find(plazagroup =>
                        {
                            return plazagroup.PlazaGroupId == match.PlazaGroupId;
                        });
                        if (null != exist) return; // already exist.

                        results.Add(match); // Append if not exists
                    }
                });
            }

            return results;
        }

        #endregion

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
        /// <summary>
        /// Gets TSB Coupon Balance Summary.
        /// </summary>
        /// <returns></returns>
        public static TSBCouponBalance GetTSBCouponBalance()
        {
            var ret = TSBCouponBalance.GetTSBBalance().Value();
            if (null == ret)
            {
                ret = new TSBCouponBalance();
            }
            return ret;
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
            Refresh();

            this.Credit = new CreditManager(this);
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~CurrentTSBManager()
        {
            this.Credit = null;
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

            if (null != this.Credit) this.Credit.Refresh();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Credit Manager.
        /// </summary>
        public CreditManager Credit { get; private set; }
        /// <summary>
        /// Gets Current TSB Coupon Balance
        /// </summary>
        public TSBCouponBalance TSBCouponBalance
        {
            get { return TAAPI.GetTSBCouponBalance();  }
            set { }
        }
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

    #region CreditManager (TSB)

    /// <summary>
    /// The CreditManager Class.
    /// </summary>
    public class CreditManager : INotifyPropertyChanged
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        private CreditManager() : base()
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="manager">The CurrentTSBManager instance.</param>
        public CreditManager(CurrentTSBManager manager) : this()
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
        ~CreditManager()
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

        #region Credit Methods

        private void CheckInitBalance()
        {
            var tran = TSBCreditTransaction.GetInitialTransaction().Value();
            if (null != tran && tran.BHTTotal == decimal.Zero)
            {
                tran.AmountBHT1 = 10000;
                tran.AmountBHT2 = 10000;
                tran.AmountBHT5 = 10000;
                tran.AmountBHT10 = 20000;
                tran.AmountBHT20 = 25000;
                tran.AmountBHT50 = 25000;
                tran.AmountBHT100 = 30000;
                tran.AmountBHT500 = 50000;
                tran.AmountBHT1000 = 5000;

                TSBCreditTransaction.SaveTransaction(tran);
            }
        }

        private void LoadCredits()
        {
            CheckInitBalance();
            var balance = Models.TSBCreditBalance.GetCurrent().Value();
            if (null == TSBBalance)
            {
                TSBBalance = balance;
            }
            else
            {
                // Update values.
                //balance.AssignTo(TSBBalance);
                TSBBalance = balance;
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
            LoadCredits();
        }

        #endregion

        #region Public Properties

        #region Manager

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
            get { return (null != Current) ? Current.CollectorNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region TSB Credit

        /// <summary>
        /// Gets TSB Balance.
        /// </summary>
        public TSBCreditBalance TSBBalance { get; private set; }

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

    #region User Credit Manager classes

    #region UserCreditManager (abstract)

    /// <summary>
    /// User Credit Manager (abstract) class.
    /// </summary>
    public abstract class UserCreditManager : INotifyPropertyChanged
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserCreditManager() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~UserCreditManager()
        {
            if (null != this.Transaction)
            {
                this.Transaction.PropertyChanged += Transaction_PropertyChanged;
            }
            this.Transaction = null;
        }

        #endregion

        #region Private/Protected Methods

        /// <summary>
        /// Raise Property Changed Event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void RaiseChanged(string propertyName)
        {
            PropertyChanged.Call(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Transaction_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.Calc();
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Calculate.
        /// </summary>
        protected abstract void Calc();
        /// <summary>
        /// Save.
        /// </summary>
        /// <returns>Returns true if sace success.</returns>
        public abstract bool Save();

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup Current User Credit Balance.
        /// </summary>
        /// <param name="current">The User Credit Balance instance.</param>
        public void Setup(UserCreditBalance current)
        {
            this.UserBalance = current;
            this.IsNew = (null == this.UserBalance);
            if (this.IsNew)
            {
                this.UserBalance = new UserCreditBalance();
            }

            this.Transaction = new UserCreditTransaction();

            this.TSBBalance = TSBCreditBalance.GetCurrent().Value();
            this.ResultBalance = new TSBCreditBalance();

            this.TSBBalance.AssignTo(this.ResultBalance);


            // Hook Event to recalcuate when transaction's property changed.
            this.Transaction.PropertyChanged += Transaction_PropertyChanged;
        }
        /// <summary>
        /// Set Current User.
        /// </summary>
        /// <param name="user">The User instance.</param>
        public void SetUser(User user)
        {
            User = user;
            if (null != User)
            {
                if (null != UserBalance)
                {
                    UserBalance.UserId = User.UserId;
                    UserBalance.FullNameEN = User.FullNameEN;
                    UserBalance.FullNameTH = User.FullNameTH;
                    // Assign UserId to BagNo
                    UserBalance.BagNo = User.UserId;
                }
            }
            else
            {
                if (null != UserBalance)
                {
                    UserBalance.UserId = null;
                    UserBalance.FullNameEN = null;
                    UserBalance.FullNameTH = null;
                    // Assign UserId to BagNo/BeltNo (so set as null)
                    UserBalance.BagNo = null;
                    UserBalance.BeltNo = null;
                }
            }

            RaiseChanged("CollectorId");
            RaiseChanged("CollectorNameEN");
            RaiseChanged("CollectorNameTH");
        }
        /// <summary>
        /// Checks has negative amount.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasNegative() { return false; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Plaza Group.
        /// </summary>
        public PlazaGroup PlazaGroup { get; set; }
        /// <summary>
        /// Gets or sets User.
        /// </summary>
        public User User { get; private set; }
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

        /// <summary>
        /// Checks is new UserBalance.
        /// </summary>
        public bool IsNew { get; private set; }
        /// <summary>
        /// Gets Current user credit (original).
        /// </summary>
        public UserCreditBalance UserBalance { get; private set; }
        /// <summary>
        /// Gets Editable Transaction.
        /// </summary>
        public UserCreditTransaction Transaction { get; private set; }
        /// <summary>
        /// Gets Current TSB Balance.
        /// </summary>
        public TSBCreditBalance TSBBalance { get; private set; }
        /// <summary>
        /// Gets result TSB Balance after apply transaction.
        /// </summary>
        public TSBCreditBalance ResultBalance { get; private set; }

        #endregion

        #region Public Events

        /// <summary>
        /// The PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    #endregion

    #region UserCreditBorrowManager

    /// <summary>
    /// User Credit Borrow Manager class.
    /// </summary>
    public class UserCreditBorrowManager : UserCreditManager
    {
        #region Override Methods

        protected override void Calc()
        {
            if (null == ResultBalance || null == TSBBalance || null == Transaction)
                return;

            ResultBalance.AmountST25 = TSBBalance.AmountST25 - Transaction.AmountST25;
            ResultBalance.AmountST50 = TSBBalance.AmountST50 - Transaction.AmountST50;
            ResultBalance.AmountBHT1 = TSBBalance.AmountBHT1 - Transaction.AmountBHT1;
            ResultBalance.AmountBHT2 = TSBBalance.AmountBHT2 - Transaction.AmountBHT2;
            ResultBalance.AmountBHT5 = TSBBalance.AmountBHT5 - Transaction.AmountBHT5;
            ResultBalance.AmountBHT10 = TSBBalance.AmountBHT10 - Transaction.AmountBHT10;
            ResultBalance.AmountBHT20 = TSBBalance.AmountBHT20 - Transaction.AmountBHT20;
            ResultBalance.AmountBHT50 = TSBBalance.AmountBHT50 - Transaction.AmountBHT50;
            ResultBalance.AmountBHT100 = TSBBalance.AmountBHT100 - Transaction.AmountBHT100;
            ResultBalance.AmountBHT500 = TSBBalance.AmountBHT500 - Transaction.AmountBHT500;
            ResultBalance.AmountBHT1000 = TSBBalance.AmountBHT1000 - Transaction.AmountBHT1000;
        }

        public override bool HasNegative()
        {
            return (
                ResultBalance.AmountST25 < 0 ||
                ResultBalance.AmountST50 < 0 ||
                ResultBalance.AmountBHT1 < 0 ||
                ResultBalance.AmountBHT2 < 0 ||
                ResultBalance.AmountBHT5 < 0 ||
                ResultBalance.AmountBHT10 < 0 ||
                ResultBalance.AmountBHT20 < 0 ||
                ResultBalance.AmountBHT50 < 0 ||
                ResultBalance.AmountBHT100 < 0 ||
                ResultBalance.AmountBHT500 < 0 ||
                ResultBalance.AmountBHT1000 < 0);
        }

        public override bool Save()
        {
            bool result = false;
            // Check User Balance is already inserted
            if (null != UserBalance && UserBalance.UserCreditId == 0)
            {
                // not inserted so insert new record.
                if (string.IsNullOrWhiteSpace(UserBalance.UserId) && null != User)
                {
                    UserBalance.UserId = User.UserId;
                    UserBalance.FullNameEN = User.FirstNameEN;
                    UserBalance.FullNameTH = User.FirstNameTH;
                }

                if (null != PlazaGroup)
                {
                    UserBalance.TSBId = PlazaGroup.TSBId;
                    UserBalance.PlazaGroupId = PlazaGroup.PlazaGroupId;
                }

                var exist = UserCreditBalance.GetCurrentBalance(UserBalance.UserId, UserBalance.PlazaGroupId).Value();
                if (null != exist && exist.UserCreditId != 0)
                {
                    // Already exist so used exist id instead.
                    UserBalance.UserCreditId = exist.UserCreditId;
                    UserBalance.State = exist.State; // Update Balance State.
                }
                else
                {
                    // Not exist so set Balance State to initial.
                    UserBalance.State = UserCreditBalance.StateTypes.Initial;
                }
                // Save.
                var newBalance = UserCreditBalance.SaveUserCreditBalance(UserBalance).Value();
                int pkid = (null != newBalance) ? newBalance.UserCreditId : 0;
                // Resync Id.
                UserBalance.UserCreditId = pkid;
            }
            // Save User Credit Transaction.
            if (null != Transaction && null != UserBalance)
            {
                Transaction.UserCreditId = UserBalance.UserCreditId;
                Transaction.TransactionType = UserCreditTransaction.TransactionTypes.Borrow;
                if (string.IsNullOrWhiteSpace(Transaction.TSBId))
                {
                    Transaction.TSBId = UserBalance.TSBId;
                }
                if (string.IsNullOrWhiteSpace(Transaction.PlazaGroupId))
                {
                    Transaction.PlazaGroupId = UserBalance.PlazaGroupId;
                }
                if (string.IsNullOrWhiteSpace(Transaction.UserId))
                {
                    Transaction.UserId = UserBalance.UserId;
                    Transaction.FullNameEN = UserBalance.FullNameEN;
                    Transaction.FullNameTH = UserBalance.FullNameTH;
                }

                UserCreditTransaction.SaveUserCreditTransaction(Transaction);

                // For Update User Bag Number and balance
                var balance = UserCreditBalance.GetCurrentBalance(
                    UserBalance.UserId, UserBalance.PlazaGroupId).Value();
                if (null != balance)
                {
                    // For Update User Bag Number and balance
                    var usr = User.GetByUserId(balance.UserId).Value();
                    // For Update User Bag Number and balance
                    var usrCdt = new TAAUserCredit();
                    usrCdt.TSBId = balance.TSBId;
                    usrCdt.UserId = balance.UserId;
                    usrCdt.UserPrefix = (null != usr) ? usr.PrefixTH : string.Empty;
                    usrCdt.UserFirstName = (null != usr) ? usr.FirstNameTH : balance.FullNameTH;
                    usrCdt.UserLastName = (null != usr) ? usr.LastNameTH : string.Empty;
                    //usrCdt.BagNo = (balance.State == UserCreditBalance.StateTypes.Initial) ? null : balance.BagNo;
                    usrCdt.BagNo = balance.BagNo;
                    usrCdt.CreditDate = balance.UserCreditDate;
                    usrCdt.Credit = balance.BHTTotal;
                    usrCdt.flag = 0; // Borrow so flag is 0
                    // write to quque
                    TAxTODMQService.Instance.WriteQueue(usrCdt);
                }
                // Find current TSB balance.
                var tsbBal = TSBCreditBalance.GetCurrent(TAAPI.TSB).Value();
                if (null != tsbBal)
                {
                    // For Update TSB balance
                    var tsbCdt = new TAATSBCredit();
                    tsbCdt.TSBId = tsbBal.TSBId;
                    tsbCdt.Amnt1 = tsbBal.AmountBHT1;
                    tsbCdt.Amnt2 = tsbBal.AmountBHT2;
                    tsbCdt.Amnt5 = tsbBal.AmountBHT5;
                    tsbCdt.Amnt10 = tsbBal.AmountBHT10;
                    tsbCdt.Amnt20 = tsbBal.AmountBHT20;
                    tsbCdt.Amnt50 = tsbBal.AmountBHT50;
                    tsbCdt.Amnt100 = tsbBal.AmountBHT100;
                    tsbCdt.Amnt500 = tsbBal.AmountBHT500;
                    tsbCdt.Amnt1000 = tsbBal.AmountBHT1000;
                    tsbCdt.Remark = null;
                    tsbCdt.Updatedate = DateTime.Now;
                    TAxTODMQService.Instance.WriteQueue(tsbCdt);
                }

                result = true; // save success.
            }

            return result;
        }

        #endregion
    }

    #endregion

    #region UserCreditReturnManager

    /// <summary>
    /// User Credit Return Manager class.
    /// </summary>
    public class UserCreditReturnManager : UserCreditManager
    {
        #region Override Methods

        protected override void Calc()
        {
            if (null == ResultBalance || null == TSBBalance || null == Transaction)
                return;
        }

        public override bool Save()
        {
            var result = false;
            if (null != Transaction && null != UserBalance)
            {
                Transaction.UserCreditId = UserBalance.UserCreditId;
                Transaction.TransactionType = UserCreditTransaction.TransactionTypes.Return;

                // Set TSB/PlazaGroup/User's Name (EN/TH).
                if (string.IsNullOrWhiteSpace(Transaction.TSBId))
                {
                    Transaction.TSBId = UserBalance.TSBId;
                }
                if (string.IsNullOrWhiteSpace(Transaction.PlazaGroupId))
                {
                    Transaction.PlazaGroupId = UserBalance.PlazaGroupId;
                }
                if (string.IsNullOrWhiteSpace(Transaction.UserId))
                {
                    Transaction.UserId = UserBalance.UserId;
                    Transaction.FullNameEN = UserBalance.FullNameEN;
                    Transaction.FullNameTH = UserBalance.FullNameTH;
                }

                UserCreditTransaction.SaveUserCreditTransaction(Transaction);

                // Check is total borrow is reach zero.
                var balance = UserCreditBalance.GetCurrentBalance(
                    UserBalance.UserId, UserBalance.PlazaGroupId).Value();
                if (null != balance)
                {
                    int flag = 0;
                    if (balance.BHTTotal <= decimal.Zero)
                    {
                        // change source state.
                        UserBalance.State = UserCreditBalance.StateTypes.Completed;
                        UserCreditBalance.SaveUserCreditBalance(UserBalance);
                        // Note: if Balance is zero @flag = 1
                        flag = 1;
                    }
                    // For Update User Bag Number and balance
                    var usr = User.GetByUserId(balance.UserId).Value();
                    // For Update User Bag Number and balance
                    var usrCdt = new TAAUserCredit();
                    usrCdt.TSBId = balance.TSBId;
                    usrCdt.UserId = balance.UserId;
                    usrCdt.UserPrefix = (null != usr) ? usr.PrefixTH : string.Empty;
                    usrCdt.UserFirstName = (null != usr) ? usr.FirstNameTH : balance.FullNameTH;
                    usrCdt.UserLastName = (null != usr) ? usr.LastNameTH : string.Empty;
                    //usrCdt.BagNo = (balance.State == UserCreditBalance.StateTypes.Initial) ? null : balance.BagNo;
                    usrCdt.BagNo = balance.BagNo;
                    usrCdt.CreditDate = balance.UserCreditDate;
                    usrCdt.Credit = balance.BHTTotal;
                    usrCdt.flag = flag;
                    // write to quque
                    TAxTODMQService.Instance.WriteQueue(usrCdt);

                    // Find current TSB balance.
                    var tsbBal = TSBCreditBalance.GetCurrent(TAAPI.TSB).Value();
                    if (null != tsbBal)
                    {
                        // For Update TSB balance
                        var tsbCdt = new TAATSBCredit();
                        tsbCdt.TSBId = tsbBal.TSBId;
                        tsbCdt.Amnt1 = tsbBal.AmountBHT1;
                        tsbCdt.Amnt2 = tsbBal.AmountBHT2;
                        tsbCdt.Amnt5 = tsbBal.AmountBHT5;
                        tsbCdt.Amnt10 = tsbBal.AmountBHT10;
                        tsbCdt.Amnt20 = tsbBal.AmountBHT20;
                        tsbCdt.Amnt50 = tsbBal.AmountBHT50;
                        tsbCdt.Amnt100 = tsbBal.AmountBHT100;
                        tsbCdt.Amnt500 = tsbBal.AmountBHT500;
                        tsbCdt.Amnt1000 = tsbBal.AmountBHT1000;
                        tsbCdt.Remark = null;
                        tsbCdt.Updatedate = DateTime.Now;
                        // write to quque
                        TAxTODMQService.Instance.WriteQueue(tsbCdt);
                    }
                }

                result = true;
            }
            return result;
        }

        public override bool HasNegative()
        {
            return (Transaction.BHTTotal > UserBalance.BHTTotal);
        }

        #endregion
    }

    #endregion

    #region UserCreditTransactionManager

    /// <summary>
    /// User Credit Transaction Manager.
    /// </summary>
    public class UserCreditTransactionManager : INotifyPropertyChanged
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserCreditTransactionManager() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~UserCreditTransactionManager() { }

        #endregion

        #region Private/Protected Methods

        /// <summary>
        /// Raise Property Changed Event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void RaiseChanged(string propertyName)
        {
            PropertyChanged.Call(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Set Current User.
        /// </summary>
        /// <param name="user">The User instance.</param>
        public void SetUser(User user)
        {
            User = user;
            RaiseChanged("CollectorId");
            RaiseChanged("CollectorNameEN");
            RaiseChanged("CollectorNameTH");
        }
        /// <summary>
        /// Refresh.
        /// </summary>
        public void Refresh()
        {
            if (!SearchDate.HasValue || SearchDate == DateTime.MinValue)
            {
                Transactions = new List<UserCreditTransaction>();
                return;
            }
            DateTime start = SearchDate.Value.Date;
            DateTime end = start.AddDays(1);
            var shifts = TSBShift.GetTSBShifts(start).Value();
            Transactions = UserCreditTransaction.GetUserCreditTransactions(
                TAAPI.TSB, User, start, end).Value();
            if (null != Transactions && null != shifts)
            {
                Transactions.ForEach(tran => 
                {
                    var shift = shifts.Find(shf =>
                    {
                        bool matchBegin = shf.Begin <= tran.TransactionDate;
                        bool macthEnd = ((shf.End.HasValue && shf.End > tran.TransactionDate) || !shf.End.HasValue);
                        return matchBegin && macthEnd;
                    });
                    if (null != shift)
                    {
                        tran.SupervisorId = shift.UserId;
                        tran.SupervisorNameEN = shift.FullNameEN;
                        tran.SupervisorNameTH = shift.FullNameTH;
                    }
                });
            }
        }
        /// <summary>
        /// Cancel Transaction.
        /// </summary>
        /// <param name="item">The UserCreditTransaction instance.</param>
        public void CancelTransaction(UserCreditTransaction item)
        {
            /*
            if (null == item) return;

            bool needUpdate = false;

            if (item.TransactionType == UserCreditTransaction.TransactionTypes.Borrow)
            {
                item.TransactionType = UserCreditTransaction.TransactionTypes.UndoBorrow;
                needUpdate = true;
            }
            else if (item.TransactionType == UserCreditTransaction.TransactionTypes.Return)
            {
                item.TransactionType = UserCreditTransaction.TransactionTypes.UndoReturn;
                needUpdate = true;
            }
            // Check Need update.
            if (needUpdate)
            {
                UserCreditTransaction.SaveUserCreditTransaction(item);

                var creditBalance = UserCreditBalance.GetBalance(item.UserCreditId).Value();
                if (null != creditBalance)
                {
                    if (string.IsNullOrEmpty(creditBalance.RevenueId))
                    {
                        creditBalance.State = UserCreditBalance.StateTypes.Initial;
                    }
                    else
                    {
                        creditBalance.State = UserCreditBalance.StateTypes.Received;
                    }
                    UserCreditBalance.SaveUserCreditBalance(creditBalance);
                }
                //Refresh();
            }
            */
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Plaza Group.
        /// </summary>
        public PlazaGroup PlazaGroup { get; set; }
        /// <summary>
        /// Gets or sets User.
        /// </summary>
        public User User { get; private set; }
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
        /// <summary>
        /// Gets Transactions.
        /// </summary>
        public List<UserCreditTransaction> Transactions { get; private set; }
        /// <summary>
        /// Gets or sets Search Date.
        /// </summary>
        public DateTime? SearchDate { get; set; }

        #endregion

        #region Public Events

        /// <summary>
        /// The PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    #endregion

    #endregion

    #region TSB Coupon Manager related classes

    #region TSBCouponItem

    /// <summary>
    /// The TSB Coupon Item class.
    /// </summary>
    public class TSBCouponItem
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private TSBCouponItem() : base() { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">The Original TSBTransaction.</param>
        public TSBCouponItem(TSBCouponTransaction value) : this()
        {
            Transaction = value;
            if (null != value)
            {
                // Assign Original value.
                TransactionType = value.TransactionType;
                UserId = value.UserId;
                FullNameEN = value.FullNameEN;
                FullNameTH = value.FullNameTH;
                ReceiveDate = value.UserReceiveDate;
                SoldBy = value.SoldBy;
                SoldByFullNameEN = value.SoldByFullNameEN;
                SoldByFullNameTH = value.SoldByFullNameTH;
                SoldDate = value.SoldDate;
                FinishFlag = value.FinishFlag;
            }
        }

        #endregion

        #region Private Methods

        private bool UserIdChanged()
        {
            if (null == Transaction) return false;
            string val1 = Transaction.UserId;
            string val2 = UserId;

            if (string.IsNullOrEmpty(val1) && string.IsNullOrEmpty(val2))
            {
                // TODO: Need this code in new version.
                //
                //if (val1 == null && val2 != null)
                //{
                //    return true; // val2 is string.Empty.
                //}
                //else if (val1 != null && val2 == null)
                //{
                //    return true; // val1 is string.Empty.
                //}
                //else if (val1 != null && val2 != null)
                //{
                //    return true; // Both is string.Empty.
                //}
                //else
                //{
                //    return false; // Both is null.
                //}

                return false; // Both is null.
            }
            else if (!string.IsNullOrEmpty(val1) && string.IsNullOrEmpty(val2))
            {
                return true; // val1 is null, val2 has value
            }
            else if (string.IsNullOrEmpty(val1) && !string.IsNullOrEmpty(val2))
            {
                return true; // val1 is has value, val2 is null
            }
            else
            {
                return (string.CompareOrdinal(val1, val2) != 0);
            }
        }

        private bool ReceivedDateChanged()
        {
            if (null == Transaction) return false;
            DateTime? val1 = Transaction.UserReceiveDate;
            DateTime? val2 = ReceiveDate;
            if (!val1.HasValue && !val2.HasValue)
            {
                return false; // Both is null.
            }
            else if (!val1.HasValue && val2.HasValue)
            {
                return true; // val1 is null, val2 has value
            }
            else if (val1.HasValue && !val2.HasValue)
            {
                return true; // val1 is has value, val2 is null
            }
            else
            {
                return (val1.Value != val2.Value);
            }
        }

        private bool SoldByChanged()
        {
            if (null == Transaction) return false;
            string id1 = Transaction.SoldBy;
            string id2 = SoldBy;

            if (string.IsNullOrEmpty(id1) && string.IsNullOrEmpty(id2))
            {
                return false;
            }
            else if (!string.IsNullOrEmpty(id1) && string.IsNullOrEmpty(id2))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(id1) && !string.IsNullOrEmpty(id2))
            {
                return true;
            }
            else
            {
                return (string.CompareOrdinal(id1, id2) != 0);
            }
        }

        private bool SoldDateChanged()
        {
            if (null == Transaction) return false;
            DateTime? val1 = Transaction.SoldDate;
            DateTime? val2 = SoldDate;
            if (!val1.HasValue && !val2.HasValue)
            {
                return false; // Both is null.
            }
            else if (!val1.HasValue && val2.HasValue)
            {
                return true; // val1 is null, val2 has value
            }
            else if (val1.HasValue && !val2.HasValue)
            {
                return true; // val1 is has value, val2 is null
            }
            else
            {
                return (val1.Value != val2.Value);
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var code = string.Empty;
            if (null != Transaction) code = Transaction.CouponId;
            return code.GetHashCode();
        }
        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (null == obj || !(obj is TSBCouponItem))
                return false;
            var target = obj as TSBCouponItem;
            if (null == target) return false;

            return GetHashCode() == target.GetHashCode();
        }
        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (null != Transaction) return Transaction.CouponId;
            return string.Empty;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Has Changed.
        /// </summary>
        /// <returns></returns>
        public bool HasChanged()
        {
            bool ret = false;
            if (null != Transaction)
            {
                bool bTransactionType = (Transaction.TransactionType != TransactionType);

                bool bUserId = UserIdChanged();
                bool bUserReceiveDate = ReceivedDateChanged();
                bool bSoldBy = SoldByChanged();
                bool bSoldDate = SoldDateChanged();

                bool bFinishFlag = (Transaction.FinishFlag != FinishFlag);

                if (bTransactionType ||
                    bUserId || bUserReceiveDate ||
                    bSoldBy || bSoldDate ||
                    bFinishFlag)
                {
                    ret = true;
                }
            }
            return ret;
        }
        /// <summary>
        /// Apply Changes.
        /// </summary>
        public void ApplyChanges()
        {
            if (null != Transaction)
            {
                Transaction.TransactionType = TransactionType;
                Transaction.UserId = string.IsNullOrWhiteSpace(UserId) ? null : UserId;
                Transaction.FullNameEN = FullNameEN;
                Transaction.FullNameTH = FullNameTH;
                Transaction.UserReceiveDate = ReceiveDate;
                Transaction.SoldBy = string.IsNullOrWhiteSpace(SoldBy) ? null : SoldBy;
                Transaction.SoldByFullNameEN = SoldByFullNameEN;
                Transaction.SoldByFullNameTH = SoldByFullNameTH;
                Transaction.SoldDate = SoldDate;
                Transaction.FinishFlag = FinishFlag;
            }
        }
        /// <summary>
        /// Convert to Server Model.
        /// </summary>
        /// <returns>Returns server model instance.</returns>
        public TAServerCouponTransaction ToServer()
        {
            return (null != Transaction) ? Transaction.ToServer() : null;
        }
        /// <summary>
        /// Commit.
        /// </summary>
        /// <returns>Returns true if has changed and need to save to database.</returns>
        public bool Commit()
        {
            bool hasChanged = HasChanged();
            if (hasChanged)
            {
                ApplyChanges(); // apply changed
            }
            return hasChanged;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets Original TSB Transaction.</summary>
        public TSBCouponTransaction Transaction { get; private set; }

        /// <summary>Gets CouponId.</summary>
        public string CouponId
        {
            get { return (null != Transaction) ? Transaction.CouponId : string.Empty; }
            set { }
        }

        /// <summary>Gets Coupon Type.</summary>
        public CouponType CouponType
        {
            get { return (null != Transaction) ? Transaction.CouponType : CouponType.Unknown; }
            set { }
        }
        /// <summary>Gets Foreground Color.</summary>
        public SolidColorBrush Foreground
        {
            get { return (null != Transaction) ? Transaction.Foreground : TSBCouponTransaction.BlackForeground; }
            set { }
        }

        /// <summary>Gets or sets TransactionType.</summary>
        public TSBCouponTransactionTypes TransactionType { get; set; }
        /// <summary>Gets or sets UserId (received).</summary>
        public string UserId { get; set; }
        /// <summary>Gets or sets FullNameEH (received).</summary>
        public string FullNameEN { get; set; }
        /// <summary>Gets or sets FullNameTH (received).</summary>
        public string FullNameTH { get; set; }
        /// <summary>Gets or sets Receive datetime.</summary>
        public DateTime? ReceiveDate { get; set; }
        /// <summary>Gets or sets SoldBy (User).</summary>
        public string SoldBy { get; set; }
        /// <summary>Gets or sets SoldByFullNameEH (received).</summary>
        public string SoldByFullNameEN { get; set; }
        /// <summary>Gets or sets SoldByFullNameTH (received).</summary>
        public string SoldByFullNameTH { get; set; }
        /// <summary>Gets or sets Receive datetime.</summary>
        public DateTime? SoldDate { get; set; }
        /// <summary>Gets or sets Finished Flag.</summary>
        public TSBCouponFinishedFlags FinishFlag { get; set; }

        #endregion
    }

    #endregion

    #region TSBCouponManager (abstract)

    /// <summary>
    /// TSB Coupon Manager (abstract) class.
    /// </summary>
    public abstract class TSBCouponManager : INotifyPropertyChanged
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBCouponManager() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TSBCouponManager()
        {
            if (null == Coupons)
            {
                Coupons.Clear();
            }
            Coupons = null;
        }

        #endregion

        #region Private/Protected Methods

        /// <summary>
        /// Raise Property Changed Event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void RaiseChanged(string propertyName)
        {
            PropertyChanged.Call(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Load Coupons.
        /// </summary>
        protected void LoadCoupons()
        {
            if (null == Coupons) Coupons = new List<TSBCouponItem>();
            Coupons.Clear();
            if (null != User)
            {
                var coupons = TSBCouponTransaction.GetTSBCouponTransactions(TAAPI.TSB).Value();
                if (null != coupons && coupons.Count > 0)
                {
                    coupons.ForEach(coupon =>
                    {
                        if (coupon.CouponType == CouponType.BHT40 || coupon.CouponType == CouponType.BHT90)
                        {
                            Console.WriteLine("{0}", coupon.CouponId);
                        }
                        Coupons.Add(new TSBCouponItem(coupon));
                    });
                }
            }
            OnLoadCoupons();
        }
        /// <summary>
        /// On Load Coupons.
        /// </summary>
        protected virtual void OnLoadCoupons() { }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Save.
        /// </summary>
        /// <returns>Returns true if sace success.</returns>
        public abstract bool Save();

        #endregion

        #region Public Method

        #region User

        /// <summary>
        /// Set Current User.
        /// </summary>
        /// <param name="user">The User instance.</param>
        public void SetUser(User user)
        {
            User = user;
            if (null != User)
            {
                LoadCoupons();
            }
            RaiseChanged("CollectorId");
            RaiseChanged("CollectorNameEN");
            RaiseChanged("CollectorNameTH");
        }

        #endregion

        #endregion

        #region Public Properties

        #region User related properties

        /// <summary>
        /// Gets or sets User.
        /// </summary>
        public User User { get; private set; }
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

        #region Coupons

        /// <summary>Gets all Coupons on current TSB.</summary>
        public List<TSBCouponItem> Coupons { get; private set; }

        #endregion

        #endregion

        #region Public Events

        /// <summary>
        /// The PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    #endregion

    #region TSBCouponBorrowManager

    /// <summary>
    /// TSB Coupon Borrow Manager class.
    /// </summary>
    public class TSBCouponBorrowManager : TSBCouponManager
    {
        #region Override Methods

        /// <summary>
        /// Save.
        /// </summary>
        /// <returns>Returns true if sace success.</returns>
        public override bool Save()
        {
            if (null == Coupons || Coupons.Count <= 0) return true;

            MethodBase med = MethodBase.GetCurrentMethod();

            string msg = string.Empty;

            msg = string.Format("Begin Save: {0:HH:mm:ss.fff}", DateTime.Now);
            med.Info(msg);
            Console.WriteLine(msg);

            List<TSBCouponTransaction> saveList = new List<TSBCouponTransaction>();

            Coupons.ForEach(coupon =>
            {
                if (null != coupon)
                {
                    if (null != coupon.Transaction && coupon.Commit())
                    {
                        saveList.Add(coupon.Transaction); // keep transaction to save later.
                    }
                }
            });

            if (null != saveList && saveList.Count > 0)
            {

                msg = string.Format("Update local db start: {0:HH:mm:ss.fff}", DateTime.Now);
                med.Info(msg);
                Console.WriteLine(msg);

                TSBCouponTransaction.SaveTransactions(saveList);

                msg = string.Format("Update local db finished: {0:HH:mm:ss.fff}", DateTime.Now);
                med.Info(msg);
                Console.WriteLine(msg);

                msg = string.Format("generate server queue count: {0}", saveList.Count);
                med.Info(msg);
                Console.WriteLine(msg);

                msg = string.Format("generate server queue start: {0:HH:mm:ss.fff}", DateTime.Now);
                med.Info(msg);
                Console.WriteLine(msg);

                saveList.ForEach(localTran =>
                {
                    // insert coupon status into table
                    if (localTran.TransactionType == TSBCouponTransactionTypes.Lane ||
                        localTran.TransactionType == TSBCouponTransactionTypes.SoldByLane)
                    {
                        // insert.
                        TSBCouponBorrowStatus.Borrow(localTran.UserId, localTran.CouponId);
                    }
                    else if (localTran.TransactionType == TSBCouponTransactionTypes.Stock ||
                        localTran.TransactionType == TSBCouponTransactionTypes.CancelOrRemove)
                    {
                        // Remove.
                        TSBCouponBorrowStatus.Return(localTran.CouponId);
                    }

                    // Write Queue
                    TAServerCouponTransaction tran = localTran.ToServer();
                    if (null != tran)
                    {
                        TAxTODMQService.Instance.WriteQueue(tran);
                    }
                });

                msg = string.Format("generate server queue finished: {0:HH:mm:ss.fff}", DateTime.Now);
                med.Info(msg);
                Console.WriteLine(msg);
            }

            msg = string.Format("Finished Save: {0:HH:mm:ss.fff}", DateTime.Now);
            med.Info(msg);
            Console.WriteLine(msg);

            return true;
        }

        #endregion

        #region Public Methods

        #region Refresh

        /// <summary>
        /// Refresh.
        /// </summary>
        public void Refresh()
        {
            LoadCoupons();
        }

        #endregion

        #region For Borrow/Return between Stock-Lane

        /// <summary>
        /// Borrow from Stock back to Lane User.
        /// </summary>
        /// <param name="item"></param>
        public void Borrow(TSBCouponItem item)
        {
            if (null == item || null == User) return;

            // Not allow if original is SoldByLane or SoldByTSB
            if (item.Transaction.TransactionType == TSBCouponTransactionTypes.SoldByLane ||
                item.Transaction.TransactionType == TSBCouponTransactionTypes.SoldByTSB) return;

            item.TransactionType = TSBCouponTransactionTypes.Lane;
            item.UserId = User.UserId;
            item.FullNameEN = User.FullNameEN;
            item.FullNameTH = User.FullNameTH;
            item.ReceiveDate = new DateTime?(DateTime.Now);
        }
        /// <summary>
        /// Retrun from Lane back to stock.
        /// </summary>
        /// <param name="item"></param>
        public void Return(TSBCouponItem item)
        {
            if (null == item || null == User) return;

            // Not allow if original is SoldByLane or SoldByTSB
            if (item.Transaction.TransactionType == TSBCouponTransactionTypes.SoldByLane ||
                item.Transaction.TransactionType == TSBCouponTransactionTypes.SoldByTSB) return;

            item.TransactionType = TSBCouponTransactionTypes.Stock;
            item.UserId = null;
            item.FullNameEN = null;
            item.FullNameTH = null;
            item.ReceiveDate = new DateTime?();
        }

        #endregion

        #endregion

        #region Public Properties

        #region C35

        /// <summary>Gets or set 35 BHT Coupon Filter.</summary>
        public string C35StockFilter { get; set; }
        /// <summary>Gets all 35 BHT Coupons in Stock of current TSB.</summary>
        public List<TSBCouponItem> C35Stocks 
        {
            get 
            {
                if (null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = false;
                    if (string.IsNullOrEmpty(C35StockFilter))
                    {
                        ret = (
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT35
                        );
                    }
                    else
                    {
                        ret = (
                            item.CouponId.Contains(C35StockFilter) &&
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT35
                        );
                    }
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }
        /// <summary>Gets all 35 BHT Coupons on Lane by current User.</summary>
        public List<TSBCouponItem> C35Lanes
        {
            get
            {
                if (null == User || null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = (
                        (item.TransactionType == TSBCouponTransactionTypes.Lane || 
                        item.TransactionType == TSBCouponTransactionTypes.SoldByLane) &&
                        item.UserId == User.UserId &&
                        item.CouponType == CouponType.BHT35
                    );
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }

        #endregion

        #region C40

        /// <summary>Gets or set 40 BHT Coupon Filter.</summary>
        public string C40StockFilter { get; set; }
        /// <summary>Gets all 40 BHT Coupons in Stock of current TSB.</summary>
        public List<TSBCouponItem> C40Stocks
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = false;
                    if (string.IsNullOrEmpty(C40StockFilter))
                    {
                        ret = (
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT40
                        );
                    }
                    else
                    {
                        ret = (
                            item.CouponId.Contains(C40StockFilter) &&
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT40
                        );
                    }
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }
        /// <summary>Gets all 35 BHT Coupons on Lane by current User.</summary>
        public List<TSBCouponItem> C40Lanes
        {
            get
            {
                if (null == User || null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = (
                        (item.TransactionType == TSBCouponTransactionTypes.Lane ||
                        item.TransactionType == TSBCouponTransactionTypes.SoldByLane) &&
                        item.UserId == User.UserId &&
                        item.CouponType == CouponType.BHT40
                    );
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }

        #endregion

        #region C80

        /// <summary>Gets or set 80 BHT Coupon Filter.</summary>
        public string C80StockFilter { get; set; }
        /// <summary>Gets all 80 BHT Coupons in Stock of current TSB.</summary>
        public List<TSBCouponItem> C80Stocks
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = false;
                    if (string.IsNullOrEmpty(C80StockFilter))
                    {
                        ret = (
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT80
                        );
                    }
                    else
                    {
                        ret = (
                            item.CouponId.Contains(C80StockFilter) &&
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT80
                        );
                    }
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }
        /// <summary>Gets all 80 BHT Coupons on Lane by current User.</summary>
        public List<TSBCouponItem> C80Lanes
        {
            get
            {
                if (null == User || null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = (
                        (item.TransactionType == TSBCouponTransactionTypes.Lane ||
                        item.TransactionType == TSBCouponTransactionTypes.SoldByLane) &&
                        item.UserId == User.UserId &&
                        item.CouponType == CouponType.BHT80
                    );
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }

        #endregion

        #region C90

        /// <summary>Gets or set 90 BHT Coupon Filter.</summary>
        public string C90StockFilter { get; set; }
        /// <summary>Gets all 90 BHT Coupons in Stock of current TSB.</summary>
        public List<TSBCouponItem> C90Stocks
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = false;
                    if (string.IsNullOrEmpty(C90StockFilter))
                    {
                        ret = (
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT90
                        );
                    }
                    else
                    {
                        ret = (
                            item.CouponId.Contains(C90StockFilter) &&
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT90
                        );
                    }
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }
        /// <summary>Gets all 90 BHT Coupons on Lane by current User.</summary>
        public List<TSBCouponItem> C90Lanes
        {
            get
            {
                if (null == User || null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = (
                        (item.TransactionType == TSBCouponTransactionTypes.Lane ||
                        item.TransactionType == TSBCouponTransactionTypes.SoldByLane) &&
                        item.UserId == User.UserId &&
                        item.CouponType == CouponType.BHT90
                    );
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }

        #endregion

        #endregion
    }

    #endregion

    #region TSBCouponReturnManager

    /// <summary>
    /// TSB Coupon Return Manager class.
    /// </summary>
    public class TSBCouponReturnManager : TSBCouponManager
    {
        #region Override Methods

        /// <summary>
        /// On Load Coupons.
        /// </summary>
        protected override void OnLoadCoupons()
        {
            if (null == Coupons) return;
        }
        /// <summary>
        /// Save.
        /// </summary>
        /// <returns>Returns true if sace success.</returns>
        public override bool Save()
        {
            if (null == Coupons || Coupons.Count <= 0) return true;

            MethodBase med = MethodBase.GetCurrentMethod();

            string msg = string.Empty;

            msg = string.Format("Begin Save: {0:HH:mm:ss.fff}", DateTime.Now);
            med.Info(msg);
            Console.WriteLine(msg);

            List<TSBCouponTransaction> saveList = new List<TSBCouponTransaction>();

            Coupons.ForEach(coupon =>
            {
                if (null != coupon.Transaction && coupon.Commit())
                {
                    saveList.Add(coupon.Transaction); // keep transaction to save later.
                }
            });

            if (null != saveList && saveList.Count > 0)
            {
                msg = string.Format("Update local db start: {0:HH:mm:ss.fff}", DateTime.Now);
                med.Info(msg);
                Console.WriteLine(msg);

                TSBCouponTransaction.SaveTransactions(saveList);

                msg = string.Format("Update local db finished: {0:HH:mm:ss.fff}", DateTime.Now);
                med.Info(msg);
                Console.WriteLine(msg);

                msg = string.Format("generate server queue count: {0}", saveList.Count);
                med.Info(msg);
                Console.WriteLine(msg);

                msg = string.Format("generate server queue start: {0:HH:mm:ss.fff}", DateTime.Now);
                med.Info(msg);
                Console.WriteLine(msg);

                saveList.ForEach(localTran =>
                {
                    // Remove all from Coupon Borrow Table
                    TSBCouponBorrowStatus.Return(localTran.CouponId);

                    // Write Queue
                    TAServerCouponTransaction tran = localTran.ToServer();
                    if (null != tran)
                    {
                        TAxTODMQService.Instance.WriteQueue(tran);
                    }
                });

                msg = string.Format("generate server queue finished: {0:HH:mm:ss.fff}", DateTime.Now);
                med.Info(msg);
                Console.WriteLine(msg);
            }

            msg = string.Format("Finished Save: {0:HH:mm:ss.fff}", DateTime.Now);
            med.Info(msg);
            Console.WriteLine(msg);

            return true;
        }

        #endregion

        #region Public Methods

        #region For UnsoldByLane/SoldByLane Stock-Lane

        /// <summary>
        /// Set As Unsold by lane for return back to Stock.
        /// </summary>
        /// <param name="item"></param>
        public void UnsoldByLane(TSBCouponItem item)
        {
            if (null == item || null == item.Transaction || null == User) return;
            // Not allow if original is sold or soldbylant
            if (item.Transaction.TransactionType != TSBCouponTransactionTypes.Lane &&
                item.Transaction.TransactionType != TSBCouponTransactionTypes.SoldByLane) return;

            item.TransactionType = TSBCouponTransactionTypes.Lane;
            item.SoldBy = null;
            item.SoldByFullNameEN = null;
            item.SoldByFullNameTH = null;
            item.SoldDate = new DateTime?();
            // User information should not change until save call.
            /*
            item.UserId = User.UserId;
            item.FullNameEN = User.FullNameEN;
            item.FullNameTH = User.FullNameTH;
            item.ReceiveDate = DateTime.Now;
            */
        }
        /// <summary>
        /// Set As Sold By Lane.
        /// </summary>
        /// <param name="item"></param>
        public void SoldByLane(TSBCouponItem item)
        {
            if (null == item || null == item.Transaction || null == User) return;
            // Not allow if original not on lane or soldbylant
            if (item.Transaction.TransactionType != TSBCouponTransactionTypes.Lane &&
                item.Transaction.TransactionType != TSBCouponTransactionTypes.SoldByLane) return;

            item.TransactionType = TSBCouponTransactionTypes.SoldByLane;
            // User information should not change until save call.
            /*
            item.UserId = null;
            item.FullNameEN = null;
            item.FullNameTH = null;
            item.ReceiveDate = new DateTime?();
            */
            item.SoldBy = User.UserId;
            item.SoldByFullNameEN = User.FullNameEN;
            item.SoldByFullNameTH = User.FullNameTH;
            item.SoldDate = new DateTime?(DateTime.Now);
        }

        #endregion

        #region MarkAsCompleted/ReturnToStock

        public void MarkCompleted()
        {
            if (null == Coupons) return;
            Coupons.ForEach(item =>
            {
                if (item.UserId != User.UserId) 
                    return; // Not coupon for current user.
                if (item.TransactionType != TSBCouponTransactionTypes.SoldByLane)
                    return;
                item.FinishFlag = 0;
            });
        }

        public void ReturnToStock()
        {
            if (null == Coupons) return;
            Coupons.ForEach(item =>
            {
                if (item.UserId != User.UserId)
                    return; // Not coupon for current user.
                if (item.TransactionType != TSBCouponTransactionTypes.Lane)
                    return;
                item.TransactionType = TSBCouponTransactionTypes.Stock;
                item.UserId = null;
                item.FullNameEN = null;
                item.FullNameTH = null;
                item.ReceiveDate = new DateTime?();
            });
        }

        #endregion

        #region Refresh

        /// <summary>
        /// Refresh.
        /// </summary>
        public void Refresh()
        {
            LoadCoupons();
        }

        #endregion

        #endregion

        #region Public Properties

        #region C35

        /// <summary>Gets or set 35 BHT Coupon Sold By Lane Filter.</summary>
        public string C35SoldByLaneFilter { get; set; }
        /// <summary>Gets all 35 BHT Coupons in Sold by current user.</summary>
        public List<TSBCouponItem> C35UserSolds
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0 || null == User)
                    return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = false;
                    if (string.IsNullOrEmpty(C35SoldByLaneFilter))
                    {
                        ret = (
                            item.TransactionType == TSBCouponTransactionTypes.SoldByLane &&
                            item.CouponType == CouponType.BHT35 &&
                            item.UserId == User.UserId
                        );
                    }
                    else
                    {
                        ret = (
                            item.CouponId.Contains(C35SoldByLaneFilter) &&
                            item.TransactionType == TSBCouponTransactionTypes.SoldByLane &&
                            item.CouponType == CouponType.BHT35 &&
                            item.UserId == User.UserId
                        );
                    }
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }
        /// <summary>Gets all 35 BHT Coupons on Lane by current User.</summary>
        public List<TSBCouponItem> C35UserOnHands
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0 || null == User)
                    return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = (
                        item.TransactionType == TSBCouponTransactionTypes.Lane &&
                        item.CouponType == CouponType.BHT35 &&
                        item.UserId == User.UserId
                    );
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }

        #endregion

        #region C40

        /// <summary>Gets or set 35 BHT Coupon Sold By Lane Filter.</summary>
        public string C40SoldByLaneFilter { get; set; }
        /// <summary>Gets all 40 BHT Coupons in Sold by current user.</summary>
        public List<TSBCouponItem> C40UserSolds
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0 || null == User)
                    return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = false;
                    if (string.IsNullOrEmpty(C40SoldByLaneFilter))
                    {
                        ret = (
                            item.TransactionType == TSBCouponTransactionTypes.SoldByLane &&
                            item.CouponType == CouponType.BHT40 &&
                            item.UserId == User.UserId
                        );
                    }
                    else
                    {
                        ret = (
                            item.CouponId.Contains(C40SoldByLaneFilter) &&
                            item.TransactionType == TSBCouponTransactionTypes.SoldByLane &&
                            item.CouponType == CouponType.BHT40 &&
                            item.UserId == User.UserId
                        );
                    }
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }
        /// <summary>Gets all 40 BHT Coupons on Lane by current User.</summary>
        public List<TSBCouponItem> C40UserOnHands
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0 || null == User)
                    return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = (
                        item.TransactionType == TSBCouponTransactionTypes.Lane &&
                        item.CouponType == CouponType.BHT40 &&
                        item.UserId == User.UserId
                    );
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }

        #endregion

        #region C80

        /// <summary>Gets or set 80 BHT Coupon Sold By Lane Filter.</summary>
        public string C80SoldByLaneFilter { get; set; }
        /// <summary>Gets all 80 BHT Coupons in Sold by current user.</summary>
        public List<TSBCouponItem> C80UserSolds
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0 || null == User) 
                    return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = false;
                    if (string.IsNullOrEmpty(C80SoldByLaneFilter))
                    {
                        ret = (
                            item.TransactionType == TSBCouponTransactionTypes.SoldByLane &&
                            item.CouponType == CouponType.BHT80 &&
                            item.UserId == User.UserId
                        );
                    }
                    else
                    {
                        ret = (
                            item.CouponId.Contains(C80SoldByLaneFilter) &&
                            item.TransactionType == TSBCouponTransactionTypes.SoldByLane &&
                            item.CouponType == CouponType.BHT80 &&
                            item.UserId == User.UserId
                        );
                    }
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }
        /// <summary>Gets all 80 BHT Coupons on Lane by current User.</summary>
        public List<TSBCouponItem> C80UserOnHands
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0 || null == User)
                    return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = (
                        item.TransactionType == TSBCouponTransactionTypes.Lane &&
                        item.CouponType == CouponType.BHT80 &&
                            item.UserId == User.UserId
                    );
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }

        #endregion

        #region C90

        /// <summary>Gets or set 90 BHT Coupon Sold By Lane Filter.</summary>
        public string C90SoldByLaneFilter { get; set; }
        /// <summary>Gets all 90 BHT Coupons in Sold by current user.</summary>
        public List<TSBCouponItem> C90UserSolds
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0 || null == User)
                    return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = false;
                    if (string.IsNullOrEmpty(C90SoldByLaneFilter))
                    {
                        ret = (
                            item.TransactionType == TSBCouponTransactionTypes.SoldByLane &&
                            item.CouponType == CouponType.BHT90 &&
                            item.UserId == User.UserId
                        );
                    }
                    else
                    {
                        ret = (
                            item.CouponId.Contains(C90SoldByLaneFilter) &&
                            item.TransactionType == TSBCouponTransactionTypes.SoldByLane &&
                            item.CouponType == CouponType.BHT90 &&
                            item.UserId == User.UserId
                        );
                    }
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }
        /// <summary>Gets all 90 BHT Coupons on Lane by current User.</summary>
        public List<TSBCouponItem> C90UserOnHands
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0 || null == User)
                    return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = (
                        item.TransactionType == TSBCouponTransactionTypes.Lane &&
                        item.CouponType == CouponType.BHT90 &&
                            item.UserId == User.UserId
                    );
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }

        #endregion

        #endregion
    }

    #endregion

    #region TSBCouponSoldManager

    /// <summary>
    /// TSB Coupon Sold Manager class.
    /// </summary>
    public class TSBCouponSoldManager : TSBCouponManager
    {
        #region Override Methods

        /// <summary>
        /// Save.
        /// </summary>
        /// <returns>Returns true if save success.</returns>
        public override bool Save()
        {
            if (null == Coupons || Coupons.Count <= 0) return true;

            MethodBase med = MethodBase.GetCurrentMethod();

            string msg = string.Empty;

            msg = string.Format("Begin Save: {0:HH:mm:ss.fff}", DateTime.Now);
            med.Info(msg);
            Console.WriteLine(msg);

            List<TSBCouponTransaction> saveList = new List<TSBCouponTransaction>();
            Coupons.ForEach(coupon =>
            {
                if (null != coupon)
                {
                    if (coupon.TransactionType != TSBCouponTransactionTypes.SoldByTSB)
                        return;
                    coupon.FinishFlag = 0; // Mask as finished.
                    if (SoldDate.HasValue)
                    {
                        coupon.SoldDate = new DateTime?(SoldDate.Value);
                    }

                    if (null != coupon.Transaction && coupon.Commit())
                    {
                        saveList.Add(coupon.Transaction); // keep transaction to save later.
                    }    
                }
            });

            if (null != saveList && saveList.Count > 0)
            {
                // update invoice id
                var invoiceId = this.InvoiceId;
                saveList.ForEach(coupon => 
                {
                    coupon.TSBInvoiceId = invoiceId; // keep id in each coupon before save.
                });
                // increase TSB Coupon Sale invoice running number.
                UniqueCode.IncreaseUniqueId("TSBCouponSaleInvoice");

                msg = string.Format("Update local db start: {0:HH:mm:ss.fff}", DateTime.Now);
                med.Info(msg);
                Console.WriteLine(msg);

                TSBCouponTransaction.SaveTransactions(saveList);

                msg = string.Format("Update local db finished: {0:HH:mm:ss.fff}", DateTime.Now);
                med.Info(msg);
                Console.WriteLine(msg);

                msg = string.Format("generate server queue count: {0}", saveList.Count);
                med.Info(msg);
                Console.WriteLine(msg);

                msg = string.Format("generate server queue start: {0:HH:mm:ss.fff}", DateTime.Now);
                med.Info(msg);
                Console.WriteLine(msg);

                saveList.ForEach(localTran => 
                {
                    // Write Queue
                    TAServerCouponTransaction tran = localTran.ToServer();
                    if (null != tran)
                    {
                        TAxTODMQService.Instance.WriteQueue(tran);
                    }
                });

                msg = string.Format("generate server queue finished: {0:HH:mm:ss.fff}", DateTime.Now);
                med.Info(msg);
                Console.WriteLine(msg);
            }

            msg = string.Format("Finished Save: {0:HH:mm:ss.fff}", DateTime.Now);
            med.Info(msg);
            Console.WriteLine(msg);

            return true;
        }

        #endregion

        #region Public Methods

        #region Refresh

        /// <summary>
        /// Refresh.
        /// </summary>
        public void Refresh()
        {
            LoadCoupons();
        }

        #endregion

        #region For TSB Sold/Unsold between Stock-TSB

        /// <summary>
        /// Sold Coupon from Stock back to Sold (TSB).
        /// </summary>
        /// <param name="item"></param>
        public void Sold(TSBCouponItem item)
        {
            if (null == item || null == User) return;

            // Not allow if original is not Stock
            if (item.TransactionType != TSBCouponTransactionTypes.Stock) return;

            item.TransactionType = TSBCouponTransactionTypes.SoldByTSB;
            item.SoldBy = User.UserId;
            item.SoldByFullNameEN = User.FullNameEN;
            item.SoldByFullNameTH = User.FullNameTH;
            item.SoldDate = new DateTime?(DateTime.Now);

            //item.ReceiveDate = new DateTime?(); // make sure mark not has receive date.
        }
        /// <summary>
        /// Unsold coupon from Sold (TSB) back to stock.
        /// </summary>
        /// <param name="item"></param>
        public void Unsold(TSBCouponItem item)
        {
            if (null == item || null == User) return;

            // Not allow if original is not SoldByTSB
            if (item.TransactionType != TSBCouponTransactionTypes.SoldByTSB) return;

            item.TransactionType = TSBCouponTransactionTypes.Stock;
            item.SoldBy = null;
            item.SoldByFullNameEN = null;
            item.SoldByFullNameTH = null;
            item.SoldDate = new DateTime?();

            //item.ReceiveDate = new DateTime?(); // make sure mark not has receive date.
        }

        #endregion

        #endregion

        #region Public Properties

        #region C35

        /// <summary>Gets or set 35 BHT Coupon Filter.</summary>
        public string C35StockFilter { get; set; }
        /// <summary>Gets all 35 BHT Coupons in Stock of current TSB.</summary>
        public List<TSBCouponItem> C35Stocks
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = false;
                    if (string.IsNullOrEmpty(C35StockFilter))
                    {
                        ret = (
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT35
                        );
                    }
                    else
                    {
                        ret = (
                            item.CouponId.Contains(C35StockFilter) &&
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT35
                        );
                    }
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }
        /// <summary>Gets all 35 BHT Coupons on Sold.</summary>
        public List<TSBCouponItem> C35Solds
        {
            get
            {
                if (null == User || null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = (
                        item.TransactionType == TSBCouponTransactionTypes.SoldByTSB &&
                        //item.UserId == User.UserId &&
                        item.CouponType == CouponType.BHT35
                    );
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }

        #endregion

        #region C40

        /// <summary>Gets or set 40 BHT Coupon Filter.</summary>
        public string C40StockFilter { get; set; }
        /// <summary>Gets all 40 BHT Coupons in Stock of current TSB.</summary>
        public List<TSBCouponItem> C40Stocks
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = false;
                    if (string.IsNullOrEmpty(C40StockFilter))
                    {
                        ret = (
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT40
                        );
                    }
                    else
                    {
                        ret = (
                            item.CouponId.Contains(C40StockFilter) &&
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT40
                        );
                    }
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }
        /// <summary>Gets all 40 BHT Coupons on Sold.</summary>
        public List<TSBCouponItem> C40Solds
        {
            get
            {
                if (null == User || null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = (
                        item.TransactionType == TSBCouponTransactionTypes.SoldByTSB &&
                        //item.UserId == User.UserId &&
                        item.CouponType == CouponType.BHT40
                    );
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }

        #endregion

        #region C80

        /// <summary>Gets or set 80 BHT Coupon Filter.</summary>
        public string C80StockFilter { get; set; }
        /// <summary>Gets all 80 BHT Coupons in Stock of current TSB.</summary>
        public List<TSBCouponItem> C80Stocks
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = false;
                    if (string.IsNullOrEmpty(C80StockFilter))
                    {
                        ret = (
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT80
                        );
                    }
                    else
                    {
                        ret = (
                            item.CouponId.Contains(C80StockFilter) &&
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT80
                        );
                    }
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }
        /// <summary>Gets all 80 BHT Coupons on Sold.</summary>
        public List<TSBCouponItem> C80Solds
        {
            get
            {
                if (null == User || null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = (
                        item.TransactionType == TSBCouponTransactionTypes.SoldByTSB &&
                        //item.UserId == User.UserId &&
                        item.CouponType == CouponType.BHT80
                    );
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }

        #endregion

        #region C90

        /// <summary>Gets or set 90 BHT Coupon Filter.</summary>
        public string C90StockFilter { get; set; }
        /// <summary>Gets all 90 BHT Coupons in Stock of current TSB.</summary>
        public List<TSBCouponItem> C90Stocks
        {
            get
            {
                if (null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = false;
                    if (string.IsNullOrEmpty(C90StockFilter))
                    {
                        ret = (
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT90
                        );
                    }
                    else
                    {
                        ret = (
                            item.CouponId.Contains(C90StockFilter) &&
                            item.TransactionType == TSBCouponTransactionTypes.Stock &&
                            item.CouponType == CouponType.BHT90
                        );
                    }
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }
        /// <summary>Gets all 90 BHT Coupons on Sold.</summary>
        public List<TSBCouponItem> C90Solds
        {
            get
            {
                if (null == User || null == Coupons || Coupons.Count <= 0) return new List<TSBCouponItem>();

                var results = Coupons.FindAll(item =>
                {
                    bool ret = (
                        item.TransactionType == TSBCouponTransactionTypes.SoldByTSB &&
                        //item.UserId == User.UserId &&
                        item.CouponType == CouponType.BHT90
                    );
                    return ret;
                }).OrderBy(x => x.CouponId).ToList();

                return results;
            }
        }

        #endregion

        #region Invoice/SoldDate

        /// <summary>
        /// Gets current TSB Coupon sold invoice id.
        /// </summary>
        public string InvoiceId
        {
            get 
            {
                var unique = UniqueCode.GetUniqueId("TSBCouponSaleInvoice").Value();
                string yr = DateTime.Now.ToThaiDateTimeString("yy");
                string autoId = (null != unique) ? yr + unique.LastNumber.ToString("D5") : string.Empty; // auto generate.
                return autoId;
            }
        }
        /// <summary>
        /// Gets or sets Sold Date.
        /// </summary>
        public DateTime? SoldDate { get; set; }

        #endregion

        #endregion
    }

    #endregion

    #region TSBCouponEditManager (static)

    public static class TSBCouponEditManager
    {
        public static List<TSBCouponTransaction> GetSoldCoupon35s(DateTime soldDate)
        {
            var coupons = TSBCouponTransaction.GetTSBCoupon35sSoldByDate(TAAPI.TSB, soldDate).Value();             
            return coupons;
        }
        public static List<TSBCouponTransaction> GetSoldCoupon40s(DateTime soldDate)
        {
            var coupons = TSBCouponTransaction.GetTSBCoupon40sSoldByDate(TAAPI.TSB, soldDate).Value();
            return coupons;
        }
        public static List<TSBCouponTransaction> GetSoldCoupon80s(DateTime soldDate)
        {
            var coupons = TSBCouponTransaction.GetTSBCoupon80sSoldByDate(TAAPI.TSB, soldDate).Value();
            return coupons;
        }
        public static List<TSBCouponTransaction> GetSoldCoupon90s(DateTime soldDate)
        {
            var coupons = TSBCouponTransaction.GetTSBCoupon90sSoldByDate(TAAPI.TSB, soldDate).Value();
            return coupons;
        }
        public static List<TSBCouponTransaction> GetStockCoupon35s()
        {
            var coupons = TSBCouponTransaction.GetTSBCoupon35sStock(TAAPI.TSB).Value();
            return coupons;
        }
        public static List<TSBCouponTransaction> GetStockCoupon40s()
        {
            var coupons = TSBCouponTransaction.GetTSBCoupon40sStock(TAAPI.TSB).Value();
            return coupons;
        }
        public static List<TSBCouponTransaction> GetStockCoupon80s()
        {
            var coupons = TSBCouponTransaction.GetTSBCoupon80sStock(TAAPI.TSB).Value();
            return coupons;
        }
        public static List<TSBCouponTransaction> GetStockCoupon90s()
        {
            var coupons = TSBCouponTransaction.GetTSBCoupon90sStock(TAAPI.TSB).Value();
            return coupons;
        }
        public static void VoidCoupon(TSBCouponTransaction tran)
        {
            if (null == tran) return;

            tran.TransactionType = TSBCouponTransactionTypes.Stock;
            tran.UserId = null;
            tran.FullNameEN = null;
            tran.FullNameTH = null;
            tran.UserReceiveDate = null;
            // clear laneid
            tran.LaneId = null;
            tran.SoldBy = null;
            tran.SoldByFullNameEN = null;
            tran.SoldByFullNameTH = null;
            tran.SoldDate = new DateTime?();
            tran.FinishFlag = TSBCouponFinishedFlags.Avaliable;
            tran.TSBInvoiceId = null; // clear invoice id
            TSBCouponTransaction.SaveTransaction(tran);

            // Write Queue
            TAServerCouponTransaction item = tran.ToServer();
            if (null != item)
            {
                TAxTODMQService.Instance.WriteQueue(item);
            }
        }

        public static void SwapCoupon(TSBCouponTransaction src, TSBCouponTransaction dst)
        {
            if (null == src || null == dst) return;
            // Dest => Update data from source
            dst.TransactionType = src.TransactionType;
            dst.UserId = src.UserId;
            dst.FullNameEN = src.FullNameEN;
            dst.FullNameTH = src.FullNameTH;
            // TSB and PlazaGroup.
            dst.TSBId = src.TSBId;
            dst.TSBNameEN = src.TSBNameEN;
            dst.TSBNameTH = src.TSBNameTH;
            dst.PlazaGroupId = src.PlazaGroupId;
            dst.PlazaGroupNameEN = src.PlazaGroupNameEN;
            dst.PlazaGroupNameTH = src.PlazaGroupNameTH;
            // User Received date.
            dst.UserReceiveDate = src.UserReceiveDate;
            // update laneid
            dst.LaneId = src.LaneId;

            dst.SoldBy = src.SoldBy;
            dst.SoldByFullNameEN = src.SoldByFullNameEN;
            dst.SoldByFullNameTH = src.SoldByFullNameTH;
            dst.SoldDate = src.SoldDate;
            dst.FinishFlag = src.FinishFlag;
            dst.TSBInvoiceId = src.TSBInvoiceId;
            //dst.LastUpdate = src.LastUpdate;
            TSBCouponTransaction.SaveTransaction(dst);

            // Write Queue
            /*
            TAServerCouponTransaction item1 = dst.ToServer();
            if (null != item1)
            {
                TAxTODMQService.Instance.WriteQueue(item1);
            }
            */

            // Source => to stock
            src.TransactionType = TSBCouponTransactionTypes.Stock;
            src.UserId = null;
            src.FullNameEN = null;
            src.FullNameTH = null;
            src.UserReceiveDate = null;
            src.SoldBy = null;
            src.SoldByFullNameEN = null;
            src.SoldByFullNameTH = null;
            src.SoldDate = new DateTime?();
            src.FinishFlag = TSBCouponFinishedFlags.Avaliable;
            src.TSBInvoiceId = null; // clear invoice id
            // clear laneid
            src.LaneId = null;
            TSBCouponTransaction.SaveTransaction(src);

            // Write Queue
            /*
            TAServerCouponTransaction item2 = src.ToServer();
            if (null != item2)
            {
                TAxTODMQService.Instance.WriteQueue(item2);
            }
            */

            TAAEditserialno exch1 = new TAAEditserialno();
            exch1.TSBId = src.TSBId;
            exch1.Oldserialno = src.CouponId;
            exch1.Newserialno = dst.CouponId;
            // Write Queue
            TAxTODMQService.Instance.WriteQueue(exch1);
        }
    }

    #endregion

    #endregion

    #region Exchange Manager related classes

    #region InternalExchangeManager

    /// <summary>
    /// The Internal Exchange Manager class. 
    /// Used in InternalExchangePage to get list of internal exchange transactions (TSBCreditTransaction).
    /// </summary>
    public class InternalExchangeManager
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public InternalExchangeManager() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~InternalExchangeManager() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Refresh.
        /// </summary>
        public void Refresh()
        {
            if (!SearchDate.HasValue || SearchDate == DateTime.MinValue)
            {
                Transactions = new List<TSBCreditTransaction>();
                return;
            }
            Transactions = TSBCreditTransaction.GetReplaceTransactions(
                SearchDate.Value).Value();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets TSBCreditTransaction List for internal exchanges.
        /// </summary>
        public List<TSBCreditTransaction> Transactions { get; private set; }
        /// <summary>
        /// Gets or sets Search Date.
        /// </summary>
        public DateTime? SearchDate { get; set; }

        #endregion
    }

    #endregion

    #region TSBReplaceCreditManager

    /// <summary>
    /// TSBReplaceCreditManager class. 
    /// Used in InternalExchangeWindow to update credit replace transactions (in/out TSBCreditTransaction).
    /// </summary>
    public class TSBReplaceCreditManager
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBReplaceCreditManager() : base()
        {
            this.ReplaceIn = new TSBCreditTransaction();
            this.ReplaceIn.HasRemark = true;
            this.ReplaceOut = new TSBCreditTransaction();
            this.ReplaceOut.HasRemark = true;
        }

        #endregion

        #region Private Methods

        private bool CheckCanReplaceOut()
        {
            bool ret = false;

            if (null == this.TSB) return ret;
            if (null == this.ReplaceOut) return ret;
            var tsbBal = TSBCreditBalance.GetCurrent(TAAPI.TSB).Value();
            if (null == tsbBal) return ret;

            if (this.ReplaceOut.AmountST25 > tsbBal.AmountST25) return ret;
            if (this.ReplaceOut.AmountST50 > tsbBal.AmountST50) return ret;
            if (this.ReplaceOut.AmountBHT1 > tsbBal.AmountBHT1) return ret;
            if (this.ReplaceOut.AmountBHT2 > tsbBal.AmountBHT2) return ret;
            if (this.ReplaceOut.AmountBHT5 > tsbBal.AmountBHT5) return ret;
            if (this.ReplaceOut.AmountBHT10 > tsbBal.AmountBHT10) return ret;
            if (this.ReplaceOut.AmountBHT20 > tsbBal.AmountBHT20) return ret;
            if (this.ReplaceOut.AmountBHT50 > tsbBal.AmountBHT50) return ret;
            if (this.ReplaceOut.AmountBHT100 > tsbBal.AmountBHT100) return ret;
            if (this.ReplaceOut.AmountBHT500 > tsbBal.AmountBHT500) return ret;
            if (this.ReplaceOut.AmountBHT1000 > tsbBal.AmountBHT1000) return ret;

            ret = true; // all ok.

            return ret;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Save Internal Replace TSB Credit (in/out)
        /// </summary>
        public void Save()
        {
            if (null == this.TSB) return;
            if (null == this.ReplaceOut || null == this.ReplaceIn) return;
            // set group Id and TSB id.
            Guid groupId = Guid.NewGuid();
            DateTime dt = DateTime.Now;

            this.ReplaceOut.TSBId = this.TSB.TSBId;
            this.ReplaceOut.GroupId = groupId;
            this.ReplaceOut.TransactionDate = dt;
            this.ReplaceOut.SupervisorId = this.Supervisor.UserId;
            this.ReplaceOut.SupervisorNameEN = this.Supervisor.FullNameEN;
            this.ReplaceOut.SupervisorNameTH = this.Supervisor.FullNameTH;
            this.ReplaceOut.TransactionType = TSBCreditTransaction.TransactionTypes.ReplaceOut;

            this.ReplaceIn.TSBId = this.TSB.TSBId;
            this.ReplaceIn.GroupId = groupId;
            this.ReplaceIn.TransactionDate = dt;
            this.ReplaceIn.SupervisorId = this.Supervisor.UserId;
            this.ReplaceIn.SupervisorNameEN = this.Supervisor.FullNameEN;
            this.ReplaceIn.SupervisorNameTH = this.Supervisor.FullNameTH;
            this.ReplaceIn.TransactionType = TSBCreditTransaction.TransactionTypes.ReplaceIn;

            TSBCreditTransaction.SaveTransaction(this.ReplaceOut);
            TSBCreditTransaction.SaveTransaction(this.ReplaceIn);

            // Find current TSB balance.
            var tsbBal = TSBCreditBalance.GetCurrent(TAAPI.TSB).Value();
            if (null != tsbBal)
            {
                // For Update TSB balance
                var tsbCdt = new TAATSBCredit();
                tsbCdt.TSBId = tsbBal.TSBId;
                tsbCdt.Amnt1 = tsbBal.AmountBHT1;
                tsbCdt.Amnt2 = tsbBal.AmountBHT2;
                tsbCdt.Amnt5 = tsbBal.AmountBHT5;
                tsbCdt.Amnt10 = tsbBal.AmountBHT10;
                tsbCdt.Amnt20 = tsbBal.AmountBHT20;
                tsbCdt.Amnt50 = tsbBal.AmountBHT50;
                tsbCdt.Amnt100 = tsbBal.AmountBHT100;
                tsbCdt.Amnt500 = tsbBal.AmountBHT500;
                tsbCdt.Amnt1000 = tsbBal.AmountBHT1000;
                tsbCdt.Remark = null;
                tsbCdt.Updatedate = DateTime.Now;
                TAxTODMQService.Instance.WriteQueue(tsbCdt);
            }
        }

        #endregion

        #region Public Properties

        #region TSB

        /// <summary>
        /// Gets Current TSB.
        /// </summary>
        public TSB TSB { get; set; }
        /// <summary>
        /// Gets Supervisor.
        /// </summary>
        public User Supervisor { get; set; }

        #endregion

        #region Replace In/Out

        /// <summary>
        /// Gets Replace Out.
        /// </summary>
        public TSBCreditTransaction ReplaceOut { get; private set; }
        /// <summary>
        /// Gets Replace In.
        /// </summary>
        public TSBCreditTransaction ReplaceIn { get; private set; }
        /// <summary>
        /// Check can replace out (check is amount in TSB is enough?).
        /// </summary>
        public bool CanReplaceOut
        {
            get { return CheckCanReplaceOut(); }
            private set { }
        }

        #endregion

        #region Checks Is Equal amount

        /// <summary>
        /// Checks Is Equal amount.
        /// </summary>
        public bool IsEquals
        {
            get
            {
                return (this.ReplaceOut.BHTTotal == this.ReplaceIn.BHTTotal);
            }
        }

        #endregion

        #endregion
    }

    #endregion

    #region TSBRequestCreditManager

    /// <summary>
    /// TSBRequestCreditManager class.
    /// Used in RequestExchangeWindow to Request credit exchange transaction.
    /// </summary>
    public class TSBRequestCreditManager : INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBRequestCreditManager() : base()
        {
            
        }

        #endregion

        #region Utils class

        public class Utils
        {
            public static TAAExchangeHeader CreateRequestExchangeHeader(TSBExchangeGroup group, TSBExchangeTransaction req)
            {
                if (null == group || null == req)
                    return null;

                TAAExchangeHeader ret = new TAAExchangeHeader();

                ret.TSBId = req.TSBId;
                ret.UserId = req.UserId;

                ret.RequestId = group.PkId;
                ret.TranactionDate = req.TransactionDate;

                ret.ExchangeBHT = req.ExchangeBHT;
                ret.BorrowBHT = req.BorrowBHT;
                ret.AdditionalBHT = req.AdditionalBHT;

                ret.PeriodBegin = req.PeriodBegin;
                ret.PeriodEnd = req.PeriodEnd;

                ret.Remark = req.Remark;

                //ret.FinishFlag = 1; // completed.
                //ret.Status = "C"; // Cancel
                return ret;
            }
            public static List<TAARequestExchangeItem> CreateRequestExchangeDetails(TSBExchangeGroup group, 
                TSBExchangeTransaction req)
            {
                if (null == group || null == req)
                    return null;

                List<TAARequestExchangeItem> rets = new List<TAARequestExchangeItem>();

                List<MCurrency> currencies = MCurrency.GetCurrencies().Value();
                currencies.ForEach(currency =>
                {
                    if (currency.currencyDenomId == 7)
                        return; // ignore 10BHT note

                    var item = new TAARequestExchangeItem();
                    item.TSBId = req.TSBId;
                    item.RequestId = group.PkId;
                    item.CurrencyDenomId = currency.currencyDenomId;

                    if (currency.denomValue == (decimal)0.25)
                    {
                        item.CurrencyValue = req.AmountST25;
                        item.CurrencyCount = req.CountST25;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == (decimal)0.5)
                    {
                        item.CurrencyValue = req.AmountST50;
                        item.CurrencyCount = req.CountST50;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 1)
                    {
                        item.CurrencyValue = req.AmountBHT1;
                        item.CurrencyCount = req.CountBHT1;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 2)
                    {
                        item.CurrencyValue = req.AmountBHT2;
                        item.CurrencyCount = req.CountBHT2;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 5)
                    {
                        item.CurrencyValue = req.AmountBHT5;
                        item.CurrencyCount = req.CountBHT5;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 10)
                    {
                        item.CurrencyValue = req.AmountBHT10;
                        item.CurrencyCount = req.CountBHT10;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 20)
                    {
                        item.CurrencyValue = req.AmountBHT20;
                        item.CurrencyCount = req.CountBHT20;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 50)
                    {
                        item.CurrencyValue = req.AmountBHT50;
                        item.CurrencyCount = req.CountBHT50;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 100)
                    {
                        item.CurrencyValue = req.AmountBHT100;
                        item.CurrencyCount = req.CountBHT100;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 500)
                    {
                        item.CurrencyValue = req.AmountBHT500;
                        item.CurrencyCount = req.CountBHT500;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 1000)
                    {
                        item.CurrencyValue = req.AmountBHT1000;
                        item.CurrencyCount = req.CountBHT1000;
                        rets.Add(item);
                    }
                });
                return rets;
            }

            public static List<TAAReceiveExchangeItem> CreateReceiveExchangeDetails(TSBExchangeGroup group,
                TSBExchangeTransaction recv)
            {
                if (null == group || null == recv)
                    return null;

                List<TAAReceiveExchangeItem> rets = new List<TAAReceiveExchangeItem>();

                List<MCurrency> currencies = MCurrency.GetCurrencies().Value();
                currencies.ForEach(currency =>
                {
                    if (currency.currencyDenomId == 7)
                        return; // ignore 10BHT note

                    var item = new TAAReceiveExchangeItem();
                    item.TSBId = recv.TSBId;
                    item.RequestId = group.PkId;
                    item.CurrencyDenomId = currency.currencyDenomId;

                    if (currency.denomValue == (decimal)0.25)
                    {
                        item.CurrencyValue = recv.AmountST25;
                        item.CurrencyCount = recv.CountST25;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == (decimal)0.5)
                    {
                        item.CurrencyValue = recv.AmountST50;
                        item.CurrencyCount = recv.CountST50;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 1)
                    {
                        item.CurrencyValue = recv.AmountBHT1;
                        item.CurrencyCount = recv.CountBHT1;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 2)
                    {
                        item.CurrencyValue = recv.AmountBHT2;
                        item.CurrencyCount = recv.CountBHT2;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 5)
                    {
                        item.CurrencyValue = recv.AmountBHT5;
                        item.CurrencyCount = recv.CountBHT5;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 10)
                    {
                        item.CurrencyValue = recv.AmountBHT10;
                        item.CurrencyCount = recv.CountBHT10;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 20)
                    {
                        item.CurrencyValue = recv.AmountBHT20;
                        item.CurrencyCount = recv.CountBHT20;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 50)
                    {
                        item.CurrencyValue = recv.AmountBHT50;
                        item.CurrencyCount = recv.CountBHT50;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 100)
                    {
                        item.CurrencyValue = recv.AmountBHT100;
                        item.CurrencyCount = recv.CountBHT100;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 500)
                    {
                        item.CurrencyValue = recv.AmountBHT500;
                        item.CurrencyCount = recv.CountBHT500;
                        rets.Add(item);
                    }
                    else if (currency.denomValue == 1000)
                    {
                        item.CurrencyValue = recv.AmountBHT1000;
                        item.CurrencyCount = recv.CountBHT1000;
                        rets.Add(item);
                    }
                });
                return rets;
            }
        }

        #endregion

        #region Private Methods

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks is both total amount is match.
        /// </summary>
        /// <returns>Returns true if match.</returns>
        public bool IsMatchAmount()
        {
            bool ret = false;

            if (null != this.Request)
            {
                decimal detailTotal = this.Request.GrandTotalBHT;
                decimal creditTotal = this.Request.BHTTotal;
                ret = (detailTotal == creditTotal);
            }

            return ret;
        }
        /// <summary>
        /// Checks has period begin/end date.
        /// </summary>
        /// <returns>Returns true if period begin/end date assigned (if required)</returns>
        public bool CheckPeriod()
        {
            bool ret = false;

            if (null != this.Request)
            {
                if (this.Request.AdditionalBHT != decimal.Zero || this.Request.BorrowBHT != decimal.Zero)
                {
                    ret = (this.Request.PeriodBegin.HasValue && this.Request.PeriodEnd.HasValue);
                }
                else
                {
                    ret = true; // only exchange so no need period begin/end.
                }
            }

            return ret;
        }
        /// <summary>
        /// New Request.
        /// </summary>
        public void NewRequest() 
        {
            IsNew = true;
            this.Group = new TSBExchangeGroup();
            //this.Group.PkId; // auto number.
            this.Group.GroupId = Guid.NewGuid();
            this.Group.RequestDate = DateTime.Now;
            this.Group.RequestType = TSBExchangeGroup.RequestTypes.Account;
            this.Group.State = TSBExchangeGroup.StateTypes.Request;
            this.Group.FinishFlag = TSBExchangeGroup.FinishedFlags.Avaliable;
            this.Group.TSBId = this.TSB.TSBId;
            this.Group.TSBNameEN = this.TSB.TSBNameEN;
            this.Group.TSBNameTH = this.TSB.TSBNameTH;

            this.Request = new TSBExchangeTransaction();
            this.Request.TransactionType = TSBExchangeTransaction.TransactionTypes.Request;
            this.Request.TransactionDate = DateTime.Now;
            this.Request.Description = "แลกเปลี่ยนเงินยืม/ทอน";
            this.Request.GroupId = this.Group.GroupId;
            this.Request.TSBId = this.Group.TSBId;
            this.Request.TSBNameEN = this.Group.TSBNameEN;
            this.Request.TSBNameTH = this.Group.TSBNameTH;
            this.Request.UserId = this.SupervisorId;
            this.Request.FullNameEN = this.FullNameEN;
            this.Request.FullNameTH = this.FullNameTH;
        }
        /// <summary>
        /// Load Request.
        /// </summary>
        /// <param name="requestId">The request Id (PK).</param>
        /// <returns>Returns true if found group by request id.</returns>
        public bool LoadRequest(int requestId) 
        {
            IsNew = false;

            var grp = TSBExchangeGroup.GetTSBExchangeGroup(this.TSB, requestId).Value();
            if (null != grp)
            {
                this.Group = grp;
                // get request transaction.
                this.Request = TSBExchangeTransaction.GetTransaction(
                    this.TSB, grp.GroupId, TSBExchangeTransaction.TransactionTypes.Request).Value();
                this.Request.Description = "รายการขอแลกเงินจากด่าน";
                this.Request.HasRemark = true;
                this.Request.ShowExtendInfo = true;

                this.Approve = TSBExchangeTransaction.GetTransaction(
                    this.TSB, grp.GroupId, TSBExchangeTransaction.TransactionTypes.Approve).Value();
                if (null != this.Approve)
                {
                    this.Approve.Description = "รายการอนุมัติจากบัญชี";
                    this.Approve.HasRemark = true;
                    this.Approve.ShowExtendInfo = true;
                }

                var apprv = TSBExchangeTransaction.CloneTransaction(this.Approve);
                if (null != apprv)
                {
                    // prepare received transaction.
                    var recv = apprv;
                    recv.TransactionId = 0; // NEW
                    recv.TransactionDate = DateTime.Now;
                    recv.TransactionType = TSBExchangeTransaction.TransactionTypes.Received;
                    recv.UserId = TAApp.User.Current.UserId; // set cancel user.
                    recv.FullNameEN = TAApp.User.Current.FullNameEN;
                    recv.FullNameTH = TAApp.User.Current.FullNameTH;
                    recv.FinishFlag = TSBExchangeTransaction.FinishedFlags.Avaliable; // set finished flag.
                    recv.Description = "เงินที่ได้รับจริง";
                    recv.HasRemark = true;
                    recv.Remark = string.Empty;
                    recv.ShowExtendInfo = false;
                    this.Receive = recv;

                    // prepare exchange out (return to account dept) transaction.
                    var exchange = new TSBExchangeTransaction();
                    exchange.TransactionId = 0; // NEW
                    exchange.TransactionDate = DateTime.Now;
                    exchange.TransactionType = TSBExchangeTransaction.TransactionTypes.Exchange;
                    exchange.UserId = TAApp.User.Current.UserId; // set cancel user.
                    exchange.FullNameEN = TAApp.User.Current.FullNameEN;
                    exchange.FullNameTH = TAApp.User.Current.FullNameTH;
                    exchange.FinishFlag = TSBExchangeTransaction.FinishedFlags.Avaliable; // set finished flag.
                    exchange.Description = "จ่ายออก ธนบัตร/เหรียญ";
                    exchange.HasRemark = true;
                    exchange.Remark = string.Empty;
                    exchange.ShowExtendInfo = false;
                    this.ExchangeOut = exchange;
                }
                else
                {
                    this.Approve = null;
                    this.Receive = null;
                    this.ExchangeOut = null;
                }
            }
            else
            {
                this.Group = null;
                this.Request = null;
                this.Approve = null;
                this.Receive = null;
                this.ExchangeOut = null;
            }

            return (grp != null);
        }
        /// <summary>
        /// Cancel Request
        /// </summary>
        public void CancelRequest()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null != this.Group && null != this.Request && this.Group.State == TSBExchangeGroup.StateTypes.Request)
            {
                string msg = string.Empty;

                // change group flag.
                this.Group.State = TSBExchangeGroup.StateTypes.Canceled;
                this.Group.FinishFlag = TSBExchangeGroup.FinishedFlags.Completed;


                // clone transaction from Request transaction.
                var cancelTran = TSBExchangeTransaction.CloneTransaction(this.Request);
                // update cancel information.
                cancelTran.TransactionDate = DateTime.Now;
                cancelTran.TransactionType = TSBExchangeTransaction.TransactionTypes.Canceled;

                cancelTran.UserId = TAApp.User.Current.UserId; // set cancel user.
                cancelTran.FullNameEN = TAApp.User.Current.FullNameEN;
                cancelTran.FullNameTH = TAApp.User.Current.FullNameTH;
                // set finished flag to completed.
                cancelTran.FinishFlag = TSBExchangeTransaction.FinishedFlags.Completed;

                // Save group
                var ret = TSBExchangeGroup.SaveTSBExchangeGroup(this.Group).Value();
                if (null != ret)
                {
                    msg = "TSB Exchange Group successfully saved.";
                }
                else msg = "TSB Exchange Group failed to saved.";
                // Write log.
                med.Info(msg);

                // Save transaction
                var ret2 = TSBExchangeTransaction.SaveTransaction(cancelTran).Value();
                if (null != ret2)
                {
                    msg = "TSB Exchange Transaction (Request) successfully saved.";
                }
                else msg = "TSB Exchange Transaction (Request) failed to saved.";
                // Write log.
                med.Info(msg);

                // Write to queue
                TAAExchangeHeader header = Utils.CreateRequestExchangeHeader(this.Group, this.Request);
                header.FinishFlag = 1; // completed.
                header.Status = "C"; // Cancel

                Task.Run(() =>
                {
                    TAxTODMQService.Instance.WriteQueue(header);
                });
            }
        }
        /// <summary>
        /// Save Request (both group/transaction).
        /// </summary>
        public void SaveRequest()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null != this.Group && null != this.Request)
            {
                string msg = string.Empty;

                // Save group
                var ret = TSBExchangeGroup.SaveTSBExchangeGroup(this.Group).Value();
                if (null != ret)
                {
                    msg = "TSB Exchange Group successfully saved.";
                }
                else msg = "TSB Exchange Group failed to saved.";
                // Write log.
                med.Info(msg);

                // Check transaction period.
                if (this.Request.AdditionalBHT == decimal.Zero && this.Request.BorrowBHT == decimal.Zero)
                {
                    // Reset date time.
                    // Note: disable reset date time.
                    //this.Request.PeriodBegin = new DateTime?();
                    //this.Request.PeriodEnd = new DateTime?();
                }
                // Save transaction
                var ret2 = TSBExchangeTransaction.SaveTransaction(this.Request).Value();
                if (null != ret2)
                {
                    msg = "TSB Exchange Transaction (Request) successfully saved.";
                }
                else msg = "TSB Exchange Transaction (Request) failed to saved.";
                // Write log.
                med.Info(msg);

                // Write to queue
                TAAExchangeHeader header = Utils.CreateRequestExchangeHeader(this.Group, this.Request);
                if (null == header)
                {
                    msg = "Cannot create request exchange header.";
                    med.Err(msg);
                    return;
                }
                header.FinishFlag = 0;
                header.Status = "R"; // Request

                List<TAARequestExchangeItem> items = Utils.CreateRequestExchangeDetails(this.Group, this.Request);
                if (null == items)
                {
                    msg = "Cannot create request exchange details.";
                    med.Err(msg);
                    return;
                }

                Task.Run(() => 
                {
                    TAxTODMQService.Instance.WriteQueue(header);
                    TAxTODMQService.Instance.WriteQueue(items);
                });
            }
        }

        /// <summary>
        /// Save Receive Approved's request.
        /// </summary>
        public void SaveReceived()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null != this.Group)
            {
                string msg = string.Empty;
                // change group flag.
                this.Group.State = TSBExchangeGroup.StateTypes.Received;

                var groupId = this.Group.GroupId;
                var reqId = this.Group.PkId;
                var tsbId = this.Group.TSBId;
                // set finished flag to completed.
                this.Group.FinishFlag = TSBExchangeGroup.FinishedFlags.Completed;

                // Save group
                var ret = TSBExchangeGroup.SaveTSBExchangeGroup(this.Group).Value();
                if (null != ret)
                {
                    msg = "TSB Exchange Group successfully saved.";
                }
                else msg = "TSB Exchange Group failed to saved.";
                // Write log.
                med.Info(msg);

                if (null != this.Receive)
                {
                    var tran = this.Receive;

                    // update reveiced information.
                    tran.GroupId = groupId;
                    tran.TransactionDate = DateTime.Now;
                    tran.TransactionType = TSBExchangeTransaction.TransactionTypes.Received;

                    tran.TSBId = tsbId;
                    tran.TSBNameEN = this.Group.TSBNameEN;
                    tran.TSBNameTH = this.Group.TSBNameTH;

                    tran.UserId = TAApp.User.Current.UserId; // set cancel user.
                    tran.FullNameEN = TAApp.User.Current.FullNameEN;
                    tran.FullNameTH = TAApp.User.Current.FullNameTH;

                    if (tran.AdditionalBHT == decimal.Zero && tran.BorrowBHT == decimal.Zero)
                    {
                        // Reset date time.
                        //tran.PeriodBegin = new DateTime?();
                        //tran.PeriodEnd = new DateTime?();
                        // exchange only
                        tran.FinishFlag = TSBExchangeTransaction.FinishedFlags.Completed;
                    }
                    else
                    {
                        // need to returns additional/borrow
                        tran.FinishFlag = TSBExchangeTransaction.FinishedFlags.Avaliable;
                    }

                    // Save transaction
                    var ret2 = TSBExchangeTransaction.SaveTransaction(tran).Value();
                    if (null != ret2)
                    {
                        msg = "TSB Exchange Transaction (Receive) successfully saved.";
                    }
                    else msg = "TSB Exchange Transaction (Receive) failed to saved.";
                    // Write log.
                    med.Info(msg);

                    // Create TSB Credit Transaction (received)
                    var creditTran = new TSBCreditTransaction();
                    creditTran.TransactionDate = DateTime.Now;
                    creditTran.GroupId = groupId;
                    creditTran.TransactionType = TSBCreditTransaction.TransactionTypes.Received;

                    creditTran.TSBId = this.Group.TSBId;
                    creditTran.TSBNameEN = this.Group.TSBNameEN;
                    creditTran.TSBNameTH = this.Group.TSBNameTH;

                    creditTran.SupervisorId = TAApp.User.Current.UserId; // set current sup id.
                    creditTran.SupervisorNameEN = TAApp.User.Current.FullNameEN;
                    creditTran.SupervisorNameTH = TAApp.User.Current.FullNameTH;

                    creditTran.AmountST25 = tran.AmountST25;
                    creditTran.AmountST50 = tran.AmountST50;
                    creditTran.AmountBHT1 = tran.AmountBHT1;
                    creditTran.AmountBHT2 = tran.AmountBHT2;
                    creditTran.AmountBHT5 = tran.AmountBHT5;
                    creditTran.AmountBHT10 = tran.AmountBHT10;
                    creditTran.AmountBHT20 = tran.AmountBHT20;
                    creditTran.AmountBHT50 = tran.AmountBHT50;
                    creditTran.AmountBHT100 = tran.AmountBHT100;
                    creditTran.AmountBHT500 = tran.AmountBHT500;
                    creditTran.AmountBHT1000 = tran.AmountBHT1000;

                    creditTran.ExchangeBHT = tran.ExchangeBHT; // เงินขอแลก
                    creditTran.BorrowBHT = tran.BorrowBHT; // เงินยืมเพิ่ม
                    creditTran.AdditionalBHT = tran.AdditionalBHT; // เพิ่มวงเงิน

                    // Save transaction
                    var ret3 = TSBCreditTransaction.SaveTransaction(creditTran).Value();
                    if (null != ret3)
                    {
                        msg = "TSB Credit Transaction (Receive) successfully saved.";
                    }
                    else msg = "TSB Credit Transaction (Receive) failed to saved.";
                    // Write log.
                    med.Info(msg);
                }

                if (null != this.ExchangeOut)
                {
                    var tran = this.ExchangeOut;

                    // update exchange out information.
                    tran.GroupId = groupId;
                    tran.TransactionDate = DateTime.Now;
                    tran.TransactionType = TSBExchangeTransaction.TransactionTypes.Exchange;

                    tran.TSBId = tsbId;
                    tran.TSBNameEN = this.Group.TSBNameEN;
                    tran.TSBNameTH = this.Group.TSBNameTH;

                    tran.UserId = TAApp.User.Current.UserId; // set cancel user.
                    tran.FullNameEN = TAApp.User.Current.FullNameEN;
                    tran.FullNameTH = TAApp.User.Current.FullNameTH;
                    // set finished flag to completed.
                    tran.FinishFlag = TSBExchangeTransaction.FinishedFlags.Completed;

                    // Reset Additional, Borrow
                    tran.AdditionalBHT = decimal.Zero;
                    tran.BorrowBHT = decimal.Zero;
                    tran.ExchangeBHT = tran.BHTTotal; // set exchange BHT same as BHT total.

                    // Reset date time.
                    tran.PeriodBegin = new DateTime?();
                    tran.PeriodEnd = new DateTime?();

                    // Save transaction
                    var ret2 = TSBExchangeTransaction.SaveTransaction(tran).Value();
                    if (null != ret2)
                    {
                        msg = "TSB Exchange Transaction (Exchange) successfully saved.";
                    }
                    else msg = "TSB Exchange Transaction (Exchange) failed to saved.";
                    // Write log.
                    med.Info(msg);

                    // Create TSB Credit Transaction (exchange)
                    var creditTran = new TSBCreditTransaction();
                    creditTran.TransactionDate = DateTime.Now;
                    creditTran.GroupId = groupId;
                    creditTran.TransactionType = TSBCreditTransaction.TransactionTypes.Exchange;

                    creditTran.TSBId = this.Group.TSBId;
                    creditTran.TSBNameEN = this.Group.TSBNameEN;
                    creditTran.TSBNameTH = this.Group.TSBNameTH;

                    creditTran.SupervisorId = TAApp.User.Current.UserId; // set current sup id.
                    creditTran.SupervisorNameEN = TAApp.User.Current.FullNameEN;
                    creditTran.SupervisorNameTH = TAApp.User.Current.FullNameTH;

                    creditTran.AmountST25 = tran.AmountST25;
                    creditTran.AmountST50 = tran.AmountST50;
                    creditTran.AmountBHT1 = tran.AmountBHT1;
                    creditTran.AmountBHT2 = tran.AmountBHT2;
                    creditTran.AmountBHT5 = tran.AmountBHT5;
                    creditTran.AmountBHT10 = tran.AmountBHT10;
                    creditTran.AmountBHT20 = tran.AmountBHT20;
                    creditTran.AmountBHT50 = tran.AmountBHT50;
                    creditTran.AmountBHT100 = tran.AmountBHT100;
                    creditTran.AmountBHT500 = tran.AmountBHT500;
                    creditTran.AmountBHT1000 = tran.AmountBHT1000;

                    creditTran.ExchangeBHT = tran.ExchangeBHT; // เงินขอแลก
                    creditTran.BorrowBHT = tran.BorrowBHT; // เงินยืมเพิ่ม
                    creditTran.AdditionalBHT = tran.AdditionalBHT; // เพิ่มวงเงิน

                    // Save transaction
                    var ret3 = TSBCreditTransaction.SaveTransaction(creditTran).Value();
                    if (null != ret3)
                    {
                        msg = "TSB Credit Transaction (Exchange) successfully saved.";
                    }
                    else msg = "TSB Credit Transaction (Exchange) failed to saved.";
                    // Write log.
                    med.Info(msg);
                }

                // Write to queue
                TAAExchangeHeader header = Utils.CreateRequestExchangeHeader(this.Group, this.Receive);
                header.FinishFlag = 1; // finished
                header.Status = "F"; // received

                List<TAAReceiveExchangeItem> items = Utils.CreateReceiveExchangeDetails(this.Group, this.Receive);
                if (null == items)
                {
                    msg = "Cannot create receive exchange details.";
                    med.Err(msg);
                    return;
                }

                // Update TSB Balance to TA Server
                var tsbBal = TSBCreditBalance.GetCurrent(TAAPI.TSB).Value(); // Find current TSB balance.
                TAATSBCredit tsbCdt = null;
                if (null != tsbBal)
                {
                    // For Update TSB balance
                    tsbCdt = new TAATSBCredit();
                    tsbCdt.TSBId = tsbBal.TSBId;
                    tsbCdt.Amnt1 = tsbBal.AmountBHT1;
                    tsbCdt.Amnt2 = tsbBal.AmountBHT2;
                    tsbCdt.Amnt5 = tsbBal.AmountBHT5;
                    tsbCdt.Amnt10 = tsbBal.AmountBHT10;
                    tsbCdt.Amnt20 = tsbBal.AmountBHT20;
                    tsbCdt.Amnt50 = tsbBal.AmountBHT50;
                    tsbCdt.Amnt100 = tsbBal.AmountBHT100;
                    tsbCdt.Amnt500 = tsbBal.AmountBHT500;
                    tsbCdt.Amnt1000 = tsbBal.AmountBHT1000;
                    tsbCdt.Remark = null;
                    tsbCdt.Updatedate = DateTime.Now;

                    msg = "Send Update TSB Credit Balance to TA Server.";
                    med.Info(msg);
                }
                else
                {
                    msg = "Cannot find TSB Credit Balance. No data send to TA Server.";
                    med.Err(msg);
                }

                Task.Run(() =>
                {
                    TAxTODMQService.Instance.WriteQueue(header);
                    TAxTODMQService.Instance.WriteQueue(items);
                    TAxTODMQService.Instance.WriteQueue(tsbCdt);
                });
            }
        }


        #endregion

        #region Public Properties

        #region TSB/Supervisor

        /// <summary>
        /// Gets Current TSB.
        /// </summary>
        public TSB TSB { get { return TAAPI.TSB; } set { } }
        /// <summary>
        /// Gets Supervisor.
        /// </summary>
        public User Supervisor
        {
            get { return TAApp.User.Current; }
            set { }
        }
        /// <summary>
        /// Gets Supervisor Id (Chief).
        /// </summary>
        public string SupervisorId
        {
            get { return (null != TAApp.User.Current) ? TAApp.User.Current.UserId : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Full Name EN (Chief).
        /// </summary>
        public string FullNameEN
        {
            get { return (null != TAApp.User.Current) ? TAApp.User.Current.FullNameEN : string.Empty; }
            set { }
        }
        /// <summary>
        /// Gets Full Name TH (Chief).
        /// </summary>
        public string FullNameTH
        {
            get { return (null != TAApp.User.Current) ? TAApp.User.Current.FullNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region IsNew

        /// <summary>
        /// Gets is new request.
        /// </summary>
        public bool IsNew { get; private set; }

        #endregion

        #region Exchange(group/transaction)

        /// <summary>
        /// Gets TSB Exchange Group.
        /// </summary>
        public TSBExchangeGroup Group { get; private set; }
        /// <summary>
        /// Gets TSB Exchange Request Transaction.
        /// </summary>
        public TSBExchangeTransaction Request { get; private set; }
        /// <summary>
        /// Gets TSB Exchange Approve Transaction.
        /// </summary>
        public TSBExchangeTransaction Approve { get; private set; }
        /// <summary>
        /// Gets TSB Exchange Receive Transaction.
        /// </summary>
        public TSBExchangeTransaction Receive { get; private set; }
        /// <summary>
        /// Gets TSB Exchange Out Transaction.
        /// </summary>
        public TSBExchangeTransaction ExchangeOut { get; private set; }

        #endregion

        #endregion

        #region Public Events

        /// <summary>
        /// The PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    #endregion

    #endregion
}
