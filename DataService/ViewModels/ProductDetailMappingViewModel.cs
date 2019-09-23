using DataService.APIViewModels;
using DataService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModels
{
    public partial class ProductDetailMappingViewModel
    {
        public ProductViewModel Product { get; set; }
        public StoreViewModel Store { get; set; }
    }
    public partial class ProductDetailMappingAPIViewModel
    {
        public ProductDetailMappingAPIViewModel(ProductDetailMapping j)
        {
        }

        public ProductAPIViewModel Product { get; set; }
        public StoreAPIViewModel Store { get; set; }
    }
}
