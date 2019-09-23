using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using DataService.APIViewModels;
using DataService.BaseConnect;
using DataService.Models.APIModels;
using DataService.Models.Entities.Repositories;
using DataService.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataService.Models.Entities.Services
{

    public partial interface IProductService
    {
        #region old
        IQueryable<Product> GetListProduct();
        Product GetProductBySeoName(string seoName);
        IQueryable<Product> GetListProductByCatId(int catId);
        ProductAPIViewModel GetProductFromProductDetailMapping(int store_id, int product_id);
        #endregion

        List<ProductAPIViewModel> GetProductByRequest(ProductRequest<string> request);

        int GetCount(ProductRequest<string> request);

        ProductAPIViewModel GetProductById(int id);

        ProductAPIViewModel UpdateProduct(ProductAPIViewModel product);

        ProductAPIViewModel CreateProduct(ProductAPIViewModel product);
        ProductAPIViewModel GetProductDeliveryFee();
        ProductAPIViewModel GetProductByCode(string productCode);
        List<string> getUrl(int id);
        List<ProductAPIViewModel> getProductVariation(int? gen_id);
        List<ProductAPIViewModel> getProductExtraById(int id);
        IQueryable<Product> getQueryProductExtraById(int id);
    }

    public partial class ProductService : IProductService
    {
        public List<ProductAPIViewModel> getProductExtraById(int id)
        {
            var cateMappingService = DependencyUtils.Resolve<ICategoryExtraMappingService>();
            var productCateSerivce = DependencyUtils.Resolve<IProductCategoryService>();
            var product = Repository.GetActive(p => p.ProductID == id).FirstOrDefault();
            var cateId = cateMappingService.GetCategoryExtraMapping(product.CatID);
            if (cateId != null)
            {
                var list = productCateSerivce.GetCategoryById(cateId.ExtraCategoryId);
                if (list == null)
                {
                    return new List<ProductAPIViewModel>();
                }
                else
                {
                    return list.productsVM;

                }
            }
            else
            {
                return new List<ProductAPIViewModel>();
            }



        }
        public IQueryable<Product> getQueryProductExtraById(int id)
        {
            
            var cateMappingRepo = DependencyUtils.Resolve<ICategoryExtraMappingRepository>();
            
            var product = Repository.GetActive();
            var productById = Repository.GetActive(p => p.ProductID == id);
            var extraMappingQ = cateMappingRepo.Get(cem => cem.IsEnable == true);

            // get CateExtraMapping by ProductId 
            var query1 = from t1 in productById
                         join t2 in extraMappingQ
             on t1.CatID equals t2.PrimaryCategoryId
                         select t2;
            // Get ProductExtra by query 1
            var productF = from t1 in product
                           join t2 in query1
                           on t1.CatID equals t2.ExtraCategoryId
                           select t1;

            return productF;


        }
        #region old
        //public IQueryable<Product> GetListProduct()
        //{
        //    return this.Get(p => p.Active == true && p.ProductType != 6);
        //}
        //public Product GetProductBySeoName(string seoName)
        //{
        //    return this.Get(p => p.SeoName == seoName).FirstOrDefault();
        //}

        //public Product GetProductById(int id)
        //{
        //    return this.Get(p => p.ProductID == id && p.ProductType != 6).FirstOrDefault();
        //}

        //public IQueryable<ProductViewModel> GetListProductByCatId(int catId)
        //{          
        //    return this.Get(p => p.CatID == catId);
        //}

        public ProductAPIViewModel GetProductFromProductDetailMapping(int store_id, int product_id)
        {
            var repository = DependencyUtils.Resolve<IProductDetailMappingRepository>();
            var pro = repository.FirstOrDefaultActive(p => p.ProductID == product_id && p.StoreID == store_id);
            if (pro != null)
            {
                return new ProductAPIViewModel(pro.Product);
            }
            return null;
        }

        public IQueryable<Product> GetListProduct()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> GetListProductByCatId(int catId)
        {
            throw new NotImplementedException();
        }

        public Product GetProductBySeoName(string seoName)
        {
            throw new NotImplementedException();
        }

        #endregion
        //private List<int> GetListCateExtraIdByCateId(int cate_id)
        //{
        //    var repo = DependencyUtils.Resolve<ICategoryExtraMappingRepository>();
        //    return repo.GetActive(p => p.PrimaryCategoryId == cate_id).Select(p => p.ExtraCategoryId).ToList();
        //}
        public ProductAPIViewModel GetProductByCode(string productCode)
        {
            var product = Repository.FirstOrDefaultActive(p => p.Code == productCode);
            if (product != null)
            {
                return new ProductAPIViewModel(product);
            }
            return null;
        }
        public List<string> getUrl(int id)
        {

            var repo = DependencyUtils.Resolve<IProductImageRepository>();
            var a = repo.GetActive(p => p.ProductId == id).AsEnumerable().Select(p => p.ImageUrl).ToList();
            return a;
        }
        //private List<ProductAPIViewModel> GetListProductExtra(int cate_id)
        //{
        //    var result = new List<ProductAPIViewModel>();
        //    var list = GetListCateExtraIdByCateId(cate_id);
        //    foreach (var item in list)
        //    {
        //        var temp = Repository.GetActive(p => p.CatID == item).AsQueryable().ProjectTo<ProductAPIViewModel>(this.AutoMapperConfig).ToList();
        //        result.AddRange(temp);
        //    }
        //    return result;
        //}

        public List<ProductAPIViewModel> getProductVariation(int? gen_id)
        {
            if (gen_id != null)
            {
                return Repository.GetActive(p => p.GeneralProductId == gen_id).AsQueryable().ToList().Select(p => new ProductAPIViewModel(p)
                {
                    PicURLs = getUrl(p.ProductID),
                    ProductVariations = getProductVariation(p.ProductID),
                    BrandName = p.ProductBrand.ToViewModel<ProductBrand, ProductBrandAPIViewModel>().Name

                    // ProductBrandVM = p.ProductBrand.ToViewModel<ProductBrand, ProductBrandAPIViewModel>()
                }).ToList();
            }
            return null;
        }

        public ProductAPIViewModel GetProductDeliveryFee()
        {
            var product = Repository.FirstOrDefaultActive(p => p.ProductName.Equals(ConstantManager.DELIVERY));
            if (product != null)
            {
                return new ProductAPIViewModel(product);
            }
            return null;
        }

        public ProductAPIViewModel CreateProduct(ProductAPIViewModel product)
        {
            var productE = product.ToEntity();
            this.Create(productE);
            return new ProductAPIViewModel(productE);
        }

        public ProductAPIViewModel UpdateProduct(ProductAPIViewModel product)
        {
            var productE = Repository.FirstOrDefault(c => c.ProductID == product.ProductID);
            if (productE != null)
            {
                var customerET = product.ToEntity();
                Repository.Edit(productE);
                Repository.Save();
                product = new ProductAPIViewModel(productE);
                return product;
            }
            return null;

        }

        public ProductAPIViewModel GetProductById(int id)
        {
            var product = Repository.FirstOrDefaultActive(c => c.ProductID == id);
            if (product != null)
            {
                var productVM = new ProductAPIViewModel();
                //if ((Sky.TryParse<List<Att2Model>>(product.Att2) != null))
                //{
                productVM = new ProductAPIViewModel(product)
                {
                    ProductVariations = getProductVariation(product.GeneralProductId),
                    //  ProductExtra = GetListProductExtra(product.CatID),
                    PicURLs = getUrl(product.ProductID),
                    BrandName = product.ProductBrand.ToViewModel<ProductBrand, ProductBrandAPIViewModel>().Name
                    //  ProductBrandVM = product.ProductBrand.ToViewModel<ProductBrand, ProductBrandAPIViewModel>()
                };
                //}
                //else
                //{
                //    productVM = new ProductAPIViewModel(product)
                //    {
                //        ProductVariations = getProductVariation(product.GeneralProductId),
                //        //  ProductExtra = GetListProductExtra(product.CatID),
                //        PicURLs = getUrl(product.ProductID),
                //        BrandName = product.ProductBrand.ToViewModel<ProductBrand, ProductBrandAPIViewModel>().Name
                //        //  ProductBrandVM = product.ProductBrand.ToViewModel<ProductBrand, ProductBrandAPIViewModel>()
                //    };
                //}

                return productVM;
            }
            return null;
        }

        public int GetCount(ProductRequest<string> request)
        {
            var listProduct = GetProductByRequest(request);
            return listProduct.Count;
        }

        public List<ProductAPIViewModel> GetProductByRequest(ProductRequest<string> request)
        {
            var ProductDetailMappingRepo = DependencyUtils.Resolve<IProductDetailMappingRepository>();

            var listProduct = new List<ProductAPIViewModel>();
            var productIds = new List<int>();
            int index = (int)request.Page;
            int range = (index - 1) * (int)request.Limit;
            string unSign = "";
            if (request.Name != null)
            {
                 unSign = request.Name.Trim().Replace(" ", "-");
            }
           

            string[] listId;
            //string[] teststring = {"123","567" };
            //var b = ",123,234,";
            //var c = teststring.Intersect(b).Any();
            var productTemp = Repository.GetActive().Where(p => (request.SinceId == null || p.ProductID >= request.SinceId) &&
                               (p.GeneralProductId == null) &&
                               (request.CreateAtMin == null || p.CreateTime > request.CreateAtMin) &&
                               (request.CreateAtMax == null || p.CreateTime < request.CreateAtMax) &&
                               (request.Name == null || ( p.ProductName.ToUpper().Contains(request.Name.ToUpper()) || p.ProductNameEng.ToUpper().Contains(unSign.ToUpper()))) &&
                               (request.ProductBrandId == null || p.ProductBrandId == request.ProductBrandId));




            if (request.CateIds != null)
            {
                var CateIdsArr = request.CateIds.Split(',');
                if (CateIdsArr.Length == 1)
                {
                    productTemp = productTemp.Where(p => ((string.IsNullOrEmpty(p.CateIds) && p.CatID.ToString().Equals(request.CateIds)) ||
                                   !string.IsNullOrEmpty(p.CateIds) && p.CateIds.Contains("," + request.CateIds + ",")));
                }
                else
                {

                    productTemp = productTemp.Where(p => ((string.IsNullOrEmpty(p.CateIds) && CateIdsArr.Any(a => a.Equals(p.CatID.ToString()))) ||
                                               !string.IsNullOrEmpty(p.CateIds) && CateIdsArr.Any(a => p.CateIds.Contains("," + a + ","))));
                }
            }


            if (request.ColIds != null)
            {
                var ProductCollectionRepo = DependencyUtils.Resolve<IProductCollectionRepository>();
                var listColIds = request.ColIds.Split(',');
                var collectIds = ProductCollectionRepo.GetActive(q => q.StoreId == request.StoreId && q.BrandId == request.BrandId);
                var query2 = from p in collectIds
                             where listColIds.Contains(p.Id.ToString())
                             select p;
                var ColRepo = DependencyUtils.Resolve<IProductCollectionItemMappingRepository>();

                IQueryable<Product> product1 = null;

                var check = query2.AsQueryable().ToList();
                if (check.Count() > 0)
                {
                    var tb1 = productTemp;
                    //var tb2 = query2.FirstOrDefault().ProductCollectionItemMappings.AsQueryable().ToList();
                    var tb2 = query2.SelectMany(p => p.ProductCollectionItemMappings);

                    //var tb2 = item.ProductCollectionItemMappings.AsQueryable();
                    product1 = from t1 in tb1
                               join t2 in tb2
                           on t1.ProductID equals t2.ProductId
                               select t1;
                    //product1.AddRange(temp);
                }
                else
                {
                    return null;
                }
                if (request.Ids != null)
                {
                    listId = request.Ids.Split(',');
                    product1 = from p in product1
                               where listId.Contains(p.ProductID.ToString())
                               select p;
                }
                product1 = product1.OrderBy(p => p.ProductID).Skip(range).Take(request.Limit.Value);
                listProduct = product1.ToList().Select(p => new ProductAPIViewModel(p)
                {
                    ProductVariations = getProductVariation(p.ProductID),
                    PicURLs = getUrl(p.ProductID),
                    BrandName = p.ProductBrand.ToViewModel<ProductBrand, ProductBrandAPIViewModel>().Name
                }).ToList();

            }
            else
            {

                var table1 = ProductDetailMappingRepo.GetActive(pd => pd.StoreID == request.StoreId && pd.Active == true)
                .Where(pd => (pd.SaleMethodEnum & request.SaleEnum) == request.SaleEnum);
                var table2 = productTemp;
                var detail = from t1 in table1
                             join t2 in table2
                           on t1.ProductID equals t2.ProductID
                             select t1;
                var product = from t1 in table1
                              join t2 in table2
                            on t1.ProductID equals t2.ProductID
                              select t2;


                if (request.Ids != null)
                {
                    listId = request.Ids.Split(',');
                    product = from p in product
                              where listId.Contains(p.ProductID.ToString())
                              select p;
                }

                product = product.OrderBy(p => p.ProductID).Skip(range).Take(request.Limit.Value);
                listProduct = product.AsQueryable().ToList().Select(p => new ProductAPIViewModel(p)
                {
                    ProductVariations = getProductVariation(p.ProductID),
                    PicURLs = getUrl(p.ProductID),
                    Price = detail.Where(d => d.ProductID == p.ProductID).FirstOrDefault().Price.Value,
                    BrandName = p.ProductBrand.ToViewModel<ProductBrand, ProductBrandAPIViewModel>().Name
                }).ToList();
            }

            //IEnumerable<ProductAPIViewModel> filterCate = null;
            //if (request.CateIds != null)
            //{
            //    var CateIdsArr = request.CateIds.Split(',');
            //    if (CateIdsArr.Length == 1)
            //    {
            //        filterCate = listProduct.Where(p => (( string.IsNullOrEmpty(p.CateIds) && p.CatID.ToString().Equals(request.CateIds)) ||
            //                       !string.IsNullOrEmpty(p.CateIds) && Array.Exists(p.CateIds.Split(','), e => e == request.CateIds)));
            //    }
            //    else
            //    {
            //        filterCate = listProduct.Where(p => ((string.IsNullOrEmpty(p.CateIds) && Array.Exists(CateIdsArr, e => e == p.CatID.ToString())) ||
            //                                   !string.IsNullOrEmpty(p.CateIds) && p.CateIds.Split(',').Intersect(CateIdsArr).Any()));
            //    }
            //}
            //else
            //{
            //    filterCate = listProduct;
            //}
            //listProduct = filterCate.AsQueryable().ToList();

            return listProduct;
            //int index = (int)request.Page;
            //int range = (index - 1) * (int)request.Limit;
            //return orderBy.Skip(range).Take(request.Limit.Value).ToList();
            //}
            //if (request.ColIds != null && request.StoreId == null)
            //{
            //    var ColRepo = DependencyUtils.Resolve<IProductCollectionItemMappingRepository>();
            //    var listColIds = request.ColIds.Split(',');
            //    foreach (var item in listColIds)
            //    {
            //        var temp = Int32.Parse(item);
            //        var cols = ColRepo.GetActive(p => p.ProductCollectionId == temp).Select(a => a.ProductId).ToList();
            //        productIds.AddRange(cols);
            //    }
            //}


            //var product = productTmp.Where(p => 
            //    (request.SinceId == null || p.ProductID >= request.SinceId) &&
            //    (p.GeneralProductId == null) &&
            //    (request.CreateAtMin == null || p.CreateTime > request.CreateAtMin) &&
            //    (request.CreateAtMax == null || p.CreateTime < request.CreateAtMax) &&
            //    (request.Name == null || p.ProductName.ToUpper().Contains(request.Name.ToUpper())) &&
            //    (request.ProductBrandId == null || p.ProductBrandId == request.ProductBrandId) &&
            //    (request.ColIds == null || productIds.Contains(p.ProductID)));





            // filter catId 

            // join ProductDetail - Product



            //else
            //{

            //    var productVM = join.ToList().Select(p => new ProductAPIViewModel(p)
            //    {
            //        ProductVariations = getProductVariation(p.ProductID),
            //        PicURLs = getUrl(p.ProductID),
            //        BrandName = p.ProductBrand.ToViewModel<ProductBrand, ProductBrandAPIViewModel>().Name
            //    }).ToList();

            //    listProduct.AddRange(productVM);
            //}


        }

    }
}
