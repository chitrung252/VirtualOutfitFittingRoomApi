using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.APIViewModels
{
    public class MembershipTypeAPIViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.MembershipType>
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("type_name")]
        public string TypeName { get; set; }
        [JsonProperty("append_code")]
        [JsonIgnore]
        public string AppendCode { get; set; }
        [JsonProperty("brand_id")]
        [JsonIgnore]
        public int BrandId { get; set; }
        [JsonProperty("active")]
        [JsonIgnore]
        public Nullable<bool> Active { get; set; }
        [JsonProperty("is_mobile")]
        [JsonIgnore]
        public Nullable<bool> IsMobile { get; set; }
        [JsonProperty("type_level")]
        [JsonIgnore]
        public Nullable<int> TypeLevel { get; set; }
        [JsonProperty("type_point")]
        [JsonIgnore]
        public Nullable<int> TypePoint { get; set; }

        public MembershipTypeAPIViewModel() : base() { }
        public MembershipTypeAPIViewModel(DataService.Models.Entities.MembershipType entity) : base(entity) { }
    }
}
