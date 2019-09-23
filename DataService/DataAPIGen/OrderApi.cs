using DataService.Models.Entities;
using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.DataAPIGen
{
    public partial class OrderApi
    {
        public OrderViewModel GetOrderByInvoiceId(string invoiceId)
        {
            var order = this.BaseService.Get(q => q.InvoiceID == invoiceId).FirstOrDefault();
            return order;
        }

        public OrderViewModel GetOrderByRentID(int rentId)
        {
            var order = this.BaseService.Get(q => q.RentID == rentId).FirstOrDefault();
            return order;
        }

        public void SaveOrder(OrderViewModel orderViewModel)
        {
            var order = new Order();
            order.CheckInDate = orderViewModel.CheckInDate;
            order.CheckOutDate = orderViewModel.CheckOutDate;
            order.TotalAmount = orderViewModel.TotalAmount;
            order.Discount = orderViewModel.Discount;
            order.DiscountOrderDetail = orderViewModel.DiscountOrderDetail;
            order.FinalAmount = orderViewModel.FinalAmount;
            order.OrderStatus = orderViewModel.OrderStatus;
            order.OrderType = orderViewModel.RentType;
            order.IsFixedPrice = orderViewModel.IsFixedPrice;
            order.GroupPaymentStatus = orderViewModel.GroupPaymentStatus;
            order.OrderDetailsTotalQuantity = orderViewModel.OrderDetailsTotalQuantity;
            this.BaseService.SaveOrder(order);
        }
    }
}
