using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.APIViewModels
{
    public class MembershipAPIViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.Membership>
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("customer_id")]
        public Nullable<int> CustomerId { get; set; }
        [JsonProperty("csv")]
        [JsonIgnore]
        public string CSV { get; set; }
        [JsonProperty("active")]
        [JsonIgnore]
        public bool Active { get; set; }
        [JsonProperty("status")]
        [JsonIgnore]
        public Nullable<int> Status { get; set; }
        [JsonProperty("c_level")]
        [JsonIgnore]
        public Nullable<int> C_Level { get; set; }
        [JsonProperty("membership_type_id")]
        public Nullable<int> MembershipTypeId { get; set; }
        [JsonProperty("is_sample")]
        [JsonIgnore]
        public Nullable<bool> IsSample { get; set; }
        [JsonProperty("store_id")]
        [JsonIgnore]
        public Nullable<int> StoreId { get; set; }
        [JsonProperty("product_code")]
        [JsonIgnore]
        public string ProductCode { get; set; }
        [JsonProperty("initial_value")]
        [JsonIgnore]
        public Nullable<double> InitialValue { get; set; }
        [JsonProperty("membership_code")]
        public string MembershipCode { get; set; }
        [JsonProperty("create_time")]
        [JsonIgnore]
        public Nullable<System.DateTime> CreateTime { get; set; }
        [JsonProperty("create_by")]
        [JsonIgnore]
        public string CreateBy { get; set; }
        [JsonProperty("customer")]
        [JsonIgnore]
        public CustomerAPIViewModel CustomerVM { get; set; }
        [JsonProperty("membership_type")]
        public  MembershipTypeAPIViewModel MembershipTypeVM { get; set; }
        [JsonProperty("cards")]
        public List<CardAPIViewModel> CardVM { get; set; }
        [JsonProperty("accounts")]
        public List<AccountAPIViewModel> AccountVMs { get; set; }

        public MembershipAPIViewModel() : base() { }
        public MembershipAPIViewModel(DataService.Models.Entities.Membership entity) : base(entity) { }
    }
}
