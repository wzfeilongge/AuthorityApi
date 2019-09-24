using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authoritiy.IServices;
using Authority.Model.Model;
using Authority.Web.Api.ControllerModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Authority.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ERPController : ControllerBase
    {
        private readonly ISaleServices _saleServices;

        private readonly IProductServices _productServices;

        private readonly ILogger<ERPController> _Apiloger;

        public ERPController(ISaleServices saleServices, IProductServices productServices, ILogger<ERPController> Apiloger)
        {
            _saleServices = saleServices;
            _productServices = productServices;
            _Apiloger = Apiloger;
        }

        #region 销售类


        /// <summary>
        /// 查询销售记录
        /// </summary>
        /// <param name="Rule">查询的规则</param>
        /// <param name="Count">页码大小</param>
        /// <param name="Page">页码</param>
        /// <returns></returns>
        [HttpGet("QeurySale",Name = "QeurySale")]
        [Authorize(Policy ="SystemOrAdmin")]
        public async Task<IActionResult> QeurySale([FromBody] SaleQuery saleQuery)
        {
            var list = await _saleServices.QuerySale(saleQuery.Rule, saleQuery.Count, saleQuery.Page);
            if (list.Count>0) {
                return Ok(new SucessModelData<List<Sale>>(list));
            }
            return Ok(new SucessModelData<object>(null));
        }

        [HttpPost("AddSale", Name = "AddSale")]
        [Authorize(Policy = "SystemOrAdmin")]
        public async Task<IActionResult> AddSale([FromBody] Sale sale)
        {
            if (sale!=null)
            {
             
                var istrue =  await _saleServices.AddSale(sale);
                if (istrue) {

                    return Ok(new SucessModel());
                }
            }
            return Ok(new JsonFailCatch("增加销售记录失败"));
        }

        #endregion

        #region  商品类

        /// <summary>
        /// 查询所有的商品
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryProduct", Name = "QueryProduct")]
        [Authorize(Policy = "SystemOrAdmin")]
        public async Task<IActionResult> QueryProduct()
        {
            var list = await _productServices.QueryProductList();
            if (list!=null) {
                return Ok(new SucessModelData<List<Product>>(list));
            }
            return Ok(new SucessModelData<object>(null));
        }

        /// <summary>
        /// 指定名称查询商品
        /// </summary>
        /// <param name="queryProductName">查询商品Model</param>
        /// <returns></returns>
        [HttpGet("QueryProduct", Name = "QueryProduct")]
        [Authorize(Policy = "SystemOrAdmin")]
        public async Task<IActionResult> QueryProductName([FromBody] QueryProductName queryProductName)
        {
            var QueryList = await _productServices.QueryListInforName(queryProductName.ProductName);

            if (QueryList!=null) {
                return Ok(new SucessModelData<List<Product>>(QueryList));
            }

            return Ok(new SucessModelData<object>(null));
        }

        /// <summary>
        /// 指定名称查询商品的库存
        /// </summary>
        /// <param name="queryProductName">查询商品Model</param>
        /// <returns></returns>
        [HttpGet("QueryProductCount", Name = "QueryProductCount")]
        [Authorize(Policy = "SystemOrAdmin")]
        public async Task<IActionResult> QueryProductCount([FromBody] QueryProductId queryProductName)
        {
            var list = await _productServices.QueryListInforquantity(queryProductName.Id);
            if (list!=null) {
                return Ok(new SucessModelData<int>(list.Quantity));
            }
            return Ok(new SucessModelData<int>(0));
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("EditProduct", Name = "EditProduct")]
        [Authorize(Policy = "SystemOrAdmin")]
        public async Task<IActionResult> EditProduct([FromBody] Product product)
        {
            var flag =  await _productServices.EditProduct(product);
            if (flag!=null) {

                return Ok(new SucessModel());
            }
            return Ok(new JsonFailCatch("修改失败"));
        }

        #endregion
    }
}