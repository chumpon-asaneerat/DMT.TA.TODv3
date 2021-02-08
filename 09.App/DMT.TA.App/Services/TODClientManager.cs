#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
//using System.Windows.Forms;
//using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Threading;

using DMT.Configurations;
using DMT.Services;
using DMT.Models;
using DMT.Models.ExtensionMethods;

using NLib;
using NLib.IO;
using NLib.Services;
using NLib.Reflection;

using RestSharp;
using System.Threading.Tasks;

#endregion

namespace DMT.Services
{
    using ops = Operations.TOD.Notify;

    /// <summary>
    /// The TODClientManager class.
    /// </summary>
    public class TODClientManager
    {
        #region Singelton

        private static TODClientManager _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static TODClientManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TODClientManager))
                    {
                        _instance = new TODClientManager();
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
        private TODClientManager() : base()
        {
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TODClientManager()
        {

        }

        #endregion

        #region Public Methods

        #region Change

        /// <summary>
        /// Raise all TOD TSBShiftChanged.
        /// </summary>
        /// <param name="value"></param>
        public void TODTSBShiftChanged()
        {
            if (null == TAConfigManager.Instance.Value || null == TAConfigManager.Instance.Value.Services) return;
            var configs = TAConfigManager.Instance.Value.Services.TODApps;

            // Set operation DMT.
            Operations.TOD.DMT = TAConfigManager.Instance; // required for NetworkId

            if (null != configs && configs.Count > 0)
            {
                configs.ForEach(cfg =>
                {
                    Operations.TOD.Config = cfg; // varies by client config
                    // Notify TOD - TSBShiftChanged.
                    var ret = ops.TSBShiftChanged();
                    if (null == ret || ret.Failed)
                    {
                        Console.WriteLine("Send to TOD failed.");
                    }
                });
            }
        }

        #endregion

        #endregion
    }
}
