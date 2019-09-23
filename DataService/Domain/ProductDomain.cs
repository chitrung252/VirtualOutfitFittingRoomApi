using AutoMapper.QueryableExtensions;
using DataService.APIViewModels;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Models.Entities.Repositories;
using DataService.Models.Entities.Services;
using DataService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Domain
{
    public interface IProductDomain
    {
        #region old
        /// <summary>
        /// Author: PhatPNH
        /// </summary>
        /// <param name="terminalId">It's StoreId</param>
        /// <param name="saleMethodEnum">May be EatIn , Delivery , TakeAway</param>
        /// <returns></returns>
        List<ProductAPIViewModel> GetProductListByTerminalIdAndSaleMethod(int terminalId, int saleMethodEnum);
        List<ProductAPIViewModel> GetAll();
        #endregion

        BaseResponse<List<ProductAPIViewModel>> GetProductByRequest(ProductRequest<string> request);

        int getCount(ProductRequest<string> request);

        ProductAPIViewModel GetProductById(int id);

        ProductAPIViewModel UpdateProduct(ProductAPIViewModel product);

        ProductAPIViewModel CreateProduct(ProductAPIViewModel product);

        ProductAPIViewModel GetProductDeliveryFee();
    }
    public class ProductDomain : BaseDomain, IProductDomain
    {
        public ProductAPIViewModel CreateProduct(ProductAPIViewModel product)
        {
            var productService = this.Service<IProductService>();
            return productService.CreateProduct(product);
        }

        public ProductAPIViewModel UpdateProduct(ProductAPIViewModel product)
        {
            var productService = this.Service<IProductService>();

            return productService.UpdateProduct(product);
        }

        public ProductAPIViewModel GetProductById(int id)
        {
            var productService = this.Service<IProductService>();
            return productService.GetProductById(id);
        }

        public int getCount(ProductRequest<string> request)
        {
            var productService = this.Service<IProductService>();
            return productService.GetCount(request);
        }

        public BaseResponse<List<ProductAPIViewModel>> GetProductByRequest(ProductRequest<string> request)
        {
            var productService = this.Service<IProductService>();
            try
            {
                if (request.StoreId == null)
                {
                    throw ApiException.Get(false, "StoreId is required!", ResultEnum.StoreIdNotFound, HttpStatusCode.BadRequest);
                }
                var listProduct = productService.GetProductByRequest(request);
                if (listProduct == null || listProduct.Count <= 0  )
                {
                    throw ApiException.Get(true, ConstantManager.MES_PRODUCT_NOTFOUND, ResultEnum.ProductNotFound, HttpStatusCode.OK);
                }
                return BaseResponse<List<ProductAPIViewModel>>.Get(true, ConstantManager.MES_SUCCESS,listProduct, ResultEnum.Success);

            }
            catch (Exception e)
            {
                if (e is ApiException)
                {
                    throw e;
                }
                else
                {
                    throw ApiException.Get(false, e.ToString(), ResultEnum.InternalError, HttpStatusCode.InternalServerError);
                }
            }

        }

        public ProductAPIViewModel GetProductDeliveryFee()
        {
            var productService = this.Service<IProductService>();
            throw new NotImplementedException();
        }

        #region old
        public List<ProductAPIViewModel> GetProductListByTerminalIdAndSaleMethod(int terminalId, int saleMethodEnum)
        {
            throw new NotImplementedException();
            //var productList = new List<ProductAPIViewModel>();
            //// get ProductService and StoreProduct
            //var productService = this.Service<IProductDetailMappingService>();
            //var storeService = this.Service<IStoreService>();
            //// get StoreViewModel by StoreId
            //var store = storeService.GetStoreById(terminalId);

            //// listProduct by 'and bit' saleMethodEnum
            //var listP = productService
            //    .GetProductAPIByStoreID(terminalId, store.BrandId.Value)
            //    .Where(p => (saleMethodEnum == 0 || (p.Product.SaleMethodEnum.Value & saleMethodEnum) == saleMethodEnum))
            //    //.Take(10) // get 10 product
            //    ;
            //foreach (var item in listP)
            //{
            //    if (item.Product != null)
            //    {
            //        // get each Product in List Product
            //        var p = item.Product;


            //        if (p.IsFixedPrice == false)
            //        {
            //            // Fix Price if Price Change by Store
            //            var priceProduct = productService.GetPriceByStore(terminalId, item.Product.ProductID);
            //            if (priceProduct != 0)
            //            {
            //                p.Price = priceProduct;
            //            }
            //            else
            //            {
            //                p.Price = item.Product.Price;
            //            }
            //            // Update Discount by Storeid and ProductId
            //            var discountProduct = productService.GetDiscountByStore(terminalId, item.Product.ProductID);
            //            if (priceProduct != 0)
            //            {
            //                p.DiscountPercent = discountProduct;
            //            }
            //            else
            //            {
            //                p.DiscountPercent = item.Product.DiscountPercent;
            //            }

            //        }
            //        productList.Add(p);
            //    }
            //}

            //return productList;

        }

        public ProductAPIViewModel GetProductFromProductDetailMapping(int store_id, int product_id)
        {

            var service = this.Service<IProductService>();
            return service.GetProductFromProductDetailMapping(store_id, product_id);
        }

        public List<ProductAPIViewModel> GetAll()
        {
            var productService = this.Service<IProductService>();
            return productService.GetActive().AsQueryable().ProjectTo<ProductAPIViewModel>(this.AutoMapperConfig).ToList();
        }
        #endregion
    }
}
