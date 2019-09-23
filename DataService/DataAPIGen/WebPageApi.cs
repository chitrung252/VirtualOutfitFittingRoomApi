using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace DataService.DataAPIGen
{
    public partial class WebPageApi
    {
        //public Dictionary<string,string> GetAllRoute()
        //{
        //    return this.BaseService.GetActiveWebPage().Select(p => new WebPageViewModel
        //    {
        //        SeoName = p.SeoName
        //    }).Where(p => p.SeoName != null).Distinct().ToDictionary(p => p.SeoName, p => p.SeoName);
        //}
    }
}
