using Authoritiy.IServices.BaseServices;
using Authority.Model.BankModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authoritiy.IServices
{
   public interface IBusinessManServices : IBaseEntity<BusinessMan>
    {

        Task<BusinessMan> Create(int TaskType);


        Task<int> Modify(BusinessMan businessMan);


    }
}
