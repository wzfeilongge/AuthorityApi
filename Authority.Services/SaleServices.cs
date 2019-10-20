using Authoritiy.IServices;
using Authority.Common.HttpHelper;
using Authority.IRepository;
using Authority.Model.Model;
using Authority.Services.BaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authority.Services
{
    public class SaleServices : BaseService<Sale>, ISaleServices
    {
        private readonly ISaleRepository _saleRepository;

        private readonly IProductRepository _productRepository;

        public SaleServices(ISaleRepository saleRepository, IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            BaseDal = saleRepository;

        }


        [UseTran]
        /// <summary>
        /// 增加一个销售的记录表
        /// </summary>
        /// <param name="sale"></param>
        /// <returns></returns>
        public async Task<bool> AddSale(Sale sale)
        {
            if (sale != null)
            {
                var product = await _productRepository.GetModelAsync(u => u.Id == sale.ProductId && u.Quantity >= sale.Quantity);
                if (product != null)
                {
                    var model = await _saleRepository.AddModel(sale);
                    if (model > 0)
                    {
                        product.Quantity -= sale.Quantity;
                        var flag = await _productRepository.Modify(product);
                        if (flag > 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="Count"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<List<Sale>> QuerySale(int rule, int Count, int Page)
        {
            var list = await _saleRepository.Query(u => u.Id > 0);
            if (rule == 1)
            {
                list.OrderBy(u => u.Price);
                return list.Skip((Count - 1) * Page).Take(Page).ToList();
            }
            if (rule == 2)
            {
                list.OrderBy(u => u.SaleDate);
                return list.Skip((Count - 1) * Page).Take(Page).ToList();
            }
            return null;
        }
    }
}
