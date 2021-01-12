#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

//using NLib.Services;
using DMT.Models;
using DMT.Services;

#endregion


namespace DMT.Controls
{
    /// <summary>
    /// The TAApp class.
    /// </summary>
    public static class TAApp
    {
        /// <summary>
        /// Gets or sets Current TA User.
        /// </summary>
        public static class User
        {
            public static Models.User Current { get; set; }
        }
    }
}
