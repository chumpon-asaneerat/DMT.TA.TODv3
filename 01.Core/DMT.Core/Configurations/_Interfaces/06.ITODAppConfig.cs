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
    #region ITODAppConfig Interface

    /// <summary>
    /// The ITODAppConfig inferface.
    /// </summary>
    public interface ITODAppConfig
    {
        /// <summary>
        /// Gets TODApp Config.
        /// </summary>
        TODAppWebServiceConfig TODApp { get; }
    }

    #endregion
}
