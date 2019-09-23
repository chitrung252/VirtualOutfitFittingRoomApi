using DataService.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace DataService.APIViewModels
{
    public class RatingAPIViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.Rating>
    {
        //[DataMember(Name = "id")]
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }
        //[DataMember(Name = "user_id")]
        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public string UserId { get; set; }
        //[DataMember(Name = "product_id" )]
        [JsonProperty("product_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> ProductId { get; set; }
        //[DataMember(Name = "create_time")]
        [JsonProperty("create_time", NullValueHandling = NullValueHandling.Ignore)]
        public System.DateTime CreateTime { get; set; }
        //[DataMember(Name = "star")]
        [JsonProperty("star", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public Nullable<int> Star { get; set; }
        //[DataMember(Name = "review_content")]
        [JsonProperty("review_content", NullValueHandling = NullValueHandling.Ignore)]
        public string ReviewContent { get; set; }
        //[DataMember(Name = "active")]
        [JsonProperty("active", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public bool Active { get; set; }
        //[DataMember(Name = "review_email")]
        [JsonProperty("review_email", NullValueHandling = NullValueHandling.Ignore)]
        public string ReviewEmail { get; set; }
        //[DataMember(Name = "review_name")]
        [JsonProperty("review_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ReviewName { get; set; }
        //[DataMember(Name = "verified")]
        [JsonProperty("verified", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public Nullable<bool> Verified { get; set; }
        //[DataMember(Name = "quanlity")]
        [JsonProperty("quanlity", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> Rate1 { get; set; }
        //[DataMember(Name = "space_store")]
        [JsonProperty("space_store", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> Rate2 { get; set; }
        //[DataMember(Name = "quanlity_service")]
        [JsonProperty("quanlity_service", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> Rate3 { get; set; }
        //[DataMember(Name = "rate_4")]
        [JsonProperty("rate_4", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public Nullable<int> Rate4 { get; set; }
        //[DataMember(Name = "rate_5")]
        [JsonProperty("rate_5", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public Nullable<int> Rate5 { get; set; }
        [JsonProperty("customer_id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public Nullable<int> CustomerId { get; set; }
        [JsonProperty("order_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> OrderId { get; set; }


        public RatingAPIViewModel() : base() { }
        public RatingAPIViewModel(DataService.Models.Entities.Rating entity) : base(entity) { }
    }
}
