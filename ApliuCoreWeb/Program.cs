using ApliuCoreWeb.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ApliuCoreWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Apliu.Standard.Tools.Logger.WriteLogWeb(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(AppsettingJson.GetuUserDefinedSetting("UseUrls"))
                .UseStartup<Startup>();
    }
}
