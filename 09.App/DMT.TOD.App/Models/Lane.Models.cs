#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;

using DMT.Configurations;
using DMT.Services;
using DMT.Models;

using NLib.Reflection;

#endregion

namespace DMT.Models
{
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
        /// <summary>
        /// Check is userid is in cache.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>Returns true if user Id is exist in cache.</returns>
        public bool Contains(string userId)
        {
            return _users.ContainsKey(userId);
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
                    var usr = User.GetByUserId(userId).Value();
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

    #region LaneJob

    /// <summary>
    /// The LaneJob class.
    /// </summary>
    public class LaneJob
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private LaneJob() { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="job">The SCWJob instance.</param>
        /// <param name="lane">The Lane instance.</param>
        /// <param name="user">The User instance.</param>
        public LaneJob(SCWJob job, Lane lane, User user)
        {
            this.Job = job;
            this.Lane = lane;
            this.User = user;
        }

        #endregion

        #region Public Methods

        #endregion

        #region Public Properties

        #region Model instance(s)

        /// <summary>Gets LaneJob.</summary>
        public SCWJob Job { get; private set; }

        /// <summary>Gets Lane.</summary>
        public Lane Lane { get; private set; }

        /// <summary>Gets User.</summary>
        public User User { get; private set; }

        #endregion

        #region Job

        /// <summary>Gets Job No.</summary>
        public int? JobNo
        {
            get { return (null != Job) ? Job.jobNo : null; }
            set { }
        }

        /// <summary>Gets Begin Job DateTime.</summary>
        public DateTime? Begin
        {
            get { return (null != Job) ? Job.bojDateTime : null; }
            set { }
        }
        /// <summary>Gets Begin Job DateTime in string.</summary>
        public string BeginDateTimeString
        {
            get
            {
                string val = (null != Job && Job.bojDateTime.HasValue) ?
                    Job.bojDateTime.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss") : string.Empty;
                return val;
            }
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
        /// <summary>Gets End Job DateTime in string.</summary>
        public string EndDateTimeString
        {
            get
            {
                string val = (null != Job && Job.eojDateTime.HasValue) ?
                    Job.eojDateTime.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss") : string.Empty;
                return val;
            }
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
        public int? LaneNo
        {
            get { return (null != Lane) ? Lane.LaneNo : 0; }
            set { }
        }
        /// <summary>Gets SCW Plaza Id.</summary>
        public int? SCWPlazaId
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

        #endregion
    }

    #endregion
}
