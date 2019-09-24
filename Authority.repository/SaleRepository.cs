using Authority.IRepository;
using Authority.Model.Model;
using Authority.repository.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Repository
{
  public class SaleRepository : BaseRepository<Sale>, ISaleRepository
    {

        public SaleRepository(ILogger<BaseRepository<Sale>> logger) : base(logger)
        {

        }
    }
}
