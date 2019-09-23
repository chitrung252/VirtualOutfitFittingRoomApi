//using DataService.APIViewModels;
using DataService.Models;
using DataService.Models.APIModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyConnect.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;


namespace SkyConnect.API.Identities
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request, HttpStatusCode statusCode,int resultCode)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
            StatusCode = statusCode;
            ResultCode = resultCode;
        }

        public string ReasonPhrase { get; private set; }

        public HttpRequestMessage Request { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public int ResultCode { get; private set; }

        public  Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
                return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return new HttpResponseMessage()
            {
                StatusCode = StatusCode,
                Content = new JsonContent(new BaseResponse<string>()
                {
                    ResultCode = ResultCode,
                    Success = false,
                    Message = ReasonPhrase,
                })
            };
            //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            //response.RequestMessage = Request;
            //response.ReasonPhrase = ReasonPhrase;
            //return response;
        }
    }
    public class MobileAuthentication : Attribute, IAuthenticationFilter
    {
        #region old
        //public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        //{
        //    HttpRequestMessage request = context.Request;
        //    AuthenticationHeaderValue authorization = request.Headers.Authorization;
        //    var claimPrincipal = (ClaimsPrincipal)context.Principal;
        //    // get List role
        //    var role = claimPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        //    if (role.Contains(RoleTypeEnum.Reception.ToString()))
        //    {
        //        return;
        //    }
        //    else if(role.Contains(RoleTypeEnum.MobileUser.ToString()) && role.Contains(RoleTypeEnum.ActiveUser.ToString()))
        //    {
        //        // get CustomerID from Token Claims
        //        var customerIdClaim = claimPrincipal.Claims.Where(c => c.Type == "CustomerId")
        //              .Select(c => c.Value).SingleOrDefault();
        //        // get Custimer_id from uri
        //        var customerId = request.RequestUri.Segments[request.RequestUri.Segments.Length - 1];
        //        //var body = request.Content.ReadAsAsync<JObject>().Result;
        //        //var customerId = body.SelectToken("customer_id").ToString();
        //        var customera= request.RequestUri;
        //        if (customerId != customerIdClaim)
        //        {
        //            context.ErrorResult = new AuthenticationFailureResult(ConstantManager.MES_REQUEST_DENY, request,HttpStatusCode.NotAcceptable,(int)ResultEnum.CustomerIdNotMatch);
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        context.ErrorResult = new AuthenticationFailureResult(ConstantManager.MES_REQUEST_DENY, request, HttpStatusCode.NotAcceptable, (int)ResultEnum.RoleNotSupport);
                
        //    }
        //}
        #endregion 
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {

            HttpRequestMessage request = context.Request;
            // get token from request 
            AuthenticationHeaderValue authorization = request.Headers.Authorization;
            if(authorization == null)
            {
                context.ErrorResult = new AuthenticationFailureResult(ConstantManager.MES_REQUEST_DENY, request, HttpStatusCode.NotAcceptable, (int)ResultEnum.BearerTokenNotFound);
            }
            else
            {
                var schema = authorization.Scheme;
                var claimPrincipal = (ClaimsPrincipal)context.Principal;
                // get List role
                var role = claimPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                if (role.Contains(RoleTypeEnum.Reception.ToString()) || role.Contains(RoleTypeEnum.MobileUser.ToString()) && role.Contains(RoleTypeEnum.ActiveUser.ToString()))
                {
                    return;
                }
                else
                {
                    context.ErrorResult = new AuthenticationFailureResult(ConstantManager.MES_REQUEST_DENY, request, HttpStatusCode.NotAcceptable, (int)ResultEnum.RoleNotSupport);
                }
            }
           
            //else if (role.Contains(RoleTypeEnum.MobileUser.ToString()) && role.Contains(RoleTypeEnum.ActiveUser.ToString()))
            //{
            //    return;
            //    ////var order = JsonConvert.DeserializeObject<OrderAPIViewModel>(request.Content.ReadAsStringAsync().Result);
            //    //var order = JsonConvert.DeserializeObject<dynamic>(request.Content.ReadAsStringAsync().Result);

            //    //// get CustomerID from Token Claims
            //    //var customerIdClaim = claimPrincipal.Claims.Where(c => c.Type == "CustomerId")
            //    //      .Select(c => c.Value).SingleOrDefault();

            //    //order.CustomerID = Int32.Parse(customerIdClaim);
            //    //// get Custimer_id from uri
            //    //request.Content = new JsonContent(order);
            //}

        }
        private Tuple<string, string> ExtractUserNameAndPassword(string parameter)
        {
            var abcxyz = parameter;
            return new Tuple<string, string>("username", "password");
        }

        public bool AllowMultiple => true;
        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
        }
    }
}