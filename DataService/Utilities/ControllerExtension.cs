using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataService.Utilities
{
    public static class ControllerExtension
    {
        public static T Service<T>(this Controller controller) where T : class
        {
            return DependencyUtils.Resolve<T>();
        }
    }
}
