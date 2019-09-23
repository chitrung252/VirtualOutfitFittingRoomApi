using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataService.APIViewModels;
using DataService.Domain;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Models.APIModels.Promotion;
using DataService.Models.Entities;
using DataService.Utilities;
using DataService.ViewModels;
using Newtonsoft.Json;
using SkyConnect.API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;

namespace SkyConnect.API.Controllers.APIs
{
    #region VoucherApi interface
    public partial interface IVoucherApiController
    {
        HttpResponseMessage GetVoucher(VoucherQueryRequest<dynamic> request);
        HttpResponseMessage CreateVoucher(VoucherAPIViewModel model);
        HttpResponseMessage UpdateVoucher(VoucherAPIViewModel model);
        HttpResponseMessage CheckVoucher(CheckVoucherViewModel request);
        //HttpResponseMessage UseVoucher(PromotionRequestViewModel model);

    }
    #endregion
    [Authorize]
    [RoutePrefix("api/voucher")]
    public class VoucherApiController : ApiController, IVoucherApiController
    {
        //[Route("")]
        //[HttpPost]
        public HttpResponseMessage CreateVoucher(VoucherAPIViewModel model)
        {
            var newVoucher = model.ToEntity();
            var pDomain = new PromotionDomain();
            try
            {
                newVoucher = pDomain.Create(newVoucher);
                var res = new BaseResponse<string>()
                {
                    Data = "Success",
                    Message = "Success",
                    Success = true,
                    ResultCode = (int)DataService.Models.ResultEnum.Success
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
                    ResultCode = (int)DataService.Models.ResultEnum.InternalError,
                    Success = false
                };
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(res),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetVoucher([FromUri]VoucherQueryRequest<dynamic> request)
        {
            var response = new BaseResponse<List<VoucherAPIViewModel>>();
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
            request.MembershipVM = member;
            request.BrandId = customer.BrandId;

            //if (voucher.Count == 0)
            //{
            //    var res = BaseResponse<dynamic>.Get(false, "Không tìm thấy voucher nào", null, ResultEnum.VoucherNotFound);
            //    resp.Content = new JsonContent(res);
            //    resp.StatusCode = HttpStatusCode.NotFound;
            //    return resp;
            //}
            try
            {
                var voucher = pDomain.GetVoucher(request).ToList();
                response = BaseResponse<List<VoucherAPIViewModel>>.Get(false, "Thành công", voucher, ResultEnum.Success);
            }
            catch (ApiException e)
            {
                resp.StatusCode = e.StatusCode;
                response = BaseResponse<List<VoucherAPIViewModel>>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception e)
            {
                response = BaseResponse<List<VoucherAPIViewModel>>.Get(false, e.Message, null, ResultEnum.InternalError);
                resp.StatusCode = HttpStatusCode.InternalServerError;
            }
            resp.Content = new JsonContent(response);
            return resp;
        }

        //[Route("update")]
        //[HttpPut]
        public HttpResponseMessage UpdateVoucher(VoucherAPIViewModel model)
        {
            var updateVoucher = model.ToEntity();
            var pDomain = new PromotionDomain();
            var oldVoucher = pDomain.GetVoucher(model.VoucherID);
            if (oldVoucher == null)
            {
                var res = new BaseResponse<string>()
                {
                    Message = "Not found",
                    Success = false,
                    ResultCode = (int)DataService.Models.ResultEnum.VoucherNotFound
                };
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(res),
                    StatusCode = HttpStatusCode.NotFound
                };
            };

            try
            {
                updateVoucher = pDomain.Update(updateVoucher);
                var res = new BaseResponse<string>()
                {
                    Data = "Success",
                    Message = "Success",
                    Success = true,
                    ResultCode = (int)DataService.Models.ResultEnum.Success
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
                    ResultCode = (int)DataService.Models.ResultEnum.InternalError,
                    Success = false
                };
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(res),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        #region Business
        [Route("check")]
        [HttpPost]
        public HttpResponseMessage CheckVoucher(CheckVoucherViewModel request)
        {
            //Logger.Log("Store " + model.terminalId);
            //Logger.Log("|CheckVoucherCode| begin method");
            var response = new BaseResponse<dynamic>();
            var claimPrincipal = (ClaimsPrincipal)RequestContext.Principal;
            var customerId = claimPrincipal.Claims.Where(c => c.Type == "CustomerId").Select(c => c.Value).SingleOrDefault();
            var cDomain = new CustomerDomain();
            var id = Int32.Parse(customerId);
            var customer = cDomain.GetCustomerById(id);
            var oDomain = new OrderDomain();

            var resp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };

            try
            {
                DateTime time = DataService.Models.Utils.GetCurrentDateTime();
                var pDomain = new PromotionDomain();
                var orderVM = new OrderAPIViewModel();
                var voucher = pDomain.GetVoucher(request.VoucherCode);
                var mbs = customer.MembershipVM;

                orderVM.OrderDetails = request.Data;
                orderVM.StoreID = request.StoreId;
                AddInfo(orderVM, request);
                oDomain.CalculateOrderPrice(orderVM, time);

                //temp: each voucher has only 1 detail now
                var applyResult = pDomain.IsVoucherValidFor(voucher, orderVM, mbs);
                orderVM = pDomain.ApplyPromotionToOrder(orderVM, applyResult, mbs);

                response = BaseResponse<dynamic>.Get(true, "Thành công", orderVM, ResultEnum.Success);
            }
            catch (ApiException e)
            {
                resp.StatusCode = e.StatusCode;
                response = BaseResponse<dynamic>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception e)
            {
                resp.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<dynamic>.Get(false, e.ToString(), null, ResultEnum.InternalError);
            }

            resp.Content = new JsonContent(response);
            return resp;
        }

        //[Route("use")]
        //[HttpPut]
        //public HttpResponseMessage UseVoucher(PromotionRequestViewModel model)
        //{
        //    //Logger.Log("Store " + model.terminalId + " - " + (model.Order == null ? "null order" : model.Order.OrderCode + "-" + model.Order.OrderId));
        //    //Logger.Log("|UseVoucherCode| begin method");
        //    try
        //    {
        //        var pDomain = new PromotionDomain();
        //        var voucher = pDomain.GetVoucher(model.VoucherCode);
        //        var available = pDomain.IsVoucherAvailable(voucher);
        //        if (available.Success)
        //        {
        //            var useResult = pDomain.UseVoucher(voucher, model);
        //            //Logger.Log("|UserVoucherCode| result: " + useResult.Message + "-" + useResult.Error, true);
        //            return new HttpResponseMessage()
        //            {
        //                Content = new JsonContent(useResult),
        //                StatusCode = HttpStatusCode.OK
        //            };
        //        }
        //        else
        //        {
        //            return new HttpResponseMessage()
        //            {
        //                Content = new JsonContent(available),
        //                StatusCode = HttpStatusCode.OK
        //            };
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //Logger.Log("|UserVoucherCode| error:\r\n" + JsonConvert.SerializeObject(e), true);
        //        var res = new BaseResponse<string>
        //        {
        //            Success = false,
        //            Message = "Có lỗi xảy ra",
        //            Error = "",
        //            ResultCode = (int)DataService.Models.ResultEnum.InternalError
        //        };
        //        return new HttpResponseMessage()
        //        {
        //            Content = new JsonContent(res),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //    }
        //}
        #endregion

        #region Mapping models
        private void AddInfo(OrderAPIViewModel src, VoucherQueryRequest<List<OrderDetailAPIViewModel>> request)
        {
            if (!request.BrandId.HasValue)
                throw ApiException.Get(false, "Thiếu thông tin hãng", ResultEnum.BrandIdNotFound, HttpStatusCode.BadRequest);
            var prdDomain = new ProductDomain();
            src.BrandId = request.BrandId.Value;
            foreach (var oD in request.Data)
            {
                oD.ProductCode = prdDomain.GetProductById(oD.ProductID).Code;
            }
        }

        private void AddInfo(OrderAPIViewModel src, CheckVoucherViewModel request)
        {
            if (!request.BrandId.HasValue && !request.StoreId.HasValue)
                throw ApiException.Get(false, "Thiếu thông tin brand", ResultEnum.BrandIdNotFound, HttpStatusCode.BadRequest);
            var prdDomain = new ProductDomain();
            src.BrandId = request.BrandId.Value;
            if (request.BrandId == -1 && request.StoreId >0)
            {
                var storeApi = new DataService.Domain.StoreDomain();
                var store = storeApi.GetStoreByStoreId(request.StoreId.Value);
                src.BrandId = store.BrandId;
            }
            else 
                throw ApiException.Get(false, "Thiếu thông tin brand", ResultEnum.BrandIdNotFound, HttpStatusCode.BadRequest);
            foreach (var oD in request.Data)
            {
                oD.ProductCode = prdDomain.GetProductById(oD.ProductID).Code;
            }
        }

        private Order MapOrder(OrderAPIViewModel src)
        {
            var order = src.ToEntity();
            order.OrderDetails = src.OrderDetails.Select(oD => new OrderDetail()
            {
                ProductID = oD.ProductID,
                Quantity = oD.Quantity,
                TmpDetailId = oD.TmpDetailId,
                ParentId = oD.ParentId,
                ProductOrderType = oD.ProductOrderType
            }).ToList();
            return order;
        }
        private OrderDetail MapOrderDetail(OrderDetailAPIViewModel src)
        {
            return src.ToEntity();
        }
        private IEnumerable<OrderDetail> MapOrderDetailList(IEnumerable<OrderDetailAPIViewModel> srcList)
        {
            return srcList.Select(oD => MapOrderDetail(oD)).ToList();
        }
        #endregion

    }
    
    public class CheckVoucherViewModel
    {
        [JsonProperty("voucher_id")]
        public int? VoucherId { get; set; }
        [JsonProperty("voucher_code")]
        public string VoucherCode { get; set; }
        [JsonProperty("store_id")]
        public Nullable<int> StoreId { get; set; }
        [JsonProperty("brand_id")]
        public Nullable<int> BrandId { get; set; } = -1;
        [JsonProperty("data")]
        public List<OrderDetailAPIViewModel> Data { get; set; }
    }
}
