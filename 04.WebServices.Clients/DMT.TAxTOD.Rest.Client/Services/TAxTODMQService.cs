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

        #region Internal Variables

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

        #region Check

        private void CheckSendError(MethodBase med, string fullFileName, NRestResult ret)
        {
            if (null == ret)
            {
                // Error may be cannot connect to WS. Move to error folder for resend.
                med.Err("Cannot connect to TA Server Web Service. Move to 'Error' folder.");
                // Error
                MoveToError(fullFileName);
            }
            else
            {
                if (!ret.Ok && ret.HttpStatus != HttpStatus.Success)
                {
                    // Connection has HTTP Status Code not in 200-399. so move to error folder for resend.
                    med.Err("Send data to TA Server Web Service failed (HTTP error). Move to 'Error' folder.");
                    // Error
                    MoveToError(fullFileName);
                }
                else if (!ret.Ok && ret.HttpStatus == HttpStatus.Success)
                {
                    // Connection is OK but result is not valid. so no need to resend anymore.
                    med.Err("Send data to TA Server Web Service success but content is invalid. Move to 'Invalid' folder.");
                    // Invalid
                    MoveToInvalid(fullFileName);
                }
                else if (ret.Ok && ret.HttpStatus != HttpStatus.Success)
                {
                    // Connection has HTTP Status Code not in 200-399. so move to error folder for resend.
                    med.Err("Send data to TA Server Web Service failed (HTTP error). Move to 'Error' folder.");
                    // Error
                    MoveToError(fullFileName);
                }
                else if (ret.Ok && ret.HttpStatus == HttpStatus.Success)
                {
                    // Result is OK, so HttpStatus is success.
                    med.Info("Send data to TA Server Web Service success and content is valid. Move to 'Backup' folder.");
                    // Success
                    MoveToBackup(fullFileName);
                }
                else
                {
                    med.Err("Send data to TA Server Web Service failed <Unhandled case>. Wait for next loop for resend.");
                }
            }
        }

        private void CheckResendError(MethodBase med, string fullFileName, NRestResult ret)
        {
            if (null == ret)
            {
                // Error may be cannot connect to WS. Wait for next loop.
                med.Err("Cannot connect to TA Server Web Service. Wait for next loop for resend.");
            }
            else
            {
                if (!ret.Ok && ret.HttpStatus != HttpStatus.Success)
                {
                    // Connection has HTTP Status Code not in 200-399. so move to error folder for resend.
                    med.Err("Send data to TA Server Web Service failed (HTTP error). Wait for next loop for resend.");
                }
                else if (!ret.Ok && ret.HttpStatus == HttpStatus.Success)
                {
                    // Connection is OK but result is not valid. so no need to resend anymore.
                    med.Err("Send data to TA Server Web Service success but content is invalid. Move to 'Invalid' folder.");
                    // Invalid
                    MoveErrorToInvalid(fullFileName);
                }
                else if (ret.Ok && ret.HttpStatus != HttpStatus.Success)
                {
                    // Connection has HTTP Status Code not in 200-399. so move to error folder for resend.
                    med.Err("Send data to TA Server Web Service failed (HTTP error). Wait for next loop for resend.");
                }
                else if (ret.Ok && ret.HttpStatus == HttpStatus.Success)
                {
                    // Result is OK, so HttpStatus is success.
                    med.Info("Send data to TA Server Web Service success and content is valid. Move to 'Backup' folder.");
                    // Success
                    MoveErrorToBackup(fullFileName);
                }
                else
                {
                    med.Err("Send data to TA Server Web Service failed <Unhandled case>. Wait for next loop for resend.");
                }
            }
        }


        #endregion

        #region Send

        private void SendTSBCreditBalance(string fullFileName, Models.TAATSBCredit value) 
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Credit.TSB.Save(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendUserCreditBalance(string fullFileName, Models.TAAUserCredit value) 
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Credit.User.Save(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendTAServerCouponTransaction(string fullFileName, Models.TAServerCouponTransaction value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Coupon.Save(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendUpdateCouponReceived(string fullFileName, Models.TAServerCouponReceived value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Coupon.Received(value.Serialno);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendSaveAR(string fullFileName, Models.SAPSaveAR value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.SAP.SaveAR(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendChangeTSBShift(string fullFileName, Models.TSBShift value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.TOD.TSBShift.Save(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendChangeUserShift(string fullFileName, Models.UserShift value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.TOD.UserShift.Save(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendRevenueEntry(string fullFileName, Models.RevenueEntry value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.TOD.RevenueEntry.Save(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendRequestExchangeHeader(string fullFileName, Models.TAAExchangeHeader value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Exchange.SaveRequestDocument(value);
            CheckSendError(med, fullFileName, ret);
            if (ret.Ok)
            {
                OnSendExchange.Call(this, EventArgs.Empty);
            }
        }

        private void SendRequestExchangeItem(string fullFileName, Models.TAARequestExchangeItem value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Exchange.SaveRequestItem(value);
            CheckSendError(med, fullFileName, ret);
        }

        private void SendRequestExchangeItem(string fullFileName, Models.TAAApproveExchangeItem value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = ops.Exchange.SaveApproveItem(value);
            CheckSendError(med, fullFileName, ret);
        }

        #endregion

        #region Resend (from error folder)

        private void ResendTSBCreditBalance(string fullFileName, Models.TAATSBCredit value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.Credit.TSB.Save(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendUserCreditBalance(string fullFileName, Models.TAAUserCredit value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.Credit.User.Save(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendTAServerCouponTransaction(string fullFileName, Models.TAServerCouponTransaction value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.Coupon.Save(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendUpdateCouponReceived(string fullFileName, Models.TAServerCouponReceived value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.Coupon.Received(value.Serialno);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendSaveAR(string fullFileName, Models.SAPSaveAR value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.SAP.SaveAR(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendChangeTSBShift(string fullFileName, Models.TSBShift value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.TOD.TSBShift.Save(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendChangeUserShift(string fullFileName, Models.UserShift value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.TOD.UserShift.Save(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendRevenueEntry(string fullFileName, Models.RevenueEntry value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.TOD.RevenueEntry.Save(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendRequestExchangeHeader(string fullFileName, Models.TAAExchangeHeader value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.Exchange.SaveRequestDocument(value);
            CheckResendError(med, fullFileName, ret);
            if (ret.Ok)
            {
                OnSendExchange.Call(this, EventArgs.Empty);
            }
        }

        private void ResendRequestExchangeItem(string fullFileName, Models.TAARequestExchangeItem value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.Exchange.SaveRequestItem(value);
            CheckResendError(med, fullFileName, ret);
        }

        private void ResendRequestExchangeItem(string fullFileName, Models.TAAApproveExchangeItem value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("Resend file: " + fullFileName);

            var ret = ops.Exchange.SaveApproveItem(value);
            CheckResendError(med, fullFileName, ret);
        }

        #endregion

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

            if (fullFileName.Contains("update.tsb.credit.balance"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAATSBCredit>();
                    SendTSBCreditBalance(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("update.user.credit.balance"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAAUserCredit>();
                    SendUserCreditBalance(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("save.coupon.transaction"))
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
                    MoveToInvalid(fullFileName);
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
                    MoveToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("sap.send.ar"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.SAPSaveAR>();
                    SendSaveAR(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("tsb.shift.change"))
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
            else if (fullFileName.Contains("save.request.exchange.header"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAAExchangeHeader>();
                    SendRequestExchangeHeader(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("save.request.exchange.item"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAARequestExchangeItem>();
                    SendRequestExchangeItem(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("save.approve.exchange.item"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAAApproveExchangeItem>();
                    SendRequestExchangeItem(fullFileName, value);
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

            if (fullFileName.Contains("update.tsb.credit.balance"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAATSBCredit>();
                    ResendTSBCreditBalance(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveErrorToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("update.user.credit.balance"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAAUserCredit>();
                    ResendUserCreditBalance(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveErrorToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("save.coupon.transaction"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAServerCouponTransaction>();
                    ResendTAServerCouponTransaction(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveErrorToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("update.coupon.received"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAServerCouponReceived>();
                    ResendUpdateCouponReceived(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveErrorToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("sap.send.ar"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.SAPSaveAR>();
                    ResendSaveAR(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveErrorToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("tsb.shift.change"))
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
            else if (fullFileName.Contains("save.request.exchange.header"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAAExchangeHeader>();
                    ResendRequestExchangeHeader(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveErrorToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("save.request.exchange.item"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAARequestExchangeItem>();
                    ResendRequestExchangeItem(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveErrorToInvalid(fullFileName);
                }
            }
            else if (fullFileName.Contains("save.approve.exchange.item"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TAAApproveExchangeItem>();
                    ResendRequestExchangeItem(fullFileName, value);
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
                // Not Supports file.
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
        /// <param name="value">The TAATSBCredit instance.</param>
        public void WriteQueue(Models.TAATSBCredit value)
        {
            if (null == value) return;
            string fileName = GetFileName("update.tsb.credit.balance");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }
        /// <summary>
        /// Write Queue.
        /// </summary>
        /// <param name="value">The TAAUserCredit instance.</param>
        public void WriteQueue(Models.TAAUserCredit value)
        {
            if (null == value) return;
            string fileName = GetFileName("update.user.credit.balance");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }
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
        /// <summary>
        /// Write Queue.
        /// </summary>
        /// <param name="value">The SAPSaveAR instance.</param>
        public void WriteQueue(Models.SAPSaveAR value)
        {
            if (null == value) return;
            string fileName = GetFileName("sap.send.ar");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }
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
        /// <summary>
        /// Save Request Exchange Header.
        /// </summary>
        /// <param name="value">The TAAExchangeHeader instance.</param>
        public void WriteQueue(Models.TAAExchangeHeader value)
        {
            if (null == value) return;
            string fileName = GetFileName("save.request.exchange.header");
            string msg = value.ToJson(false);
            WriteFile(fileName, msg);
        }
        /// <summary>
        /// Save Request Exchange Item.
        /// </summary>
        /// <param name="values">The List of TAARequestExchangeItem instance.</param>
        public void WriteQueue(List<Models.TAARequestExchangeItem> values)
        {
            if (null == values) return;
            for (int i = 0; i < values.Count; i++)
            {
                if (null == values[i]) 
                    continue;
                int iCnt = i + 1;
                string fileName = GetFileName("save.request.exchange.item." + iCnt.ToString("D2"));
                string msg = values[i].ToJson(false);
                WriteFile(fileName, msg);
            }
        }
        /// <summary>
        /// Save Approve Exchange Item.
        /// </summary>
        /// <param name="values">The List of TAAApproveExchangeItem instance.</param>
        public void WriteQueue(List<Models.TAAApproveExchangeItem> values)
        {
            if (null == values) return;
            for (int i = 0; i < values.Count; i++)
            {
                if (null == values[i])
                    continue;
                int iCnt = i + 1;
                string fileName = GetFileName("save.approve.exchange.item." + iCnt.ToString("D2"));
                string msg = values[i].ToJson(false);
                WriteFile(fileName, msg);
            }
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Events

        /// <summary>
        /// OnSendExchange event handler.
        /// </summary>
        public event System.EventHandler OnSendExchange;


        #endregion
    }
}
