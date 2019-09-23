using AutoMapper.QueryableExtensions;
using DataService.APIViewModels;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Models.APIModels.Promotion;
using DataService.Models.Entities;
using DataService.Models.Entities.Services;
using DataService.Utilities;
using DataService.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Transactions;

namespace DataService.Domain
{
    public partial interface IPromotionDomain
    {
        IEnumerable<Partner> GetPartner(Expression<Func<Partner, bool>> expr);
        IEnumerable<Partner> GetPartner();

        IEnumerable<Promotion> GetPromotion(Expression<Func<Promotion, bool>> expr);
        IEnumerable<Promotion> GetPromotion();
        IEnumerable<Promotion> GetPromotion(PromotionQueryRequest<dynamic> request);
        Promotion GetPromotion(int pId);
        Promotion GetPromotion(string pCode);
        Promotion Update(Promotion p);
        Promotion Create(Promotion p);

        IEnumerable<PromotionDetail> GetPromotionDetail(Expression<Func<PromotionDetail, bool>> expr);
        IEnumerable<PromotionDetail> GetPromotionDetail();
        PromotionDetail GetPromotionDetail(int pDetailId);
        PromotionDetail GetPromotionDetail(string pDetailCode);
        IEnumerable<PromotionDetail> GetPromotionDetailsByPromotion(string promotionCode);

        IEnumerable<Voucher> GetVoucher(Expression<Func<Voucher, bool>> expr);
        IEnumerable<Voucher> GetVoucher();
        List<VoucherAPIViewModel> GetVoucher(VoucherQueryRequest<dynamic> request);
        Voucher GetVoucher(int voucherId);
        Voucher GetVoucher(string voucherCode);
        Voucher Update(Voucher p);
        Voucher Create(Voucher p);

        Promotion ChangePromotionActiveState(int promotionId, bool active);
        Promotion ChangePromotionActiveState(string promotionCode, bool active);
        Promotion ChangePromotionActiveState(Promotion promotion, bool active);

        BaseResponse<Promotion> IsPromotionValidFor(int promotionId, OrderAPIViewModel order, string cardCode = null);
        BaseResponse<Promotion> IsPromotionValidFor(string promotionCode, OrderAPIViewModel order, string cardCode = null);
        BaseResponse<Promotion> IsPromotionValidFor(Promotion promotion, OrderAPIViewModel order, Membership membership = null);

        BaseResponse<Voucher> IsVoucherValidFor(int voucherId, OrderAPIViewModel order, string cardCode = null);
        BaseResponse<Voucher> IsVoucherValidFor(string voucherCode, OrderAPIViewModel order, string cardCode = null);
        BaseResponse<Voucher> IsVoucherValidFor(Voucher voucher, OrderAPIViewModel order, Membership membership = null);

        BaseResponse<PromotionDetail> IsPromotionDetailValidFor(int pDetailId, OrderAPIViewModel order, string cardCode = null);
        BaseResponse<PromotionDetail> IsPromotionDetailValidFor(string pDetailCode, OrderAPIViewModel order, string cardCode = null);
        BaseResponse<PromotionDetail> IsPromotionDetailValidFor(PromotionDetail pDetail, OrderAPIViewModel order, Membership membership = null);

        BaseResponse<PromotionDetail> IsPromotionDetailValidFor(int pDetailId, IEnumerable<OrderDetailAPIViewModel> OrderDetailAPIViewModels, string cardCode = null);
        BaseResponse<PromotionDetail> IsPromotionDetailValidFor(string pDetailCode, IEnumerable<OrderDetailAPIViewModel> OrderDetailAPIViewModels, string cardCode = null);
        BaseResponse<PromotionDetail> IsPromotionDetailValidFor(PromotionDetail pDetail, IEnumerable<OrderDetailAPIViewModel> OrderDetailAPIViewModels, Membership membership = null);

        BaseResponse<Voucher> IsVoucherAvailable(int voucherId);
        BaseResponse<Voucher> IsVoucherAvailable(string voucherCode);
        BaseResponse<Voucher> IsVoucherAvailable(Voucher voucher);

        BaseResponse<PromotionResponseViewModel> UseVoucher(Voucher voucherEntity, PromotionRequestViewModel baseVoucher);
        int CashBackPromotion(Membership mbs,double totalAmount);
    }

    public class PromotionDomain : BaseDomain, IPromotionDomain
    {

