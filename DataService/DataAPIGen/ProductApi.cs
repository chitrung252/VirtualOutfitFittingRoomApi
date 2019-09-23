using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace DataService.DataAPIGen
{
    public partial class ProductApi
    {
        public List<ProductViewModel> GetListProduct()
        {
            return this.BaseService.GetListProduct().Select(p => new ProductViewModel
            {
                CatID = p.CatID,
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Description = p.Description,
                PicURL = p.PicURL,
                Price = p.Price,
                SeoName = p.SeoName,
                SeoDescription = p.SeoDescription,
                SeoKeyWords = p.SeoKeyWords
            }).ToList();
        }

        public ProductViewModel GetProductBySeoName(string seoName)
        {
            return (new ProductViewModel(this.BaseService.GetProductBySeoName(seoName)));
        }

        public ProductViewModel GetProductById(int id)
        {
            //return (new ProductViewModel(this.BaseService.GetProductById(id)));
            throw new NotImplementedException();
        }

        public List<ProductViewModel> GetListProductByCatId(int catId)
        {
            return this.BaseService.GetListProductByCatId(catId).Select(p => new ProductViewModel
            {
                CatID = p.CatID,
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Description = p.Description,
                PicURL = p.PicURL,
                Price = p.Price
            }).ToList();
        }

        public Dictionary<string,string> GetAllRoute()
        {
            return this.BaseService.GetListProduct().Select(p => new ProductViewModel
            {
                SeoName = p.SeoName
            }).Where(p => p.SeoName != null).Distinct().ToDictionary(p => p.SeoName, p => p.SeoName);
        }
    }
}
