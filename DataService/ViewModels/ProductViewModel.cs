using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModels
{
   public class ProductAPIViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.Product>
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<double> ProductPrice { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<bool> IsActived { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CategoryID { get; set; }

        public ProductAPIViewModel() : base() { }
        public ProductAPIViewModel(DataService.Models.Entities.Product entity) : base(entity) { }
    }
}
