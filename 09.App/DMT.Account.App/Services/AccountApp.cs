#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

//using NLib.Services;
using DMT.Models;
using DMT.Services;

#endregion

namespace DMT
{
    /// <summary>
    /// The AccountApp class.
    /// </summary>
    public static class AccountApp
    {
        /// <summary>
        /// Gets or sets Current Account User.
        /// </summary>
        public static class User
        {
            public static Models.User Current { get; set; }
        }
    }
}