        private IPromotionService _promotionService;
        protected IPromotionService PromotionService
        {
            get
            {
                if (_promotionService == null)
                    _promotionService = this.Service<IPromotionService>();
                return _promotionService;
            }
        }
        private IVoucherService _voucherService;
        protected IVoucherService VoucherService
        {
            get
            {
                if (_voucherService == null)
                    _voucherService = this.Service<IVoucherService>();
                return _voucherService;
            }
        }
        private IPromotionDetailService _promoDetailService;
        protected IPromotionDetailService PromoDetailService
        {
            get
            {
                if (_promoDetailService == null)
                    _promoDetailService = this.Service<IPromotionDetailService>();
                return _promoDetailService;
            }
        }
        private IPartnerService _partnerService;
        protected IPartnerService PartnerService
        {
            get
            {
                if (_partnerService == null)
                    _partnerService = this.Service<IPartnerService>();
                return _partnerService;
            }
        }
        #region CashBack
        public int CashBackPromotion(Membership mbs,double totalAmount)
        {
            IPromotionService promotionService = DependencyUtils.Resolve<IPromotionService>();
            var promotionDetailService = DependencyUtils.Resolve<IPromotionDetailService>();
            int point = 0;
            // get membershipType
            var mbsType = mbs.MembershipType;
            // code check promotion CashBack
            var code = mbsType.AppendCode + "_" + mbs.MembershipCode;
            // get promotionCashBack
            var promotion = promotionService.GetByPromoCode(ConstantManager.PROMOTION_CASH_BACK);
            if(promotion == null)
            {
                return point;
            }
            // get list promotionDetail CashBack
            var promotionDetail = promotionDetailService.GetDetailByCode(promotion.PromotionCode).ToList();
            foreach (var item in promotionDetail)
            {
                var check = Regex.IsMatch(code, item.RegExCode);
                if (check)
                {
                    point = (int)(totalAmount / 10000 * item.PointTrade);

                    return point;
                }
            }
            return point;
        }
        #endregion

        #region Partner CRUD
        public IEnumerable<Partner> GetPartner()
        {
            return PartnerService.GetActive();
        }

        public IEnumerable<Partner> GetPartner(Expression<Func<Partner, bool>> expr)
        {
            return PartnerService.GetActive(expr);
        }
        #endregion

        #region Promotion CRUD
        public IEnumerable<Promotion> GetPromotion()
        {
            return PromotionService.GetActive();
        }

        public IEnumerable<Promotion> GetPromotion(Expression<Func<Promotion, bool>> expr)
        {
            return PromotionService.GetActive(expr);
        }

        public Promotion ChangePromotionActiveState(int promotionId, bool active)
        {
            var promotion = GetPromotion(promotionId);
            return ChangePromotionActiveState(promotion, active);
        }

        public Promotion ChangePromotionActiveState(string promotionCode, bool active)
        {
            var promotion = GetPromotion(promotionCode);
            return ChangePromotionActiveState(promotion, active);
        }

        public Promotion ChangePromotionActiveState(Promotion promotion, bool active)
        {
            if (active)
                PromotionService.Activate(promotion);
            else
                PromotionService.Deactivate(promotion);
            return promotion;
        }

        public Promotion GetPromotion(int pDetailId)
        {
            return PromotionService.GetActive(p => p.PromotionID == pDetailId).FirstOrDefault();
        }

        public Promotion GetPromotion(string pCode)
        {
            return PromotionService.GetActive(p => p.PromotionCode == pCode).FirstOrDefault();
        }

        public Promotion Update(Promotion p)
        {
            PromotionService.Update(p);
            return p;
        }

        public Promotion Create(Promotion p)
        {
            PromotionService.Create(p);
            return p;
        }

        #endregion

        #region Voucher CRUD
        public IEnumerable<Voucher> GetVoucher(Expression<Func<Voucher, bool>> expr)
        {
            return VoucherService.GetActive(expr);
        }

        public IEnumerable<Voucher> GetVoucher()
        {
            return VoucherService.GetActive();
        }

        public Voucher Update(Voucher p)
        {
            VoucherService.Update(p);
            return p;
        }

        public Voucher Create(Voucher p)
        {
            VoucherService.Create(p);
            return p;
        }

        public Voucher GetVoucher(string voucherCode)
        {
            return GetVoucher(v => v.VoucherCode == voucherCode).FirstOrDefault();
        }

