using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace ApliuCoreWeb.Models
{
    public class AppsettingJson
    {
        /// <summary>
        /// 设置并获取配置节点对象 var c =SetConfig<Cad>("Cad", (p => p.b = "123"));
        /// </summary>  
        public static T SetConfig<T>(string key, Action<T> action, string fileName = "appsettings.json") where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(fileName, optional: true, reloadOnChange: true)
               .Build();
            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .Configure<T>(action)
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return appconfig;
        }

        /// <summary>
        /// 获取配置节点对象 var result =GetSetting<Logging>("Logging");
        /// </summary>   
        public static T GetSetting<T>(string key, string fileName = "appsettings.json") where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .Add(new JsonConfigurationSource { Path = fileName, Optional = false, ReloadOnChange = true })
               .Build();
            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return appconfig;
        }

        public static String GetSetting(string key, string fileName = "appsettings.json")
        {
            IConfiguration config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .Add(new JsonConfigurationSource { Path = fileName, Optional = false, ReloadOnChange = true })
               .Build();
            String value = config.GetSection(key).Value?.ToString();

            return value;
        }
    }
}
