using Authority.IRepository;
using Authority.Model.Model;
using Authority.repository.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Repository
{
   public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ILogger<BaseRepository<Product>> logger) : base(logger)
        {

        }
    }
}
