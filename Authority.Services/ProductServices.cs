using Authoritiy.IServices;
using Authority.IRepository;
using Authority.Model.Model;
using Authority.Services.BaseService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authority.Services
{
    public class ProductServices : BaseService<Product>, IProductServices
    {
        private readonly IProductRepository _productRepository;
         private readonly ILogger<Product> _myLogger;
        public ProductServices(IProductRepository productRepository, ILogger<Product> myLogger)
        {
            _productRepository = productRepository;
            _myLogger = myLogger;
        }
        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public  async Task<bool> AddProduct(Product product)
        {
            if (product!=null) {
               var model=await _productRepository.AddModel(product);
                if (model>0) {
                    return true;
                }              
            }
            return false;
        }

        /// <summary>
        /// 删除Model
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public  async Task<bool> DelProduct(Product product)
        {
            if (product!=null) {
                var model = await _productRepository.DelBy(u=>u.Id==product.Id&&u.ProductName==product.ProductName);
                if (model > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public  async Task<Product> EditProduct(Product product)
        {
            if (product!=null)
            {
                var queryModel = await _productRepository.GetModelAsync(u=>u.Id==product.Id);
                queryModel.ProductName = product.ProductName;
                queryModel.Quantity = product.Quantity;
                var model=await _productRepository.Modify(product);
                if (model>0) {
                    return product;
                }
            }
            return null;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> Modfiy(Product product)
        {
            int istrue = await _productRepository.Modify(product);
            if (istrue > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 商品名称查询商品
        /// </summary>
        /// <param name="ProductName"></param>
        /// <returns></returns>
        public async Task<List<Product>> QueryListInforName(string ProductName)
        {
            return await _productRepository.Query(u => u.ProductName == ProductName);
        }
        
        /// <summary>
        /// 查询商品库存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  async Task<Product> QueryListInforquantity(int id)
        {
            return await _productRepository.GetModelAsync(u => u.Id==id);
        }

        /// <summary>
        /// 查询所有商品
        /// </summary>
        /// <returns></returns>
        public  async Task<List<Product>> QueryProductList()
        {
            return await _productRepository.Query(u=>u.Id>0);
        }
    }
}
