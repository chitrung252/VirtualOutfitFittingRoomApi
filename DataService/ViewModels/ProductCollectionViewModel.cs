using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModels
{
    public partial class ProductCollectionViewModel
    {
        
    }

    public class SpecialProductCollectionViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string bannerUrl { get; set; }
        public string seoName { get; set; }
        public string seoDescription { get; set; }
        public string seoKeyWord { get; set; }
        public string url { get; set; }
        public List<SpecialProductViewModel> ListProduct { get; set; }
        public bool defaultSelected { get; set; }

        public SpecialProductCollectionViewModel()
        {
            ListProduct = new List<SpecialProductViewModel>();
        }
    }
}
