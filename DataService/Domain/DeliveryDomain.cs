using DataService.APIViewModels;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Models.Entities.Services;
using DataService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Domain
{

    public interface IDeliveryDomain
    {
        DeliveryAPIViewModel GetDeliveryById(int deliId);

        List<DeliveryAPIViewModel> GetDeliveries();
        BaseResponse<DeliveryAPIViewModel> CreateDelivery(DeliveryAPIViewModel deli);

        List<DeliveryAPIViewModel> GetDeliveriesByCustomerId(int customerId);

        BaseResponse<DeliveryAPIViewModel> UpdateDelivery(DeliveryAPIViewModel deli);
        BaseResponse<string> DeleteDelivery(int deliveryId, int customerId);

    }
    public class DeliveryDomain : BaseDomain, IDeliveryDomain
    {
        public List<DeliveryAPIViewModel> GetDeliveries()
        {
            var deliService = this.Service<IDeliveryInfoService>();
            return deliService.GetDeliveries();
        }

        public DeliveryAPIViewModel GetDeliveryById(int deliId)
        {
            var deliService = this.Service<IDeliveryInfoService>();
            return deliService.GetDeliveryById(deliId);
        }

        public BaseResponse<DeliveryAPIViewModel> CreateDelivery(DeliveryAPIViewModel deli)
        {
            var deliService = this.Service<IDeliveryInfoService>();
            if (deli.CustomerId == 0)
            {
                throw ApiException.Get(false, ConstantManager.MES_CREATE_DELIVERY_FAIL, ResultEnum.CustomerIdInTokenWrong, HttpStatusCode.NotFound);
            }
            var deliveryInfoOld = deliService.GetDeliveriesByCustomerId(deli.CustomerId.Value);
            if(deliveryInfoOld.Count == (int)ConstantManager.MAX_DELIVERYINFO)
            {
                throw ApiException.Get(false, ConstantManager.MES_DELIVERY_MAX, ResultEnum.DeliveryMax, HttpStatusCode.BadRequest);
            }
            if (deliveryInfoOld.Count() == 0)
            {
                deli.isDefaultDeliveryInfo = true;
            }
            if (deli.isDefaultDeliveryInfo == true)
            {
                foreach (var item in deliveryInfoOld)
                {
                    if (item.isDefaultDeliveryInfo == true)
                    {
                        item.isDefaultDeliveryInfo = false;
                        UpdateDelivery(item);
                    }
                }
            }
            var deliVM = deliService.CreateDelivery(deli);
            if(deliVM != null)
            {
                return BaseResponse<DeliveryAPIViewModel>.Get(true, ConstantManager.MES_CREATE_DELIVERY_SUCCESS, deliVM, (int)ResultEnum.Success);
            }
            else
            {
                throw ApiException.Get(false, ConstantManager.MES_CREATE_DELIVERY_FAIL, ResultEnum.CreateFail, HttpStatusCode.NotFound);
            }
          
            
        }

        public List<DeliveryAPIViewModel> GetDeliveriesByCustomerId(int customerId)
        {
            var deliService = this.Service<IDeliveryInfoService>();
            return deliService.GetDeliveriesByCustomerId(customerId);
        }

        public BaseResponse<DeliveryAPIViewModel> UpdateDelivery(DeliveryAPIViewModel deliVM)
        {
            var deliService = this.Service<IDeliveryInfoService>();
            if (deliVM.CustomerId == 0)
            {
                throw ApiException.Get(false, ConstantManager.MES_DELETE_DELIVERY_FAIL, ResultEnum.CustomerIdInTokenWrong, HttpStatusCode.NotFound);
            }
            var deliveryInfoOld = deliService.GetDeliveriesByCustomerId(deliVM.CustomerId.Value);
            var deli = deliveryInfoOld.Where(p => p.Id == deliVM.Id).FirstOrDefault();
            if (deli == null)
            {
                throw ApiException.Get(false, ConstantManager.MES_DELIVERYID_WRONG, ResultEnum.UpdateFail, HttpStatusCode.NotFound);
            }
            else
            {
                if (deliVM.isDefaultDeliveryInfo == true)
                {
                    foreach (var item in deliveryInfoOld)
                    {
                        if (item.isDefaultDeliveryInfo == true)
                        {
                            item.isDefaultDeliveryInfo = false;
                            deliService.UpdateDelivery(item);
                        }
                    }
                }

                deliVM = deliService.UpdateDelivery(deliVM);
                if (deliVM == null)
                {
                    throw ApiException.Get(false, ConstantManager.MES_UPDATE_FAIL, ResultEnum.UpdateFail, HttpStatusCode.NotFound);
                }
                else
                {
                   return BaseResponse<DeliveryAPIViewModel>.Get(true, ConstantManager.MES_UPDATE_SUCCESS, deliVM, (int)ResultEnum.Success);
                }
            }

        }
        

        public BaseResponse<string> DeleteDelivery(int deliveryId, int customerId)
        {
            if (customerId == 0)
            {
                throw ApiException.Get(false, ConstantManager.MES_DELETE_DELIVERY_FAIL, ResultEnum.CustomerIdInTokenWrong, HttpStatusCode.NotFound);
            }
            var deliService = this.Service<IDeliveryInfoService>();
            var deliveryInfoOld = deliService.GetDeliveriesByCustomerId(customerId);
            var deli = deliveryInfoOld.Where(p => p.Id == deliveryId).FirstOrDefault();
            if (deli == null)
            {
                throw ApiException.Get(false, ConstantManager.MES_DELIVERYID_WRONG, ResultEnum.DeleteFail, HttpStatusCode.NotFound);
            }
            else
            {
                deliService.DeleteDelivery(deli);
                return BaseResponse<string>.Get(true, ConstantManager.MES_DELETE_DELIVERY_SUCCESS, null, ResultEnum.Success);
            }

        }
    }
}
