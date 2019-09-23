using AutoMapper.QueryableExtensions;
using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.DataAPIGen
{

    public partial class ProductCollectionItemMappingApi
    {

        public List<ProductCollectionItemMappingViewModel> GetCollectionItemById(int Id)
        {
            return this.BaseService.GetCollectionItemById(Id).Select(c => new ProductCollectionItemMappingViewModel
            {
                Id = c.Id,
                ProductCollectionId = c.ProductCollectionId,
                ProductId = c.ProductId,
                Active = c.Active,
                Position = c.Position
            }).ToList();
        }

        public ProductCollectionItemMappingViewModel GetFirstCollection(int id)
        {
            return (new ProductCollectionItemMappingViewModel(this.BaseService.GetFirstCollection(id)));
        }
    }
}
