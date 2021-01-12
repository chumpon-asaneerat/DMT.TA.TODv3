#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using NLib.Reflection;
using DMT.Services;

#endregion

namespace DMT.Models
{
    using localOps = Services.Operations.Plaza; // reference to static class.
    //using todOps = Services.Operations.SCW.TOD; // reference to static class.

    #region UserCache

    /// <summary>
    /// The User Cache class.
    /// </summary>
    public class UserCache
    {
        #region Internal Variables

        private Dictionary<string, User> _users = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserCache() : base()
        {
            _users = new Dictionary<string, User>();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~UserCache()
        {
            if (null != _users)
            {
                _users.Clear();
            }
            _users = null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clear.
        /// </summary>
        public void Clear()
        {
            if (null == _users) return;
            _users.Clear();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Indexer access User in cache by UserId.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>Returns match User by specificed parameter.</returns>
        public User this[string userId] 
        {
            get 
            {
                if (string.IsNullOrEmpty(userId)) return null;

                if (!_users.ContainsKey(userId))
                {
                    var search = Search.User.ById.Create(userId);
                    var usr = localOps.Security.User.Search.ById(search).Value();
                    if (null == usr) return null;
                    // add to cache.
                    _users.Add(userId, usr);
                }

                return _users[userId];
            }
            set { }
        }

        #endregion
    }

    #endregion

    #region LaneInfo

    /// <summary>
    /// The LaneInfo class.
    /// </summary>
    public class LaneInfo : INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private LaneInfo() : base() 
        {
            Jobs = new List<LaneJob>();
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lane">The Lane model instance.</param>
        public LaneInfo(Lane lane) : this()
        {
            this.Lane = lane;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Raise PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Assign jobs.
        /// </summary>
        /// <param name="jobs">The list of jobs.</param>
        public void Assign(List<SCWJob> jobs)
        {
            UserCache cache = new UserCache();

            Jobs.Clear();
            if (null == jobs) return;

            var matchJobs = jobs.FindAll(job => 
            {
                return job.plazaId == Lane.SCWPlazaId && job.laneId == Lane.LaneNo;
            });
            if (null == matchJobs || matchJobs.Count <= 0)
                return;

            matchJobs.ForEach(job => 
            {
                var usr = cache[job.staffId];
                var inst = new LaneJob(Lane, job, usr);
                Jobs.Add(inst);
                // Check EndDateTime if null so it should be current job/user
                if (!job.eojDateTime.HasValue)
                {
                    this.Job = inst;
                    this.User = inst.User;
                }
            });
        }

        #endregion

        #region Public Properties

        #region Model instance(s)

        /// <summary>Gets Lane Model instance.</summary>
        public Lane Lane { get; private set; }

        /// <summary>Gets User.</summary>
        public User User { get; private set; }

        /// <summary>Gets LaneJob.</summary>
        public LaneJob Job { get; private set; }

        #endregion

        #region TSB

        /// <summary>Gets TSB Id.</summary>
        public string TSBId
        {
            get { return (null != Lane) ? Lane.TSBId : string.Empty; }
            set { }
        }
        /// <summary>Gets TSB Name EN.</summary>
        public string TSBNameEN
        {
            get { return (null != Lane) ? Lane.TSBNameEN : string.Empty; }
            set { }
        }
        /// <summary>Gets TSB Name TH.</summary>
        public string TSBNameTH
        {
            get { return (null != Lane) ? Lane.TSBNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region PlazaGroup

        /// <summary>Gets Plaza Group Id.</summary>
        public string PlazaGroupId
        {
            get { return (null != Lane) ? Lane.PlazaGroupId : string.Empty; }
            set { }
        }
        /// <summary>Gets Plaza Group Name EN.</summary>
        public string PlazaGroupNameEN
        {
            get { return (null != Lane) ? Lane.PlazaGroupNameEN : string.Empty; }
            set { }
        }
        /// <summary>Gets Plaza Grop Name TH.</summary>
        public string PlazaGroupNameTH
        {
            get { return (null != Lane) ? Lane.PlazaGroupNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region Plaza

        /// <summary>Gets Plaza Id.</summary>
        public string PlazaId
        {
            get { return (null != Lane) ? Lane.PlazaId : string.Empty; }
            set { }
        }
        /// <summary>Gets Plaza Name EN.</summary>
        public string PlazaNameEN
        {
            get { return (null != Lane) ? Lane.PlazaNameEN : string.Empty; }
            set { }
        }
        /// <summary>Gets Plaza Name TH.</summary>
        public string PlazaNameTH
        {
            get { return (null != Lane) ? Lane.PlazaNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region Lane

        /// <summary>Gets Lane Id.</summary>
        public string LaneId
        {
            get { return (null != Lane) ? Lane.LaneId : string.Empty; }
            set { }
        }
        /// <summary>Gets Lane No.</summary>
        public int LaneNo
        {
            get { return (null != Lane) ? Lane.LaneNo : 0; }
            set { }
        }
        /// <summary>Gets Lane Type.</summary>
        public string LaneType
        {
            get { return (null != Lane) ? Lane.LaneType : string.Empty; }
            set { }
        }
        /// <summary>Gets SCW Plaza Id.</summary>
        public int SCWPlazaId
        {
            get { return (null != Lane) ? Lane.SCWPlazaId : 0; }
            set { }
        }

        #endregion

        #region User

        /// <summary>Gets User Id.</summary>
        public string UserId
        {
            get { return (null != User) ? User.UserId : string.Empty; }
            set { }
        }
        /// <summary>Gets User Full Name EN.</summary>
        public string FullNameEN
        {
            get { return (null != User) ? User.FullNameEN : string.Empty; }
            set { }
        }
        /// <summary>Gets User Full Name TH.</summary>
        public string FullNameTH
        {
            get { return (null != User) ? User.FullNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region LaneJob

        /// <summary>Gets Job No.</summary>
        public int JobNo
        {
            get { return (null != Job) ? Job.JobNo : 0; }
            set { }
        }

        /// <summary>Gets Begin Job DateTime.</summary>
        public DateTime? Begin
        {
            get { return (null != Job) ? Job.Begin : null; }
            set { }
        }
        /// <summary>Gets Begin Job Date in string.</summary>
        public string BeginDateString
        {
            get
            {
                string val = (null != Job && Job.Begin.HasValue) ?
                    Job.Begin.Value.ToThaiDateTimeString("dd/MM/yyyy") : string.Empty;
                return val;
            }
            set { }
        }
        /// <summary>Gets Begin Job Time in string.</summary>
        public string BeginTimeString
        {
            get
            {
                string val = (null != Job && Job.Begin.HasValue) ?
                    Job.Begin.Value.ToThaiTimeString() : string.Empty;
                return val;
            }
            set { }
        }

        /// <summary>Gets End Job DateTime.</summary>
        public DateTime? End
        {
            get { return (null != Job) ? Job.End : null; }
            set { }
        }
        /// <summary>Gets End Job Date in string.</summary>
        public string EndDateString
        {
            get
            {
                string val = (null != Job && Job.End.HasValue) ?
                    Job.End.Value.ToThaiDateTimeString("dd/MM/yyyy") : string.Empty;
                return val;
            }
            set { }
        }
        /// <summary>Gets End Job Time in string.</summary>
        public string EndTimeString
        {
            get
            {
                string val = (null != Job && Job.End.HasValue) ?
                    Job.End.Value.ToThaiTimeString() : string.Empty;
                return val;
            }
            set { }
        }

        /// <summary>Check Has Job.</summary>
        public bool HasJob { get { return null != Job; } set { } }

        #endregion

        #region Jobs

        /// <summary>Gets list of jobs on lane.</summary>
        public List<LaneJob> Jobs { get; private set; }

        #endregion

        #endregion

        #region Public Events

        /// <summary>
        /// The PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets Current TSB Lanes.
        /// </summary>
        /// <returns>Returns avaliable lanes on current TSB.</returns>
        public static List<LaneInfo> GetLanes()
        {
            List<LaneInfo> results = new List<LaneInfo>();
            var tsb = localOps.Infrastructure.TSB.Current().Value();
            if (null != tsb)
            {
                var lanes = localOps.Infrastructure.Lane.Search.ByTSB(tsb).Value();
                if (null != lanes && lanes.Count > 0)
                {
                    lanes.ForEach(lane =>
                    {
                        var inst = new LaneInfo(lane);
                        results.Add(inst);
                    });
                }
            }
            return results;
        }

        #endregion
    }

    #endregion

    #region LaneJob

    /// <summary>
    /// The Lane Job class.
    /// </summary>
    public class LaneJob : INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private LaneJob() : base()
        {
            this.Selected = false;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lane">The Lane model instance.</param>
        /// <param name="job">The Lane model instance.</param>
        /// <param name="user">The User model instance.</param>
        public LaneJob(Lane lane, SCWJob job, User user) : this()
        {
            this.Lane = lane;
            this.Job = job;
            this.User = user;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Raise PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets item selected.</summary>
        public bool Selected { get; set; }

        #region Model instance(s)

        /// <summary>Gets Lane Model instance.</summary>
        public Lane Lane { get; private set; }

        /// <summary>Gets User.</summary>
        public User User { get; private set; }

        /// <summary>Gets SCWJob.</summary>
        public SCWJob Job { get; private set; }

        #endregion

        #region TSB

        /// <summary>Gets TSB Id.</summary>
        public string TSBId
        {
            get { return (null != Lane) ? Lane.TSBId : string.Empty; }
            set { }
        }
        /// <summary>Gets TSB Name EN.</summary>
        public string TSBNameEN
        {
            get { return (null != Lane) ? Lane.TSBNameEN : string.Empty; }
            set { }
        }
        /// <summary>Gets TSB Name TH.</summary>
        public string TSBNameTH
        {
            get { return (null != Lane) ? Lane.TSBNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region PlazaGroup

        /// <summary>Gets Plaza Group Id.</summary>
        public string PlazaGroupId
        {
            get { return (null != Lane) ? Lane.PlazaGroupId : string.Empty; }
            set { }
        }
        /// <summary>Gets Plaza Group Name EN.</summary>
        public string PlazaGroupNameEN
        {
            get { return (null != Lane) ? Lane.PlazaGroupNameEN : string.Empty; }
            set { }
        }
        /// <summary>Gets Plaza Grop Name TH.</summary>
        public string PlazaGroupNameTH
        {
            get { return (null != Lane) ? Lane.PlazaGroupNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region Plaza

        /// <summary>Gets Plaza Id.</summary>
        public string PlazaId
        {
            get { return (null != Lane) ? Lane.PlazaId : string.Empty; }
            set { }
        }
        /// <summary>Gets Plaza Name EN.</summary>
        public string PlazaNameEN
        {
            get { return (null != Lane) ? Lane.PlazaNameEN : string.Empty; }
            set { }
        }
        /// <summary>Gets Plaza Name TH.</summary>
        public string PlazaNameTH
        {
            get { return (null != Lane) ? Lane.PlazaNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region Lane

        /// <summary>Gets Lane Id.</summary>
        public string LaneId
        {
            get { return (null != Lane) ? Lane.LaneId : string.Empty; }
            set { }
        }
        /// <summary>Gets Lane No.</summary>
        public int LaneNo
        {
            get { return (null != Lane) ? Lane.LaneNo : 0; }
            set { }
        }
        /// <summary>Gets Lane Type.</summary>
        public string LaneType
        {
            get { return (null != Lane) ? Lane.LaneType : string.Empty; }
            set { }
        }
        /// <summary>Gets SCW Plaza Id.</summary>
        public int SCWPlazaId
        {
            get { return (null != Lane) ? Lane.SCWPlazaId : 0; }
            set { }
        }

        #endregion

        #region User

        /// <summary>Gets User Id.</summary>
        public string UserId
        {
            get { return (null != User) ? User.UserId : string.Empty; }
            set { }
        }
        /// <summary>Gets User Full Name EN.</summary>
        public string FullNameEN
        {
            get { return (null != User) ? User.FullNameEN : string.Empty; }
            set { }
        }
        /// <summary>Gets User Full Name TH.</summary>
        public string FullNameTH
        {
            get { return (null != User) ? User.FullNameTH : string.Empty; }
            set { }
        }

        #endregion

        #region SCWJob

        /// <summary>Gets Job No.</summary>
        public int JobNo
        {
            get { return (null != Job) ? Job.jobNo.Value : 0; }
            set { }
        }

        /// <summary>Gets Begin Job DateTime.</summary>
        public DateTime? Begin
        {
            get { return (null != Job) ? Job.bojDateTime : null; }
            set { }
        }
        /// <summary>Gets Begin Job Date in string.</summary>
        public string BeginDateString
        {
            get
            {
                string val = (null != Job && Job.bojDateTime.HasValue) ?
                    Job.bojDateTime.Value.ToThaiDateTimeString("dd/MM/yyyy") : string.Empty;
                return val;
            }
            set { }
        }
        /// <summary>Gets Begin Job Time in string.</summary>
        public string BeginTimeString
        {
            get
            {
                string val = (null != Job && Job.bojDateTime.HasValue) ?
                    Job.bojDateTime.Value.ToThaiTimeString() : string.Empty;
                return val;
            }
            set { }
        }

        /// <summary>Gets End Job DateTime.</summary>
        public DateTime? End
        {
            get { return (null != Job) ? Job.eojDateTime : null; }
            set { }
        }
        /// <summary>Gets End Job Date in string.</summary>
        public string EndDateString
        {
            get
            {
                string val = (null != Job && Job.eojDateTime.HasValue) ?
                    Job.eojDateTime.Value.ToThaiDateTimeString("dd/MM/yyyy") : string.Empty;
                return val;
            }
            set { }
        }
        /// <summary>Gets End Job Time in string.</summary>
        public string EndTimeString
        {
            get
            {
                string val = (null != Job && Job.eojDateTime.HasValue) ?
                    Job.eojDateTime.Value.ToThaiTimeString() : string.Empty;
                return val;
            }
            set { }
        }

        /// <summary>Check Has Job.</summary>
        public bool HasJob { get { return null != Job; } set { } }

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

    #region LaneEMV

    /// <summary>
    /// The Lane EMV class.
    /// </summary>
    public class LaneEMV : INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private LaneEMV() : base()
        {

        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transaction">The Transaction model instance.</param>
        public LaneEMV(SCWEMVTransaction transaction) : this()
        {
            Transaction = transaction;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Raise PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Public Properties

        #region Model instance(s)

        /// <summary>Gets Lane Model instance.</summary>
        public SCWEMVTransaction Transaction { get; private set; }

        #endregion

        #region LaneNo

        /// <summary>Gets LaneNo.</summary>
        public int LaneNo
        {
            get { return (null != Transaction) ? Transaction.laneId : 0; }
            set { }
        }

        #endregion

        #region TrxDateTime

        /// <summary>Gets Begin Job DateTime.</summary>
        public DateTime? TrxDateTime
        {
            get { return (null != Transaction) ? Transaction.trxDateTime : null; }
            set { }
        }
        /// <summary>Gets Trx Date in string.</summary>
        public string TrxDateString
        {
            get
            {
                string val = (null != Transaction && Transaction.trxDateTime.HasValue) ?
                    Transaction.trxDateTime.Value.ToThaiDateTimeString("dd/MM/yyyy") : string.Empty;
                return val;
            }
            set { }
        }
        /// <summary>Gets Trx Time in string.</summary>
        public string TrxTimeString
        {
            get
            {
                string val = (null != Transaction && Transaction.trxDateTime.HasValue) ?
                    Transaction.trxDateTime.Value.ToThaiTimeString() : string.Empty;
                return val;
            }
            set { }
        }

        #endregion

        #region Amount

        /// <summary>Gets Amount.</summary>
        public decimal Amount
        {
            get { return (null != Transaction) ? Transaction.amount.Value : 0; }
            set { }
        }

        #endregion

        #region ApproveCode

        /// <summary>Gets ApproveCode.</summary>
        public string ApproveCode
        {
            get { return (null != Transaction) ? Transaction.approvCode : string.Empty; }
            set { }
        }

        #endregion

        #region RefNo

        /// <summary>Gets refNo.</summary>
        public string RefNo
        {
            get { return (null != Transaction) ? Transaction.refNo : string.Empty; }
            set { }
        }

        #endregion

        #region User

        /// <summary>Gets User Id.</summary>
        public string UserId
        {
            get { return (null != Transaction) ? Transaction.staffId : string.Empty; }
            set { }
        }
        /// <summary>Gets User Full Name EN.</summary>
        public string FullNameEN
        {
            get { return (null != Transaction) ? Transaction.staffNameEn : string.Empty; }
            set { }
        }
        /// <summary>Gets User Full Name TH.</summary>
        public string FullNameTH
        {
            get { return (null != Transaction) ? Transaction.staffNameTh : string.Empty; }
            set { }
        }

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

    #region LaneQRCode

    /// <summary>
    /// The Lane QR Code class.
    /// </summary>
    public class LaneQRCode : INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private LaneQRCode() : base()
        {

        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transaction">The Transaction model instance.</param>
        public LaneQRCode(SCWQRCodeTransaction transaction) : this()
        {
            Transaction = transaction;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Raise PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Public Properties

        #region Model instance(s)

        /// <summary>Gets Lane Model instance.</summary>
        public SCWQRCodeTransaction Transaction { get; private set; }

        #endregion

        #region LaneNo

        /// <summary>Gets LaneNo.</summary>
        public int LaneNo
        {
            get { return (null != Transaction) ? Transaction.laneId : 0; }
            set { }
        }

        #endregion

        #region TrxDateTime

        /// <summary>Gets Begin Job DateTime.</summary>
        public DateTime? TrxDateTime
        {
            get { return (null != Transaction) ? Transaction.trxDateTime : null; }
            set { }
        }
        /// <summary>Gets Trx Date in string.</summary>
        public string TrxDateString
        {
            get
            {
                string val = (null != Transaction && Transaction.trxDateTime.HasValue) ?
                    Transaction.trxDateTime.Value.ToThaiDateTimeString("dd/MM/yyyy") : string.Empty;
                return val;
            }
            set { }
        }
        /// <summary>Gets Trx Time in string.</summary>
        public string TrxTimeString
        {
            get
            {
                string val = (null != Transaction && Transaction.trxDateTime.HasValue) ?
                    Transaction.trxDateTime.Value.ToThaiTimeString() : string.Empty;
                return val;
            }
            set { }
        }

        #endregion

        #region Amount

        /// <summary>Gets Amount.</summary>
        public decimal Amount
        {
            get { return (null != Transaction) ? Transaction.amount.Value : 0; }
            set { }
        }

        #endregion

        #region ApproveCode

        /// <summary>Gets ApproveCode.</summary>
        public string ApproveCode
        {
            get { return (null != Transaction) ? Transaction.approvCode : string.Empty; }
            set { }
        }

        #endregion

        #region RefNo

        /// <summary>Gets RefNo.</summary>
        public string RefNo
        {
            get { return (null != Transaction) ? Transaction.refNo : string.Empty; }
            set { }
        }

        #endregion

        #region User

        /// <summary>Gets User Id.</summary>
        public string UserId
        {
            get { return (null != Transaction) ? Transaction.staffId : string.Empty; }
            set { }
        }
        /// <summary>Gets User Full Name EN.</summary>
        public string FullNameEN
        {
            get { return (null != Transaction) ? Transaction.staffNameEn : string.Empty; }
            set { }
        }
        /// <summary>Gets User Full Name TH.</summary>
        public string FullNameTH
        {
            get { return (null != Transaction) ? Transaction.staffNameTh : string.Empty; }
            set { }
        }

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
}
