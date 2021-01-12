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
        [ActionName(RouteConsts.TOD.Notify.ShiftChanged.Name)]
        //[AllowAnonymous]
        public NDbResult ShiftChanged()
        {
            NDbResult result = new NDbResult();
            result.Success();
            TODNotifyService.Instance.RaiseShiftChanged();
            return result;
        }
    }
}
