#region Using

using System;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    partial class TODNotifyController
    {
        [HttpPost]
        [ActionName(RouteConsts.TOD.Notify.TSBChanged.Name)]
        //[AllowAnonymous]
        public NDbResult TSBChanged()
        {
            NDbResult result = new NDbResult();
            result.Success();
            TODNotifyService.Instance.RaiseTSBChanged();
            return result;
        }
    }
}
