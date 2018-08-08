using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApliuCoreWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Apliu.Standard.Tools.Logger.WriteLogWeb("开启Apliu Core Web服务");
            CreateWebHostBuilder(args).Build().Run();
            Apliu.Standard.Tools.Logger.WriteLogWeb("关闭Apliu Core Web服务");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(ApliuCoreWeb.Models.AppsettingJson.GetuUserDefinedSetting("UseUrls"))
                .UseStartup<Startup>();
    }
}
