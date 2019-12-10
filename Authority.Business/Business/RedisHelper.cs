using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Business.Business
{
   public  static class RedisHelper
    {

        public static RedisClient GetRedisClient()
        {

            return new RedisClient("127.0.0.1", 6379, "123456");

        }


    }
}
