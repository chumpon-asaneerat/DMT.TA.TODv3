#region Using

using System;
using System.Web.Http;
using DMT.Configurations;
using DMT.Models;

#endregion

namespace DMT.Services
{
    partial class TANotifyController
    {
        [HttpPost]
        [ActionName(RouteConsts.TA.Notify.Unregister.Name)]
        //[AllowAnonymous]
        public NDbResult Unregister([FromBody] TODAppWebServiceConfig value)
        {
            NDbResult result = new NDbResult();
            result.Success();
            TODClientManager.Instance.Unregister(value);
            return result;
        }
    }
}
