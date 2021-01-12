#region Using

using System;
using System.IO;
using System.Reflection;
using NLib;

#endregion

namespace DMT.Services
{
    public class SCWResourceManager
    {
        #region Resouce Related Methods and Properties

        private static Assembly Current { get { return typeof(SCWResourceManager).Assembly; } }

        /// <summary>
        /// Gets View SQL Script (from embedded resource).
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>Returns load view sql script.</returns>
        public static string GetScript(string resourceName)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            string ret = string.Empty;
            if (!string.IsNullOrWhiteSpace(resourceName))
            {
                try
                {
                    using (Stream stream = Current.GetManifestResourceStream(resourceName))
                    {
                        if (null != stream)
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                if (null != reader)
                                {
                                    ret = reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    ret = string.Empty;
                }
            }
            return ret;
        }
        /// <summary>
        /// Gets Json Resource Path.
        /// </summary>
        public static string JsonResourcePath { get { return "DMT.Resources.Json"; } }

        private static string GetEmbeddedResourceName(string resourceName)
        {
            return JsonResourcePath + "." + resourceName + ".json";
        }

        private static string GetJson(string resourceName)
        {
            string script = string.Empty;
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                string embededResourceName = GetEmbeddedResourceName(resourceName);
                script = GetScript(embededResourceName);
            }
            catch (Exception ex)
            {
                med.Err(ex);
                script = string.Empty;
            }
            return script;
        }

        #endregion

        #region public properties (static)

        /// <summary>
        /// Gets loginAudit parameter json script.
        /// </summary>
        public static string loginAudit { get { return GetJson("loginAudit"); } }
        /// <summary>
        /// Gets cardAllowList parameter json script.
        /// </summary>
        public static string cardAllowList { get { return GetJson("cardAllowList"); } }
        /// <summary>
        /// Gets couponBookList parameter json script.
        /// </summary>
        public static string couponBookList { get { return GetJson("couponBookList"); } }
        /// <summary>
        /// Gets couponList parameter json script.
        /// </summary>
        public static string couponList { get { return GetJson("couponList"); } }
        /// <summary>
        /// Gets currencyDenomList parameter json script.
        /// </summary>
        public static string currencyDenomList { get { return GetJson("currencyDenomList"); } }
        /// <summary>
        /// Gets jobList parameter json script.
        /// </summary>
        public static string jobList { get { return GetJson("jobList"); } }
        /// <summary>
        /// Gets jobList2 parameter json script.
        /// </summary>
        public static string jobList2 { get { return GetJson("jobList2"); } }
        /// <summary>
        /// Gets declare parameter json script.
        /// </summary>
        public static string declare { get { return GetJson("declare"); } }
        /// <summary>
        /// Gets emvTransactionList parameter json script.
        /// </summary>
        public static string emvTransactionList { get { return GetJson("emvTransactionList"); } }
        /// <summary>
        /// Gets qrcodeTransactionList parameter json script.
        /// </summary>
        public static string qrcodeTransactionList { get { return GetJson("qrcodeTransactionList"); } }
        /// <summary>
        /// Gets saveCheifDuty parameter json script.
        /// </summary>
        public static string saveCheifDuty { get { return GetJson("saveCheifDuty"); } }
        /// <summary>
        /// Gets cheifOnDuty parameter json script.
        /// </summary>
        public static string cheifOnDuty { get { return GetJson("cheifOnDuty"); } }
        /// <summary>
        /// Gets changePassword parameter json script.
        /// </summary>
        public static string changePassword { get { return GetJson("changePassword"); } }
        /// <summary>
        /// Gets passwordExpiresDays parameter json script.
        /// </summary>
        public static string passwordExpiresDays { get { return GetJson("passwordExpiresDays"); } }

        #endregion
    }
}
