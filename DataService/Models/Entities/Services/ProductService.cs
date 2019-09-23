using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models.Entities.Services
{
    public partial interface IProductService
    {
        List<Product> GetList(int? id);
    }
   public partial class ProductService : IProductService 
    {
        public List<Product> GetList(int? id)
        {
            return Repository.Get(x => (x.ProductID == id || id == null )).ToList();
        }
    }
}
