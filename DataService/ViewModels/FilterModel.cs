using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModels
{
    public class FilterModel
    {
        public string FitlerName { get; set; }
        public List<string> ListItemFilter { get; set; }

        public FilterModel()
        {
            this.ListItemFilter = new List<String>();
        }

        public void ConvertToFilter(string filterText)
        {
            var filter = filterText.Split(':');
            this.FitlerName = filter[0];
            this.ListItemFilter = filter[1].Split(';').ToList();
        }
    }
}
