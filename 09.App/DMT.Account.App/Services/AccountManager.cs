#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.IO;
//using System.Windows.Forms;
//using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection;

using DMT.Configurations;
using DMT.Controls;
using DMT.Services;
using DMT.Models;
using DMT.Models.ExtensionMethods;

using NLib;
using NLib.IO;
using NLib.Services;
using NLib.Reflection;

using RestSharp;
using System.Windows.Media;

#endregion

namespace DMT.Services
{
    #region AccountAPI

    /// <summary>
    /// The AccountAPI class.
    /// </summary>
    public static class AccountAPI
    {
        #region Static Properties

        /// <summary>The NetWorkId for SCW.</summary>
        public static int NetworkId
        {
            get { return AccountConfigManager.Instance.DMT.networkId; }
        }

        #region Shifts

        /// <summary>
        /// Gets Shifts.
        /// </summary>
        public static List<Models.Shift> Shifts
        {
            get { return Models.Shift.GetShifts().Value(); }
        }

        #endregion

        #endregion

        #region Static Methods

        /// <summary>
        /// Search User By partial User Id.
        /// </summary>
        /// <param name="userId">The partial User Id.</param>
        /// <param name="permissions">The permission roles.</param>
        /// <param name="title">The Window Title (optional).</param>
        /// <returns>Returns UserSearchResult instance.</returns>
        public static UserSearchResult SearchUser(string userId,
            string[] permissions,
            string title = "กรุณาเลือกพนักงานเก็บเงิน")
        {
            if (string.IsNullOrEmpty(userId))
                return new UserSearchResult() { User = null, IsCanceled = true };
            UserSearchManager.Instance.Title = title;
            return UserSearchManager.Instance.SelectUser(userId, permissions);
        }

        #endregion
    }

    #endregion
}
