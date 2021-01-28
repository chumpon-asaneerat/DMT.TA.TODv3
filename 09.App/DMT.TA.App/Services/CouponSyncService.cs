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

using DMT.Configurations;
using DMT.Controls;
using DMT.Services;
using DMT.Models;
using DMT.Models.ExtensionMethods;

using NLib;
using NLib.IO;
using NLib.Services;
using NLib.Reports.Rdlc;
using NLib.Reflection;

using RestSharp;

#endregion

namespace DMT.Services
{
    using taServerops = Services.Operations.TAxTOD; // reference to static class.

    #region EventHandler and EventArgs

    #endregion

    #region CouponSyncService

    /// <summary>
    /// The Coupon Sync Service class.
    /// </summary>
    public class CouponSyncService
    {
        #region Singelton

        private static CouponSyncService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static CouponSyncService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(CouponSyncService))
                    {
                        _instance = new CouponSyncService();
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
        private CouponSyncService() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~CouponSyncService()
        {
            Shutdown();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start.
        /// </summary>
        public void Start()
        {

        }
        /// <summary>
        /// Shutdown.
        /// </summary>
        public void Shutdown()
        {

        }

        #endregion

        #region Public Events



        #endregion
    }

    #endregion
}
