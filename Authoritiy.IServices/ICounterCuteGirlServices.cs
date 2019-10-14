using Authoritiy.IServices.BaseServices;
using Authority.Model.Model.BankModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authoritiy.IServices
{
  public  interface ICounterCuteGirlServices : IBaseEntity<CounterCuteGirl>
    {

        /// <summary>
        /// 呼叫 办理业务
        /// </summary>
        /// <param name="counterCuteGirl"></param>
        /// <returns></returns>
        Task<bool> CallNumber(CounterCuteGirl counterCuteGirl,int TakeId);


        /// <summary>
        /// 重新呼叫
        /// </summary>
        /// <param name="counterCuteGirl"></param>
        /// <param name="TakeId"></param>
        /// <returns></returns>
        Task<bool> RepeatNumber(CounterCuteGirl counterCuteGirl ,int TakeId );


        /// <summary>
        /// 结束办理业务
        /// </summary>
        /// <param name="counterCuteGirl"></param>
        /// <returns></returns>
        Task<bool> ShutdownCallNumber(CounterCuteGirl counterCuteGirl);



 








    }
}
