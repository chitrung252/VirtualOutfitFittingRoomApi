using DataService.APIViewModels;
using DataService.Models;
using DataService.Models.Entities.Services;
using DataService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.ViewModels;
using Redis.Cache;
using StackExchange.Redis;
using Newtonsoft.Json;
using DataService.Utilities;
using RedisCache;
using System.Net;
using DataService.Models.APIModels;
using System.Transactions;

namespace DataService.Domain
{
    public interface IOrderDomain
    {
        OrderAPIViewModel AddOrderFromPOS(OrderAPIViewModel model);
        BaseResponse<OrderHistoryAPIViewModel> AddOrderFromMobile(OrderAPIViewModel order);
        BaseResponse<List<OrderHistoryAPIViewModel>> GetOrderHistoryByRequest(OrderRequest<string> request);
        void CalculateOrderPrice(OrderAPIViewModel order, DateTime time);
        OrderAPIViewModel ApplyPromotionToOrder(OrderAPIViewModel order, Promotion promotion, PromotionDetail pDetail);
    }


    public class OrderDomain : BaseDomain, IOrderDomain
    {

        //[trungtran]
        public OrderAPIViewModel ApplyPromotionToOrder(OrderAPIViewModel order, Promotion promotion, PromotionDetail pDetail)
        {
            //temp: apply only for discount promotion
            if (promotion.ApplyLevel == (int)PromotionApplyLevelEnum.Order)
            {
                if (pDetail.DiscountAmount != null)
                {
                    order.Discount = (double)pDetail.DiscountAmount.Value;
                    order.FinalAmount = order.TotalAmount - order.Discount;
                }
                else if (pDetail.DiscountRate != null)
                {
                    order.Discount = order.TotalAmount * pDetail.DiscountRate.Value / 100;
                    order.FinalAmount = order.TotalAmount - order.Discount;
                    order.DiscountRate = pDetail.DiscountRate.Value;
                }
            }
            else if (promotion.ApplyLevel == (int)PromotionApplyLevelEnum.OrderDetail)
            {
                if (pDetail.DiscountAmount != null)
                {
                    var discount = 0.0;
                    foreach (var oD in order.OrderDetails)
                    {
                        oD.Discount = (double)pDetail.DiscountAmount.Value;
                        oD.FinalAmount = oD.TotalAmount - oD.Discount;
                        discount += oD.Discount;
                    }
                    order.Discount = order.TotalAmount - discount;
                    order.FinalAmount = order.TotalAmount - order.Discount;
                }
                else if (pDetail.DiscountRate != null)
                {
                    var rate = pDetail.DiscountRate.Value;
                    var discount = 0.0;
                    foreach (var oD in order.OrderDetails)
                    {
                        oD.Discount = oD.TotalAmount * rate / 100;
                        oD.FinalAmount = oD.TotalAmount - oD.Discount;
                        discount += oD.Discount;
                    }
                    order.Discount = order.TotalAmount - discount;
                    order.FinalAmount = order.TotalAmount - order.Discount;
                }
            }

            return order;
        }

