using Authoritiy.IServices;
using Authority.IRepository;
using Authority.Model.BankModel;
using Authority.Services.BaseService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authority.Services
{
    public class BusinessManServices : BaseService<BusinessMan> ,IBusinessManServices
    {
        private readonly IBusinessManRepository _BusinessRepository;
        public BusinessManServices(IBusinessManRepository BusinessRepository)
        {
            _BusinessRepository = BusinessRepository;
        }

        public    async Task<BusinessMan> Create(int TaskType)
        {
            var resultCout = await _BusinessRepository.Query(u => u.Istrue == false);
            var BusinessMan = new BusinessMan
            {
                TakeNumber = DateTime.Now,
                Istrue = false,
                ResultName=resultCout.Count,
                BusinessType=TaskType                
            };
            return await _BusinessRepository.Add(BusinessMan);         
        }

        public  async Task<int> Modify(BusinessMan businessMan)
        {
            return await _BusinessRepository.Modify(businessMan);
        }
    }
}
