using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace DataService.DataAPIGen
{
    public partial class ProductCollectionApi
    {
        public List<ProductCollectionViewModel> GetCollection()
        {
            return this.BaseService.GetCollection().Select(c => new ProductCollectionViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                SEO = c.SEO,
                SEODescription = c.SEODescription,
                SEOKeyword = c.SEOKeyword
            }).ToList();
        }

        public ProductCollectionViewModel GetCollectionBySeoName(string seoName)
        {
            return (new ProductCollectionViewModel(this.BaseService.GetCollectionBySeoName(seoName)));
        }

        public Dictionary<string, string> GetAllRoute()
        {
            return this.BaseService.GetCollection().Select(p => new ProductCollectionViewModel
            {
                SEO = p.SEO
            }).Where(p => p.SEO != null).Distinct().ToDictionary(p => p.SEO, p => p.SEO);
        }
    }
}