        public void CalculateOrderPrice(OrderAPIViewModel order, DateTime time)
        {
            #region OrderDetail

            #region Service and variable
            IProductService productService = DependencyUtils.Resolve<IProductService>();
            double orderDetailTotalAmount = 0;
            double orderDetailFinalAmount = 0;
            //double discountOrderDetail = 0;
            //biến giảm giá trên mỗi sản phẩm
            double discountEachProduct = 0;
            //biến giảm giá trên toàn hóa đơn
            double discount = 0;
            bool checkDeliveryFee = false;
            double finalAmount = 0;
            double totalAmount = 0;
            double deliveryFee = 0;
            //add order detail have product is a delivery fee
            //giảm giá trên từng sản phẩm, hóa đơn tùy theo rule ở hàm checkPromotionRUle
            int quantity = 0;//check quantity trong order gửi 1 đơn hàng và có quantity trong đó
            #endregion

            foreach (var item in order.OrderDetails)
            {
                if (item.ParentId == 0)
                {
                    item.ParentId = null;
                }

                if (item.Quantity <= 0)
                {
                    throw ApiException.Get(false, ConstantManager.MES_CREATE_ORDER_NEGATIVE_QUANTITY, ResultEnum.OrderDetailQuantity, HttpStatusCode.BadRequest);
                }
                var product = productService.GetProductById(item.ProductID);
                if (product == null)
                {
                    throw ApiException.Get(false, ConstantManager.MES_CREATE_ORDER_NOT_FOUND_PRODUCT, ResultEnum.ProductNotFound, HttpStatusCode.NotFound);
                }
                item.ProductType = product.ProductType;
                item.TotalAmount = product.Price * item.Quantity;

                orderDetailTotalAmount += item.TotalAmount;
                item.UnitPrice = product.Price;
                //lấy giảm giá theo rule 
                PromotionRule rule = new PromotionRule();
                rule = PromotionRuleUtility.checkPromotionRule(order, quantity, item);
                //check rule 1, 2 dc định nghĩ ở ConstantManager
                if (rule.rule == ConstantManager.PROMOTION_RULE_2 || rule.countProduct)
                {
                    item.Discount = rule.discountAmount;
                }
                else if (rule.rule == ConstantManager.PROMOTION_RULE_1)
                {
                    discount = rule.discountAmount;
                }

                quantity = rule.quantity;
                item.FinalAmount = item.TotalAmount - item.Discount;
                orderDetailFinalAmount += item.FinalAmount;
                discountEachProduct += item.Discount;
                //discountOrderDetail += item.Discount;
                item.OrderDate = time;

            }
            foreach (var item in order.OrderDetails)
            {
                if(item.ProductType != (int)ProductTypeEnum.ServiceFee)
                {
                    totalAmount = productService.GetProductById(item.ProductID).Price * item.Quantity;
                    finalAmount += totalAmount - item.Discount;
                    if (finalAmount >= ConstantManager.DELIVERY_FREE || order.OrderType != (int)OrderTypeEnum.MobileDelivery)
                    {
                        checkDeliveryFee = true;
                    }
                }
               
            }

            if (!checkDeliveryFee)
            {
                var tmpOrderDetail = order.OrderDetails.ToList();
                var productDelivery = productService.GetProductDeliveryFee();
                OrderDetailAPIViewModel deliveryOrderDt = new OrderDetailAPIViewModel();
                deliveryOrderDt.ProductID = productDelivery.ProductID;
                deliveryOrderDt.Quantity = 1;
                deliveryOrderDt.TotalAmount = productDelivery.Price;
                deliveryOrderDt.FinalAmount = productDelivery.Price;
                deliveryOrderDt.UnitPrice = productDelivery.Price;
                deliveryFee = productDelivery.Price;

                deliveryOrderDt.ProductOrderType = (int)Models.ProductOrderType.Single;
                deliveryOrderDt.OrderDate = time;
                deliveryOrderDt.ProductType = (int)ProductTypeEnum.ServiceFee;
                tmpOrderDetail.Add(deliveryOrderDt);
                order.OrderDetails = tmpOrderDetail;
            }
            #endregion
            #region Order
            order.CheckInDate = time;
            var vatAmount = 0; //VAT 10%
            #region edit promotion

            #endregion

            order.TotalAmount = orderDetailTotalAmount;
            order.Discount = discount;
            order.DiscountOrderDetail = discountEachProduct;
            order.FinalAmount = orderDetailTotalAmount + deliveryFee - vatAmount - discount - discountEachProduct;//l?y order detail sum l?i => ra du?c order thi?t c?a passio
                                                                                                    //order.DiscountOrderDetail = discountOrderDetail;
                                                                                                    //gán giản giá trên từng sản phẩm cho order
            #endregion
        }

