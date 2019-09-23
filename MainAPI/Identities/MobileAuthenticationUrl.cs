using DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace SkyConnect.API.Identities
{

    public class MobileAuthenticationUrl : Attribute, IAuthenticationFilter
    {
        #region old
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;
            var claimPrincipal = (ClaimsPrincipal)context.Principal;
            // get List role
            var role = claimPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            if (role.Contains(RoleTypeEnum.Reception.ToString()))
            {
                return;
            }
            else if (role.Contains(RoleTypeEnum.MobileUser.ToString()) && role.Contains(RoleTypeEnum.ActiveUser.ToString()))
            {
                // get CustomerID from Token Claims
                var customerIdClaim = claimPrincipal.Claims.Where(c => c.Type == "CustomerId")
                      .Select(c => c.Value).SingleOrDefault();
                // get Custimer_id from uri
                var customerId = request.RequestUri.Segments[request.RequestUri.Segments.Length - 1];
                //var body = request.Content.ReadAsAsync<JObject>().Result;
                //var customerId = body.SelectToken("customer_id").ToString();
                var customera = request.RequestUri;
                if (customerId != customerIdClaim)
                {
                    context.ErrorResult = new AuthenticationFailureResult(ConstantManager.MES_REQUEST_DENY, request, HttpStatusCode.NotAcceptable, (int)ResultEnum.CustomerIdNotMatch);
                }
                else
                {
                    return;
                }
            }
            else
            {
                context.ErrorResult = new AuthenticationFailureResult(ConstantManager.MES_REQUEST_DENY, request, HttpStatusCode.NotAcceptable, (int)ResultEnum.RoleNotSupport);

            }
        }
        #endregion
        public bool AllowMultiple => true;



        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
        }
    }
}