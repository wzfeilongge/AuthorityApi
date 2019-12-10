using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authoritiy.IServices;
using Authority.Model.BankModel;
using Authority.Model.Model.BankModel;

namespace Authority.Business
{
    public class BankBusiness : IBankHandle
    {
        private readonly IBusinessManServices _businessManServices;

        private readonly ICounterCuteGirlServices _counterCuteGirlServices;

        public BankBusiness(IBusinessManServices businessManServices, ICounterCuteGirlServices counterCuteGirlServices)
        {
            _businessManServices = businessManServices;
            _counterCuteGirlServices = counterCuteGirlServices;
        }

        public async Task<Queue<BusinessMan>> GetBusinessMan()
        {
            var model = (await _businessManServices.Query(obj => obj.Istrue == false)).OrderBy(u => u.TakeNumber);
            return  new Queue<BusinessMan>(model);

        }

        public  async Task<Queue<CounterCuteGirl>> GetGirls()
        {
            var model = (await _counterCuteGirlServices.Query(obj => obj.IsTrue)).OrderBy(u=>u.CounterNumber);
            return new Queue<CounterCuteGirl>(model);
           
        }
    }
}
