using DataService.Models.Entities;
using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.DataAPIGen
{
    public partial class OrderDetailApi
    {
        public List<OrderDetailViewModel>GetOrderDetailByRentId(int rentId)
        {
            var result = this.BaseService.Get(q => q.RentID == rentId).ToList();
            return result;
        }
        public void SaveOrderDetail(OrderDetailViewModel orderDetailViewModel)
        {
            var orderDetail = new OrderDetail();
            orderDetail.OrderDate = orderDetailViewModel.OrderDate;
            orderDetail.ParentId = orderDetailViewModel.ParentId ;
            orderDetail.Status = orderDetailViewModel.Status;
            orderDetail.IsAddition = orderDetailViewModel.IsAddition;
            orderDetail.UnitPrice = orderDetailViewModel.UnitPrice;
            orderDetail.StoreId = orderDetailViewModel.StoreId;
            orderDetail.RentID = orderDetailViewModel.RentID;
            orderDetail.ProductID = orderDetailViewModel.ProductID;
            orderDetail.Quantity = orderDetailViewModel.Quantity;
            orderDetail.TotalAmount = orderDetailViewModel.TotalAmount;
            orderDetail.Discount = orderDetailViewModel.Discount;
            orderDetail.FinalAmount = orderDetailViewModel.FinalAmount;
            this.BaseService.SaveOrderDetail(orderDetail);
        }
    }
}