        public Voucher GetVoucher(int voucherId)
        {
            return VoucherService.GetActive(p => p.VoucherID == voucherId).FirstOrDefault();
        }

        public List<VoucherAPIViewModel> GetVoucher(VoucherQueryRequest<dynamic> request)
        {
            var voucher = GetVoucher().Where(v => (1 == 1 || v.Promotion.BrandId == request.BrandId) &&
                                       (request.VoucherId == null || v.VoucherID == request.VoucherId) &&
                                       (request.VoucherCode == null || v.VoucherCode == request.VoucherCode) &&
                                       (request.PromotionId == null || v.PromotionID == request.PromotionId) &&
                                       (request.PromotionCode == null || v.Promotion.PromotionCode == request.PromotionCode));

            //if (request.VoucherId != null)
            //    voucher = voucher.Where(v => v.VoucherID == request.VoucherId);
            //if (request.VoucherCode != null)
            //    voucher = voucher.Where(v => v.VoucherCode == request.VoucherCode);
            //if (request.PromotionId != null)
            //    voucher = voucher.Where(v => v.PromotionID == request.PromotionId);
            //if (request.PromotionCode != null)
            //    voucher = voucher.Where(v => v.Promotion.PromotionCode == request.PromotionCode);
            if (request.AvailableOnly)
            {
                if (request.MembershipVM == null)
                    throw ApiException.Get(false, "Thiếu thông tin thành viên", ResultEnum.MembershipNotFound, HttpStatusCode.BadRequest);
                voucher = voucher.Where(v => !v.isUsed.Value
                    && v.UsedQuantity < v.Quantity && v.MembershipCardId == request.MembershipVM.Id);
            }
            if (request.UsedOnly)
            {
                if (request.MembershipVM == null)
                    throw ApiException.Get(false, "Thiếu thông tin thành viên", ResultEnum.MembershipNotFound, HttpStatusCode.BadRequest);
                voucher = voucher.Where(v => v.isUsed.Value
                    && v.UsedQuantity >= v.Quantity && v.MembershipCardId == request.MembershipVM.Id);
            }
            var voucherVM = voucher.AsQueryable().ProjectTo<VoucherAPIViewModel>(this.AutoMapperConfig).ToList();
            if (voucherVM.Count < 0)
            {
                throw ApiException.Get(false, "Không tìm thấy voucher nào", ResultEnum.VoucherNotFound, HttpStatusCode.NotFound);
            }
            return voucherVM;
        }
        #endregion

        #region PromotionDetail CRUD
        public IEnumerable<PromotionDetail> GetPromotionDetail(Expression<Func<PromotionDetail, bool>> expr)
        {
            return PromoDetailService.GetActive(expr);
        }

        public IEnumerable<PromotionDetail> GetPromotionDetail()
        {
            return PromoDetailService.GetActive();
        }

        public PromotionDetail GetPromotionDetail(int pDetailId)
        {
            return GetPromotionDetail(pD => pD.PromotionDetailID == pDetailId).FirstOrDefault();
        }

        public PromotionDetail GetPromotionDetail(string pDetailCode)
        {
            return GetPromotionDetail(pD => pD.PromotionDetailCode == pDetailCode).FirstOrDefault();
        }

        public IEnumerable<PromotionDetail> GetPromotionDetailsByPromotion(string promotionCode)
        {
            return GetPromotionDetail(pD => pD.PromotionCode == promotionCode);
        }

        public IEnumerable<Promotion> GetPromotion(PromotionQueryRequest<dynamic> request)
        {
            var promotion = GetPromotion();
            if (request.PromotionID != null)
                promotion = promotion.Where(p => p.PromotionID == request.PromotionID);
            if (request.PromotionCode != null)
                promotion = promotion.Where(p => p.PromotionCode == request.PromotionCode);
            //if (request.AvailableOnly)
            //{
            //    if (membership == null)
            //        throw ApiException.Get(false, "Thiếu thông tin thành viên", ResultEnum.CustomerNotFound, HttpStatusCode.BadRequest);
            //    promotion = promotion.Where(p =>
            //        (!p.IsVoucher.Value && p.IsForMember &&
            //            CheckMembership(p, GetPromotionDetailsByPromotion(p.PromotionCode), membership).Success)
            //        || (p.IsVoucher.Value && p.VoucherQuantity > 1 && p.VoucherUsedQuantity < p.VoucherQuantity)
            //        || (p.VoucherQuantity == 1 &&
            //            GetVoucher(v => v.PromotionID == p.PromotionID).FirstOrDefault().MembershipId == membership.Id)
            //    );
            //}
            //else if (request.UsedOnly)
            //{
            //    if (membership == null)
            //        throw ApiException.Get(false, "Thiếu thông tin thành viên", ResultEnum.CustomerNotFound, HttpStatusCode.BadRequest);

            //    promotion = promotion.Where(p =>
            //        p.Vouchers.Where(v => v.MembershipId == membership.Id && v.isUsed.Value).Any()
            //    );
            //}
            return promotion;
        }
        #endregion

