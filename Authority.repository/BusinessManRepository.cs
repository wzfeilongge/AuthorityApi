using Authority.IRepository;
using Authority.Model.BankModel;
using Authority.repository.Base;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Repository
{
    public class BusinessManRepository : BaseRepository<BusinessMan>, IBusinessManRepository
    {
        public BusinessManRepository(ILogger<BaseRepository<BusinessMan>> logger) :base(logger)
        {

        }





    }
}


