#region Usings

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
                balance.AssignTo(TSBBalance);
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
                UserBalance.UserId = User.UserId;
                UserBalance.FullNameEN = User.FullNameEN;
                UserBalance.FullNameTH = User.FullNameTH;
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
                var inst = UserCreditBalance.GetCurrentBalance(
                    UserBalance.UserId, UserBalance.PlazaGroupId).Value();
                if (null != inst)
                {
                    if (inst.BHTTotal <= decimal.Zero)
                    {
                        // change source state.
                        UserBalance.State = UserCreditBalance.StateTypes.Completed;
                        UserCreditBalance.SaveUserCreditBalance(UserBalance);
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

    #endregion

    #region TSB Coupon Manager related classes

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
            }
        }

        #endregion

        #region Override Methods

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

        /// <summary>Gets or sets TransactionType.</summary>
        public TSBCouponTransactionTypes TransactionType { get; set; }

        #endregion
    }


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
            /*
            if (null != this.Transaction)
            {
                this.Transaction.PropertyChanged += Transaction_PropertyChanged;
            }
            this.Transaction = null;
            */
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

        private void LoadCoupons()
        {
            if (null == Stock35) Stock35 = new ObservableCollection<TSBCouponItem>();
            Stock35.Clear();

            if (null == User35) User35 = new ObservableCollection<TSBCouponItem>();
            User35.Clear();

            if (null == Stock80) Stock80 = new ObservableCollection<TSBCouponItem>();
            Stock80.Clear();

            if (null == User80) User80 = new ObservableCollection<TSBCouponItem>();
            User80.Clear();

            if (null == User) return;
            var coupons = TSBCouponTransaction.GetTSBCouponTransactions(TAAPI.TSB).Value();
            if (null != coupons && coupons.Count > 0)
            {
                coupons.ForEach(coupon => 
                { 
                    
                });
            }
        }

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

        #region Coupons

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

        #region Coupon Collections properties

        public ObservableCollection<TSBCouponItem> Stock35 { get; private set; }
        public ObservableCollection<TSBCouponItem> Stock80 { get; private set; }

        public ObservableCollection<TSBCouponItem> User35 { get; private set; }
        public ObservableCollection<TSBCouponItem> User80 { get; private set; }

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
            return false;
        }

        #endregion
    }

    #endregion

    #region TSBCouponRetrunManager

    /// <summary>
    /// TSB Coupon Return Manager class.
    /// </summary>
    public class TSBCouponRetrunManager : TSBCouponManager
    {
        #region Override Methods

        /// <summary>
        /// Save.
        /// </summary>
        /// <returns>Returns true if sace success.</returns>
        public override bool Save()
        {
            return false;
        }

        #endregion
    }

    #endregion

    #endregion
}
