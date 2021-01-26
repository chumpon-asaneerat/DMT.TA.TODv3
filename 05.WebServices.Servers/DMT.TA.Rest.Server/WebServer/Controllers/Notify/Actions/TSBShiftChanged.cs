#region Using

using System;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    partial class TANotifyController
    {
        [HttpPost]
        [ActionName(RouteConsts.TA.Notify.TSBShiftChanged.Name)]
        //[AllowAnonymous]
        public NDbResult TSBShiftChanged()
        {
            NDbResult result = new NDbResult();
            result.Success();
            TANotifyService.Instance.RaiseTSBShiftChanged();
            return result;
        }
    }
}
