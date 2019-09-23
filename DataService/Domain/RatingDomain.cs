using DataService.APIViewModels;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Models.Entities.Services;
using DataService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Domain
{
    public interface IRatingDomain
    {
        BaseResponse<List<RatingAPIViewModel>> GetRatingByProductId(int product_id);
        BaseResponse<List<RatingAPIViewModel>> GetRatingByOrderId(int orderId);
        BaseResponse<RatingAPIViewModel> CreateRatingProduct(RatingAPIViewModel rating);
        BaseResponse<RatingAPIViewModel> UpdateRatingProduct(RatingAPIViewModel rating);
    }
    public class RatingDomain : BaseDomain, IRatingDomain
    {
        public BaseResponse<RatingAPIViewModel> CreateRatingProduct(RatingAPIViewModel rating)
        {
            var service = this.Service<IRatingService>();
            var productService = this.Service<IProductService>();
            var customerService = this.Service<ICustomerService>();
            var orderService = this.Service<IOrderService>();
            try
            {
                if (rating.CustomerId == 0)
                {
                    throw ApiException.Get(false, ConstantManager.MES_CUSTOMER_NOTFOUND, ResultEnum.CustomerIdInTokenWrong, HttpStatusCode.BadRequest);
                }
                var customer = customerService.GetCustomerById(rating.CustomerId.Value);
                if (customer == null)
                {
                    throw ApiException.Get(false, ConstantManager.MES_CUSTOMER_NOTFOUND, ResultEnum.CustomerNotFound, HttpStatusCode.BadRequest);
                }
                else
                {
                    
                    if (rating.ProductId != null && rating.OrderId != null)
                    {
                        throw ApiException.Get(false, ConstantManager.MES_RATE_VALID, ResultEnum.RateValid, HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        rating.ReviewEmail = customer.Email;
                        rating.ReviewName = customer.Name;
                        rating.Verified = false;
                        if (rating.ProductId != null)
                        {
                            var check = productService.GetProductById(rating.ProductId.Value);
                            if (check == null)
                            {
                                throw ApiException.Get(false, ConstantManager.MES_PRODUCT_NOTEXIST, ResultEnum.ProductNotExist, HttpStatusCode.BadRequest);
                            }

                        }
                        if (rating.OrderId != null)
                        {
                            var check = orderService.GetOrderById(rating.OrderId.Value);
                            if (check == null)
                            {
                                throw ApiException.Get(false, ConstantManager.MES_ORDER_NOT_FOUND, ResultEnum.OrderNotFound, HttpStatusCode.BadRequest);
                            }
                            if(check.CustomerID != rating.CustomerId)
                            {
                                throw ApiException.Get(false, ConstantManager.MES_REQUEST_DENY, ResultEnum.CustomerIdNotMatch, HttpStatusCode.BadRequest);
                            }
                        }
                        var rt = service.CreateRatingProduct(rating);
                        return BaseResponse<RatingAPIViewModel>.Get(true, ConstantManager.MES_RATING_CREATE_SUCCESS, rt, ResultEnum.Success);
                    }

                }


            }
            catch (Exception ex)
            {

                if (ex is ApiException)
                {
                    throw ex;
                }
                else
                {
                    throw ApiException.Get(false, ConstantManager.MES_RATING_CREATE_FAIL, ResultEnum.CreateFail, HttpStatusCode.InternalServerError);
                }
            }


        }

        public BaseResponse<List<RatingAPIViewModel>> GetRatingByOrderId(int orderId)
        {
            try
            {
                var service = this.Service<IRatingService>();
                var list = service.GetRatingByOrderId(orderId);
                if (list == null)
                {
                    throw ApiException.Get(true, ConstantManager.MES_RATINGPRODUCT_NOTFOUNT, ResultEnum.RatingProductNotFound, HttpStatusCode.OK);

                }
                return BaseResponse<List<RatingAPIViewModel>>.Get(true, ConstantManager.MES_RATINGPRODUCT_OK, list, ResultEnum.Success);

            }
            catch (Exception ex)
            {
                if (ex is ApiException)
                {
                    throw ex;
                }
                else
                {
                    throw ApiException.Get(false, ex.ToString(), ResultEnum.InternalError, HttpStatusCode.InternalServerError);
                }

            }
            throw new NotImplementedException();
        }

        public BaseResponse<List<RatingAPIViewModel>> GetRatingByProductId(int product_id)
        {
            try
            {
                var service = this.Service<IRatingService>();
                var list = service.GetRatingByProductId(product_id);
                if (list == null)
                {
                    throw ApiException.Get(true, ConstantManager.MES_RATINGPRODUCT_NOTFOUNT, ResultEnum.RatingProductNotFound, HttpStatusCode.OK);

                }
                return BaseResponse<List<RatingAPIViewModel>>.Get(true, ConstantManager.MES_RATINGPRODUCT_OK, list, ResultEnum.Success);

            }
            catch (Exception ex)
            {
                if (ex is ApiException)
                {
                    throw ex;
                }
                else
                {
                    throw ApiException.Get(false, ex.ToString(), ResultEnum.InternalError, HttpStatusCode.InternalServerError);
                }

            }

        }

        public BaseResponse<RatingAPIViewModel> UpdateRatingProduct(RatingAPIViewModel rating)
        {

            var service = this.Service<IRatingService>();
            var list = service.GetRatingByProductId(rating.Id);
            if (list == null)
            {
                throw ApiException.Get(false, ConstantManager.MES_RATINGPRODUCT_NOTEXIST, ResultEnum.RatingProductNotExist, HttpStatusCode.BadRequest);
            }
            var service2 = this.Service<IProductService>();
            var list2 = service2.GetProductById(rating.ProductId.Value);
            if (list2 == null)
            {
                throw ApiException.Get(false, ConstantManager.MES_PRODUCT_NOTEXIST, ResultEnum.ProductNotExist, HttpStatusCode.BadRequest);
            }
            var rt = service.UpdateRatingProduct(rating);
            return BaseResponse<RatingAPIViewModel>.Get(true, ConstantManager.MES_RATINGPRODUCT_UPDATE_SUCCESS, rt, ResultEnum.Success);
        }
    }
}
