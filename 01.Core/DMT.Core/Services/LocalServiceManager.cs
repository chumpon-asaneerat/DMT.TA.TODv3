#region Usings

using System;
using System.Collections.Generic;
using NLib;
using NLib.ServiceProcess;

#endregion

namespace DMT.Services
{
    #region InstalledStatus

    /// <summary>
    /// The DMT Window Service Installed Status class.
    /// </summary>
    public class InstalledStatus
    {
        #region Public properties

        /// <summary>
        /// Gets (or internal set) all service count.
        /// </summary>
        public int ServiceCount { get; set; }
        /// <summary>
        /// Gets (or internal set) service install count.
        /// </summary>
        public int InstalledCount { get; set; }
        /// <summary>
        /// Gets (or internal set) is Plaza Local Service installed.
        /// </summary>
        public bool PlazaLocalServiceInstalled { get; set; }

        #endregion
    }

    #endregion

    #region LocalServiceManager

    /// <summary>
    /// The Local Windows Service Manager class.
    /// </summary>
    public class LocalServiceManager
    {
        #region Singelton

        private static LocalServiceManager _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static LocalServiceManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(LocalServiceManager))
                    {
                        _instance = new LocalServiceManager();
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
        private LocalServiceManager() : base()
        {
            ServiceMonitor = new NServiceMonitor();
            // Init windows service monitor.
            InitWindowsServices();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~LocalServiceManager()
        {
            // Shutdown windows service monitor.
            if (null != ServiceMonitor)
            {
                ServiceMonitor.Shutdown();
            }
            ServiceMonitor = null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Init windows service list to monitor.
        /// </summary>
        private void InitWindowsServices()
        {
            if (null == ServiceMonitor)
                return;
            // Init Service to monitor
            ServiceMonitor.ServiceNames.Clear();
            string path = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);

            // Append Local Plaza Window Service application
            ServiceMonitor.ServiceNames.Add(
                new NServiceName()
                {
                    // The Service Name must match the name that declare name 
                    // in NServiceInstaller inherited class
                    ServiceName = DMT.AppConsts.WindowsService.Local.ServiceName,
                    // The File Name must match actual path related to entry (main execute)
                    // assembly.
                    FileName = System.IO.Path.Combine(path, AppConsts.WindowsService.Local.ExecutableFileName)
                });
        }

        #endregion

        #region Public Methods

        #region Install/Uninstall/CheckInstalled

        /// <summary>
        /// Install all registered windows services.
        /// </summary>
        public void Install()
        {
            if (null == ServiceMonitor)
                return;
            ServiceMonitor.InstallAll();
        }
        /// <summary>
        /// Uninstall all registered windows services.
        /// </summary>
        public void Uninstall()
        {
            if (null == ServiceMonitor)
                return;
            ServiceMonitor.UninstallAll();
        }
        /// <summary>
        /// Checks services installed status.
        /// </summary>
        /// <returns>Returns ServiceStatus instance.</returns>
        public InstalledStatus CheckInstalled()
        {
            InstalledStatus result = new InstalledStatus();
            result.ServiceCount = 0;
            result.InstalledCount = 0;
            result.PlazaLocalServiceInstalled = false;
            if (null != ServiceMonitor)
            {
                try
                {
                    NServiceInfo[] srvs = ServiceMonitor.ServiceInformations;
                    if (null != srvs)
                    {
                        result.ServiceCount = srvs.Length;
                        foreach (NServiceInfo srvInfo in srvs)
                        {
                            if (srvInfo.IsInstalled)
                            {
                                ++result.InstalledCount;
                                if (srvInfo.ServiceName == AppConsts.WindowsService.Local.ServiceName)
                                {
                                    result.PlazaLocalServiceInstalled = true;
                                }
                            }
                        }
                    }
                }
                catch { }
            }
            return result; // return scan result.
        }

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Instance of Windows Services Monitor.
        /// </summary>
        public NServiceMonitor ServiceMonitor { get; private set; }

        #endregion
    }

    #endregion
}
