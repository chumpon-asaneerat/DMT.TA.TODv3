#region Using

using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Timers;
using System.Threading.Tasks;

using NLib;
using NLib.IO;

using DMT.Configurations;
using DMT.Models;
using DMT.Services;

#endregion

namespace DMT.Services
{
    // TODO: Need to implements common class to provide each message to generate file, read data from file and process data

    using ops = Services.Operations.TA; // reference to static class.

    /// <summary>
    /// The TA Message Queue Service class.
    /// </summary>
    public class TAMQService : JsonMessageTransferService
    {
        #region Singelton

        private static TAMQService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static TAMQService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TAMQService))
                    {
                        _instance = new TAMQService();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private TAMQService() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TAMQService()
        {
            Shutdown();
        }

        #endregion

        #region Private Methods

        #region Send

        private void SendChangeTSBShift(string fullFileName, Models.TSBShift value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Shift.TSB.Update(value);
            if (null == ret || !ret.Ok)
            {
                // Error may be cannot connect to WS. Wait for next loop.
                med.Err("Cannot connect to TA App Web Service.");
                return;
            }
            // Success
            MoveToBackup(fullFileName);
        }

        private void SendChangeUserShift(string fullFileName, Models.UserShift value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Shift.User.Update(value);
            if (null == ret || !ret.Ok)
            {
                // Error may be cannot connect to WS. Wait for next loop.
                med.Err("Cannot connect to TA App Web Service.");
                return;
            }
            // Success
            MoveToBackup(fullFileName);
        }

        private void SendRevenueEntry(string fullFileName, Models.RevenueEntry value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Revenue.Update(value);
            if (null == ret || !ret.Ok)
            {
                // Error may be cannot connect to WS. Wait for next loop.
                med.Err("Cannot connect to TA App Web Service.");
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
        protected override string FolderName { get { return "taa.ws.msgs"; } }
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

            if (fullFileName.Contains("tsb.shift.change"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TSBShift>();
                    SendChangeTSBShift(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToError(fullFileName);
                }
            }
            else if (fullFileName.Contains("user.shift.change"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.UserShift>();
                    SendChangeUserShift(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToError(fullFileName);
                }
            }
            else if (fullFileName.Contains("revenue.entry.update"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.RevenueEntry>();
                    SendRevenueEntry(fullFileName, value);
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
                // process not Supports file.
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
        /// <param name="value">The TSBShift instance.</param>
        public void WriteQueue(Models.TSBShift value)
        {
            if (null == value) return;
            string fileName = GetFileName("tsb.shift.change");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }
        /// <summary>
        /// Write Queue.
        /// </summary>
        /// <param name="value">The UserShift instance.</param>
        public void WriteQueue(Models.UserShift value)
        {
            if (null == value) return;
            string fileName = GetFileName("user.shift.change");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }
        /// <summary>
        /// Write Queue.
        /// </summary>
        /// <param name="value">The RevenueEntry instance.</param>
        public void WriteQueue(Models.RevenueEntry value)
        {
            if (null == value) return;
            string fileName = GetFileName("revenue.entry.update");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }

        #endregion

        #region Public Properties

        #endregion
    }
}
