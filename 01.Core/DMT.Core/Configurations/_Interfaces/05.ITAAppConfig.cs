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
    #region ITAAppConfig Interface

    /// <summary>
    /// The ITAAppConfig inferface.
    /// </summary>
    public interface ITAAppConfig
    {
        /// <summary>
        /// Gets TAApp Config.
        /// </summary>
        TAAppWebServiceConfig TAApp { get; }
    }

    #endregion
}