        #region Business
        public BaseResponse<PromotionResponseViewModel> UseVoucher(Voucher voucherEntity, PromotionRequestViewModel request)
        {
            //Logger.Log("|UseVoucherTransaction| begin method");
            var resp = new BaseResponse<PromotionResponseViewModel>()
            {
                Success = true,
                Message = "Sữ dụng voucher thành công",
                Error = "Không có lỗi nha"
            };
            using (var trans = new TransactionScope(TransactionScopeOption.Required))
            {
                var brandService = this.Service<IBrandService>();
                var pService = PromotionService;
                var vService = VoucherService;
                var brandId = brandService.GetBrandOfStore(request.StoreId).Id;
                // Update số lượng sử dụng promotion
                var promotion = pService.Get<int>(voucherEntity.PromotionID);
                // Giảm số lượng đang hoạt động
                voucherEntity.UsedQuantity += 1;
                promotion.VoucherUsedQuantity += 1;
                if (voucherEntity.Quantity <= voucherEntity.UsedQuantity)
                {
                    voucherEntity.Active = false;
                    voucherEntity.isUsed = true;
                    voucherEntity.IsGetted = true;
                }
                //Logger.Log("|UseVoucherTransaction| update voucher");
                vService.Update(voucherEntity);
                if (promotion.VoucherQuantity <= promotion.VoucherUsedQuantity)
                {
                    promotion.Active = false;
                }
                pService.Update(promotion);
                //Logger.Log("|UseVoucherTransaction| update promotion");

                //check if voucher is in house or of partner
                if (promotion.ApplyToPartner != null)
                {
                    if (promotion.ApplyToPartner == 0) // temp InHouse id
                    {
                        //use voucher in house
                    }
                    else
                    {
                        var result = UsePartnerVoucher(request);
                        if (!result.Success)
                            return result;
                        resp.Message = result.Message;
                        resp.Data = result.Data;
                    }
                }
                //Logger.Log("|UseVoucherTransaction| partner confirm result: " + resp.Message + "-" + resp.Error);
                trans.Complete();
                resp.ResultCode = (int)Models.ResultEnum.Success;
                return resp;
            }
        }

        private BaseResponse<PromotionResponseViewModel> UsePartnerVoucher(PromotionRequestViewModel request)
        {
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri("http://apiurl");
                var resp = http.PostAsync("api/voucher/use", new JsonContent(request)).Result;
                return JsonConvert.DeserializeObject<BaseResponse<PromotionResponseViewModel>>(
                    resp.Content.ReadAsStringAsync().Result);
            }
        }

        public BaseResponse<Voucher> IsVoucherAvailable(string voucherCode)
        {
            var voucher = GetVoucher(voucherCode);
            return IsVoucherAvailable(voucher);
        }

        public BaseResponse<Voucher> IsVoucherAvailable(int voucherId)
        {
            var voucher = GetVoucher(voucherId);
            return IsVoucherAvailable(voucher);
        }

        public BaseResponse<Voucher> IsVoucherAvailable(Voucher voucher)
        {
            //Logger.Log("|CheckVoucherCode| voucherCode: " + model.voucherCode);
            BaseResponse<Voucher> res;
            if (voucher != null)
            {
                if ((voucher.isUsed != null && voucher.isUsed == false && voucher.Quantity > voucher.UsedQuantity)
                    || (voucher.isUsed == null && voucher.Active && voucher.Quantity > voucher.UsedQuantity))
                {
                    //Logger.Log("|CheckVoucherCode| Có thể sử dụng voucher", true);
                    res = new BaseResponse<Voucher>
                    {
                        Success = true,
                        Message = "Voucher có thể áp dụng",
                        ResultCode = (int)ResultEnum.Success,
                        Data = voucher
                    };
                    return res;
                }
                else
                {
                    //Logger.Log("|CheckVoucherCode| Voucher đã được sử dụng", true);
                    throw ApiException.Get(false, "Voucher đã được sử dụng", ResultEnum.VoucherUsed, HttpStatusCode.BadRequest);
                }
            }

            throw ApiException.Get(false, "Mã Voucher không tồn tại", ResultEnum.VoucherNotFound, HttpStatusCode.NotFound);
        }

