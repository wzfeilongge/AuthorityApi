using Authority.IRepository.Base;
using Authority.Model.Model.BankModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authority.IRepository
{
   public interface ICounterCuteGirlRepository : IBaseRepository<CounterCuteGirl>
    {
        Task<bool> AddGirl(CounterCuteGirl girl);

        Task<bool> EditGirl(CounterCuteGirl girl);


    }
}
