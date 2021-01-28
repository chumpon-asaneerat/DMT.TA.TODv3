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
        [ActionName(RouteConsts.TOD.Notify.IsAlive.Name)]
        //[AllowAnonymous]
        public NDbResult<IsAliveResult> IsAlive()
        {
            NDbResult<IsAliveResult> result = new NDbResult<IsAliveResult>();
            result.Success(new IsAliveResult() { TimeStamp = DateTime.Now });
            return result;
        }
    }
}
