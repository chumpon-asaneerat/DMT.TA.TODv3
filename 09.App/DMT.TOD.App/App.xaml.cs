#define SINGELTON_APP

#region Using

using System;
using System.Windows;

using NLib;
using NLib.Logs;

using DMT.Configurations;
using DMT.Models;
using DMT.Models.ExtensionMethods;
using DMT.Services;

#endregion

namespace DMT
{
    using taaOps = Services.Operations.TA;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Services.TODWebServer appServ = null;

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
                    ProductName = AppConsts.Application.TOD.ApplicationName,
                    /* For Application Version */
                    Version = AppConsts.Application.TOD.Version,
                    Minor = AppConsts.Application.TOD.Minor,
                    Build = AppConsts.Application.TOD.Build,
                    LastUpdate = AppConsts.Application.TOD.LastUpdate
                },
                /* Setup Storage */
                Storage = new NAppStorage()
                {
                    StorageType = NAppFolder.ProgramData
                },
                /* Setup Behaviors */
                Behaviors = new NAppBehaviors()
                {
                    //***********************************************************************************
                    // NOTE:
                    //***********************************************************************************
                    // WHEN Use in same PC required to change port number otherwiser app will die.
                    //***********************************************************************************
#if SINGELTON_APP
                    /* Set to true for allow only one instance of application can execute an runtime */
                    IsSingleAppInstance = true,
                    /* Set to true for enable Debuggers this value should always be true */
                    EnableDebuggers = true
#else
                    /* Set to true for allow only one instance of application can execute an runtime */
                    IsSingleAppInstance = false,
                    /* Set to true for enable Debuggers this value should always be true */
                    EnableDebuggers = true
#endif
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

            var splash = new TOD.Windows.SplashScreenWindow();
            splash.Setup(5);
            splash.Show();

            // Load Config service.
            splash.Next("Load configuration");
            TODConfigManager.Instance.ConfigChanged += Service_ConfigChanged;
            TODConfigManager.Instance.LoadConfig();

            // Setup config reference to all rest client class.
            splash.Next("Setting up web service client(s).");
            Services.Operations.TA.Config = TODConfigManager.Instance;
            Services.Operations.TA.DMT = TODConfigManager.Instance; // required for NetworkId
            Services.Operations.SCW.Config = TODConfigManager.Instance;
            Services.Operations.SCW.DMT = TODConfigManager.Instance; // required for NetworkId

            splash.Next("Start configuration monitoring service.");
            TODConfigManager.Instance.Start(); // Start File Watcher.

            // Start Web Server.
            splash.Next("Start local TOD (application) web server.");
            appServ = new Services.TODWebServer();
            appServ.Start();

            // Set NotifyService
            TODNotifyService.Instance.TSBChanged += TSBChanged;
            TODNotifyService.Instance.TSBShiftChanged += TSBShiftChanged;

            // Sync data.
            SyncTSBShift();

            // start message resend manager.
            Services.MQResendService.Instance.Start();

            splash.Next("TOD Application successfully loaded.");
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

            // Release NotifyService event.
            TODNotifyService.Instance.TSBChanged -= TSBChanged;
            TODNotifyService.Instance.TSBShiftChanged -= TSBShiftChanged;

            // Shutdown File Watcher.
            TODConfigManager.Instance.Shutdown();

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
            Services.Operations.TA.Config = TODConfigManager.Instance;
            Services.Operations.TA.DMT = TODConfigManager.Instance; // required for NetworkId

            Services.Operations.SCW.Config = TODConfigManager.Instance;
            Services.Operations.SCW.DMT = TODConfigManager.Instance; // required for NetworkId
        }

        private void TSBChanged(object sender, EventArgs e)
        {
            RuntimeManager.Instance.RaiseTSBChanged();
        }

        private void TSBShiftChanged(object sender, EventArgs e)
        {
            SyncTSBShift();
            RuntimeManager.Instance.RaiseTSBShiftChanged();
        }

        private void SyncTSBShift()
        {
            var curr = Models.TSBShift.GetTSBShift().Value();
            var taShift = taaOps.Shift.TSB.Current().Value();
            if (null != taShift)
            {
                if (null != curr && curr.TSBShiftId != taShift.TSBShiftId)
                {
                    Models.TSBShift.ChangeShift(taShift);
                }
            }
        }
    }
}
