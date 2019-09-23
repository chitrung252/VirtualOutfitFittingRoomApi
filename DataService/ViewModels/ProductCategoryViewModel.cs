using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModels
{
    public partial class ProductCategoryViewModel
    {
    }

    public class SpecialProductCategoryViewModel
    {
        public int CateId { get; set; }
        public string CateName { get; set; }
        public string PicUrl { get; set; }
        public string Description { get; set; }
        public string SeoName { get; set; }
        public string SeoDecription { get; set; }
        public string SeoKeyWord { get; set; }
        public List<SpecialProductViewModel> ListProduct { get; set; }
        public string url { get; set; }
        public bool defaultSelected { get; set; }

        public SpecialProductCategoryViewModel()
        {
            if(ListProduct == null)
                ListProduct = new List<SpecialProductViewModel>();
        }
    }
}
