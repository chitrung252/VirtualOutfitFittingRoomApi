using DataService.APIViewModels;
using DataService.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.APIViewModels
{
    public class ProductCategoryAPIViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.ProductCategory>
    {

        [JsonProperty("cate_id")]
        public  int CateID { get; set; }
        [JsonProperty("cate_name")]
        public string CateName { get; set; } = "";
        [JsonProperty("cate_name_eng")]
        public string CateNameEng { get; set; } = "";
        [JsonProperty("type")]
        public int Type { get; set; } 
        [JsonProperty("is_displayed")]
        [JsonIgnore]
        public  bool IsDisplayed { get; set; }
        [JsonProperty("is_display_website")]
        [JsonIgnore]
        public  bool IsDisplayedWebsite { get; set; }
        [JsonProperty("is_extra")]
        [JsonIgnore]
        public  bool IsExtra { get; set; }
        [JsonProperty("display_order")]
        public  int DisplayOrder { get; set; }
        [JsonProperty("adjustment_note")]
        [JsonIgnore]
        public  string AdjustmentNote { get; set; }
        [JsonProperty("store_id")]
        [JsonIgnore]
        public  Nullable<int> StoreId { get; set; }
        [JsonProperty("seo_name")]
        [JsonIgnore]
        public  string SeoName { get; set; }
        [JsonProperty("seo_keyword")]
        [JsonIgnore]
        public  string SeoKeyword { get; set; }
        [JsonProperty("seo_description")]
        [JsonIgnore]
        public  string SeoDescription { get; set; }
        [JsonProperty("image_font_awsome_css")]
        [JsonIgnore]
        public  string ImageFontAwsomeCss { get; set; }
        [JsonProperty("parent_cate_id")]
        public Nullable<int> ParentCateId { get; set; } = 0;
        [JsonProperty("position")]
        public Nullable<int> Position { get; set; } = 0;
        [JsonProperty("active")]
        [JsonIgnore]
        public  bool Active { get; set; }
        [JsonProperty("brand_id")]
        [JsonIgnore]
        public  Nullable<int> BrandId { get; set; }
        [JsonProperty("pic_url")]
        public string PicUrl { get; set; } = "";
        [JsonProperty("banner_url")]
        [JsonIgnore]
        public  string BannerUrl { get; set; }
        [JsonProperty("description")]

        public string Description { get; set; } = "";
        [JsonProperty("description_eng")]

        public string DescriptionEng { get; set; } = "";
        [JsonProperty("banner_description")]
        [JsonIgnore]
        public  string BannerDescription { get; set; }
        [JsonProperty("banner_description_eng")]
        [JsonIgnore]
        public  string BannerDescriptionEng { get; set; }
        [JsonProperty("att1")]

        public string Att1 { get; set; } = "";
        [JsonProperty("att2")]

        public string Att2 { get; set; } = "";
        [JsonProperty("att3")]

        public string Att3 { get; set; } = "";
        [JsonProperty("att4")]
        [JsonIgnore]
        public  string Att4 { get; set; }
        [JsonProperty("att5")]
        [JsonIgnore]
        public  string Att5 { get; set; }
        [JsonProperty("att6")]
        [JsonIgnore]
        public  string Att6 { get; set; }
        [JsonProperty("att7")]
        [JsonIgnore]
        public  string Att7 { get; set; }
        [JsonProperty("att8")]
        [JsonIgnore]
        public  string Att8 { get; set; }
        [JsonProperty("att9")]
        [JsonIgnore]
        public  string Att9 { get; set; }
        [JsonProperty("att10")]
        [JsonIgnore]
        public  string Att10 { get; set; }
        [JsonProperty("vat")]
        [JsonIgnore]
        public  Nullable<double> VAT { get; set; }
        [JsonProperty("products")]
        public List<ProductAPIViewModel> productsVM { get; set; }
        
        public ProductCategoryAPIViewModel() : base() { }
        public ProductCategoryAPIViewModel(DataService.Models.Entities.ProductCategory entity) : base(entity) { }
    }

}
