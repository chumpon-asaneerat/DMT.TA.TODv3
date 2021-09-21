#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    #region Parameter class

    /// <summary>The SCWChangePassword class.</summary>
    public class SCWChangePassword
    {
        /// <summary>Gets or sets staffId.</summary>
        [PropertyMapName("staffId")]
        public string staffId { get; set; }

        /// <summary>Gets or sets password.</summary>
        [PropertyMapName("password")]
        public string password { get; set; }

        /// <summary>Gets or sets newPassword.</summary>
        [PropertyMapName("newPassword")]
        public string newPassword { get; set; }

        /// <summary>Gets or sets confirmNewPassword.</summary>
        [PropertyMapName("confirmNewPassword")]
        public string confirmNewPassword { get; set; }
    }

    #endregion

    #region Result related classes

    /// <summary>The SCWChangePasswordResult class.</summary>
    public class SCWChangePasswordResult : SCWResult
    {

    }

    #endregion
}
