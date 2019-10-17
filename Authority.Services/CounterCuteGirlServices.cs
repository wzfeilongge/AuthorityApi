using Authoritiy.IServices;
using Authority.IRepository;
using Authority.Model.Model.BankModel;
using Authority.Services.BaseService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authority.Services
{
    public class CounterCuteGirlServices : BaseService<CounterCuteGirl>, ICounterCuteGirlServices
    {

        private readonly ICounterCuteGirlRepository _counterCuteGirlRepository;

        private readonly IBusinessManRepository _BusinessManRepository;

        public CounterCuteGirlServices(ICounterCuteGirlRepository counterCuteGirlRepository, IBusinessManRepository BusinessManRepository)
        {
            _counterCuteGirlRepository = counterCuteGirlRepository;
            _BusinessManRepository = BusinessManRepository;
            BaseDal = counterCuteGirlRepository;
        }
        public  async Task<bool> CallNumber(CounterCuteGirl counterCuteGirl, int TakeId)
        {
            if (counterCuteGirl!=null) 
            {
                var model=await  _BusinessManRepository.GetModelAsync(u=>u.Id==TakeId);
                model.Istrue = true;
                int count=await _BusinessManRepository.Modify(model);
                if (count>0) {
                    return true;
                }
            }
            return false;
        }

        public  async Task<bool> RepeatNumber(CounterCuteGirl counterCuteGirl, int TakeId)
        {
            if (counterCuteGirl != null)
            {
                var model = await _BusinessManRepository.GetModelAsync(u => u.Id == TakeId);
                if (!model.Istrue) {
                    return true;
                }
            }
            return false;
        }

        public  async Task<bool> ShutdownCallNumber(CounterCuteGirl counterCuteGirl)
        {
            if (counterCuteGirl != null)
            {
                counterCuteGirl.IsTrue = false;
                var count = await _counterCuteGirlRepository.Modify(counterCuteGirl);
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
