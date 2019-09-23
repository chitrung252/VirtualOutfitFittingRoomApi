using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.APIViewModels
{
    public class OrderHistoryAPIViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.Order>
    {
        [JsonProperty("rent_id", NullValueHandling = NullValueHandling.Ignore)]
        public int RentID { get; set; }
        [JsonProperty("invoice_id")]
        public string InvoiceID { get; set; }
        [JsonProperty("check_in_date", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> CheckInDate { get; set; }
        [JsonProperty("check_out_date", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> CheckOutDate { get; set; }
        [JsonProperty("total_amount", NullValueHandling = NullValueHandling.Ignore)]
        public double TotalAmount { get; set; }
        [JsonProperty("delivery_fee", NullValueHandling = NullValueHandling.Ignore)]
        public double DeliveryFee { get; set; }
        [JsonProperty("point", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<double> MemberPoint { get; set; }
        [JsonProperty("discount", NullValueHandling = NullValueHandling.Ignore)]
        public double Discount { get; set; }
        [JsonProperty("discount_rate", NullValueHandling = NullValueHandling.Ignore)]
        public double DiscountRate { get; set; }
        [JsonProperty("discount_order_detail", NullValueHandling = NullValueHandling.Ignore)]
        public double DiscountOrderDetail { get; set; }
        [JsonProperty("final_amount", NullValueHandling = NullValueHandling.Ignore)]
        public double FinalAmount { get; set; }
        [JsonProperty("order_status", NullValueHandling = NullValueHandling.Ignore)]
        public int OrderStatus { get; set; }
        [JsonProperty("order_type_name", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderTypeName { get; set; }
        [JsonProperty("notes", NullValueHandling = NullValueHandling.Ignore)]
        public string Notes { get; set; }
        [JsonProperty("delivery_address", NullValueHandling = NullValueHandling.Ignore)]
        public string DeliveryAddress { get; set; }
        [JsonProperty("delivery_status", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> DeliveryStatus { get; set; }
        [JsonProperty("delivery_receiver", NullValueHandling = NullValueHandling.Ignore)]
        public string DeliveryReceiver { get; set; }
        [JsonProperty("delivery_phone", NullValueHandling = NullValueHandling.Ignore)]
        public string DeliveryPhone { get; set; }
        [JsonProperty("receiver", NullValueHandling = NullValueHandling.Ignore)]
        public string Receiver { get; set; }
        [JsonProperty("payment_type", NullValueHandling = NullValueHandling.Ignore)]
        public int PaymentType { get; set; }
        [JsonProperty("store", NullValueHandling = NullValueHandling.Ignore)]
        public StoreAPIViewModel StoreVM { get; set; }
        [JsonProperty("order_details", NullValueHandling = NullValueHandling.Ignore)]
        public List<OrderDetailsHistoryAPIViewModel> OrderDetailVM { get; set; }

        public OrderHistoryAPIViewModel() : base() { }
        public OrderHistoryAPIViewModel(DataService.Models.Entities.Order entity) : base(entity) { }
    }
}
