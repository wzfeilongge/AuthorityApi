using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.Web.Api.ControllerModel
{
    public class ERPRequest
    {


    }
    public class SaleQuery {

        /// <summary>
        /// 查询的规则
        /// </summary>
        public int Rule { get; set; }

        /// <summary>
        /// 页码数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }

    }

    public class QueryProductName {

        /// <summary>
        /// 要查询的商品名称
        /// </summary>
        public string ProductName { get; set; }
    }

    public class QueryProductId
    {

        /// <summary>
        /// 要查询的商品Id
        /// </summary>
        public int Id { get; set; }
    }



}
