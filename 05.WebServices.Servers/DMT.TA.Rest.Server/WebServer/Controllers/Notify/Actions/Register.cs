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
        [ActionName(RouteConsts.TA.Notify.Register.Name)]
        //[AllowAnonymous]
        public NDbResult Register([FromBody] TODAppWebServiceConfig value)
        {
            NDbResult result = new NDbResult();
            result.Success();
            TODClientManager.Instance.Register(value);
            return result;
        }
    }
}
