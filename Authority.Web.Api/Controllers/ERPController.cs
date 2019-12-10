using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authoritiy.IServices;
using Authority.Common.HttpHelper;
using Authority.Model.Model;
using Authority.Web.Api.ControllerModel;
using Authority.Web.Api.PolicyRequirement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Authority.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = Permissions.Name)]
    public class ERPController : ControllerBase
    {
        private readonly ISaleServices _saleServices;

        private readonly IProductServices _productServices;

        private readonly ILogger<ERPController> _Apiloger;

        //private readonly IUnitOfWork _unitOfWork;

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
        /// <param name="saleQuery"></param>
        /// <returns></returns>
        [HttpGet("QeurySale", Name = "QeurySale")]
        public async Task<IActionResult> QeurySale([FromBody] SaleQuery saleQuery)
        {
            var list = await _saleServices.QuerySale(saleQuery.Rule, saleQuery.Count, saleQuery.Page);
            if (list.Count > 0)
            {
                return Ok(new SucessModelData<List<Sale>>(list));
            }
            return Ok(new SucessModelData<object>(null));
        }

        /// <summary>
        /// 销售商品
        /// </summary>
        /// <param name="sale"></param>
        /// <returns></returns>
        [HttpPost("AddSale", Name = "AddSale")]
        public async Task<IActionResult> AddSale([FromBody] Sale sale)
        {
            if (ModelState.IsValid)
            {

                var istrue = await _saleServices.AddSale(sale);
                if (istrue)
                {
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
        [AllowAnonymous]
        public async Task<IActionResult> QueryProduct()
        {
            var list = await _productServices.QueryProductList();
            if (list != null)
            {
                return Ok(new SucessModelData<List<Product>>(list));
            }
            return Ok(new SucessModelData<object>(null));
        }

        /// <summary>
        /// 指定名称查询商品
        /// </summary>
        /// <param name="queryProductName">查询商品Model</param>
        /// <returns></returns>
        [HttpGet("QueryProductName", Name = "QueryProductName")]
        [AllowAnonymous]
        public async Task<IActionResult> QueryProductName([FromBody] QueryProductName queryProductName)
        {
            if (ModelState.IsValid)
            {
                var QueryList = await _productServices.QueryListInforName(queryProductName.ProductName);

                if (QueryList != null)
                {
                    return Ok(new SucessModelData<List<Product>>(QueryList));
                }
            }
            return Ok(new SucessModelData<object>(null));
        }

        /// <summary>
        /// 指定名称查询商品的库存
        /// </summary>
        /// <param name="queryProductName">查询商品Model</param>
        /// <returns></returns>
        [HttpGet("QueryProductCount", Name = "QueryProductCount")]
        [AllowAnonymous]
        public async Task<IActionResult> QueryProductCount([FromBody] QueryProductId queryProductName)
        {
            if (ModelState.IsValid)
            {
                var list = await _productServices.QueryListInforquantity(queryProductName.Id);
                if (list != null)
                {
                    return Ok(new SucessModelData<int>(list.Quantity));
                }
            }
            return Ok(new SucessModelData<int>(0));
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("EditProduct", Name = "EditProduct")]
        public async Task<IActionResult> EditProduct([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                var flag = await _productServices.EditProduct(product);

                if (flag != null)
                {
                    return Ok(new SucessModel());
                }
            }
            return Ok(new JsonFailCatch("修改失败"));
        }
    }
    #endregion
}