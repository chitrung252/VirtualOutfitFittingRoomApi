using DataService.APIViewModels;
using DataService.Models;
using DataService.Models.Entities.Services;
using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Utilities
{
    public class PromotionRule
    {
        public int rule { get; set; }
        public double discountAmount { get; set; }
        public int quantity { get; set; }
        public bool countProduct { get; set; }
    }
    public class ProductDiscount
    {
        public int product_id { get; set; }
        public double discount_rate { get; set; }
        public decimal discount_amount { get; set; }
    }
    public class PromotionRuleUtility
    {
        public static PromotionRule checkPromotionRule(OrderAPIViewModel order, int quantity, OrderDetailAPIViewModel od)
        {
            #region call service
            IVoucherService voucherService = DependencyUtils.Resolve<IVoucherService>();
            IPromotionService promotionService = DependencyUtils.Resolve<IPromotionService>();
            IPromotionDetailService promotionDetailService = DependencyUtils.Resolve<IPromotionDetailService>();
            IProductService productService = DependencyUtils.Resolve<IProductService>();
            #endregion

            PromotionRule result = new PromotionRule
            {
                rule = 0,
                discountAmount = 0
            };
            #region Voucher
            var voucher = voucherService.GetVoucherIsNotUsedAndCode(order.VoucherCode);
            if (voucher == null)
            {
                return result;
            }
            #endregion
           
            
            //var voucher = voucherApi
            //TODO after have voucher
            int checkCountProduct = 0;//dùng check trường hợp gửi 2 đơn hàng giống nhau nhưng mỗi cái có 1 quantity
            
            //get date to get promotion is experied ??
            #region Promotion 
            var promotion = promotionService.GetPromotionByDateAndId(voucher.PromotionID);
            DateTime now = DateTime.Now;

            if (promotion == null)
            {
                return result;
            }
            else if (!(promotion.ApplyFromTime <= now.Hour && now.Hour <= promotion.ApplyToTime))
            {
                return result;
            }

            #endregion
            #region PromtionDetail rule 1 min , max order???
            // loop to get total amount, final amount to get check in promotiondetail
            double finalAmount = 0;

            foreach (var item in order.OrderDetails)
            {
                finalAmount += (productService.GetProductById(item.ProductID).Price * item.Quantity);
            }

            var promotionDetail = promotionDetailService.GetDetailByCode(promotion.PromotionCode).FirstOrDefault();
            if (promotionDetail == null)
            {
                return result;
            }
            //check promotion detail is have min, max order != null ???
            if (promotionDetail.MinOrderAmount != null || promotionDetail.MaxOrderAmount != null)
            {
                if (promotionDetail.MinOrderAmount != null && finalAmount < promotionDetail.MinOrderAmount)
                {
                    throw ApiException.Get(false, ConstantManager.MES_CREATE_ORDER_EXCEED_VOUCHER_MIN_MAX, ResultEnum.VoucherMin, HttpStatusCode.BadRequest);
                }
                else if (promotionDetail.MaxOrderAmount != null && finalAmount > promotionDetail.MaxOrderAmount)
                {
                    throw  ApiException.Get(false, ConstantManager.MES_CREATE_ORDER_EXCEED_VOUCHER_MIN_MAX, ResultEnum.VoucherMax, HttpStatusCode.BadRequest);
                }
                else
                {
                    try
                    {
                        double discountAmount = 0;//amount return  
                        if (promotionDetail.DiscountRate != null && promotionDetail.DiscountRate > 0)
                        {
                            //nhân với phần trăm giảm giá và trả về số tiền lun
                            discountAmount = (productService.GetProductById(od.ProductID).Price * promotionDetail.DiscountRate.Value) / 100;
                        }
                        else if (promotionDetail.DiscountAmount != null && promotionDetail.DiscountAmount > 0)
                        {
                            //nếu giảm giá theo tiền mặt
                            discountAmount = Convert.ToDouble(promotionDetail.DiscountAmount.Value);
                        }
                        result.rule = Models.ConstantManager.PROMOTION_RULE_1;
                        result.discountAmount = discountAmount;
                        return result;
                    }
                    catch
                    {
                        result.rule = 0;
                        result.discountAmount = 0;
                        return result;
                    }

                }
            }
            #endregion

            #region rule 2 buy min, max quantity of each product 
            else if (promotionDetail.BuyProductCode != null)
            {
                double discountAmount = 0;
                //check product code is in order ????
                bool checkProductCode = false;
                //list product discount 
                List<ProductDiscount> listProductDiscount = new List<ProductDiscount>();
                // var pmDetail = promotionDetailApi.GetDetailByCode(promotion.PromotionCode);
                int pDetailId = voucher.PromotionDetailID == null ? 0 : voucher.PromotionDetailID.Value;
                var pmDetail = promotionDetailService.GetDetailById(pDetailId);
                decimal tmpDiscountAmount = 0;
                double tmpDiscountRate = 0;
                string mesMinBuyProduct = "";
                bool checkCount = true;//check quanitty min order buy
                var tmpProductOrder = productService.GetProductById(od.ProductID);

                bool checkCountProductQuantity = true;//false => đơn hàng gửi lên giống nhau nhưng ko đủ quantity min order,
                                                      //true => đơn hàng gửi lên giống nhau nhưng đủ quantity
                foreach (var item in order.OrderDetails)
                {
                    if (tmpProductOrder.Code == pmDetail.BuyProductCode)
                    {
                        checkCountProduct += item.Quantity;//cộng đồn quantity trong order
                        if (checkCountProduct < pmDetail.MinBuyQuantity)
                        {
                            checkCountProductQuantity = false;//false => đơn hàng gửi lên giống nhau nhưng ko đủ quantity min order,
                        }
                        else
                        {
                            checkCountProductQuantity = true;
                        }
                    }
                }
                if (pmDetail != null)
                {
                    if (tmpProductOrder.Code == pmDetail.BuyProductCode)
                    {
                        quantity += od.Quantity;
                        tmpDiscountAmount = pmDetail.DiscountAmount == null ? 0 : pmDetail.DiscountAmount.Value * od.Quantity;
                        tmpDiscountRate = pmDetail.DiscountRate == null ? 0 : pmDetail.DiscountRate.Value;
                        checkProductCode = true;
                        if (quantity < pmDetail.MinBuyQuantity)
                        {
                            mesMinBuyProduct = pmDetail.MinBuyQuantity + " " + tmpProductOrder.ProductName;
                            checkCount = false;
                        }
                    }
                    else
                    {

                        // checkCount = true;
                    }
                }

                try
                {

                    //if true => get list product discount
                    if (checkProductCode)
                    {
                        //check amount discount and rate, return value
                        if (tmpDiscountAmount > 0)
                        {
                            discountAmount = System.Convert.ToDouble(tmpDiscountAmount);
                        }
                        else if (tmpDiscountRate > 0)
                        {
                            discountAmount = (tmpProductOrder.Price * tmpDiscountRate * od.Quantity) / 100;
                        }

                    }
                    checkProductCode = false;
                    //return list product with discount amount, rate
                }
                catch
                {
                    result.rule = 0;
                    result.discountAmount = 0;
                    result.quantity = 0;
                    result.countProduct = false;
                    return result;
                }


                if (!checkCount && !checkCountProductQuantity)
                {
                    result.rule = 0;
                    result.discountAmount = 0;
                    result.quantity = quantity;
                    result.countProduct = checkCountProductQuantity;
                    return result;

                }
                result.rule = ConstantManager.PROMOTION_RULE_2;
                result.discountAmount = discountAmount;
                result.quantity = quantity;
                result.countProduct = checkCountProductQuantity;
                return result;


            }
            result.rule = 0;
            result.discountAmount = 0;
            result.quantity = 0;
            return result;
            #endregion
        }
    }
}
