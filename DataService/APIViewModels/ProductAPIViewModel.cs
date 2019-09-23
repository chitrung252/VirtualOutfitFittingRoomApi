
using DataService.APIViewModels;
using DataService.Models.APIModels;
using DataService.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.APIViewModels
{
    
    public class ProductAPIViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.Product>
    {
        
        [JsonProperty("product_id")]
        public int ProductID { get; set; }
        [JsonProperty("product_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ProductName { get; set; } 

        [JsonProperty("brand_name")]
        public string BrandName { get; set; }

        [JsonProperty("product_name_eng")]
        [JsonIgnore]
        public string ProductNameEng { get; set; } 

        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public double Price { get; set; }

        [JsonProperty("pic_url")]
        [JsonIgnore]
        public string PicURL { get; set; } 
        [JsonProperty("pic_urls", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> PicURLs { get; set; } 

        [JsonProperty("cat_id", NullValueHandling = NullValueHandling.Ignore)]
        public int CatID { get; set; }

        [JsonProperty("is_available")]
        [JsonIgnore]
        public bool IsAvailable { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("discount_percent", NullValueHandling = NullValueHandling.Ignore)]
        public double DiscountPercent { get; set; }

        [JsonProperty("discount_price", NullValueHandling = NullValueHandling.Ignore)]
        public double DiscountPrice { get; set; }

        [JsonProperty("product_type")]
        [JsonIgnore]
        public int ProductType { get; set; }

        [JsonProperty("display_order")]
        [JsonIgnore]
        public int DisplayOrder { get; set; }

        [JsonProperty("has_extra", NullValueHandling = NullValueHandling.Ignore)]
        public bool HasExtra { get; set; }

        [JsonProperty("is_fixed_price")]
        [JsonIgnore]
        public bool IsFixedPrice { get; set; }

        [JsonProperty("pos_x")]
        [JsonIgnore]
        public Nullable<int> PosX { get; set; } = 0;

        [JsonProperty("pos_y")]
        [JsonIgnore]
        public Nullable<int> PosY { get; set; } = 0;

        [JsonProperty("color_group")]
        [JsonIgnore]
        public Nullable<int> ColorGroup { get; set; }

        [JsonProperty("group")]
        [JsonIgnore]
        public Nullable<int> Group { get; set; }

        [JsonProperty("group_id")]
        [JsonIgnore]
        public Nullable<int> GroupId { get; set; }

        [JsonProperty("is_menu_display")]
        [JsonIgnore]
        public Nullable<bool> IsMenuDisplay { get; set; }

        [JsonProperty("general_product_id")]
        [JsonIgnore]
        public Nullable<int> GeneralProductId { get; set; } = 0;

        [JsonProperty("att1", NullValueHandling = NullValueHandling.Ignore)]
        public string Att1 { get; set; }

        [JsonProperty("att2", NullValueHandling = NullValueHandling.Ignore)]
        public string Att2 { get; set; } = null;

        [JsonProperty("att3", NullValueHandling = NullValueHandling.Ignore)]
        public string Att3 { get; set; } 

        [JsonProperty("att4", NullValueHandling = NullValueHandling.Ignore)]
        
        public string Att4 { get; set; }

        [JsonProperty("att5", NullValueHandling = NullValueHandling.Ignore)]
        
        public string Att5 { get; set; }

        [JsonProperty("att6")]
        [JsonIgnore]
        public string Att6 { get; set; }

        [JsonProperty("att7", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public string Att7 { get; set; }

        [JsonProperty("att8", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public string Att8 { get; set; }

        [JsonProperty("att9", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public string Att9 { get; set; }

        [JsonProperty("att10", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public string Att10 { get; set; }

        [JsonProperty("max_extra")]
        [JsonIgnore]
        public Nullable<int> MaxExtra { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [JsonProperty("description_eng")]
        [JsonIgnore]
        public string DescriptionEng { get; set; } = "";

        [JsonProperty("introduction")]
        [JsonIgnore]
        public string Introduction { get; set; } = "";

        [JsonProperty("introduction_eng")]
        [JsonIgnore]
        public string IntroductionEng { get; set; } = "";

        [JsonProperty("print_group")]
        [JsonIgnore]
        public Nullable<int> PrintGroup { get; set; }

        [JsonProperty("seo_name")]
        public string SeoName { get; set; }

        [JsonProperty("is_home_page")]
        [JsonIgnore]
        public Nullable<int> IsHomePage { get; set; }

        [JsonProperty("web_content")]
        [JsonIgnore]
        public string WebContent { get; set; }

        [JsonProperty("seo_key_words")]
        [JsonIgnore]
        public string SeoKeyWords { get; set; }

        [JsonProperty("seo_description")]
        [JsonIgnore]
        public string SeoDescription { get; set; }

        [JsonProperty("active")]
        [JsonIgnore]
        public bool Active { get; set; }

        [JsonProperty("is_sefault_child_product")]
        [JsonIgnore]
        public int IsDefaultChildProduct { get; set; }

        [JsonProperty("position")]
        [JsonIgnore]
        public Nullable<int> Position { get; set; }

        [JsonProperty("sale_type")]
        [JsonIgnore]
        public Nullable<int> SaleType { get; set; } = 0;

        [JsonProperty("is_most_ordered")]
        [JsonIgnore]
        public bool IsMostOrdered { get; set; }

        [JsonProperty("note")]
        [JsonIgnore]
        public string Note { get; set; } = "";

        [JsonProperty("create_time", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> CreateTime { get; set; }

        [JsonProperty("rating_total")]
        [JsonIgnore]
        public Nullable<int> RatingTotal { get; set; }

        [JsonProperty("num_of_user_voted")]
        [JsonIgnore]
        public Nullable<int> NumOfUserVoted { get; set; }

        [JsonProperty("sale_method_enum")]
        [JsonIgnore]
        public Nullable<int> SaleMethodEnum { get; set; } = 0;

        [JsonProperty("cate_ids")]
        public string CateIds { get; set; }

        [JsonProperty("tag_ids")]
        [JsonIgnore]
        public string TagIds { get; set; }

        [JsonProperty("pro_related_ids")]
        [JsonIgnore]
        public string ProRelatedIds { get; set; }

        [JsonProperty("meta_data_ids")]
        [JsonIgnore]
        public string MetaDataIds { get; set; }

        [JsonProperty("on_sale")]
        [JsonIgnore]
        public Nullable<bool> OnSale { get; set; }

        [JsonProperty("sale_price", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<double> SalePrice { get; set; }

        [JsonProperty("stock", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> Stock { get; set; }

        [JsonProperty("review_allow")]
        [JsonIgnore]
        public Nullable<bool> ReviewAllow { get; set; }

        [JsonProperty("average_rating", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public string AverageRating { get; set; }

        [JsonProperty("is_allow_freeShip", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public Nullable<bool> IsAllowFreeShip { get; set; }
        [JsonProperty("product_brand_id",NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public Nullable<int> ProductBrandId { get; set; }
        [JsonProperty("pro_video_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ProVideoUrl { get; set; }
        [JsonProperty("short_decription", NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDecription { get; set; }
        [JsonProperty("short_decription_eng", NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDecriptionEng { get; set; }
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }
        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public string Size { get; set; }
        [JsonProperty("volume", NullValueHandling = NullValueHandling.Ignore)]
        public string Volume { get; set; }
        [JsonProperty("weight", NullValueHandling = NullValueHandling.Ignore)]
        public string Weight { get; set; }
        [JsonProperty("available_date", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> AvailableDate { get; set; }
        [JsonProperty("price_cost", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<double> PriceCost { get; set; }
        #region Additional Property

        [JsonProperty("product_variations", NullValueHandling = NullValueHandling.Ignore)]
        public List<ProductAPIViewModel> ProductVariations  { get; set; }

        //[JsonProperty("product_extra")]
        //public List<ProductAPIViewModel> ProductExtra { get; set; }

        #endregion

        public ProductAPIViewModel() : base() { }
        public ProductAPIViewModel(DataService.Models.Entities.Product entity) : base(entity) { }

    }
}
