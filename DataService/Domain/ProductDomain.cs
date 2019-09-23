using AutoMapper.QueryableExtensions;
using DataService.Models.Entities.Services;
using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Domain
{
   public class ProductDomain : BaseDomain
    {
        public List<ProductAPIViewModel> GetList(int? id)
        {
            var ser = this.Service<IProductService>();
            return ser.GetList(id).AsQueryable().ProjectTo<ProductAPIViewModel>(this.AutoMapperConfig).ToList();
        }

    }
}
