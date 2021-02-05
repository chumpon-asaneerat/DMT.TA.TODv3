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
    #region IRabbitMQConfig Interface

    /// <summary>
    /// The IRabbitMQConfig inferface.
    /// </summary>
    public interface IRabbitMQConfig
    {
        /// <summary>
        /// Gets RabbitMQ Config.
        /// </summary>
        RabbitMQServiceConfig RabbitMQ { get; }
    }

    #endregion
}
