﻿#region Usings

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
    using tctOps = Services.Operations.TAxTOD.TCT; // reference to static class.

    public enum TODBOJStatus
    {
        HasBOJ = 1,
        NoBOJ = 2,
        WSFailed = 0
    }

    public class TAServerManager
    {
        /// <summary>
        /// Check if User create new shift.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns true if user is already open shift.</returns>
        public static TODBOJStatus CheckTODBoj(string userId)
        {
            TODBOJStatus hasBoj = TODBOJStatus.NoBOJ;

            MethodBase med = MethodBase.GetCurrentMethod();

            try
            {
                var tsb = TSB.GetCurrent().Value();
                if (null != tsb)
                {
                    var search = Search.TAxTOD.CheckBoj.Create(tsb.TSBId, userId);
                    var ret = tctOps.CheckTODBoj(search);
                    if (ret.HttpStatus != HttpStatus.Failed)
                    {
                        // OK.
                        med.Info("CheckTODBoj - Successfully get data from WS on TA Server.");
                        if (ret.Ok && ret.Value().Count > 0)
                        {
                            hasBoj = TODBOJStatus.HasBOJ;
                        }
                        else
                        {
                            hasBoj = TODBOJStatus.NoBOJ;
                        }
                    }
                    else
                    {
                        // Error.
                        med.Err("CheckTODBoj - Cannot connect to WS on TA Server. Allow to received bag.");
                        hasBoj = TODBOJStatus.WSFailed;
                    }
                }
                else
                {
                    med.Err("CheckTODBoj - No TSB Id.");
                }
            }
            catch (Exception ex)
            {
                med.Err(ex);
                med.Err("CheckTODBoj - Detected error. Allow to received bag.");
                hasBoj = TODBOJStatus.WSFailed;
            }

            return hasBoj;
        }
    }
}
