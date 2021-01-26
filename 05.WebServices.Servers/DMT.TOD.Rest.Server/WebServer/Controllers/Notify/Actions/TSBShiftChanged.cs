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
        [ActionName(RouteConsts.TOD.Notify.TSBShiftChanged.Name)]
        //[AllowAnonymous]
        public NDbResult TSBShiftChanged()
        {
            NDbResult result = new NDbResult();
            result.Success();
            TODNotifyService.Instance.RaiseTSBShiftChanged();
            return result;
        }
    }
}
