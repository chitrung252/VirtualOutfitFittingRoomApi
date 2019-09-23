using DataService.APIViewModels;
using DataService.Models.APIModels;
using DataService.Models.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Domain
{
    public partial interface IProductCollectionDomain
    {
        List<ProductCollectionAPIViewModel> getProductCollectinon(BaseRequest<string> request);
    }
    public class ProductCollectionDomain : BaseDomain, IProductCollectionDomain
    {
        public List<ProductCollectionAPIViewModel> getProductCollectinon(BaseRequest<string> request)
        {
            var service = this.Service<IProductCollectionService>();
            return service.getProductCollectinon(request);
        }
    }
}
