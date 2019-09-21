using Authority.IRepository;
using Authority.Model.Model;
using Authority.repository.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Repository
{
   public class UserRepository: BaseRepository<User>, IUserRepository
    {

        public UserRepository(ILogger<BaseRepository<User>> logger) : base(logger)
        {

        }


    }
}
