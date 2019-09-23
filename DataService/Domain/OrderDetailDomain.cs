using DataService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Domain
{
    public class OrderDetailDomain
    {
        // Nếu có update model, update mapping API model trong đây!!!
        #region Mapper & Updater
        /// <summary>
        /// Check lại parentId của extra orderdetail,
        /// Check lại tmpId của mapping,
        /// Check lại orderId của mapping,
        /// Check lại orderdetailId của mapping,
        /// </summary>
        public void UpdateOrderDetailId(DataService.Models.Entities.Order order)
        {
            foreach (var orderDetail in order.OrderDetails)
            {
                //Lưu parentId cho orderdetail extra
                if (orderDetail.ParentId != null && orderDetail.ParentId > -1)
                {
                    var parentOrderDetail = order.OrderDetails.FirstOrDefault(od => od.TmpDetailId == orderDetail.ParentId);
                    if (parentOrderDetail != null)
                    {
                        orderDetail.ParentId = parentOrderDetail.OrderDetailID;
                    }
                }
                //Lưu lại orderPromotionMappingId cho orderdetail
                if (orderDetail.OrderPromotionMappingId != null)
                {
                    var mapping = order.OrderPromotionMappings.FirstOrDefault(m => m.TmpMappingId == orderDetail.OrderPromotionMappingId);
                    if (mapping != null)
                    {
                        orderDetail.OrderPromotionMappingId = mapping.Id;
                    }
                    else
                    {
                        orderDetail.OrderPromotionMappingId = null;
                    }
                }
                //Lưu lại orderdetailPromotionMappingId cho orderdetail
                if (orderDetail.OrderDetailPromotionMappingId != null)
                {
                    OrderDetailPromotionMapping mapping = null;
                    foreach (var od in order.OrderDetails)
                    {
                        mapping = od.OrderDetailPromotionMappings.FirstOrDefault(m => m.TmpMappingId == orderDetail.OrderDetailPromotionMappingId);
                        if (mapping != null) break;
                    }
                    if (mapping != null)
                    {
                        orderDetail.OrderDetailPromotionMappingId = mapping.Id;
                    }
                    else
                    {
                        orderDetail.OrderDetailPromotionMappingId = null;
                    }
                }
            }

            //Lưu lại tmpMappingId cho order
            foreach (var mapping in order.OrderPromotionMappings)
            {
                mapping.TmpMappingId = mapping.Id;
            }
            //Lưu lại tmpMappingId cho orderdetail
            foreach (var orderDetail in order.OrderDetails)
            {
                foreach (var mapping in orderDetail.OrderDetailPromotionMappings)
                {
                    mapping.TmpMappingId = mapping.Id;
                }
                orderDetail.TmpDetailId = orderDetail.OrderDetailID;
            }
        }
        #endregion
    }
}
