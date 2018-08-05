using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ApliuCoreWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://0.0.0.0:80")//准备转到core 2.1 使用https
                .UseStartup<Startup>()
                .Build();
    }
}
