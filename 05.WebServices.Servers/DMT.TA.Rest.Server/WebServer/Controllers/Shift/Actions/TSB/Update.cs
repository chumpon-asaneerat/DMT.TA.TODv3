#region Using

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Web.Http;
using NLib;
using DMT.Models;

#endregion

namespace DMT.Services
{
    partial class Shift
    {
        partial class TSBController
        {
            [HttpPost]
            [ActionName(RouteConsts.TA.Shift.TSB.Update.Name)]
            //[AllowAnonymous]
            public NDbResult Update([FromBody] Models.TSBShift value)
            {
                MethodBase med = MethodBase.GetCurrentMethod();

                var ret = Models.TSBShift.ChangeShift(value);
                if (null != ret && ret.Ok)
                {
                    med.Info("Generate TSBShift files (prepare for sent to all TODs).");
                    // Write to Queue for send to all TOD clients.
                    //TODClientManager.Instance.SendToTOD(value);

                    med.Info("Notify change to update new chief on TA app.");
                    TANotifyService.Instance.RaiseTSBShiftChanged();
                }
                else
                {
                    med.Info("No TSBShift files generated.");
                }

                return ret;
            }
        }
    }
}
