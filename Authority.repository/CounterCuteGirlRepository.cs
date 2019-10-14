using Authority.IRepository;
using Authority.Model.Model.BankModel;
using Authority.repository.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authority.Repository
{
    public class CounterCuteGirlRepository : BaseRepository<CounterCuteGirl>, ICounterCuteGirlRepository
    {

        
        public CounterCuteGirlRepository(ILogger<BaseRepository<CounterCuteGirl>> logger) : base(logger)
        {

        }

        /// <summary>
        /// 加入一个成员
        /// </summary>
        /// <param name="girl"></param>
        /// <returns></returns>
        public async Task<bool> AddGirl(CounterCuteGirl girl)
        {          
            var flag = await AddModel(girl);
            if (flag > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 编辑一个成员
        /// </summary>
        /// <param name="girl"></param>
        /// <returns></returns>
        public async Task<bool> EditGirl(CounterCuteGirl girl)
        {
            var model = await GetModelAsync(u => u.Id == girl.Id && u.Name == girl.Name && u.EmployeeNumber == girl.EmployeeNumber);
            if (model != null)
            {
                model.IsTrue = girl.IsTrue;
                model.CounterNumber = girl.CounterNumber;
                var ismodify = await Modify(model);
                if (ismodify > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
