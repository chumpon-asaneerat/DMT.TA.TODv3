using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using NLib;
using NLib.Logs;

namespace DMT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region Create Application Environment Options

            EnvironmentOptions option = new EnvironmentOptions()
            {
                /* Setup Application Information */
                AppInfo = new NAppInformation()
                {
                    /*  This property is required */
                    CompanyName = "DMT",
                    /*  This property is required */
                    ProductName = AppConsts.Application.PlazaServiceManager.ApplicationName,
                    /* For Application Version */
                    Version = AppConsts.Application.PlazaServiceManager.Version,
                    Minor = AppConsts.Application.PlazaServiceManager.Minor,
                    Build = AppConsts.Application.PlazaServiceManager.Build,
                    LastUpdate = AppConsts.Application.PlazaServiceManager.LastUpdate
                },
                /* Setup Storage */
                Storage = new NAppStorage()
                {
                    StorageType = NAppFolder.Custom,
                    /*  This property should set only when StorageType is Custom */
                    CustomFolder = @"D:\TestConfig\"
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

            WinAppContoller.Instance.Setup(option);

            if (option.Behaviors.IsSingleAppInstance &&
                WinAppContoller.Instance.HasMoreInstance)
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

            Form form = new MainForm();

            if (null != form)
            {
                WinAppContoller.Instance.Run(form);
            }

            // Shutdown log manager
            LogManager.Instance.Shutdown();

            WinAppContoller.Instance.Shutdown(true, 500);
        }
    }
}
