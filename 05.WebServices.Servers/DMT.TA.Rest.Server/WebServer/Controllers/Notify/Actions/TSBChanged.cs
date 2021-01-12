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
        [ActionName(RouteConsts.TA.Notify.TSBChanged.Name)]
        //[AllowAnonymous]
        public NDbResult TSBChanged()
        {
            NDbResult result = new NDbResult();
            result.Success();
            TANotifyService.Instance.RaiseTSBChanged();
            return result;
        }
    }
}
