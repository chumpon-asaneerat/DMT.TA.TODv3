#region Using

using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Timers;
using System.Threading.Tasks;

using NLib;
using NLib.IO;

#endregion

namespace DMT.Services
{
    // TODO: Need to implements common class to provide each message to generate file, read data from file and process data

    using ops = Services.Operations.SCW; // reference to static class.

    /// <summary>
    /// The SCW Message Queue Service class.
    /// </summary>
    public class SCWMQService : JsonMessageTransferService
    {
        #region Singelton

        private static SCWMQService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static SCWMQService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(SCWMQService))
                    {
                        _instance = new SCWMQService();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private SCWMQService() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~SCWMQService()
        {
            Shutdown();
        }

        #endregion

        #region Private Methods

        #region Send

        private void SendDeclare(string fullFileName, Models.SCWDeclare value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.TOD.declare(value);
            if (null == ret || null == ret.status || string.IsNullOrWhiteSpace(ret.status.code))
            {
                // Error may be cannot connect to WS. Wait for next loop.
                med.Err("Cannot connect to SCW Web Service.");
                return;
            }
            if (ret.status.code != "S200")
            {
                // Execute Result is not Success so move to error folder.
                med.Err("SCW Web Service returns error.");
                MoveToError(fullFileName);
                return;
            }
            // Success
            MoveToBackup(fullFileName);
        }

        private void SendLogInAudit(string fullFileName, Models.SCWLogInAudit value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Security.loginAudit(value);
            if (null == ret || null == ret.status || string.IsNullOrWhiteSpace(ret.status.code))
            {
                // Error may be cannot connect to WS. Wait for next loop.
                med.Err("Cannot connect to SCW Web Service.");
                return;
            }
            if (ret.status.code != "S200")
            {
                // Execute Result is not Success so move to error folder.
                med.Err("SCW Web Service returns error.");
                MoveToError(fullFileName);
                return;
            }
            // Success
            MoveToBackup(fullFileName);
        }

        private void SendChangePassword(string fullFileName, Models.SCWChangePassword value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Security.changePassword(value);
            if (null == ret || null == ret.status || string.IsNullOrWhiteSpace(ret.status.code))
            {
                // Error may be cannot connect to WS. Wait for next loop.
                med.Err("Cannot connect to SCW Web Service.");
                return;
            }
            if (ret.status.code != "S200")
            {
                // Execute Result is not Success so move to error folder.
                med.Err("SCW Web Service returns error.");
                MoveToError(fullFileName);
                return;
            }
            // Success
            MoveToBackup(fullFileName);
        }

        private void SendSaveChiefDuty(string fullFileName, Models.SCWSaveChiefDuty value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.TOD.saveCheifDuty(value);
            if (null == ret || null == ret.status || string.IsNullOrWhiteSpace(ret.status.code))
            {
                // Error may be cannot connect to WS. Wait for next loop.
                med.Err("Cannot connect to SCW Web Service.");
                return;
            }
            if (ret.status.code != "S200")
            {
                // Execute Result is not Success so move to error folder.
                med.Err("SCW Web Service returns error.");
                MoveToError(fullFileName);
                return;
            }
            // Success
            MoveToBackup(fullFileName);
        }

        #endregion

        #region Resend (from error folder)

        #endregion

        #endregion

        #region Override Methods and Properties

        /// <summary>
        /// Gets Folder Name (sub directory name).
        /// </summary>
        protected override string FolderName { get { return "scw.ws.msgs"; } }
        /// <summary>
        /// Process Json (string).
        /// </summary>
        /// <param name="fullFileName">The json full file name.</param>
        /// <param name="jsonString">The json data in string.</param>
        protected override void ProcessJson(string fullFileName, string jsonString)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            // Extract File Name.
            if (string.IsNullOrEmpty(fullFileName)) return; // skip if file name is empty.

            if (fullFileName.Contains("declare"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.SCWDeclare>();
                    SendDeclare(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToError(fullFileName);
                }
            }
            else if (fullFileName.Contains("loginaudit"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.SCWLogInAudit>();
                    SendLogInAudit(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToError(fullFileName);
                }
            }
            else if (fullFileName.Contains("changepwd"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.SCWChangePassword>();
                    SendChangePassword(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToError(fullFileName);
                }
            }
            else if (fullFileName.Contains("savechiefduty"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.SCWSaveChiefDuty>();
                    SendSaveChiefDuty(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToError(fullFileName);
                }
            }
            else
            {
                // process not staff list so Not Supports file.
                med.Err("Not Supports message.");
                MoveToNotSupports(fullFileName);
            }
        }
        /// <summary>
        /// OnStart.
        /// </summary>
        protected override void OnStart() { }
        /// <summary>
        /// OnShutdown.
        /// </summary>
        protected override void OnShutdown() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Write Queue.
        /// </summary>
        /// <param name="value">The SCWDeclare instance.</param>
        public void WriteQueue(Models.SCWDeclare value)
        {
            if (null == value) return;
            string fileName = GetFileName("declare");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }
        /// <summary>
        /// Write Queue.
        /// </summary>
        /// <param name="value">The SCWLogInAudit instance.</param>
        public void WriteQueue(Models.SCWLogInAudit value)
        {
            if (null == value) return;
            string fileName = GetFileName("loginaudit");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }
        /// <summary>
        /// Write Queue.
        /// </summary>
        /// <param name="value">The SCWChangePassword instance.</param>
        public void WriteQueue(Models.SCWChangePassword value)
        {
            if (null == value) return;
            string fileName = GetFileName("changepwd");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }
        /// <summary>
        /// Write Queue.
        /// </summary>
        /// <param name="value">The SCWSaveChiefDuty instance.</param>
        public void WriteQueue(Models.SCWSaveChiefDuty value)
        {
            if (null == value) return;
            string fileName = GetFileName("savechiefduty");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }

        #endregion

        #region Public Properties

        #endregion
    }
}
