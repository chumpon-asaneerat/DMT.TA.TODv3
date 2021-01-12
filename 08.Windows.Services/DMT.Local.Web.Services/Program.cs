#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Reflection;
using NLib;
using NLib.Logs;

#endregion

namespace DMT
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			bool usedForm = false;
			MethodBase med = MethodBase.GetCurrentMethod();
			try
			{
				#region Create Application Environment Options

				EnvironmentOptions option = new EnvironmentOptions()
				{
					/* Setup Application Information */
					AppInfo = new NAppInformation()
					{
						/*  This property is required */
						CompanyName = "DMT",
						/*  This property is required */
						ProductName = AppConsts.WindowsService.Local.ServiceName,
						/* For Application Version */
						Version = AppConsts.WindowsService.Local.Version,
						Minor = AppConsts.WindowsService.Local.Minor,
						Build = AppConsts.WindowsService.Local.Build,
						LastUpdate = AppConsts.WindowsService.Local.LastUpdate
					},
					/* Setup Storage */
					Storage = new NAppStorage()
					{
						StorageType = NAppFolder.ProgramData
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

				#region Check Arguments

				if (null != args && args.Length > 0)
				{
					if (string.Compare(args[0], "-debug", true) == 0 ||
						string.Compare(args[0], "-d", true) == 0)
					{
						usedForm = true;
					}
				}

				#endregion

				#region Setup Option to Controller and check instance

				if (usedForm)
				{
					WinAppContoller.Instance.Setup(option);

					if (option.Behaviors.IsSingleAppInstance &&
						WinAppContoller.Instance.HasMoreInstance)
					{
						return;
					}
				}
				else
				{
					WinServiceContoller.Instance.Setup(option);
				}

				#endregion

				#region Init Preload classes

				ApplicationManager.Instance.Preload(() =>
				{
					//NLib.Data.FirebirdConfig.Prepare();
				});

				#endregion

				#region Load Main UI or Service

				med.Err("App Instance GUID : " +
					ApplicationManager.InstanceGuid.ToString());

				if (usedForm)
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);

					// Start log manager
					LogManager.Instance.Start();


					Forms.MainForm form = new Forms.MainForm();

					if (null != form)
					{
						WinAppContoller.Instance.Run(form);
					}
				}
				else
				{
					// Start log manager
					LogManager.Instance.Start();

					var manager =
						new Services.PlazaDataServiceManager();
					WinServiceContoller.Instance.Run(manager, args);
				}

				#endregion
			}
			catch (Exception ex)
			{
				if (usedForm)
				{
					MessageBox.Show(ex.ToString());
				}
				med.Err(ex);
			}
			finally
			{
				// Shutdown log manager
				LogManager.Instance.Shutdown();

				if (usedForm)
					WinAppContoller.Instance.Shutdown(true);
				else WinServiceContoller.Instance.Shutdown(true);
			}
		}
	}
}
