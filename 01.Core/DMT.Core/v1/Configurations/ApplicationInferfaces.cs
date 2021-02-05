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
