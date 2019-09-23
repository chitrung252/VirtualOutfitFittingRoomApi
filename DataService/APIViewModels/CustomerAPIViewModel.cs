using DataService.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.APIViewModels
{
    public class CustomerAPIViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.Customer>
    {
        [JsonProperty("customer_id")]
        public int CustomerID { get; set; } = -1;
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("address")]
        [JsonIgnore]
        public string Address { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("fax")]
        [JsonIgnore]
        public string Fax { get; set; }
        [JsonProperty("contact_person")]
        [JsonIgnore]
        public string ContactPerson { get; set; }
        [JsonProperty("contact_person_number")]
        [JsonIgnore]
        public string ContactPersonNumber { get; set; }
        [JsonProperty("website")]
        [JsonIgnore]
        public string Website { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("type")]
        [JsonIgnore]
        public int Type { get; set; }
        [JsonProperty("account_id")]
        [JsonIgnore]
        public Nullable<int> AccountID { get; set; }
        [JsonProperty("id_card")]
        [JsonIgnore]
        public string IDCard { get; set; }
        [JsonProperty("gender")]
        public Nullable<bool> Gender { get; set; }
        [JsonProperty("birth_day")]
        public Nullable<System.DateTime> BirthDay { get; set; }
        [JsonProperty("store_register_id")]
        [JsonIgnore]
        public Nullable<int> StoreRegisterId { get; set; }
        [JsonProperty("district")]
        [JsonIgnore]
        public string District { get; set; }
        [JsonProperty("city")]
        [JsonIgnore]
        public string City { get; set; }
        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }
        [JsonProperty("customer_type_id")]
        public Nullable<int> CustomerTypeId { get; set; }
        [JsonProperty("brand_id")]
        [JsonIgnore]
        public Nullable<int> BrandId { get; set; }
        [JsonProperty("delivery_info_default")]
        [JsonIgnore]
        public Nullable<int> deliveryInfoDefault { get; set; }
        [JsonProperty("pic_url")]
        public string picURL { get; set; }
        [JsonProperty("account_phone")]
        [JsonIgnore]
        public string AccountPhone { get; set; }
        [JsonProperty("facebook_id")]
        [JsonIgnore]
        public string FacebookId { get; set; }

        #region notMapping
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("is_first_login")]
        public bool IsFirstLogin { get; set; }
        [JsonProperty("is_phone_no_login")]
        public bool IsPhoneNoLogin { get; set; }
        [JsonProperty("is_fb_connected")]
        public bool IsFbConnected { get; set; }
        [JsonProperty("balance")]
        public decimal Balance { get; set; }
        [JsonProperty("point")]
        public decimal Point { get; set; }
        [JsonProperty("membership")]
        public MembershipAPIViewModel MembershipVM { get; set; }
        [JsonIgnore]
        public AspNetUser AspUserVM { get; set; }
        #endregion

        //#region Request
        //[JsonProperty("card_code")]
        //public string CardCode { get; set; }
        //#endregion

        public CustomerAPIViewModel() : base() { }
        public CustomerAPIViewModel(DataService.Models.Entities.Customer entity) : base(entity) { }
    }
}
