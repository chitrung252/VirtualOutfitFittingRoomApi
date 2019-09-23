using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModels
{
    public partial class OrderDetailViewModel
    {

    }

    public class SpecialOrderDetailViewModel
    {
        public string RentID { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }
        public int TotalAmount { get; set; }
        public int Discount { get; set; }
        public int FinalAmount { get; set; }
    }
}
