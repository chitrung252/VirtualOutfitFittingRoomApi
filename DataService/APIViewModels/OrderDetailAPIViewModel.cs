using DataService.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.APIViewModels
{
    public class OrderDetailAPIViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.OrderDetail>
    {
        [JsonProperty("order_detail_id", NullValueHandling = NullValueHandling.Ignore)]
        public int OrderDetailID { get; set; }
        [JsonProperty("rent_id", NullValueHandling = NullValueHandling.Ignore)]
        public int RentID { get; set; }
        [JsonProperty("product_id", NullValueHandling = NullValueHandling.Ignore)]
        public int ProductID { get; set; }
        [JsonProperty("product_code", NullValueHandling = NullValueHandling.Ignore)]
        public string ProductCode { get; set; }
        [JsonProperty("product_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ProductName { get; set; }
        [JsonProperty("total_amount", NullValueHandling = NullValueHandling.Ignore)]
        public double TotalAmount { get; set; }
        [JsonProperty("quantity", NullValueHandling = NullValueHandling.Ignore)]
        public int Quantity { get; set; }
        [JsonProperty("order_date", NullValueHandling = NullValueHandling.Ignore)]
        public System.DateTime OrderDate { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int Status { get; set; }
        [JsonProperty("final_amount", NullValueHandling = NullValueHandling.Ignore)]
        public double FinalAmount { get; set; }
        [JsonProperty("is_addition", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsAddition { get; set; }
        [JsonProperty("detail_description", NullValueHandling = NullValueHandling.Ignore)]
        public string DetailDescription { get; set; }
        [JsonProperty("discount", NullValueHandling = NullValueHandling.Ignore)]
        public double Discount { get; set; }
        [JsonProperty("tax_percent", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<double> TaxPercent { get; set; }
        [JsonProperty("tax_value", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<double> TaxValue { get; set; }
        [JsonProperty("unit_price", NullValueHandling = NullValueHandling.Ignore)]
        public double UnitPrice { get; set; }
        [JsonProperty("product_type", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> ProductType { get; set; }
        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> ParentId { get; set; }
        [JsonProperty("store_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> StoreId { get; set; }
        [JsonProperty("product_order_type", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> ProductOrderType { get; set; }
        [JsonProperty("item_quantity", NullValueHandling = NullValueHandling.Ignore)]
        public int ItemQuantity { get; set; }
        [JsonProperty("tmp_detail_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> TmpDetailId { get; set; }
        [JsonProperty("order_detail_promotion_mapping_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> OrderDetailPromotionMappingId { get; set; }
        [JsonProperty("order_promotion_mapping_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> OrderPromotionMappingId { get; set; }
        [JsonProperty("order_detail_att1", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderDetailAtt1 { get; set; }
        [JsonProperty("order_detail_att2", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderDetailAtt2 { get; set; }
        [JsonProperty("order_detail_promotion_mappings", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public List<OrderDetailPromotionMappingAPIViewModel> OrderDetailPromotionMappings { get; set; }

        public OrderDetailAPIViewModel() : base() { }
        public OrderDetailAPIViewModel(DataService.Models.Entities.OrderDetail entity) : base(entity) { }
    }
}
