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

        #region Check

        private void CheckSendError(MethodBase med, string fullFileName, NRestResult ret)
        {
            if (null == ret)
            {
                // Error may be cannot connect to WS. Move to error folder for resend.
                med.Err("Cannot connect to TA App Web Service. Move to 'Error' folder.");
                // Error
                MoveToError(fullFileName);
            }
            else
            {
                if (!ret.Ok && ret.HttpStatus != HttpStatus.Success)
                {
                    // Connection has HTTP Status Code not in 200-399. so move to error folder for resend.
                    med.Err("Send data to TA App Web Service failed (HTTP error). Move to 'Error' folder.");
                    // Error
                    MoveToError(fullFileName);
                }
                else if (!ret.Ok && ret.HttpStatus == HttpStatus.Success)
                {
                    // Connection is OK but result is not valid. so no need to resend anymore.
                    med.Err("Send data to TA App Web Service success but content is invalid. Move to 'Invalid' folder.");
                    // Invalid
                    MoveToInvalid(fullFileName);
                }
                else if (ret.Ok)
                {
                    // Result is OK, so HttpStatus not concern
                    med.Info("Send data to TA App Web Service success and content is valid. Move to 'Backup' folder.");
                    // Success
                    MoveToBackup(fullFileName);
                }
                else
                {
                    med.Err("Send data to TA App Web Service failed <Unhandled case>. Wait for next loop for resend.");
                }
            }
        }

        private void CheckResendError(MethodBase med, string fullFileName, NRestResult ret)
        {
            if (null == ret)
            {
                // Error may be cannot connect to WS. Wait for next loop.
                med.Err("Cannot connect to TA App Web Service. Wait for next loop for resend.");
            }
            else
            {
                if (!ret.Ok && ret.HttpStatus != HttpStatus.Success)
                {
                    // Connection has HTTP Status Code not in 200-399. so move to error folder for resend.
                    med.Err("Send data to TA App Web Service failed (HTTP error). Wait for next loop for resend.");
                }
                else if (!ret.Ok && ret.HttpStatus == HttpStatus.Success)
                {
                    // Connection is OK but result is not valid. so no need to resend anymore.
                    med.Err("Send data to TA App Web Service success but content is invalid. Move to 'Invalid' folder.");
                    // Invalid
                    MoveErrorToInvalid(fullFileName);
                }
                else if (ret.Ok)
                {
                    // Result is OK, so HttpStatus not concern
                    med.Info("Send data to TA App Web Service success and content is valid. Move to 'Backup' folder.");
                    // Success
                    MoveErrorToBackup(fullFileName);
                }
                else
                {
                    med.Err("Send data to TA App Web Service failed <Unhandled case>. Wait for next loop for resend.");
                }
            }
        }

        #endregion

        #region Send

        private void SendChangeTSBShift(string fullFileName, Models.TSBShift value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Shift.TSB.Update(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendChangeUserShift(string fullFileName, Models.UserShift value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Shift.User.Update(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendRevenueEntry(string fullFileName, Models.RevenueEntry value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Revenue.Update(value);
            CheckSendError(med, fullFileName, ret);
        }

        #endregion

        #region Resend (from error folder)

        private void ResendChangeTSBShift(string fullFileName, Models.TSBShift value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.Shift.TSB.Update(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendChangeUserShift(string fullFileName, Models.UserShift value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.Shift.User.Update(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendRevenueEntry(string fullFileName, Models.RevenueEntry value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.Revenue.Update(value);
            CheckResendError(med, fullFileName, ret);
        }

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
                    MoveToInvalid(fullFileName);
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
                    MoveToInvalid(fullFileName);
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
                    MoveToInvalid(fullFileName);
                }
            }
            else
            {
                // process not Supports file.
                med.Err("Not Supports message.");
            }
        }
        /// <summary>
        /// Resend Json (string) from error folder.
        /// </summary>
        /// <param name="fullFileName">The json full file name.</param>
        /// <param name="jsonString">The json data in string.</param>
        protected override void ResendJson(string fullFileName, string jsonString)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            // Extract File Name.
            if (string.IsNullOrEmpty(fullFileName)) return; // skip if file name is empty.

            if (fullFileName.Contains("tsb.shift.change"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TSBShift>();
                    ResendChangeTSBShift(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveErrorToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("user.shift.change"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.UserShift>();
                    ResendChangeUserShift(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveErrorToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("revenue.entry.update"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.RevenueEntry>();
                    ResendRevenueEntry(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveErrorToInvalid(fullFileName);
                }
            }
            else
            {
                // process not Supports file.
                med.Err("Not Supports message.");
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
