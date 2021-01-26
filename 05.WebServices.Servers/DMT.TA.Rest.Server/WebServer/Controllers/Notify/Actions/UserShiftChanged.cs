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
        [ActionName(RouteConsts.TA.Notify.UserShiftChanged.Name)]
        //[AllowAnonymous]
        public NDbResult UserShiftChanged()
        {
            NDbResult result = new NDbResult();
            result.Success();
            TANotifyService.Instance.RaiseUserShiftChanged();
            return result;
        }
    }
}
