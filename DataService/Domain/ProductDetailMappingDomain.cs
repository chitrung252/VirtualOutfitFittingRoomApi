using DataService.APIViewModels;
using DataService.Models.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Domain
{
    public interface IProductDetailMappingDomain
    {
        List<ProductDetailMappingAPIViewModel> getProductDetails(int storeId);
    }
    public class ProductDetailMappingDomain : BaseDomain, IProductDetailMappingDomain
    {
        public List<ProductDetailMappingAPIViewModel> getProductDetails(int storeId)
        {
            var productDetailService = this.Service<IProductDetailMappingService>();
            return productDetailService.GetProductDetail(storeId);
        }
    }
}
