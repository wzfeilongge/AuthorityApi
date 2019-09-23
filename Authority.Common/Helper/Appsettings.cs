using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Authority.Common.Helper
{
    public class Appsettings
    {
        private static IConfiguration Configuration
        {
            get;
            set;
        }

        static Appsettings()
        {
            string path = "appsettings.json";
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).Add(new JsonConfigurationSource
            {
                Path = path,
                Optional = false,
                ReloadOnChange = true
            }).Build();
        }

        public static string app(params string[] sections)
        {
            try
            {
                string text = string.Empty;
                for (int i = 0; i < sections.Length; i++)
                {
                    text = text + sections[i] + ":";
                }
                return Configuration[text.TrimEnd(':')];
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
