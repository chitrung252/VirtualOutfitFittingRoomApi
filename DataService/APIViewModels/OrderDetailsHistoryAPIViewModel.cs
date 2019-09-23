using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.APIViewModels
{
    public class OrderDetailsHistoryAPIViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.OrderDetail>
    {
        [JsonProperty("order_detail_id", NullValueHandling = NullValueHandling.Ignore)]
        public int OrderDetailID { get; set; }
        [JsonProperty("rent_id", NullValueHandling = NullValueHandling.Ignore)]
        public int RentID { get; set; }
        [JsonProperty("total_amount", NullValueHandling = NullValueHandling.Ignore)]
        public double TotalAmount { get; set; }
        [JsonProperty("quantity", NullValueHandling = NullValueHandling.Ignore)]
        public int Quantity { get; set; }
        [JsonProperty("order_date", NullValueHandling = NullValueHandling.Ignore)]
        public System.DateTime OrderDate { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int Status { get; set; }
        [JsonProperty("product_type", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> ProductType { get; set; }
        [JsonProperty("final_amount", NullValueHandling = NullValueHandling.Ignore)]
        public double FinalAmount { get; set; }
        [JsonProperty("discount", NullValueHandling = NullValueHandling.Ignore)]
        public double Discount { get; set; }
        [JsonProperty("unit_price", NullValueHandling = NullValueHandling.Ignore)]
        public double UnitPrice { get; set; }
        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> ParentId { get; set; }
        [JsonProperty("product_order_type", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> ProductOrderType { get; set; }
        [JsonProperty("item_quantity", NullValueHandling = NullValueHandling.Ignore)]
        public int ItemQuantity { get; set; }
        [JsonProperty("tmp_detail_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> TmpDetailId { get; set; }
        [JsonProperty("product", NullValueHandling = NullValueHandling.Ignore)]
        public ProductAPIViewModel productVM { get; set; }
        [JsonProperty("product_details")]
        public List<ProductAPIViewModel> productDetailVM { get; set; } = new List<ProductAPIViewModel>();
        public OrderDetailsHistoryAPIViewModel() : base() { }
        public OrderDetailsHistoryAPIViewModel(DataService.Models.Entities.OrderDetail entity) : base(entity) { }
    }
}
