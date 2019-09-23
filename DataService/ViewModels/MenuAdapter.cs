using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModels
{
    public class MenuAdapter
    {
        public string MenuName { get; set; }
        public string URL { get; set; }
        public List<MenuAdapter> ChildsMenu { get; set; }

        public MenuAdapter()
        {
            this.ChildsMenu = new List<MenuAdapter>();
        }
    }
}
