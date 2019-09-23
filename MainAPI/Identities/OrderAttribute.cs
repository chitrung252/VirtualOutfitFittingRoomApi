using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SkyConnect.API.Identities
{
    public class OrderAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //if (actionContext == null)
            //{
            //    throw Error.ArgumentNull("actionContext");
            //}

            //if (SkipAuthorization(actionContext))
            //{
            //    return;
            //}

            //if (!IsAuthorized(actionContext))
            //{
            //    HandleUnauthorizedRequest(actionContext);
            //}
        }
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            //actionContext.ControllerContext
            return false;
        }
    }
}