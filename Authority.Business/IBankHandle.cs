using Authority.Model.BankModel;
using Authority.Model.Model.BankModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authority.Business
{
   public  interface IBankHandle
    {
        Task<Queue<CounterCuteGirl>> GetGirls();


        Task<Queue<BusinessMan>> GetBusinessMan();


    }
}
