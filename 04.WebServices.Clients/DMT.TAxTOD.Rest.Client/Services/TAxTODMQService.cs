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

    using ops = Services.Operations.TAxTOD; // reference to static class.

    /// <summary>
    /// The TAxTOD Message Queue Service class.
    /// </summary>
    public class TAxTODMQService : JsonMessageTransferService
    {
        #region Singelton

        private static TAxTODMQService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static TAxTODMQService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TAxTODMQService))
                    {
                        _instance = new TAxTODMQService();
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
        private TAxTODMQService() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TAxTODMQService()
        {
            Shutdown();
        }

        #endregion

        #region Private Methods

        private string GetFileName(string msgType)
        {

            if (string.IsNullOrWhiteSpace(msgType))
                return string.Empty;
            // Save message.
            string fileName = "msg." + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss.ffffff",
                System.Globalization.DateTimeFormatInfo.InvariantInfo) + "." + msgType;
            return fileName;
        }

        private void SendTAServerCouponTransaction(string fullFileName, Models.TAServerCouponTransaction value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Coupon.Save(value);
            if (null == ret || ret.Failed)
            {
                // Error may be cannot connect to WS. Wait for next loop.
                med.Err("Cannot connect to TA Server Web Service.");
                return;
            }
            else
            {
                // Success
                MoveToBackup(fullFileName);
            }
        }

        private void SendUpdateCouponReceived(string fullFileName, Models.TAServerCouponReceived value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Coupon.Received(value.Serialno);
            if (null == ret || ret.Failed)
            {
                // Error may be cannot connect to WS. Wait for next loop.
                med.Err("Cannot connect to TA Server Web Service.");
                return;
            }
            else
            {
                // Success
                MoveToBackup(fullFileName);
            }
        }

        #endregion

        #region Override Methods and Properties

        /// <summary>
        /// Gets Folder Name (sub directory name).
        /// </summary>
        protected override string FolderName { get { return "tasvr.ws.msgs"; } }
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

            if (fullFileName.Contains("save.coupon.transaction"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAServerCouponTransaction>();
                    SendTAServerCouponTransaction(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToError(fullFileName);
                }
            }
            else if (fullFileName.Contains("update.coupon.received"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAServerCouponReceived>();
                    SendUpdateCouponReceived(fullFileName, value);
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
        /// <param name="value">The TAServerCouponTransaction instance.</param>
        public void WriteQueue(Models.TAServerCouponTransaction value)
        {
            if (null == value) return;
            string fileName = GetFileName("save.coupon.transaction");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }
        /// <summary>
        /// Write Queue.
        /// </summary>
        /// <param name="value">The TAServerCouponReceived instance.</param>
        public void WriteQueue(Models.TAServerCouponReceived value)
        {
            if (null == value) return;
            string fileName = GetFileName("update.coupon.received");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }

        #endregion

        #region Public Properties

        #endregion
    }
}
