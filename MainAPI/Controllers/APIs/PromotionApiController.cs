using DataService.Domain;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Models.APIModels.Promotion;
using DataService.Models.Entities;
using DataService.Utilities;
using DataService.ViewModels;
using SkyConnect.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace SkyConnect.API.Controllers.APIs
{

    public partial interface IPromotionApiController
    {
        HttpResponseMessage GetPromotion(PromotionQueryRequest<dynamic> request);
        HttpResponseMessage ChangePromotionActiveState(int promotionId, bool active);
        HttpResponseMessage CreatePromotion(PromotionViewModel model);
        HttpResponseMessage UpdatePromotion(PromotionViewModel model);
    }

    //[Authorize]
    [RoutePrefix("api/promotion")]
    public class PromotionApiController : ApiController, IPromotionApiController
    {

        //[Route("change-state/{promotionId}/{active}")]
        //[HttpPut]
        public HttpResponseMessage ChangePromotionActiveState(int promotionId, bool active)
        {
            BaseResponse<string> res;
            var pDomain = new PromotionDomain();
            var promotion = pDomain.GetPromotion(promotionId);
            if (promotion != null)
            {
                try
                {
                    pDomain.ChangePromotionActiveState(promotion, active);
                    res = new BaseResponse<string>()
                    {
                        Data = "Success",
                        Message = "Success",
                        ResultCode = (int)ResultEnum.Success,
                        Success = true
                    };
                    return new HttpResponseMessage()
                    {
                        Content = new JsonContent(res),
                        StatusCode = HttpStatusCode.OK
                    };
                }
                catch (Exception e)
                {
                    res = new BaseResponse<string>()
                    {
                        Error = e.Message,
                        Message = e.Message,
                        ResultCode = (int)ResultEnum.InternalError,
                        Success = false
                    };
                    return new HttpResponseMessage()
                    {
                        Content = new JsonContent(res),
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                }
            }
            res = new BaseResponse<string>()
            {
                Error = "Cannot found promotion",
                Message = "Cannot found promotion",
                ResultCode = (int)ResultEnum.VoucherNotFound,
                Success = false
            };
            return new HttpResponseMessage()
            {
                Content = new JsonContent(res),
                StatusCode = HttpStatusCode.NotFound
            };
        }

        //[Route("")]
        //[HttpPost]
        public HttpResponseMessage CreatePromotion(PromotionViewModel model)
        {
            var newPromotion = model.ToEntity();
            var pDomain = new PromotionDomain();
            try
            {
                newPromotion = pDomain.Create(newPromotion);
                var res = new BaseResponse<string>()
                {
                    Data = "Success",
                    Message = "Success",
                    Success = true,
                    ResultCode = (int)ResultEnum.Success
                };
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(res),
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception e)
            {
                var res = new BaseResponse<string>()
                {
                    Error = e.Message,
                    Message = e.Message,
                    ResultCode = (int)ResultEnum.InternalError,
                    Success = false
                };
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(res),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        //[Route("{PromotionID?}")]
        //[HttpGet]
        public HttpResponseMessage GetPromotion(PromotionQueryRequest<dynamic> request)
        {
            var response = new BaseResponse<dynamic>();
            var claimPrincipal = (ClaimsPrincipal)RequestContext.Principal;
            var customerId = claimPrincipal.Claims.Where(c => c.Type == "CustomerId").Select(c => c.Value).SingleOrDefault();
            var cDomain = new CustomerDomain();
            var id = Int32.Parse(customerId);
            var customer = cDomain.GetCustomerById(id);
            var resp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            var pDomain = new PromotionDomain();
            Membership member = customer.MembershipVM.ToEntity();
            request.Membership = member;
            var promotion = pDomain.GetPromotion(request).ToList();

            if (promotion.Count == 0)
            {
                var res = BaseResponse<dynamic>.Get(false, "Không tìm thấy khuyến mãi nào", null, ResultEnum.PromotionNotFound);
                resp.Content = new JsonContent(res);
                resp.StatusCode = HttpStatusCode.NotFound;
                return resp;
            }
            try
            {
                response = BaseResponse<dynamic>.Get(false, "Thành công", null, ResultEnum.Success);
                if (promotion.Count == 1)
                    response.Data = promotion.FirstOrDefault();
                else
                    response.Data = promotion;
            }
            catch (ApiException e)
            {
                resp.StatusCode = e.StatusCode;
                response = BaseResponse<dynamic>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception e)
            {
                response = BaseResponse<dynamic>.Get(false, e.Message, null, ResultEnum.InternalError);
                resp.StatusCode = HttpStatusCode.InternalServerError;
            }
            resp.Content = new JsonContent(response);
            return resp;
        }

        //[Route("{PromotionID}")]
        //[HttpPut]
        public HttpResponseMessage UpdatePromotion(PromotionViewModel model)
        {
            var pDomain = new PromotionDomain();
            var oldPromotion = pDomain.GetPromotion(model.PromotionID);
            if (oldPromotion == null)
            {
                var res = new BaseResponse<string>()
                {
                    Message = "Not found",
                    Success = false,
                    ResultCode = (int)ResultEnum.VoucherNotFound
                };
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(res),
                    StatusCode = HttpStatusCode.NotFound
                };
            };
            try
            {
                var updatePromotion = model.ToEntity();
                updatePromotion = pDomain.Update(updatePromotion);
                var res = new BaseResponse<string>()
                {
                    Data = "Success",
                    Message = "Success",
                    Success = true,
                    ResultCode = (int)ResultEnum.Success
                };
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(res),
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception e)
            {
                var res = new BaseResponse<string>()
                {
                    Error = e.Message,
                    Message = e.Message,
                    ResultCode = (int)ResultEnum.InternalError,
                    Success = false
                };
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(res),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

    }
}
