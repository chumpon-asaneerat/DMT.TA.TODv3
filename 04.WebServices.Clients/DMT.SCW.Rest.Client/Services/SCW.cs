#region Usings

using System;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    /// <summary>
    /// The SCW Operation class.
    /// </summary>
    public static partial class SCW 
    {
        #region Static Methods

        #region Get Client

        /// <summary>
        /// Get Client.
        /// </summary>
        /// <returns>Returns NRestClient instance.</returns>
        public static NRestClient GetClient()
        {
            return NRestClient.CreateSCWClient(Config);
        }

        #endregion

        #region Execute(s)

        /// <summary>
        /// Execute.
        /// </summary>
        /// <typeparam name="TResult">The Result type paramter.</typeparam>
        /// <param name="url">The api url.</param>
        /// <returns>Returns TResult instance.</returns>
        public static TResult Execute<TResult>(string url)
            where TResult : new()
        {
            TResult ret = default(TResult);
            NRestClient client = GetClient();
            if (null != client)
            {
                ret = client.Execute2<TResult>(url, new { }, Timeout, UserName, Password);
            }
            return ret;

        }
        /// <summary>
        /// Execute.
        /// </summary>
        /// <typeparam name="TResult">The Result type paramter.</typeparam>
        /// <param name="url">The api url.</param>
        /// <param name="value">The parameter object.</param>
        /// <returns>Returns TResult instance.</returns>
        public static TResult Execute<TResult>(string url, object value)
            where TResult : new()
        {
            TResult ret = default(TResult);
            NRestClient client = GetClient();
            if (null != client)
            {
                if (null != value)
                    ret = client.Execute2<TResult>(url, value, Timeout, UserName, Password);
                else ret = client.Execute2<TResult>(url, new { }, Timeout, UserName, Password);
            }
            return ret;

        }
        /// <summary>
        /// Execute.
        /// </summary>
        /// <typeparam name="TResult">The Result type paramter.</typeparam>
        /// <typeparam name="TValue">The Value type parameter.</typeparam>
        /// <param name="url">The api url.</param>
        /// <param name="value">The parameter object.</param>
        /// <returns>Returns TResult instance.</returns>
        public static TResult Execute<TResult, TValue>(string url, TValue value)
            where TResult : new()
        {
            TResult ret = default(TResult);
            NRestClient client = GetClient();
            if (null != client && null != value)
            {
                ret = client.Execute2<TResult>(url, value, Timeout, UserName, Password);
            }
            return ret;

        }

        #endregion

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets or sets DMT config.
        /// </summary>
        public static IDMTConfig DMT { get; set; }
        /// <summary>
        /// Gets NetworkId.
        /// </summary>
        public static int NetworkId
        {
            get 
            {
                if (null == DMT) return 0;
                if (null == DMT.DMT) return 0;
                return DMT.DMT.networkId;
            }
        }
        /// <summary>
        /// Gets or sets service config.
        /// </summary>
        public static ISCWConfig Config { get; set; }
        /// <summary>
        /// Gets Base Address.
        /// </summary>
        public static string BaseAddress
        {
            get
            {
                if (null == Config) return string.Empty;
                if (null == Config.SCW) return string.Empty;
                if (null == Config.SCW.Service) return string.Empty;

                return string.Format(@"{0}://{1}:{2}/",
                    Config.SCW.Service.Protocol,
                    Config.SCW.Service.HostName,
                    Config.SCW.Service.PortNumber);
            }
        }
        /// <summary>
        /// Gets default execute timeout.
        /// </summary>
        public static int Timeout { get { return 1000; } }
        /// <summary>
        /// Gets user name.
        /// </summary>
        public static string UserName
        {
            get
            {
                if (null == Config) return string.Empty;
                if (null == Config.SCW) return string.Empty;
                if (null == Config.SCW.Service) return string.Empty;

                return Config.SCW.Service.UserName;
            }
        }
        /// <summary>
        /// Gets password.
        /// </summary>
        public static string Password
        {
            get
            {
                if (null == Config) return string.Empty;
                if (null == Config.SCW) return string.Empty;
                if (null == Config.SCW.Service) return string.Empty;

                return Config.SCW.Service.Password;
            }
        }

        #endregion
    }
}