        #region Check Promotion valid
        public BaseResponse<Promotion> IsPromotionValidFor(int promotionId, OrderAPIViewModel order, string cardCode = null)
        {
            var promotion = GetPromotion(promotionId);
            Membership membership = null;
            if (cardCode != null)
                membership = this.Service<IMembershipService>().GetMembershipByCode(cardCode);
            return IsPromotionValidFor(promotion, order, membership);
        }

        public BaseResponse<Promotion> IsPromotionValidFor(string promotionCode, OrderAPIViewModel order, string cardCode = null)
        {
            var promotion = GetPromotion(promotionCode);
            Membership membership = null;
            if (cardCode != null)
                membership = this.Service<IMembershipService>().GetMembershipByCode(cardCode);
            return IsPromotionValidFor(promotion, order, membership);
        }

        public BaseResponse<Promotion> IsPromotionValidFor(Promotion promotion, OrderAPIViewModel order, Membership membership = null)
        {
            var gCheck = CheckGeneralPromotionRule(promotion, order);

            var pDetails = GetPromotionDetailsByPromotion(promotion.PromotionCode);

            var mCheck = CheckMembership(promotion, pDetails, membership);

            if (promotion.ApplyLevel == (int)PromotionApplyLevelEnum.Order)
            {
                foreach (var pD in pDetails)
                {
                    var pDetailCheck = IsPromotionDetailValidFor(pD, order, membership);
                }
            }
            else
            {
                foreach (var pD in pDetails)
                {
                    var pDetailCheck = IsPromotionDetailValidFor(pD, order.OrderDetails, membership);
                }
            }

            return BaseResponse<Promotion>.Get(true, "Promotion can be applied", promotion, ResultEnum.Success, null);
        }

        //public BaseResponse<Promotion> IsPromotionValidFor(int promotionId, IEnumerable<OrderDetailAPIViewModel> OrderDetailAPIViewModels, OrderAPIViewModel order, string cardCode = null)
        //{
        //    var promotion = GetPromotion(promotionId);
        //    Membership membership = null;
        //    if (cardCode != null)
        //        membership = this.Service<IMembershipService>().GetMembershipByCode(cardCode);
        //    return IsPromotionValidFor(promotion, OrderDetailAPIViewModels, order, membership);
        //}

        //public BaseResponse<Promotion> IsPromotionValidFor(string promotionCode, IEnumerable<OrderDetailAPIViewModel> OrderDetailAPIViewModels, OrderAPIViewModel order, string cardCode = null)
        //{
        //    var promotion = GetPromotion(promotionCode);
        //    Membership membership = null;
        //    if (cardCode != null)
        //        membership = this.Service<IMembershipService>().GetMembershipByCode(cardCode);
        //    return IsPromotionValidFor(promotion, OrderDetailAPIViewModels, order, membership);
        //}

        //public BaseResponse<Promotion> IsPromotionValidFor(Promotion promotion, IEnumerable<OrderDetailAPIViewModel> OrderDetailAPIViewModels, OrderAPIViewModel order, Membership membership = null)
        //{
        //    var gCheck = CheckGeneralPromotionRule(promotion, order);
        //    if (!gCheck.Success)
        //        return BaseResponse<Promotion>.Get(false, gCheck.Message, null, ResultEnum.Success, gCheck.Error);

        //    var pDetails = GetPromotionDetailsByPromotion(promotion.PromotionCode);

        //    var mCheck = CheckMembership(promotion, pDetails, membership);
        //    if (!mCheck.Success)
        //        return BaseResponse<Promotion>.Get(false, mCheck.Message, null, ResultEnum.Success, mCheck.Error);

