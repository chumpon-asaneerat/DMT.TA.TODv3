#region Using

using System.ComponentModel;
using NLib.ServiceProcess;

#endregion

namespace DMT.Services
{
    #region PlazaDataServiceManager (Installer)

    /// <summary>
    /// Plaza Data Service Manager.
    /// </summary>
    [RunInstaller(true)]
    public class PlazaDataServiceManager : NServiceInstaller
    {
        #region Internal Variables

        private PlazaDataService _service = new PlazaDataService();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlazaDataServiceManager()
            : base()
        {
            ServiceName = AppConsts.WindowsService.Local.ServiceName;
            DisplayName = AppConsts.WindowsService.Local.DisplayName;
            Description = AppConsts.WindowsService.Local.Description;
        }

        #endregion

        #region Override properties

        /// <summary>
        /// Gets the service instance.
        /// </summary>
        public override NServiceBase Service
        {
            get
            {
                return _service;
            }
        }

        #endregion
    }

    #endregion
}
