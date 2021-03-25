#region Usings

using System;
using DMT.Models;
using DMT.Configurations;

#endregion

namespace DMT.Services.Operations
{
    /// <summary>
    /// The TAxTOD Operation class.
    /// </summary>
    public static partial class TAxTOD 
    {
        #region Static Methods

        #region Get Client

        /// <summary>
        /// Get Client.
        /// </summary>
        /// <returns>Returns NRestClient instance.</returns>
        public static NRestClient GetClient()
        {
            return NRestClient.CreateTAxTODClient(Config);
        }

        #endregion

        #region Execute(s)

        /// <summary>
        /// Execute.
        /// </summary>
        /// <param name="url">The api url.</param>
        /// <returns>Returns NRestResult instance.</returns>
        public static NRestResult Execute(string url)
        {
            NRestResult ret;
            NRestClient client = GetClient();
            if (null == client)
            {
                ret = new NRestResult();
                ret.RestInvalidConfig();
                return ret;
            }
            ret = client.Execute(url, new { }, Timeout, UserName, Password);
            return ret;

        }
        /// <summary>
        /// Execute.
        /// </summary>
        /// <param name="url">The api url.</param>
        /// <param name="value">The parameter object.</param>
        /// <returns>Returns NRestResult instance.</returns>
        public static NRestResult Execute(string url, object value)
        {
            NRestResult ret;
            NRestClient client = GetClient();
            if (null == client)
            {
                ret = new NRestResult();
                ret.RestInvalidConfig();
                return ret;
            }
            ret = client.Execute(url, value, Timeout, UserName, Password);
            return ret;

        }
        /// <summary>
        /// Execute.
        /// </summary>
        /// <typeparam name="TResult">The Result type paramter.</typeparam>
        /// <param name="url">The api url.</param>
        /// <returns>Returns NRestResult instance.</returns>
        public static NRestResult<TResult> Execute<TResult>(string url)
            where TResult : new()
        {
            NRestResult<TResult> ret;
            NRestClient client = GetClient();
            if (null == client)
            {
                ret = new NRestResult<TResult>();
                ret.RestInvalidConfig();
                return ret;
            }

            ret = client.Execute<TResult>(url, new { }, Timeout, UserName, Password);
            return ret;

        }
        /// <summary>
        /// Execute.
        /// </summary>
        /// <typeparam name="TResult">The Result type paramter.</typeparam>
        /// <param name="url">The api url.</param>
        /// <param name="value">The parameter object.</param>
        /// <returns>Returns NRestResult instance.</returns>
        public static NRestResult<TResult> Execute<TResult>(string url, object value)
            where TResult : new()
        {
            NRestResult<TResult> ret;
            NRestClient client = GetClient();
            if (null == client)
            {
                ret = new NRestResult<TResult>();
                ret.RestInvalidConfig();
                return ret;
            }

            ret = client.Execute<TResult>(url, value, Timeout, UserName, Password);
            return ret;

        }
        /// <summary>
        /// Execute.
        /// </summary>
        /// <typeparam name="TResult">The Result type paramter.</typeparam>
        /// <param name="url">The api url.</param>
        /// <param name="value">The parameter object.</param>
        /// <param name="timeout">The Timeout in millisecond.</param>
        /// <returns>Returns NRestResult instance.</returns>
        public static NRestResult<TResult> Execute<TResult>(string url, object value, int timeout)
            where TResult : new()
        {
            NRestResult<TResult> ret;
            NRestClient client = GetClient();
            if (null == client)
            {
                ret = new NRestResult<TResult>();
                ret.RestInvalidConfig();
                return ret;
            }

            ret = client.Execute<TResult>(url, value, timeout, UserName, Password);
            return ret;

        }
        /// <summary>
        /// Execute.
        /// </summary>
        /// <typeparam name="TResult">The Result type paramter.</typeparam>
        /// <typeparam name="TOut">The Output type paramter</typeparam>
        /// <param name="url">The api url.</param>
        /// <returns>Returns NRestResult instance.</returns>
        public static NRestResult<TResult, TOut> Execute<TResult, TOut>(string url)
            where TResult : new()
            where TOut : new()
        {
            NRestResult<TResult, TOut> ret;
            NRestClient client = GetClient();
            if (null == client)
            {
                ret = new NRestResult<TResult, TOut>();
                ret.RestInvalidConfig();
                return ret;
            }

            ret = client.Execute<TResult, TOut>(url, new { }, Timeout, UserName, Password);
            return ret;

        }
        /// <summary>
        /// Execute.
        /// </summary>
        /// <typeparam name="TResult">The Result type paramter.</typeparam>
        /// <typeparam name="TOut">The Output type paramter</typeparam>
        /// <param name="url">The api url.</param>
        /// <param name="value">The parameter object.</param>
        /// <returns>Returns NRestResult instance.</returns>
        public static NRestResult<TResult, TOut> Execute<TResult, TOut>(string url, object value)
            where TResult : new()
            where TOut : new()
        {
            NRestResult<TResult, TOut> ret;
            NRestClient client = GetClient();
            if (null == client)
            {
                ret = new NRestResult<TResult, TOut>();
                ret.RestInvalidConfig();
                return ret;
            }

            ret = client.Execute<TResult, TOut>(url, value, Timeout, UserName, Password);
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
        public static ITAxTODConfig Config { get; set; }
        /// <summary>
        /// Gets Base Address.
        /// </summary>
        public static string BaseAddress
        {
            get
            {
                if (null == Config) return string.Empty;
                if (null == Config.TAxTOD) return string.Empty;
                if (null == Config.TAxTOD.Service) return string.Empty;

                return string.Format(@"{0}://{1}:{2}/",
                    Config.TAxTOD.Service.Protocol,
                    Config.TAxTOD.Service.HostName,
                    Config.TAxTOD.Service.PortNumber);
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
                if (null == Config.TAxTOD) return string.Empty;
                if (null == Config.TAxTOD.Service) return string.Empty;

                return Config.TAxTOD.Service.UserName;
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
                if (null == Config.TAxTOD) return string.Empty;
                if (null == Config.TAxTOD.Service) return string.Empty;

                return Config.TAxTOD.Service.Password;
            }
        }

        #endregion
    }
}
