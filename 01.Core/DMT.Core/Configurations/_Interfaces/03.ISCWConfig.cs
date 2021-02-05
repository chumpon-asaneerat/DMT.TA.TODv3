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
    #region ISCWConfig Interface

    /// <summary>
    /// The ISCWConfig inferface.
    /// </summary>
    public interface ISCWConfig
    {
        /// <summary>
        /// Gets SCW Config.
        /// </summary>
        SCWWebServiceConfig SCW { get; }
    }

    #endregion
}
