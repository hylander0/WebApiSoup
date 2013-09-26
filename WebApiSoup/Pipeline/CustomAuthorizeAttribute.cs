using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace WebApiSoup.Pipeline
{
    public class CustomAuthorizeAttribute : AuthorizationFilterAttribute
    {
        //Not currently used.
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //if (!System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            //{
            //    actionContext.Response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Unauthorized);
            //}
            base.OnAuthorization(actionContext);

        }
    }
}