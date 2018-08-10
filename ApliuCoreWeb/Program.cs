using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApliuCoreWeb.Models;
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
                //.UseUrls(ApliuCoreWeb.Models.AppsettingJson.GetuUserDefinedSetting("UseUrls"))
                .UseKestrel(SetHostUrl)//启用Kestrel
                .UseStartup<Startup>();


        /// <summary>
        /// 配置Kestrel
        /// </summary>
        /// <param name="options"></param>
        private static void SetHostUrl(Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions options)
        {
            //依据Host类反序列化appsettings.json中指定节点
            var hostUrl = ConfigurationJson.HostUrl;
            foreach (var endpointKvp in hostUrl.Endpoints)
            {
                var endpointName = endpointKvp.Key;
                var endpoint = endpointKvp.Value;//获取appsettings.json的相关配置信息
                if (!endpoint.IsEnabled)
                {
                    continue;
                }

                var address = System.Net.IPAddress.Parse(endpoint.Address);
                options.Listen(address, endpoint.Port, opt =>
                {
                    if (endpoint.Certificate != null)//证书不为空使用UserHttps
                    {
                        if (endpoint.Certificate.Source != "File" || File.Exists(endpoint.Certificate.Path))
                        {
                            switch (endpoint.Certificate.Source)
                            {
                                case "File":
                                    opt.UseHttps(endpoint.Certificate.Path, endpoint.Certificate.Password);
                                    break;
                                default:
                                    throw new NotImplementedException($"文件 {endpoint.Certificate.Source}还没有实现");
                            }
                            //opt.UseConnectionLogging();
                        }
                    }
                });

                options.UseSystemd();
            }
        }
    }
}
