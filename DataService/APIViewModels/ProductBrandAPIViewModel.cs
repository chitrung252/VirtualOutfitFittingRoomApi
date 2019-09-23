using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.APIViewModels
{
   public class ProductBrandAPIViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.ProductBrand>
    {
        [JsonProperty("product_brand_id", NullValueHandling = NullValueHandling.Ignore)]
        public  int ProductBrandId { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public  string Name { get; set; }
        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public  string Slug { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public  string Description { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public  Nullable<int> Count { get; set; }

        public ProductBrandAPIViewModel() : base() { }
        public ProductBrandAPIViewModel(DataService.Models.Entities.ProductBrand entity) : base(entity) { }

    }
}
