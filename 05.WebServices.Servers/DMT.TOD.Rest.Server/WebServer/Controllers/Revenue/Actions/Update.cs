#region Using

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Reflection;
using NLib;
using DMT.Models;

#endregion

namespace DMT.Services
{
    partial class RevenueController
    {
        [HttpPost]
        [ActionName(RouteConsts.TOD.Revenue.Update.Name)]
        //[AllowAnonymous]
        public NDbResult Update([FromBody] Models.RevenueEntry value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var ret = Models.RevenueEntry.Save(value);
            if (null != ret && ret.Ok)
            {
                med.Info("Sync Revenue Entry from TA - Success.");
            }
            else
            {
                med.Info("Sync Revenue Entry from TA - Failed.");
            }
            return ret;
        }
    }
}
