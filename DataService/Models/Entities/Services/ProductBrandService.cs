using AutoMapper.QueryableExtensions;
using DataService.APIViewModels;
using DataService.Models.APIModels;
using DataService.Models.Entities.Repositories;
using DataService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models.Entities.Services
{
    public partial interface IProductBrandService
    {
        List<ProductBrandAPIViewModel> GetProductBrand(ProductBrandRequest<string> request);
        ProductBrandAPIViewModel CreateProductBrand(ProductBrandAPIViewModel productBrand);
        ProductBrandAPIViewModel UpdateProductBrand(ProductBrandAPIViewModel productBrand);
    }
    public partial class ProductBrandService : IProductBrandService
    {
        public List<ProductBrandAPIViewModel> GetProductBrand(ProductBrandRequest<string> request)
        {
            var repo = DependencyUtils.Resolve<IProductBrandRepository>();
            int index = (int)request.Page;
            int range = (index - 1) * (int)request.Limit;
            

            //var pro = repo.Get().AsEnumerable()
            //    .Where(p => (request.Name == null ||p.Name.ToUpper().Contains(request.Name.ToUpper()))).
            //    Select(p => new ProductBrandAPIViewModel(p)).ToList();
            var pro = repo.Get()
                .Where(p => (request.Name == null || p.Name.ToUpper().Contains(request.Name.ToUpper())));
            if (request.Ids != null)
            {
               var listId = request.Ids.Split(',');
                // filter Id 
                pro = from p in pro
                           where listId.Contains(p.ProductBrandId.ToString())
                           select p;
            }
            // OrderBy by Id of ProductBrand , page and limit 
            pro = pro.OrderBy(p => p.ProductBrandId).Skip(range).Take(request.Limit.Value);
            var productBrandVM = pro.ToList().Select(p => new ProductBrandAPIViewModel(p)).ToList();
            return productBrandVM;
        }
        public ProductBrandAPIViewModel CreateProductBrand(ProductBrandAPIViewModel productBrand)
        {
            var productBrandE = productBrand.ToEntity();
            this.Create(productBrandE);
            return new ProductBrandAPIViewModel(productBrandE);
        }

        public ProductBrandAPIViewModel UpdateProductBrand(ProductBrandAPIViewModel productBrand)        {            var productBrandE = Repository.FirstOrDefault(c => c.ProductBrandId == productBrand.ProductBrandId);            if (productBrandE != null)            {                var productBrandET = productBrand.ToEntity();                Repository.Edit(productBrandE);                Repository.Save();                productBrand = new ProductBrandAPIViewModel(productBrandET);                return productBrand;            }            return null;        }
    }
}