        //    if (promotion.ApplyLevel == (int)PromotionApplyLevelEnum.OrderDetailAPIViewModel)
        //    {
        //        foreach (var pD in pDetails)
        //        {
        //            var pDetailCheck = IsPromotionDetailValidFor(pD, OrderDetailAPIViewModels, membership);
        //            if (!pDetailCheck.Success)
        //                return BaseResponse<Promotion>.Get(false, pDetailCheck.Message, null, ResultEnum.Success, pDetailCheck.Error);
        //        }
        //    }
        //    else return BaseResponse<Promotion>.Get(
        //        false, "This is an OrderAPIViewModel level promotion", null,
        //        ResultEnum.Success, "This is an OrderAPIViewModel level promotion");

        //    return BaseResponse<Promotion>.Get(true, "Promotion can be applied", promotion, ResultEnum.Success, null);
        //}

        private BaseResponse<bool> CheckMembership(
            Promotion promotion, IEnumerable<PromotionDetail> pDetails, Membership membership = null)
        {
            if ((promotion.GiftType == (int)PromotionGiftTypeEnum.CashbackDiscountAmount
                || promotion.GiftType == (int)PromotionGiftTypeEnum.CashbackDiscountAmount
                || promotion.IsForMember))
            {
                if (membership == null)
                    throw ApiException.Get(false, "Khuyến mãi không áp dụng cho thành viên này", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);

                var code = membership.MembershipType.AppendCode + "_" + membership.MembershipCode;

                if (!pDetails.AsEnumerable().Any(pD => Regex.IsMatch(code, pD.RegExCode)))
                    throw ApiException.Get(false, "Khuyến mãi không áp dụng cho thành viên này", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);
            }

            return BaseResponse<bool>.Get(
                        true, "OK",
                        true, ResultEnum.Success, "OK");
        }

        private BaseResponse<bool> CheckGeneralPromotionRule(Promotion promotion, OrderAPIViewModel order)
        {
            if (order.BrandId != promotion.BrandId)
                throw ApiException.Get(false, "Khuyến mãi không dành cho hãng này", ResultEnum.PromotionNotApplyToBrand, HttpStatusCode.BadRequest);
            var now = DateTime.Now;
            if ((promotion.FromDate != null && now < promotion.FromDate)
                && (promotion.ToDate != null && now > promotion.ToDate))
                throw ApiException.Get(false, "Ngày áp dụng không hợp lệ", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);
            if ((promotion.ApplyFromTime != null && now.Hour < promotion.ApplyFromTime)
            && (promotion.ApplyToTime != null && now.Hour > promotion.ApplyToTime))
                throw ApiException.Get(false, "Thời gian áp dụng không hợp lệ", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);
            var sMappingService = Service<IPromotionStoreMappingService>();
            if (order.StoreID == null)
                throw ApiException.Get(false, "Thiếu thông tin cửa hàng", ResultEnum.LackOfInformation, HttpStatusCode.BadRequest);
            var mappings = sMappingService.GetActive(m => m.PromotionId == promotion.PromotionID)
                .Select(m => m.StoreId).AsEnumerable();
            if (!mappings.Contains(order.StoreID.Value))
                throw ApiException.Get(false, "Khuyến mãi không áp dụng cho cửa hàng này", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);

            return BaseResponse<bool>.Get(true, "Hợp lệ", true, ResultEnum.Success, null);
        }
        #endregion

        #region Check PromotionDetail valid
        public BaseResponse<PromotionDetail> IsPromotionDetailValidFor(int pDetailId, IEnumerable<OrderDetailAPIViewModel> OrderDetailAPIViewModels, string cardCode = null)
        {
            Membership membership = null;
            if (cardCode != null)
                membership = this.Service<IMembershipService>().GetMembershipByCode(cardCode);
            var pDetail = GetPromotionDetail(pDetailId);
            return IsPromotionDetailValidFor(pDetail, OrderDetailAPIViewModels, membership);
        }

        public BaseResponse<PromotionDetail> IsPromotionDetailValidFor(string pDetailCode, IEnumerable<OrderDetailAPIViewModel> OrderDetailAPIViewModels, string cardCode = null)
        {
            var pDetail = GetPromotionDetail(pDetailCode);
            Membership membership = null;
            if (cardCode != null)
                membership = this.Service<IMembershipService>().GetMembershipByCode(cardCode);
            return IsPromotionDetailValidFor(pDetail, OrderDetailAPIViewModels, membership);
        }

