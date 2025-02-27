﻿#define ENABLE_SYNC_SERVICE

#region Using

using System;
using System.Windows;

using NLib;
using NLib.Logs;

using DMT.Configurations;
using DMT.Services;

#endregion

namespace DMT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Services.TAWebServer appServ = null;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public App() : base()
        {
            if (null != AppDomain.CurrentDomain)
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            }
        }

        #endregion

        #region Unhandle exception

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ToString(), "Application Unhandle Exception.");
        }

        #endregion

        /// <summary>
        /// OnStartup.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Console.WriteLine("OnStartUp");
            if (null != AppDomain.CurrentDomain)
            {
                if (null != System.Threading.Thread.CurrentContext)
                {
                    Console.WriteLine("Thread CurrentContext is not null.");
                }
            }

            #region Create Application Environment Options

            EnvironmentOptions option = new EnvironmentOptions()
            {
                /* Setup Application Information */
                AppInfo = new NAppInformation()
                {
                    /*  This property is required */
                    CompanyName = "DMT",
                    /*  This property is required */
                    ProductName = AppConsts.Application.TA.ApplicationName,
                    /* For Application Version */
                    Version = AppConsts.Application.TA.Version,
                    Minor = AppConsts.Application.TA.Minor,
                    Build = AppConsts.Application.TA.Build,
                    LastUpdate = AppConsts.Application.TA.LastUpdate
                },
                /* Setup Storage */
                Storage = new NAppStorage()
                {
                    StorageType = NAppFolder.Application
                },
                /* Setup Behaviors */
                Behaviors = new NAppBehaviors()
                {
                    /* Set to true for allow only one instance of application can execute an runtime */
                    IsSingleAppInstance = true,
                    /* Set to true for enable Debuggers this value should always be true */
                    EnableDebuggers = true
                }
            };

            #endregion

            #region Setup Option to Controller and check instance

            WpfAppContoller.Instance.Setup(option);

            if (option.Behaviors.IsSingleAppInstance &&
                WpfAppContoller.Instance.HasMoreInstance)
            {
                return;
            }

            #endregion

            #region Init Preload classes

            ApplicationManager.Instance.Preload(() =>
            {

            });

            #endregion

            // Start log manager
            LogManager.Instance.Start();

            var splash = new TA.Windows.SplashScreenWindow();
            splash.Setup(5);
            splash.Show();

            // Load Config service.
            splash.Next("Load configuration");
            TAConfigManager.Instance.ConfigChanged += Service_ConfigChanged;
            TAConfigManager.Instance.LoadConfig();

            // Setup config reference to all rest client class.
            splash.Next("Setting up web service client(s).");
            Services.Operations.TAxTOD.Config = TAConfigManager.Instance;
            Services.Operations.TAxTOD.DMT = TAConfigManager.Instance; // required for NetworkId
            Services.Operations.SCW.Config = TAConfigManager.Instance;
            Services.Operations.SCW.DMT = TAConfigManager.Instance; // required for NetworkId

            splash.Next("Start configuration monitoring service.");
            TAConfigManager.Instance.Start(); // Start File Watcher.

            // Start Web Server.
            splash.Next("Start local TA (application) web server.");
            appServ = new Services.TAWebServer();
            appServ.Start();

            // Init NotifyService event.
            TANotifyService.Instance.TSBChanged += TSBChanged;
            TANotifyService.Instance.TSBShiftChanged += TSBShiftChanged;

            // Start coupon sync service.
#if ENABLE_SYNC_SERVICE
            CouponSyncService.Instance.Start();
            ExchangeSyncService.Instance.Start();
#endif
            // start message resend manager.
            Services.MQResendService.Instance.Start();

            splash.Next("TA Application successfully loaded.");
            splash.Wait(250);

            Window window = null;
            window = new MainWindow();

            if (null != window)
            {
                WpfAppContoller.Instance.Run(window);
            }

            splash.Close();
            splash = null;
        }

        /// <summary>
        /// OnExit
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            // shutdown message resend manager.
            Services.MQResendService.Instance.Shutdown();

            // Shutdown coupon sync service.
#if ENABLE_SYNC_SERVICE
            ExchangeSyncService.Instance.Shutdown();
            CouponSyncService.Instance.Shutdown();
#endif

            // Release NotifyService event.
            TANotifyService.Instance.TSBChanged -= TSBChanged;
            TANotifyService.Instance.TSBShiftChanged -= TSBShiftChanged;

            // Shutdown File Watcher.
            TAConfigManager.Instance.Shutdown();

            if (null != appServ)
            {
                appServ.Shutdown();
            }
            appServ = null;

            // Shutdown log manager
            LogManager.Instance.Shutdown();

            // Wpf shutdown process required exit code.

            /* If auto close the single instance must be true */
            bool autoCloseProcess = true;
            WpfAppContoller.Instance.Shutdown(autoCloseProcess, e.ApplicationExitCode);

            base.OnExit(e);
        }

        private void Service_ConfigChanged(object sender, EventArgs e)
        {
            // When Service Config file changed.
            // Update all related service operations.
            Services.Operations.TAxTOD.Config = TAConfigManager.Instance;
            Services.Operations.TAxTOD.DMT = TAConfigManager.Instance; // required for NetworkId

            Services.Operations.SCW.Config = TAConfigManager.Instance;
            Services.Operations.SCW.DMT = TAConfigManager.Instance; // required for NetworkId
        }

        private void TSBChanged(object sender, EventArgs e)
        {
            RuntimeManager.Instance.RaiseTSBChanged();
        }

        private void TSBShiftChanged(object sender, EventArgs e)
        {
            RuntimeManager.Instance.RaiseTSBShiftChanged(); // notify UI to update TSB Shift.
        }
    }
}
