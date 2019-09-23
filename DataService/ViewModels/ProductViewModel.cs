using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModels
{
    public partial class ProductViewModel
    {
    }

    public class SpecialProductViewModel
    {
        public int catID { get; set; }
        public int productID { get; set; }
        public string productName { get; set; }
        public string picURL { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public string seoName { get; set; }
        public string seoDecription { get; set; }
        public string seoKeyWord { get; set; }
        public string url { get; set; }
        public int RatingTotal { get; set; }
        public int NumOfUserVoting { get; set; }
        public Boolean IsFavourite { get; set; }
        public List<SpecialProductViewModel> ExtraList { get; set; }
    }
}