        public BaseResponse<PromotionDetail> IsPromotionDetailValidFor(PromotionDetail pDetail, IEnumerable<OrderDetailAPIViewModel> OrderDetailAPIViewModels, Membership membership = null)
        {
            foreach (var oD in OrderDetailAPIViewModels)
            {
                if (pDetail.BuyProductCode != null && pDetail.BuyProductCode != oD.ProductCode)
                    throw ApiException.Get(false, "Có sản phẩm không hợp lệ", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);

                if (pDetail.MinBuyQuantity != null)
                {
                    if (oD.Quantity < pDetail.MinBuyQuantity)
                        throw ApiException.Get(false, "Số lượng sản phẩm không phù hợp", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);
                }
                if (pDetail.MaxBuyQuantity != null)
                {
                    if (oD.Quantity > pDetail.MaxBuyQuantity)
                        throw ApiException.Get(false, "Số lượng sản phẩm không phù hợp", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);
                }
                var cashbackAcc = membership.Accounts.FirstOrDefault(a => a.Type == (int)AccountTypeEnum.PointAccount);
                if (cashbackAcc == null)
                    throw ApiException.Get(false, "Không thể tìm thấy tài khoản tích điểm", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);

                if (pDetail.MinPoint != null)
                {
                    if (cashbackAcc.Balance < pDetail.MinPoint)
                        throw ApiException.Get(false, "Điểm thành viên không hợp lệ", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);
                }
                if (pDetail.MaxPoint != null)
                {
                    if (cashbackAcc.Balance > pDetail.MaxPoint)
                        throw ApiException.Get(false, "Điểm thành viên không hợp lệ", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);
                }

            }

            return BaseResponse<PromotionDetail>.Get(true, "PromotionDetail can be applied", pDetail, ResultEnum.Success, null);
        }

        public BaseResponse<PromotionDetail> IsPromotionDetailValidFor(int pDetailId, OrderAPIViewModel order, string cardCode = null)
        {
            Membership membership = null;
            if (cardCode != null)
                membership = this.Service<IMembershipService>().GetMembershipByCode(cardCode);
            var pDetail = GetPromotionDetail(pDetailId);
            return IsPromotionDetailValidFor(pDetail, order, membership);
        }

        public BaseResponse<PromotionDetail> IsPromotionDetailValidFor(string pDetailCode, OrderAPIViewModel order, string cardCode = null)
        {
            Membership membership = null;
            if (cardCode != null)
                membership = this.Service<IMembershipService>().GetMembershipByCode(cardCode);
            var pDetail = GetPromotionDetail(pDetailCode);
            return IsPromotionDetailValidFor(pDetail, order, membership);
        }

        public BaseResponse<PromotionDetail> IsPromotionDetailValidFor(PromotionDetail pDetail, OrderAPIViewModel order, Membership membership = null)
        {
            if (pDetail.MinOrderAmount != null)
            {
                if (order.TotalAmount < pDetail.MinOrderAmount)
                    throw ApiException.Get(false, "Tổng thanh toán không hợp lệ", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);
            }
            if (pDetail.MaxOrderAmount != null)
            {
                if (order.TotalAmount > pDetail.MaxOrderAmount)
                    throw ApiException.Get(false, "Tỏng thanh toán không hợp lệ", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);
            }
            var cashbackAcc = membership.Accounts.FirstOrDefault(a => a.Type == (int)AccountTypeEnum.PointAccount);
            if (cashbackAcc == null)
                throw ApiException.Get(false, "Không thể tìm thấy tài khoản tích điểm", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);
            if (pDetail.MinPoint != null)
            {
                if (cashbackAcc.Balance < pDetail.MinPoint)
                    throw ApiException.Get(false, "Điểm thành viên không hợp lệ", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);
            }
            if (pDetail.MaxPoint != null)
            {
                if (cashbackAcc.Balance > pDetail.MaxPoint)
                    throw ApiException.Get(false, "Điểm thành viên không hợp lệ", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);
            }

            return BaseResponse<PromotionDetail>.Get(true, "PromotionDetail can be applied", pDetail, ResultEnum.Success, null);
        }
        #endregion

        #region Check Voucher valid
        public BaseResponse<Voucher> IsVoucherValidFor(int voucherId, OrderAPIViewModel order, string cardCode = null)
        {
            var voucher = GetVoucher(voucherId);
            Membership membership = null;
            if (cardCode != null)
                membership = this.Service<IMembershipService>().GetMembershipByCode(cardCode);
            return IsVoucherValidFor(voucher, order, membership);
        }

