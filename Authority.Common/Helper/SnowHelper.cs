using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Common.Helper
{
  public  class SnowHelper
    {
        private static long UnionId { get;  } = 1;

        private static readonly IdWorker worker = new IdWorker(UnionId,1,10);


        public static long GetSnowId() 
        {
            try
            {
                return (worker.NextId());
            }
            catch (Exception ex) {

                Console.WriteLine(ex.ToString());
                return 0;

            }
        }


    }
}
