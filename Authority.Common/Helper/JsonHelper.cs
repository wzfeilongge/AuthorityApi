using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Authority.Common.Helper
{
    public static class JsonHelper
    {

        public static (string, string) GetDbConnection(string sqlTypePath, string SqlStrPath)
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();
            var sqlType = config[sqlTypePath];
            var sqlstr = config[SqlStrPath];
            return (sqlType, sqlstr);

        }










    }
}
