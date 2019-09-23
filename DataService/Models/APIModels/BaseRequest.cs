using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Web.Mvc;
using DataService.Utilities;
using System.Runtime.Serialization;

namespace DataService.Models.APIModels
{
    
    public class BaseRequest<T>
    {
        [DataMember(Name = "page")]
        [JsonProperty("page")]
        public Nullable<int> Page { get; set; } = 1;
        [DataMember(Name = "store_id")]
        [JsonProperty("store_id")]
        public Nullable<int> StoreId { get; set; }
        [DataMember(Name = "partner_id")]
        [JsonProperty("partner_id")]
        public string PartnerId { get ; set; }
        [DataMember(Name = "brand_id")]
        [JsonProperty("brand_id")]
        public Nullable<int> BrandId { get; set; } = -1;
        [DataMember(Name = "ids")]
        [JsonProperty("ids")]
        public string Ids { get; set; }
        [DataMember(Name = "created_at_min")]
        [JsonProperty("created_at_min")]
        public DateTime? CreateAtMin { get; set; }
        [DataMember(Name = "created_at_max")]
        [JsonProperty("created_at_max")]
        public DateTime? CreateAtMax { get; set; }
        [DataMember(Name = "since_id")]
        [JsonProperty("since_id")]
        public Nullable<int> SinceId { get; set; }
        [DataMember(Name = "limit")]
        [JsonProperty("limit")]
        public Nullable<int> Limit { get; set; } = 50;
        [DataMember(Name = "updated_at_min")]
        [JsonProperty("updated_at_min")]
        public DateTime? UpdateAtMin { get; set; }
        [DataMember(Name = "updated_at_max")]
        [JsonProperty("updated_at_max")]
        public DateTime? UpdateAtMax { get; set; }
        [DataMember(Name = "data")]
        [JsonProperty("data")]
        public T Data { get; set; }
        
    }
}