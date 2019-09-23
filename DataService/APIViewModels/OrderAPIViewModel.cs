using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.APIViewModels
{
    public class OrderAPIViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.Order>
    {
        [JsonProperty("rent_id", NullValueHandling  = NullValueHandling.Ignore)]
        public int RentID { get; set; } 
        [JsonProperty("invoice_id")]
        public string InvoiceID { get; set; }
        [JsonProperty("check_in_date", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> CheckInDate { get; set; }
        [JsonProperty("check_out_date", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> CheckOutDate { get; set; }
        [JsonProperty("approve_date", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> ApproveDate { get; set; }
        [JsonProperty("total_amount", NullValueHandling = NullValueHandling.Ignore)]
        public double TotalAmount { get; set; }
        [JsonProperty("point", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Point { get; set; }
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
        [JsonProperty("rent_status", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> RentStatus { get; set; }
        [JsonProperty("order_type")]
        public int OrderType { get; set; }
        [JsonProperty("rent_type")]
        public int RentType { get; set; }
        [JsonProperty("notes", NullValueHandling = NullValueHandling.Ignore)]
        public string Notes { get; set; }
        [JsonProperty("fee_description", NullValueHandling = NullValueHandling.Ignore)]
        public string FeeDescription { get; set; }
        [JsonProperty("check_in_person", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckInPerson { get; set; }
        [JsonProperty("check_out_person", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckOutPerson { get; set; }
        [JsonProperty("approve_person", NullValueHandling = NullValueHandling.Ignore)]
        public string ApprovePerson { get; set; }
        [JsonProperty("price_group_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> PriceGroupID { get; set; }
        [JsonProperty("booking_date", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> BookingDate { get; set; }
        [JsonProperty("arrival_date", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> ArrivalDate { get; set; }
        [JsonProperty("departure_date", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> DepartureDate { get; set; }
        [JsonProperty("customer_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> CustomerID { get; set; }
        [JsonProperty("sub_rent_group_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> SubRentGroupID { get; set; }
        [JsonProperty("room_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> RoomId { get; set; }
        [JsonProperty("is_fixed_price", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsFixedPrice { get; set; }
        [JsonProperty("last_record_date", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> LastRecordDate { get; set; }
        [JsonProperty("served_person", NullValueHandling = NullValueHandling.Ignore)]
        public string ServedPerson { get; set; }
        [JsonProperty("store_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> StoreID { get; set; }
        [JsonProperty("source_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> SourceID { get; set; }
        [JsonProperty("source_type", NullValueHandling = NullValueHandling.Ignore)]
        public int SourceType { get; set; }
        [JsonProperty("delivery_address", NullValueHandling = NullValueHandling.Ignore)]
        public string DeliveryAddress { get; set; }
        [JsonProperty("delivery_status", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> DeliveryStatus { get; set; }
        [JsonProperty("order_detail_total_quantity", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> OrderDetailsTotalQuantity { get; set; }
        [JsonProperty("check_in_hour", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> CheckinHour { get; set; }
        [JsonProperty("total_invoice_print", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> TotalInvoicePrint { get; set; }
        [JsonProperty("vat", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<double> VAT { get; set; }
        [JsonProperty("vat_amount", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<double> VATAmount { get; set; }
        [JsonProperty("number_of_guest", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> NumberOfGuest { get; set; }
        [JsonProperty("att1", NullValueHandling = NullValueHandling.Ignore)]
        public string Att1 { get; set; }
        [JsonProperty("att2", NullValueHandling = NullValueHandling.Ignore)]
        public string Att2 { get; set; }
        [JsonProperty("att3", NullValueHandling = NullValueHandling.Ignore)]
        public string Att3 { get; set; }
        [JsonProperty("att4", NullValueHandling = NullValueHandling.Ignore)]
        public string Att4 { get; set; }
        [JsonProperty("att5", NullValueHandling = NullValueHandling.Ignore)]
        public string Att5 { get; set; }
        [JsonProperty("group_payment_status", NullValueHandling = NullValueHandling.Ignore)]
        public int GroupPaymentStatus { get; set; }
        [JsonProperty("delivery_receiver", NullValueHandling = NullValueHandling.Ignore)]
        public string DeliveryReceiver { get; set; }
        [JsonProperty("delivery_phone", NullValueHandling = NullValueHandling.Ignore)]
        public string DeliveryPhone { get; set; }
        [JsonProperty("last_modified_payment", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> LastModifiedPayment { get; set; }
        [JsonProperty("last_modified_order_detail", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> LastModifiedOrderDetail { get; set; }
        [JsonProperty("payment_status", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> PaymentStatus { get; set; }
        [JsonProperty("delivery_type", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> DeliveryType { get; set; }
        [JsonProperty("delivery_payment", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> DeliveryPayment { get; set; }
        [JsonProperty("invoice_status", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> InvoiceStatus { get; set; }
        [JsonProperty("ward_code", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> WardCode { get; set; }
        [JsonProperty("district_code", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> DistrictCode { get; set; }
        [JsonProperty("province_code", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> ProvinceCode { get; set; }
        [JsonProperty("promotion_partner_id", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<int> PromotionPartnerId { get; set; }
        [JsonProperty("member_point", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<double> MemberPoint { get; set; }
        [JsonProperty("receiver", NullValueHandling = NullValueHandling.Ignore)]
        public string Receiver { get; set; }
        [JsonProperty("payments", NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<PaymentAPIViewModel> Payments { get; set; }
        [JsonProperty("order_details", NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<OrderDetailAPIViewModel> OrderDetails { get; set; }
        
        [JsonProperty("order_promotion_mappings", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public ICollection<OrderPromotionMappingAPIViewModel> OrderPromotionMappings { get; set; }
        [JsonProperty("order_code", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderCode { get; set; }
        [JsonProperty("delivery_customer", NullValueHandling = NullValueHandling.Ignore)]
        public string DeliveryCustomer { get; set; }
        [JsonProperty("is_sync", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public bool IsSync { get; set; }
        [JsonProperty("brand_id", NullValueHandling = NullValueHandling.Ignore)]
        public int BrandId { get; set; }
        [JsonProperty("voucher_code", NullValueHandling = NullValueHandling.Ignore)]
        public string VoucherCode { get; set; }
        [JsonProperty("payment_type", NullValueHandling = NullValueHandling.Ignore)]
        public int PaymentType { get; set; }
        [JsonProperty("delivery_info_id", NullValueHandling = NullValueHandling.Ignore)]
        public int DeliveryInfoId { get; set; }
        public OrderAPIViewModel() : base() { }
        public OrderAPIViewModel(DataService.Models.Entities.Order entity) : base(entity) { }
    }
}
