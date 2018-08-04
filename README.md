# ApliuCoreWeb
个人兴趣网站，包含手机端、微信公众号等内容以及小工具等等，演示地址：https://apliu.xyz，由ApliuWeb转型到asp.net Core框架


Linux 运行 dotnet core web

指令： dotnet restore
dotnet publish
dotnet run
dotnet xxxx.dll


1. Program.cs 方法添加[.UseUrls("http://0.0.0.0:8000")]，不然会出现只监听127.0.0.1，外网无法访问

public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
		.UseUrls("http://0.0.0.0:8000")
                .UseStartup<Startup>()
                .Build();

2. xxxxx.csproj项目文件添加上<PublishWithAspNetCoreTargetManifest>false</PublishWithAspNetCoreTargetManifest>，否则可能会出现错误：
Error:
  An assembly specified in the application dependencies manifest (*.*.deps.json) was not found:
    package: ‘Microsoft.AspNetCore.Antiforgery‘, version: ‘2.0.3‘
    path: ‘lib/netstandard2.0/Microsoft.AspNetCore.Antiforgery.dll‘
  This assembly was expected to be in the local runtime store as the application was published using the following target manifest files:
    aspnetcore-store-2.0.8.xml　

  <PropertyGroup>
    <TargetFramework>netcoreapp2.0</TargetFramework>
    <PublishWithAspNetCoreTargetManifest>false</PublishWithAspNetCoreTargetManifest>
  </PropertyGroup>

3. SSH工具推荐使用Bitvise SSH Client

4. 发布的时候注意点，稳妥起见
	部署模式：独立
	目标运行时选择对应的环境

框架依赖的可移植仍然摸索中...