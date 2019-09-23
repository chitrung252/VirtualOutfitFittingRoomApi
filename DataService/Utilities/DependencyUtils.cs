using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataService.Utilities
{
    public static class DependencyUtils
    {
        public static T Resolve<T>() where T : class
        {
            return DependencyResolverExtensions.GetService<T>(D‌​ependencyResolver.Current);
        }
    }
}
