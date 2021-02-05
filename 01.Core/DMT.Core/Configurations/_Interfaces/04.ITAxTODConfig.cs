#region Using

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using NLib;
using NLib.IO;
using Newtonsoft.Json;
using NLib.Controls.Design;

#endregion

namespace DMT.Configurations
{
    #region ITAxTODConfig Interface

    /// <summary>
    /// The ITAxTODConfig inferface.
    /// </summary>
    public interface ITAxTODConfig
    {
        /// <summary>
        /// Gets TAxTOD Config.
        /// </summary>
        TAxTODWebServiceConfig TAxTOD { get; }
    }

    #endregion
}
