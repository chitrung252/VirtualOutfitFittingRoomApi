using DataService.Models.Entities;
using DataService.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace DataService.Models.APIModels.Promotion
{
    [ModelBinder(typeof(SkyModelBinder))]
    public class VoucherQueryRequest<T> : BaseRequest<T>
    {
        [DataMember(Name =("voucher_id"))]
        [JsonProperty("voucher_id")]
        public int? VoucherId { get; set; }
        [DataMember(Name = ("voucher_code"))]
        [JsonProperty("voucher_code")]
        public string VoucherCode { get; set; }
        [DataMember(Name = ("promotion_id"))]
        [JsonProperty("promotion_id")]
        public int? PromotionId { get; set; }
        [DataMember(Name = ("promotion_code"))]
        [JsonProperty("promotion_code")]
        public string PromotionCode { get; set; }
        [DataMember(Name = ("available_only"))]
        [JsonProperty("available_only")]
        public bool AvailableOnly { get; set; }
        [DataMember(Name = ("used_only"))]
        [JsonProperty("used_only")]
        public bool UsedOnly { get; set; }
        public Membership MembershipVM { get; set; }
    }
}
