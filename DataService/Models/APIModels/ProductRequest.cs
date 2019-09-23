using DataService.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace DataService.Models.APIModels
{
    [ModelBinder(typeof(SkyModelBinder))]
    public class ProductRequest<T> : BaseRequest<T>
    {
        [DataMember(Name = "sale_enum")]
        [JsonProperty("sale_enum")]
        public Nullable<int> SaleEnum { get; set; } = 0;
        [DataMember(Name = "cate_ids")]
        [JsonProperty("cate_ids")]
        public string CateIds { get; set; }
        [DataMember(Name = "col_ids")]
        [JsonProperty("col_ids")]
        public string ColIds { get; set; }
        [DataMember(Name = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }
        [DataMember(Name = "product_brand_id")]
        [JsonProperty("product_brand_id")]
        public Nullable<int> ProductBrandId { get; set; }

    }
}