        private void CheckVoucherUsed(string VoucherCode)
        {
            IVoucherService voucherService = DependencyUtils.Resolve<IVoucherService>();
            if (!string.IsNullOrEmpty(VoucherCode))
            {
                var voucher = voucherService.GetVoucherIsNotUsedAndCode(VoucherCode);
                if (voucher.MembershipCardId == null)
                {
                    voucher.UsedQuantity++;
                    if (voucher.UsedQuantity >= voucher.Quantity)
                    {
                        voucher.isUsed = true;
                    }
                    var voucherVM = new VoucherAPIViewModel(voucher);
                    voucher = voucherVM.ToEntity();
                    voucherService.Update(voucher);
                }
                else
                {
                    voucher.isUsed = true;
                    var voucherVM = new VoucherAPIViewModel(voucher);
                    voucher = voucherVM.ToEntity();
                    voucherService.Update(voucher);
                }
            }
        }
        public BaseResponse<OrderHistoryAPIViewModel> AddOrderFromMobile(OrderAPIViewModel order)
        {
            using (TransactionScope scope =
            new TransactionScope(TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead }))
            {
                try
                {
                    #region call service
                    DateTime time = DataService.Models.Utils.GetCurrentDateTime();
                    IStoreService storeService = DependencyUtils.Resolve<IStoreService>();
                    IDeliveryInfoService deliveryInfoService = DependencyUtils.Resolve<IDeliveryInfoService>();
                    IVoucherService voucherService = DependencyUtils.Resolve<IVoucherService>();
                    var orderService = DependencyUtils.Resolve<IOrderService>();
                    var productService = DependencyUtils.Resolve<IProductService>();
                    #endregion

                    #region set default order
                    order.DeliveryStatus = (int)DeliveryStatus.New;
                    //order.OrderType = (int)OrderTypeEnum.Delivery;
                    order.OrderStatus = (int)OrderStatusEnum.New;
                    order.InvoiceID = ConstantManager.PREFIX_MOBILE + "-" + order.StoreID + "-" + InvoiceCodeGenerator.GenerateInvoiceCode(); // truyền mã hóa don
                    order.SourceType = (int)SourceTypeEnum.Mobile;
                    order.IsSync = false;
                    int paymentType = order.PaymentType;
                    #endregion

                    #region check customer existed
                    var customer = new CustomerDomain().GetCustomerById(order.CustomerID.Value);
                    if (customer == null)
                    {
                        throw ApiException.Get(false, ConstantManager.MES_CHECK_CUSTOMERID_FAIL, ResultEnum.CustomerNotFound, HttpStatusCode.NotFound);
                    }
                    #endregion
                    #region check parent and child OrderDetail
                    foreach (var item in order.OrderDetails)
                    {
                        if (item.ParentId == 0)
                        {
                            item.ParentId = null;
                        }
                        if (item.ParentId != null && item.ParentId > -1)
                        {
                            var parentOrderDetail = order.OrderDetails.FirstOrDefault(od => od.TmpDetailId == item.ParentId);
                            if (parentOrderDetail != null)
                            {
                                var listExtra = productService.getProductExtraById(parentOrderDetail.ProductID);
                                var check = listExtra.FirstOrDefault(e => e.ProductID == item.ProductID);
                                if (check == null)
                                {
                                    throw ApiException.Get(false, ConstantManager.MES_CHILD_ORDERDETAIL_WRONG, ResultEnum.ChildOrderDetailWrong, HttpStatusCode.BadRequest);
                                }
                            }
                            else
                            {
                                throw ApiException.Get(false, ConstantManager.MES_PARENT_ORDERDETAIL_NOTFOUND, ResultEnum.ParentOrderDetailNotFound, HttpStatusCode.BadRequest);
                            }

                        }
                    }
                    #endregion

                    #region calculate price of order detail and order
                    #region check voucher existed
                    if (!string.IsNullOrEmpty(order.VoucherCode) )
                    {
                        var voucher = voucherService.GetVoucherIsNotUsedAndCode(order.VoucherCode);
                        if (voucher == null)
                        {
                            throw ApiException.Get(false, ConstantManager.MES_CREATE_ORDER_NOT_FOUND_VOUCHER, ResultEnum.VoucherNotFound, HttpStatusCode.NotFound);
                        }
                    }
                    #endregion
                    CalculateOrderPrice(order, time);
                    #endregion
                    
                    #region Update Order Type info
                    switch (order.OrderType)
                    {
                        case (int)OrderTypeEnum.MobileDelivery:
                            var deliveryInfo = deliveryInfoService.GetDeliveryById(order.DeliveryInfoId);
                            if (deliveryInfo != null && deliveryInfo.CustomerId == customer.CustomerID)
                            {
                                order.Receiver = deliveryInfo.CustomerName;
                                order.DeliveryAddress = deliveryInfo.Address;
                                order.DeliveryPhone = deliveryInfo.Phone;
                            }
                            else
                            {
                                throw ApiException.Get(true, ConstantManager.MES_DELIVERYID_WRONG, ResultEnum.DeliveryNotFound, HttpStatusCode.OK);
                            }
                            break;
                        case (int)OrderTypeEnum.AtStore:
                            order.DeliveryAddress = OrderTypeEnum.AtStore.DisplayName();
                            order.Receiver = customer.Name;
                            order.DeliveryPhone = customer.Phone;
                            break;
                        case (int)OrderTypeEnum.MobileTakeAway:
                            order.DeliveryAddress = OrderTypeEnum.MobileTakeAway.DisplayName();
                            order.Receiver = customer.Name;
                            order.DeliveryPhone = customer.Phone;
                            break;
                        default:
                            if (string.IsNullOrEmpty(order.DeliveryAddress))
                            {
                                throw ApiException.Get(true, ConstantManager.MES_ORDERTYPE_NOTSUPPORT, ResultEnum.OrderTypeNotSupport, HttpStatusCode.OK);
                            }
                            break;
                    }
                    
                    #endregion

                    //get CallCenter storeid
                    //var mobileStore = storeService.GetStoresByBrandIdAndType(order.BrandId, (int)StoreTypeEnum.MobileApp).FirstOrDefault();
                    //if (mobileStore != null)
                    //{
                    //    order.StoreID = mobileStore.ID;
                    //}

                    order.GroupPaymentStatus = 0; //Tam thoi chua xài
                    order.PaymentStatus = (int)OrderPaymentStatusEnum.Finish;
                    customer.BrandId = order.BrandId;

                    
                    #region Update payment
                    new PaymentDomain().UpdatePayment(order, customer, time);
                    #endregion
                    if (order.Payments.Count <= 0)
                    {
                        throw ApiException.Get(true, ConstantManager.MES_PAYMENTTYPE_NOTSUPPORT, ResultEnum.PaymenTypeNotSupport, HttpStatusCode.OK);
                    }
                    var orderTest = order.ToEntity();
                    var rs = orderService.CreateOrder(orderTest);

                    

                    if (rs == false)
                    {
                        throw ApiException.Get(false, ConstantManager.MES_CREATE_ORDER_FAIL, ResultEnum.CreateFail, HttpStatusCode.InternalServerError);

                    }

                    #region Update voucher after used
                    CheckVoucherUsed(order.VoucherCode);
                    #endregion
                    scope.Complete();
                    scope.Dispose();
                    var orderFinal = orderService.GetOrderByRenId(orderTest.RentID);
                    orderFinal.PaymentType = paymentType;
                    return BaseResponse<OrderHistoryAPIViewModel>.Get(true, ConstantManager.MES_CREATE_ORDER_SUCCESS, orderFinal, ResultEnum.Success);
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    if (e is ApiException)
                    {
                        throw e;
                    }
                    else
                    {
                        throw ApiException.Get(false, ConstantManager.MES_CREATE_ORDER_FAIL, ResultEnum.CreateFail, HttpStatusCode.InternalServerError);
                    }
                }
            }
        }
        private int UpdateOrderStatus(int OrderStatus, int DeliveryStatus)
        {
            var status = 0;
            #region Update status
            //Đơn hàng mới từ POS
            if (OrderStatus == (int)Models.DeliveryStatus.New)
            {
                if (OrderStatus == (int)OrderStatusEnum.PosFinished)
                {
                    status = (int)OrderStatusEnum.Finish;
                }
                else if (OrderStatus == (int)OrderStatusEnum.PosProcessing
                            || OrderStatus == (int)OrderStatusEnum.Processing) //Giữ nguyên
                {
                    status = (int)OrderStatusEnum.Processing;
                }
                else if (OrderStatus == (int)OrderStatusEnum.PosPreCancel)
                {
                    status = (int)OrderStatusEnum.PreCancel;
                }
                else if (OrderStatus == (int)OrderStatusEnum.PosCancel)
                {
                    status = (int)OrderStatusEnum.Cancel;
                }
            }
            //Đơn hàng delivery
            else if (DeliveryStatus != (int)Models.DeliveryStatus.New)
            {
                //Giao hàng thành công
                if (DeliveryStatus == (int)Models.DeliveryStatus.Finish)
                {
                    status = (int)OrderStatusEnum.Finish;
                }
                //Đơn hàng hủy trước chế biến
                else if (DeliveryStatus == (int)Models.DeliveryStatus.PreCancel)
                {
                    status = (int)OrderStatusEnum.PreCancel;
                }
                //Đơn hàng hủy sau chế biến || Giao hàng không thành công
                else if (DeliveryStatus == (int)Models.DeliveryStatus.Cancel
                            || DeliveryStatus == (int)Models.DeliveryStatus.Fail)
                {
                    status = (int)OrderStatusEnum.Cancel;
                }
            }
            #endregion
            return status;
        }
        public OrderAPIViewModel AddOrderFromPOS(OrderAPIViewModel model)
        {
            try
            {
                var status = 0;
                var orderService = DependencyUtils.Resolve<IOrderService>();
                var productService = DependencyUtils.Resolve<IProductService>();
                var paymentService = DependencyUtils.Resolve<IPaymentService>();
                var orderDetailService = DependencyUtils.Resolve<IOrderDetailService>();
                var orderDetailDomain = new OrderDetailDomain();

                //Cập nhật status đơn hàng
                status = UpdateOrderStatus(model.OrderStatus, model.DeliveryStatus.Value);


                //API return success case
                var result = new OrderAPIViewModel()
                {
                    OrderStatus = status,
                    InvoiceID = model.OrderCode,
                    DeliveryStatus = model.DeliveryStatus,
                    CheckInPerson = model.CheckInPerson,
                };

                //Check if order is existed in server
                var checkRent = orderService.GetOrderByInvoiceId(model.OrderCode);

                //Nếu chưa có sẽ add đơn hàng vào db server
                if (checkRent == null)
                {
                    result = AddOrder(status, model, result);
                }
                //Nếu đơn hàng đã có trên server sẽ update payment hoặc order detail
                else
                {
                    result = UpdateOrder(status, checkRent, model, result);
                }

                return result;
            }
            catch (Exception ex)
            {
                return new OrderAPIViewModel
                {
                    OrderStatus = model.OrderStatus,
                    InvoiceID = model.OrderCode,
                    DeliveryStatus = model.DeliveryStatus,
                    CheckInPerson = model.CheckInPerson,
                    //Something wrong, return old status
                };
            }
        }
        private OrderAPIViewModel UpdateOrder(int status, Models.Entities.Order order, OrderAPIViewModel model, OrderAPIViewModel result)
        {
            var orderService = DependencyUtils.Resolve<IOrderService>();
            var productService = DependencyUtils.Resolve<IProductService>();
            var paymentService = DependencyUtils.Resolve<IPaymentService>();
            var orderDetailService = DependencyUtils.Resolve<IOrderDetailService>();
            var orderDetailDomain = new OrderDetailDomain();
            #region Đơn hàng mới sẽ cập nhật order detail và payment
            if (model.DeliveryStatus == (int)DeliveryStatus.New)
            {
                //Không được edit đơn hàng finish trên server !!!
                if (order.OrderStatus == (int)OrderStatusEnum.Finish)
                {
                    result.OrderStatus = model.OrderStatus;
                    //Return old status
                }
                #region Start to edit if this order != finished
                //Chỉ edit khi đơn hàng chưa finish trên server 
                else if (order.OrderStatus != (int)OrderStatusEnum.Finish)
                {
                    var modifiedPayment = false;
                    var modifiedOrderDetail = false;

                    #region Nếu chưa từng có orderdetail hoặc có sự thay đổi orderdetail thì sẽ xóa hết order detail
                    if (!order.OrderDetails.Any()
                        || order.LastModifiedOrderDetail == null
                        || (order.LastModifiedOrderDetail != null &&
                            order.LastModifiedOrderDetail != model.LastModifiedOrderDetail))
                    {
                        //Delete all orderdetail
                        var lastOdList = order.OrderDetails.ToList();
                        foreach (var od in lastOdList)
                        {
                            order.OrderDetails.Remove(od);
                            var orderDetailCurrent = orderDetailService.Get(od.OrderDetailID);
                            orderDetailService.Delete(orderDetailCurrent);
                        }
                        modifiedOrderDetail = true;
                    }
                    #endregion

                    #region Đơn hàng đang xử lý hoặc đã thành công mới cập nhật payment bằng cách củ chuối xóa hết rồi insert lại
                    if (status == (int)OrderStatusEnum.Finish
                       || status == (int)OrderStatusEnum.Processing)
                    {
                        //Chưa từng có payment hoặc có sự thay đổi payment
                        if (!order.Payments.Any()
                            || order.Payments.Sum(p => p.Amount) == 0
                            || order.LastModifiedPayment == null
                            || (order.LastModifiedPayment != null &&
                                order.LastModifiedPayment != model.LastModifiedPayment))
                        {
                            //Delete all payment
                            var lastPList = order.Payments.ToList();
                            foreach (var p in lastPList)
                            {
                                order.Payments.Remove(p);
                                var currentpayment = paymentService.Get(p.PaymentID);
                                paymentService.Delete(currentpayment);
                            }
                            modifiedPayment = true;
                        }
                    }
                    #endregion

                    #region redload order db nếu có bất kì sự thay đổi
                    if (modifiedPayment || modifiedOrderDetail)
                    {
                        order = null;
                        order = orderService.GetOrderByInvoiceId(model.OrderCode);
                    }
                    #endregion

                    #region update if modify Payment or OrderDetail
                    if (modifiedOrderDetail)
                    {
                        //Vì có sự thay đổi orderdetail -> order có thay đổi -> update order
                        //Order
                        order = model.ToEntity();

                        //Orderdetail
                        foreach (var odm in model.OrderDetails.ToList())
                        {
                            var productId = productService.GetProductByCode(odm.ProductCode).ProductID;
                            var orderdetail = odm.ToEntity();
                            orderdetail.ProductID = productId;
                            orderdetail.StoreId = model.StoreID;

                            //Orderdetail Promotion Mapping
                            //Đối với đơn hàng từ POS thì phải finish mới có promotion (đơn hàng khác chưa biết..)
                            if (status == (int)OrderStatusEnum.Finish)
                            {
                                foreach (var odpm in odm.OrderDetailPromotionMappings.ToList())
                                {
                                    var mapping = odpm.ToEntity();
                                    mapping.PromotionId = (new PromotionService())
                                                .GetByPromoCode(odpm.PromotionCode).PromotionID;
                                    mapping.PromotionDetailId = (new PromotionDetailService())
                                                .GetDetailByPromotionDetailCode(odpm.PromotionDetailCode).PromotionDetailID;

                                    orderdetail.OrderDetailPromotionMappings.Add(mapping);
                                }
                            }

                            order.OrderDetails.Add(orderdetail);
                        }

                        //Order Promotion Mapping
                        //Đối với đơn hàng từ POS thì phải finish mới có promotion (đơn hàng khác chưa biết..)
                        if (status == (int)OrderStatusEnum.Finish)
                        {
                            foreach (var opm in model.OrderPromotionMappings.ToList())
                            {
                                var mapping = opm.ToEntity();
                                mapping.PromotionId = (new PromotionService())
                                            .GetByPromoCode(opm.PromotionCode).PromotionID;
                                mapping.PromotionDetailId = (new PromotionDetailService())
                                            .GetDetailByPromotionDetailCode(opm.PromotionDetailCode).PromotionDetailID;

                                order.OrderPromotionMappings.Add(mapping);
                            }
                        }
                    }

                    if (modifiedPayment)
                    {
                        //Payment
                        foreach (var p in model.Payments.ToList())
                        {
                            var payment = p.ToEntity();
                            order.Payments.Add(payment);
                        }
                    }

                    //Save
                    order.OrderStatus = status;
                    order.DeliveryStatus = model.DeliveryStatus;
                    order.CheckInPerson = model.CheckInPerson;
                    orderService.EditOrder(order);

                    if (modifiedOrderDetail)
                    {
                        //Update tmpId
                        orderDetailDomain.UpdateOrderDetailId(order);
                        orderService.EditOrder(order);
                    }
                    //Success
                    #endregion
                }
                #endregion
            }
            #endregion
            #region Đơn hàng cũ thì sẽ tiến hành cập nhật lại payment
            //Đơn hàng delivery
            else
            {
                //Không có sự thay đổi ở orderdetail
                #region Nếu đã hoàn thành hoặc đang xử lí sẽ cập nhật payment
                if (status == (int)OrderStatusEnum.Finish
                   || status == (int)OrderStatusEnum.Processing)
                {
                    var modifiedPayment = false;

                    #region delete all payment in order
                    var pList = order.Payments.ToList();
                    foreach (var p in pList)
                    {
                        modifiedPayment = true;
                        order.Payments.Remove(p);
                        var paymentcurrent = paymentService.Get(p.PaymentID);
                        paymentService.Delete(paymentcurrent);
                    }
                    #endregion

                    #region redload order db if any payment is deleted
                    if (modifiedPayment)
                    {
                        order = null;
                        order = orderService.GetOrderByInvoiceId(model.OrderCode);
                    }
                    #endregion
                    var checkStatusMomo = false;

                    #region add payment from Order view model to order again. Check if there is any Momo payment
                    foreach (var p in model.Payments.ToList())
                    {
                        var payment = p.ToEntity();
                        if (payment.Type == (int)PaymentTypeEnum.MoMo)
                        {
                            checkStatusMomo = true;
                        }
                        order.Payments.Add(payment);
                    }
                    #endregion

                    //for Order payed by Momo
                    if (checkStatusMomo && status == (int)OrderStatusEnum.Finish)
                    {
                        new PaymentDomain().SendMomoPaymentNotification(order);
                        ////update
                        //Fix Order sent Don't Have Payment when finish 
                        if (model.FinalAmount > 0)
                        {
                            if (model.FinalAmount != (model.TotalAmount - model.Discount - model.DiscountOrderDetail))
                            {
                                return new OrderAPIViewModel
                                {
                                    OrderStatus = model.OrderStatus,
                                    InvoiceID = model.OrderCode,
                                    DeliveryStatus = model.DeliveryStatus,
                                    CheckInPerson = model.CheckInPerson,
                                    //Something wrong, return old status
                                };
                            }
                            //TODO: Fix bug Miss Payment
                            if (model.Payments.Count() == 0)
                            {
                                var payment = new Payment();
                                payment.Amount = model.FinalAmount;
                                payment.Status = (int)OrderPaymentStatusEnum.Finish;
                                payment.Type = model.Att1 != null ? (int)PaymentTypeEnum.MemberPayment : (int)PaymentTypeEnum.Cash;
                                payment.PayTime = model.CheckInDate.Value;
                                payment.FCAmount = 0;
                                order.Payments.Add(payment);
                            }
                        }
                    }

                    order.OrderStatus = status;
                    order.DeliveryStatus = model.DeliveryStatus;
                    order.CheckInPerson = model.CheckInPerson;
                    orderService.EditOrder(order);
                    //Success
                }
                #endregion
                #region if status = pre cancel
                else if (status == (int)OrderStatusEnum.PreCancel)
                {
                    order.OrderStatus = status;
                    order.DeliveryStatus = model.DeliveryStatus;
                    order.CheckInPerson = model.CheckInPerson;
                    orderService.EditOrder(order);
                }
                #endregion
                else
                //Không có gì cập nhật
                {
                    result.OrderStatus = model.OrderStatus;
                    //Return old status
                }
            }
            #endregion
            return result;
        }
        private OrderAPIViewModel AddOrder(int status, OrderAPIViewModel model, OrderAPIViewModel result)
        {
            var orderService = DependencyUtils.Resolve<IOrderService>();
            var productService = DependencyUtils.Resolve<IProductService>();
            var orderDetailService = DependencyUtils.Resolve<IOrderDetailService>();
            var orderDetailDomain = new OrderDetailDomain();
            //Order
            var order = model.ToEntity();
            order.OrderStatus = status;
            order.DeliveryStatus = model.DeliveryStatus;
            order.CheckInPerson = model.CheckInPerson;

            #region cập nhật order detail
            foreach (var odm in model.OrderDetails.ToList())
            {
                var productId = productService.GetProductByCode(odm.ProductCode).ProductID;
                var orderdetail = odm.ToEntity();
                orderdetail.ProductID = productId;
                orderdetail.StoreId = model.StoreID;

                //Orderdetail Promotion Mapping
                //Đối với đơn hàng từ POS thì phải finish mới có promotion (đơn hàng khác chưa biết..)
                if (model.DeliveryStatus == (int)DeliveryStatus.New
                        && status == (int)OrderStatusEnum.Finish)
                {
                    foreach (var odpm in odm.OrderDetailPromotionMappings.ToList())
                    {
                        var mapping = odpm.ToEntity();
                        mapping.PromotionId = (new PromotionService())
                                    .GetByPromoCode(odpm.PromotionCode).PromotionID;
                        mapping.PromotionDetailId = (new PromotionDetailService())
                                    .GetDetailByPromotionDetailCode(odpm.PromotionDetailCode).PromotionDetailID;
                        orderdetail.OrderDetailPromotionMappings.Add(mapping);
                    }
                }

                order.OrderDetails.Add(orderdetail);
            }
            #endregion

            #region map Order với Promotion
            //Đối với đơn hàng từ POS thì phải finish mới có promotion (đơn hàng khác chưa biết..)
            if (model.DeliveryStatus == (int)DeliveryStatus.New
                    && status == (int)OrderStatusEnum.Finish)
            {
                foreach (var opm in model.OrderPromotionMappings.ToList())
                {
                    var mapping = opm.ToEntity();
                    mapping.PromotionId = (new PromotionService())
                                .GetByPromoCode(opm.PromotionCode).PromotionID;
                    mapping.PromotionDetailId = (new PromotionDetailService())
                                .GetDetailByPromotionDetailCode(opm.PromotionDetailCode).PromotionDetailID;
                    order.OrderPromotionMappings.Add(mapping);
                }
            }
            #endregion

            #region cập nhật payment
            //Finished || Processing Order -> Save Payment
            if (status == (int)OrderStatusEnum.Finish
                || status == (int)OrderStatusEnum.Processing)
            {
                var checkStatusMomo = false;
                //if 
                foreach (var p in model.Payments.ToList())
                {
                    var payment = p.ToEntity();
                    if (payment.Type == (int)PaymentTypeEnum.MoMo)
                    {
                        checkStatusMomo = true;
                    }
                    order.Payments.Add(payment);
                }

                if (checkStatusMomo == true && (int)OrderStatusEnum.Finish == status)
                {
                    new PaymentDomain().SendMomoPaymentNotification(order);
                }

                if (status == (int)OrderStatusEnum.Finish)
                {
                    //Fix Order sent Don't Have Payment when finish 
                    if (model.FinalAmount > 0)
                    {
                        if (model.FinalAmount != (model.TotalAmount - model.Discount - model.DiscountOrderDetail))
                        {
                            return new OrderAPIViewModel
                            {
                                OrderStatus = model.OrderStatus,
                                InvoiceID = model.OrderCode,
                                DeliveryStatus = model.DeliveryStatus,
                                CheckInPerson = model.CheckInPerson,
                                //Something wrong, return old status
                            };
                        }
                        //TODO: Fix bug Miss Payment
                        if (model.Payments.Count() == 0)
                        {
                            var payment = new Payment();
                            payment.Amount = model.FinalAmount;
                            payment.Status = (int)OrderPaymentStatusEnum.Finish;
                            payment.Type = model.Att1 != null ? (int)PaymentTypeEnum.MemberPayment : (int)PaymentTypeEnum.Cash;
                            payment.PayTime = model.CheckInDate.Value;
                            payment.FCAmount = 0;
                            order.Payments.Add(payment);
                        }
                    }
                }
            }
            else
            {
                //Không lưu payment
            }
            #endregion

            #region Save Order
            var created = orderService.CreateOrder(order);

            if (created)
            {
                //Update tmpId
                orderDetailDomain.UpdateOrderDetailId(order);
                orderService.EditOrder(order);
                //Success
            }
            else
            {
                result.OrderStatus = model.OrderStatus;
                //Fail - Return old status
            }
            #endregion
            return result;
        }



        public async Task<OrderAPIViewModel> GetOrder(int orderId)
        {
            var orderService = DependencyUtils.Resolve<IOrderService>();
            var order = await orderService.GetOrder(orderId);
            return order;
        }


        //public int CountTotalOrdersOneDay(DateTime dateTime)
        //{
        //    IOrderService orderService = DependencyUtils.Resolve<IOrderService>();
        //    int count =  orderService.CountTotalOrderInOneDay(dateTime);
        //    return count;
        //}

        public BaseResponse<List<OrderHistoryAPIViewModel>> GetOrderHistoryByRequest(OrderRequest<string> request)
        {
            var ser = this.Service<IOrderService>();

            var list = ser.GetOrderHistoryByRequest(request);
            if (list.Count <= 0)
            {
                throw ApiException.Get(true, ConstantManager.MES_ORDER_HISTORY_NOTFOUND, ResultEnum.OrderHistoryNotFound, HttpStatusCode.OK);
            }
            return BaseResponse<List<OrderHistoryAPIViewModel>>.Get(true, ConstantManager.MES_SUCCESS, list, ResultEnum.Success);
        }

    }
}
