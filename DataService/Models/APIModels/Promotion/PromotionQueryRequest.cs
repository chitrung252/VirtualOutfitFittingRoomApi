using DataService.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models.APIModels.Promotion
{
    public class PromotionQueryRequest<T> : BaseRequest<T>
    {
        [JsonProperty("promotion_id")]
        public int? PromotionID { get; set; }
        [JsonProperty("promotion_code")]
        public string PromotionCode { get; set; }
        public Membership Membership { get; set; }
    }
}
