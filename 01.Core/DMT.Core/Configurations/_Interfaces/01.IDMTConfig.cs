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
    #region IDMTConfig Interface

    /// <summary>
    /// The IDMTConfig inferface.
    /// </summary>
    public interface IDMTConfig
    {
        /// <summary>
        /// Gets DMT Config.
        /// </summary>
        DMTConfig DMT { get; }
    }

    #endregion
}
