using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace WebApiSoup.Pipeline
{
    public class AuthMessageHandler : DelegatingHandler
    {

        private const string AppIdHeader = "AppId";
        private const string AppTokenHeader = "AppToken";
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            IEnumerable<string> appIdValues;
            IEnumerable<string> appTokenValues;
            HttpStatusCode responseCode = HttpStatusCode.OK;
            Guid AppIdGuid;
            var DoesHaveAppId = request.Headers.TryGetValues(AppIdHeader, out appIdValues);
            var DoesHaveAppToken = request.Headers.TryGetValues(AppTokenHeader, out appTokenValues);
            //Forces Json to be request Body type
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");
            IPrincipal principal = null;
            if (DoesHaveAppId && DoesHaveAppToken)
            {
                if (Guid.TryParse(appIdValues.FirstOrDefault(), out AppIdGuid))
                {
                    principal = ValidateAuthentication(AppIdGuid, appTokenValues.FirstOrDefault());
                    if (principal != null) //Valid User
                    {
                        System.Threading.Thread.CurrentPrincipal = principal;
                        if (IsAppOverLimit(AppIdGuid))
                        {
                            responseCode = (HttpStatusCode)429;
                        }
                    }
                    else//User failed to authenticate
                        responseCode = HttpStatusCode.Unauthorized;
                }
            }
            else//User didn't supply Key/Token
                responseCode = HttpStatusCode.Forbidden;


            return base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;
                    if (responseCode == HttpStatusCode.OK)
                        return response;
                    else
                        return request.CreateResponse(responseCode);
                });
        }

        private IPrincipal ValidateAuthentication(Guid appId, string AuthString)
        {
            IPrincipal retval = null;
            if (AuthString != null && !String.IsNullOrWhiteSpace(AuthString))
            {
                string secret = GetSecretFromRepo(appId);
                if (secret != null)
                    if (Security.HashSecurity.IsTokenValid(appId.ToString(), secret, AuthString))
                    {
                        return new GenericPrincipal(new GenericIdentity(appId.ToString(), AppIdHeader), null);
                    }
            }
            return retval;
        }

        private string GetSecretFromRepo(Guid AppId)
        {
            using (var context = new Repository.RepoContext())
            {
                var app = context.Applications.Where(w => w.ApplicationId == AppId).FirstOrDefault();
                if (app != null)
                    return app.AppGeneratedSecret;
                else
                    return null; //Did
            }
        }

        
        private bool IsAppOverLimit(Guid AppId)
        {
            using (var context = new Repository.RepoContext())
            {
                Repository.Application app = context.Applications.Where(w => w.ApplicationId == AppId).FirstOrDefault();
                if (app != null)
                {

                    switch (app.SubscriptionType)
                    {
                        case Common.Constants.API_SUBSCRIPTION_TYPE_NO_LIMIT:
                            app.ApiAccessMetricCurrent++;
                            break;
                        case Common.Constants.API_SUBSCRIPTION_TYPE_DAILY_RESET:
                            if (app.LastDtmAccessed.DayOfYear < DateTime.UtcNow.DayOfYear)
                                app.ApiAccessMetricCurrent = 1;
                            else
                                app.ApiAccessMetricCurrent++;
                            if (app.ApiAccessMetricCurrent >= app.ApiAccessMetricLimit)
                                return true;
                            break;
                        case Common.Constants.API_SUBSCRIPTION_TYPE_MONTHLY_RESET:
                            if (app.LastDtmAccessed.Month < DateTime.UtcNow.Month)
                                app.ApiAccessMetricCurrent = 1;
                            else
                                app.ApiAccessMetricCurrent++;

                            if (app.ApiAccessMetricCurrent >= app.ApiAccessMetricLimit)
                                return true;
                            break;

                    }
                    app.LastDtmAccessed = DateTime.UtcNow;
                    context.SaveChanges();
                }
            }
            return false;
        }
    }
}