        public BaseResponse<Voucher> IsVoucherValidFor(string voucherCode, OrderAPIViewModel order, string cardCode = null)
        {
            var voucher = GetVoucher(voucherCode);
            Membership membership = null;
            if (cardCode != null)
                membership = this.Service<IMembershipService>().GetMembershipByCode(cardCode);
            return IsVoucherValidFor(voucher, order, membership);
        }

        public BaseResponse<Voucher> IsVoucherValidFor(Voucher voucher, OrderAPIViewModel order, Membership membership = null)
        {
            var available = IsVoucherAvailable(voucher);

            if (voucher.MembershipCardId != null)
                if (voucher.MembershipCardId != membership.Id)
                    throw ApiException.Get(false, "Voucher không dành cho thành viên này", ResultEnum.VoucherNotAvailable, HttpStatusCode.BadRequest);

            var pValid = IsPromotionValidFor(voucher.Promotion, order, membership);

            return BaseResponse<Voucher>.Get(true, "Voucher có thể sử dụng", voucher, ResultEnum.Success, null);
        }

        

        //public BaseResponse<Voucher> IsVoucherValidFor(int voucherId, IEnumerable<OrderDetailAPIViewModel> OrderDetailAPIViewModels, OrderAPIViewModel order, string cardCode = null)
        //{
        //    var voucher = GetVoucher(voucherId);
        //    Membership membership = null;
        //    if (cardCode != null)
        //        membership = this.Service<IMembershipService>().GetMembershipByCode(cardCode);
        //    return IsVoucherValidFor(voucher, OrderDetailAPIViewModels, order, membership);
        //}

        //public BaseResponse<Voucher> IsVoucherValidFor(string voucherCode, IEnumerable<OrderDetailAPIViewModel> OrderDetailAPIViewModels, OrderAPIViewModel order, string cardCode = null)
        //{
        //    var voucher = GetVoucher(voucherCode);
        //    Membership membership = null;
        //    if (cardCode != null)
        //        membership = this.Service<IMembershipService>().GetMembershipByCode(cardCode);
        //    return IsVoucherValidFor(voucher, OrderDetailAPIViewModels, order, membership);
        //}

        //public BaseResponse<Voucher> IsVoucherValidFor(Voucher voucher, IEnumerable<OrderDetailAPIViewModel> OrderDetailAPIViewModels, OrderAPIViewModel order, Membership membership = null)
        //{
        //    if (voucher.PromotionDetailID == null)
        //        return BaseResponse<Voucher>.Get(false, "This is an OrderAPIViewModel level voucher", null,
        //                ResultEnum.Success, "This is an OrderAPIViewModel level voucher");

        //    var available = IsVoucherAvailable(voucher);
        //    if (!available.Success)
        //        return BaseResponse<Voucher>.Get(false, available.Message, null, ResultEnum.Success, available.Error);

        //    if (voucher.MembershipId != null)
        //        if (voucher.MembershipId != membership.Id)
        //            return BaseResponse<Voucher>.Get(false, "Voucher cannot be applied to this member", null,
        //                ResultEnum.Success, "Voucher cannot be applied to this member");

        //    var promotion = GetPromotion(voucher.PromotionDetail.PromotionCode);
        //    var generalCheck = CheckGeneralPromotionRule(promotion, order);
        //    if (!generalCheck.Success)
        //        return BaseResponse<Voucher>.Get(false, generalCheck.Message, null, ResultEnum.Success, generalCheck.Error);

        //    BaseResponse<PromotionDetail> pDetailValid = null;
        //    if (promotion.ApplyLevel == (int)PromotionApplyLevelEnum.OrderAPIViewModel)
        //        pDetailValid = IsPromotionDetailValidFor(voucher.PromotionDetail, order, membership);
        //    else if (promotion.ApplyLevel == (int)PromotionApplyLevelEnum.OrderDetailAPIViewModel)
        //        pDetailValid = IsPromotionDetailValidFor(voucher.PromotionDetail, OrderDetailAPIViewModels, membership);

        //    if (!pDetailValid.Success)
        //        return BaseResponse<Voucher>.Get(false, pDetailValid.Message, null,
        //                ResultEnum.Success, pDetailValid.Error);

        //    return BaseResponse<Voucher>.Get(true, "Voucher can be applied", voucher, ResultEnum.Success, null);
        //}
        #endregion
        #endregion
    }
}
