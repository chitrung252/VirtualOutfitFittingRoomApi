using DataService.APIViewModels;
using DataService.Domain;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Utilities;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace SkyConnect.API.Controllers.APIs
{
    public interface IRatingProductController
    {
        HttpResponseMessage GetRating(RatingRequest<string> request);
        HttpResponseMessage CreateRating(RatingAPIViewModel rating);
        HttpResponseMessage UpdateRating(RatingAPIViewModel rating);

    }
    [Authorize]
    [RoutePrefix("api/rating")]
    public class RatingController : ApiController, IRatingProductController
    {
        //private DataService.Models.Identities.ApplicationUserManager _userManager;
        //public DataService.Models.Identities.ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? Request.GetOwinContext().GetUserManager<DataService.Models.Identities.ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}
        [AllowAnonymous]
        [Route("")]
        public HttpResponseMessage GetRating(RatingRequest<string> request)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            BaseResponse<TotalRatingAPIViewModel> response = new BaseResponse<TotalRatingAPIViewModel>();

            try
            {
                var domain = new RatingDomain();

                //response = domain.GetRatingByProductId(product_id);
                response = domain.GetRatingByRequest(request);
            }
            catch (ApiException e)
            {
                // catch ApiException
                responseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<TotalRatingAPIViewModel>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception ex)
            {
                responseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<TotalRatingAPIViewModel>.Get(false, ex.ToString(), null, ResultEnum.InternalError);
            }

            //return 
            responseMessage.Content = new JsonContent(response);
            return responseMessage;
        }

        #region old
        //[AllowAnonymous]
        //[Route("product/{product_id}")]
        //public HttpResponseMessage GetRating(int product_id)
        //{
        //    HttpResponseMessage responseMessage = new HttpResponseMessage();
        //    BaseResponse<List<RatingAPIViewModel>> response = new BaseResponse<List<RatingAPIViewModel>>();

        //    try
        //    {
        //        var domain = new RatingDomain();
        //        response = domain.GetRatingByProductId(product_id);
        //    }
        //    catch (ApiException e)
        //    {
        //        // catch ApiException
        //        responseMessage.StatusCode = e.StatusCode;
        //        response = BaseResponse<List<RatingAPIViewModel>>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
        //    }
        //    catch (Exception ex)
        //    {
        //        responseMessage.StatusCode = HttpStatusCode.InternalServerError;
        //        response = BaseResponse<List<RatingAPIViewModel>>.Get(false, ex.ToString(), null, ResultEnum.InternalError);
        //    }

        //    //return 
        //    responseMessage.Content = new JsonContent(response);
        //    return responseMessage;
        //}
        //[AllowAnonymous]
        //[Route("order/{orderId}")]
        //public HttpResponseMessage GetRatingByOrderId(int orderId)
        //{
        //    HttpResponseMessage responseMessage = new HttpResponseMessage();
        //    BaseResponse<List<RatingAPIViewModel>> response = new BaseResponse<List<RatingAPIViewModel>>();

        //    try
        //    {
        //        var domain = new RatingDomain();
        //        response = domain.GetRatingByOrderId(orderId);
        //    }
        //    catch (ApiException e)
        //    {
        //        // catch ApiException
        //        responseMessage.StatusCode = e.StatusCode;
        //        response = BaseResponse<List<RatingAPIViewModel>>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
        //    }
        //    catch (Exception ex)
        //    {
        //        responseMessage.StatusCode = HttpStatusCode.InternalServerError;
        //        response = BaseResponse<List<RatingAPIViewModel>>.Get(false, ex.ToString(), null, ResultEnum.InternalError);
        //    }

        //    //return 
        //    responseMessage.Content = new JsonContent(response);
        //    return responseMessage;
        //}
        #endregion

        [HttpPost]
        [Route("")]
        public HttpResponseMessage CreateRating(RatingAPIViewModel rating)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            BaseResponse<RatingAPIViewModel> response = new BaseResponse<RatingAPIViewModel>();

            try
            {
                var claimPrincipal = (ClaimsPrincipal)RequestContext.Principal;
                // get Id from Token Claims
                var customerId = claimPrincipal.Claims.Where(c => c.Type == "CustomerId").Select(c => c.Value).SingleOrDefault();
                int customerID = 0;
                Int32.TryParse(customerId, out customerID);

                DateTime time = DataService.Models.Utils.GetCurrentDateTime();
                var domain = new RatingDomain();
                rating.CreateTime = time;
                rating.Active = true;
                rating.CustomerId = customerID;
                
               response = domain.CreateRatingProduct(rating);
                
            }
            catch (ApiException e)
            {
                // catch ApiException
                responseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<RatingAPIViewModel>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception ex)
            {
                responseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<RatingAPIViewModel>.Get(false, ex.ToString(), null, ResultEnum.InternalError);
            }

            //return 
            responseMessage.Content = new JsonContent(response);
            return responseMessage;
        }
        [HttpPost]
        [Route("sub")]
        public HttpResponseMessage CreateSubRating(RatingAPIViewModel rating)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            BaseResponse<RatingAPIViewModel> response = new BaseResponse<RatingAPIViewModel>();

            try
            {
                var claimPrincipal = (ClaimsPrincipal)RequestContext.Principal;
                // get Id from Token Claims
                var customerId = claimPrincipal.Claims.Where(c => c.Type == "CustomerId").Select(c => c.Value).SingleOrDefault();
                int customerID = 0;
                Int32.TryParse(customerId, out customerID);

                DateTime time = DataService.Models.Utils.GetCurrentDateTime();
                var domain = new RatingDomain();
                rating.CreateTime = time;
                rating.Active = true;
                rating.CustomerId = customerID;

                response = domain.CreateSubRating(rating);

            }
            catch (ApiException e)
            {
                // catch ApiException
                responseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<RatingAPIViewModel>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception ex)
            {
                responseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<RatingAPIViewModel>.Get(false, ex.ToString(), null, ResultEnum.InternalError);
            }

            //return 
            responseMessage.Content = new JsonContent(response);
            return responseMessage;
        }
        [HttpPut]
        [Route("")]
        public HttpResponseMessage UpdateRating(RatingAPIViewModel rating)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            BaseResponse<RatingAPIViewModel> response = new BaseResponse<RatingAPIViewModel>();

            try
            {
                var domain = new RatingDomain();
                response = domain.UpdateRatingProduct(rating);
            }
            catch (ApiException e)
            {
                // catch ApiException
                responseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<RatingAPIViewModel>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception ex)
            {
                responseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<RatingAPIViewModel>.Get(false, ex.ToString(), null, ResultEnum.InternalError);
            }

            //return 
            responseMessage.Content = new JsonContent(response);
            return responseMessage;
            throw new NotImplementedException();
        }
    }
}
