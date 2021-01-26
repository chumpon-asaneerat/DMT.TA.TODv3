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
        [ActionName(RouteConsts.TOD.Notify.UserShiftChanged.Name)]
        //[AllowAnonymous]
        public NDbResult UserShiftChanged()
        {
            NDbResult result = new NDbResult();
            result.Success();
            TODNotifyService.Instance.RaiseUserShiftChanged();
            return result;
        }
    }
}
