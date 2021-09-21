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

        #region Check

        private void CheckSendError(MethodBase med, string fullFileName, Models.SCWResult ret)
        {
            if (null == ret)
            {
                // Error may be cannot connect to WS. Move to error folder for resend.
                med.Err("Cannot connect to SCW Web Service. Move to 'Error' folder.");
                // Error
                MoveToError(fullFileName);
            }
            else
            {
                //if (null == ret || null == ret.status || string.IsNullOrWhiteSpace(ret.status.code))
                if (!ret.Ok && ret.HttpStatus != HttpStatus.Success)
                {
                    // Connection has HTTP Status Code not in 200-399. so move to error folder for resend.
                    med.Err("Send data to SCW Web Service failed (HTTP error). Move to 'Error' folder.");
                    // Error
                    MoveToError(fullFileName);
                }
                else if (!ret.Ok && ret.HttpStatus == HttpStatus.Success)
                {
                    // Connection is OK but result is not valid. so no need to resend anymore.
                    if (null != ret.status && !string.IsNullOrWhiteSpace(ret.status.code))
                    {
                        if (ret.status.code.ToUpperInvariant() == "F500")
                        {
                            // Connection is OK but result is 'API Error'. so no need to resend anymore.
                            med.Err("Send data to SCW Web Service success but content code is 'API Error'. Move to 'F500' folder.");
                            // API Error
                            MoveToErrorF500(fullFileName);
                        }
                        else if (ret.status.code.ToUpperInvariant() == "F203")
                        {
                            // Connection is OK but result is 'User not authenticated'. so no need to resend anymore.
                            med.Err("Send data to SCW Web Service success but content code is 'User not authenticated'. Move to 'F203' folder.");
                            // User not authenticated
                            MoveToErrorF203(fullFileName);
                        }
                        else if (ret.status.code.ToUpperInvariant() == "F302")
                        {
                            // Connection is OK but result is 'API Bad request'. so no need to resend anymore.
                            med.Err("Send data to SCW Web Service success but content code is 'API Bad request'. Move to 'F302' folder.");
                            // API Bad request
                            MoveToErrorF302(fullFileName);
                        }
                        else
                        {
                            // Connection is OK but result is not success (unhandle case). so no need to resend anymore.
                            med.Err("Send data to SCW Web Service success but content code is not 'F200' (unhandle case). Move to 'FXXX' folder.");
                            // Other
                            MoveToErrorFXXX(fullFileName);
                        }
                    }
                    else
                    {
                        // Connection is OK but result is not valid. so no need to resend anymore.
                        med.Err("Send data to SCW Web Service success but content is invalid. Move to 'Invalid' folder.");
                        // Invalid
                        MoveErrorToInvalid(fullFileName);
                    }
                }
                else if (ret.Ok)
                {
                    // Result is OK, so HttpStatus not concern
                    med.Info("Send data to SCW Web Service success and content is valid. Move to 'Backup' folder.");
                    // Success
                    MoveToBackup(fullFileName);
                }
                else
                {
                    med.Err("Send data to SCW Web Service failed <Unhandled case>. Wait for next loop for resend.");
                }
            }
        }

        private void CheckResendError(MethodBase med, string fullFileName, Models.SCWResult ret)
        {
            if (null == ret)
            {
                // Error may be cannot connect to WS. Wait for next loop.
                med.Err("Cannot connect to SCW Web Service. Wait for next loop for resend.");
            }
            else
            {
                if (!ret.Ok && ret.HttpStatus != HttpStatus.Success)
                {
                    // Connection has HTTP Status Code not in 200-399. so move to error folder for resend.
                    med.Err("Send data to SCW Web Service failed (HTTP error). Wait for next loop for resend.");
                }
                else if (!ret.Ok && ret.HttpStatus == HttpStatus.Success)
                {
                    if (null != ret.status && !string.IsNullOrWhiteSpace(ret.status.code))
                    {
                        if (ret.status.code.ToUpperInvariant() == "F500")
                        {
                            // Connection is OK but result is 'API Error'. so no need to resend anymore.
                            med.Err("Send data to SCW Web Service success but content code is 'API Error'. Move to 'F500' folder.");
                            // API Error
                            MoveErrorToErrorF500(fullFileName);
                        }
                        else if (ret.status.code.ToUpperInvariant() == "F203")
                        {
                            // Connection is OK but result is 'User not authenticated'. so no need to resend anymore.
                            med.Err("Send data to SCW Web Service success but content code is 'User not authenticated'. Move to 'F203' folder.");
                            // User not authenticated
                            MoveErrorToErrorF203(fullFileName);
                        }
                        else if (ret.status.code.ToUpperInvariant() == "F302")
                        {
                            // Connection is OK but result is 'API Bad request'. so no need to resend anymore.
                            med.Err("Send data to SCW Web Service success but content code is 'API Bad request'. Move to 'F302' folder.");
                            // API Bad request
                            MoveErrorToErrorF302(fullFileName);
                        }
                        else
                        {
                            // Connection is OK but result is not success (unhandle case). so no need to resend anymore.
                            med.Err("Send data to SCW Web Service success but content code is not success (unhandle case). Move to 'FXXX' folder.");
                            // Other
                            MoveErrorToErrorFXXX(fullFileName);
                        }
                    }
                    else
                    {
                        // Connection is OK but result is not valid. so no need to resend anymore.
                        med.Err("Send data to SCW Web Service success but content is invalid. Move to 'Invalid' folder.");
                        // Invalid
                        MoveErrorToInvalid(fullFileName);
                    }

                }
                else if (ret.Ok)
                {
                    // Result is OK, so HttpStatus not concern
                    med.Info("Send data to SCW Web Service success and content is valid. Move to 'Backup' folder.");
                    // Success
                    MoveErrorToBackup(fullFileName);
                }
                else
                {
                    med.Err("Send data to SCW Web Service failed <Unhandled case>. Wait for next loop for resend.");
                }
            }
        }


        #endregion

        #region Send

        private void SendDeclare(string fullFileName, Models.SCWDeclare value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.TOD.declare(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendLogInAudit(string fullFileName, Models.SCWLogInAudit value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Security.loginAudit(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendChangePassword(string fullFileName, Models.SCWChangePassword value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Security.changePassword(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendSaveChiefDuty(string fullFileName, Models.SCWSaveChiefDuty value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.TOD.saveCheifDuty(value);
            CheckSendError(med, fullFileName, ret);
        }

        #endregion

        #region Resend (from error folder)

        private void ResendDeclare(string fullFileName, Models.SCWDeclare value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.TOD.declare(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendLogInAudit(string fullFileName, Models.SCWLogInAudit value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.Security.loginAudit(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendChangePassword(string fullFileName, Models.SCWChangePassword value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.Security.changePassword(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendSaveChiefDuty(string fullFileName, Models.SCWSaveChiefDuty value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.TOD.saveCheifDuty(value);
            CheckResendError(med, fullFileName, ret);
        }

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
        /// Resend Json (string) from error folder.
        /// </summary>
        /// <param name="fullFileName">The json full file name.</param>
        /// <param name="jsonString">The json data in string.</param>
        protected override void ResendJson(string fullFileName, string jsonString)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            // Extract File Name.
            if (string.IsNullOrEmpty(fullFileName)) return; // skip if file name is empty.

            if (fullFileName.Contains("declare"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.SCWDeclare>();
                    ResendDeclare(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                }
            }
            else if (fullFileName.Contains("loginaudit"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.SCWLogInAudit>();
                    ResendLogInAudit(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                }
            }
            else if (fullFileName.Contains("changepwd"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.SCWChangePassword>();
                    ResendChangePassword(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                }
            }
            else if (fullFileName.Contains("savechiefduty"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.SCWSaveChiefDuty>();
                    ResendSaveChiefDuty(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                }
            }
            else
            {
                // process Not Supports file.
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
