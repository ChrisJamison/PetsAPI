using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BusinessServices;

namespace PetsAPI.Filters
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private const string AuthToken = "Auth-Token";
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var provider = actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(ISessionServices)) as ISessionServices;
            if (actionContext.Request.Headers.Contains(AuthToken))
            {
                var authTokenValue = actionContext.Request.Headers.GetValues(AuthToken).First();
                if (provider != null && !provider.ValidateSession(authTokenValue))
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };
                    actionContext.Response = responseMessage;
                }

            }
            else
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            base.OnActionExecuting(actionContext);
        }
    }
}