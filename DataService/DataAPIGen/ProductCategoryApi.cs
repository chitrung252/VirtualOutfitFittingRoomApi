using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.ViewModels;
using AutoMapper.QueryableExtensions;
using DataService.Models.Entities;

namespace DataService.DataAPIGen
{
    public partial class ProductCategoryApi
    {
        //public List<ProductCategoryViewModel> GetCategory()
        //{
        //    return this.BaseService.GetCategory().Select(c => new ProductCategoryViewModel
        //    {
        //        CateID = c.CateID,
        //        CateName = c.CateName,
        //        PicUrl = c.PicUrl,
        //        Description = c.Description,
        //        SeoName = c.SeoName,
        //        SeoDescription = c.SeoDescription,
        //        SeoKeyword = c.SeoKeyword,
        //        ParentCateId = c.ParentCateId
        //    }).ToList();
        //}

        //public Dictionary<string, string> GetAllRoute()
        //{
        //    return this.BaseService.GetCategory().Select(cate => new ProductCategoryViewModel
        //    {
        //        SeoName = cate.SeoName
        //    }).Where(p => p.SeoName != null).Distinct().ToDictionary(p => p.SeoName, p => p.SeoName);
        //}

        ///// <summary>
        ///// Get all product category when:
        ///// Active == true
        ///// IsDisplay Website == true
        ///// BrandID == BrandID
        ///// </summary>
        ///// <param name="brandID">Brand ID</param>
        ///// <returns>List product category</returns>
        //public IQueryable<ProductCategory> GetActiveDisplayWebsiteProductCategoryByBrand(int brandID)
        //{
        //    return this.BaseService.GetActiveDisplayWebsiteProductCategoryByBrand(brandID);
        //}

        ///// <summary>
        ///// Hàm lấy Category bằng categoryId
        ///// </summary>
        ///// <param name="categoryId"></param>
        ///// <returns></returns>
        //public ProductCategory GetCategory(int categoryId)
        //{
        //    return this.BaseService.Get(q => q.CateID == categoryId).FirstOrDefault();
        //}
    }
}
