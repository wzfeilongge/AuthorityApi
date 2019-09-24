using Authoritiy.IServices.BaseServices;
using Authority.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authoritiy.IServices
{
  public  interface ISaleServices : IBaseEntity<Sale>
    {

        /// <summary>
        /// 增加一个销售记录
        /// </summary>
        /// <param name="sale"></param>
        /// <returns></returns>
        Task<bool> AddSale(Sale sale);


        /// <summary>
        /// 查询销售记录
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        Task<List<Sale>> QuerySale(int rule,int Count,int Page);

    }
}
