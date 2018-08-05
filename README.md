# ApliuCoreWeb
个人兴趣网站，包含手机端、微信公众号等内容以及小工具等等，演示地址：https://apliu.xyz，由ApliuWeb转型到asp.net Core框架

CentOS 安装 core 2
1.安装libicu依赖
	yum install libunwind libicu
2.添加dotnet product feed
	sudo rpm --import https://packages.microsoft.com/keys/microsoft.asc
	sudo sh -c 'echo -e "[packages-microsoft-com-prod]\nname=packages-microsoft-com-prod \nbaseurl= https://packages.microsoft.com/yumrepos/microsoft-rhel7.3-prod\nenabled=1\ngpgcheck=1\ngpgkey=https://packages.microsoft.com/keys/microsoft.asc" > /etc/yum.repos.d/dotnetdev.repo'
3.安装core SDK
	sudo yum update
	sudo yum install libunwind libicu
	sudo yum install dotnet-sdk-2.1.4 安装SDK环境

备注--sudo yum install dotnet-runtime-2.0.6 安装运行时环境或者sudo yum install aspnetcore-runtime-2.1.2

其他系统安装，请参照：https://www.microsoft.com/net/download/linux-package-manager/centos/runtime-current

切记：运行机器上的core版本一定要高于或等于发布机器上的版本，否则会出现错误，当然也可以使用独立模式部署，单独使用core运行时
"Error:
  An assembly specified in the application dependencies manifest (*.*.deps.json) was not found:
    package: ‘Microsoft.AspNetCore.Antiforgery‘, version: ‘2.0.3‘
    path: ‘lib/netstandard2.0/Microsoft.AspNetCore.Antiforgery.dll‘
  This assembly was expected to be in the local runtime store as the application was published using the following target manifest files:
    aspnetcore-store-2.0.8.xml　"


Linux 运行 dotnet core web

指令： dotnet restore
dotnet publish
dotnet run
dotnet xxxx.dll

为防止ssh工具界面关闭后导致服务停止，加上指令（xxxx为具体执行指令）：nohup xxxxxx &


1. Program.cs 方法添加[.UseUrls("http://0.0.0.0:8000")]，不然会出现只监听127.0.0.1，外网无法访问

public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
		.UseUrls("http://0.0.0.0:8000")
                .UseStartup<Startup>()
                .Build();

2. xxxxx.csproj项目文件添加上
"<PublishWithAspNetCoreTargetManifest>false</PublishWithAspNetCoreTargetManifest>"，否则可能会出现错误：
"Error:
  An assembly specified in the application dependencies manifest (*.*.deps.json) was not found:
    package: ‘Microsoft.AspNetCore.Antiforgery‘, version: ‘2.0.3‘
    path: ‘lib/netstandard2.0/Microsoft.AspNetCore.Antiforgery.dll‘
  This assembly was expected to be in the local runtime store as the application was published using the following target manifest files:
    aspnetcore-store-2.0.8.xml　"

 " <PropertyGroup>
    <TargetFramework>netcoreapp2.0</TargetFramework>
    <PublishWithAspNetCoreTargetManifest>false</PublishWithAspNetCoreTargetManifest>
  </PropertyGroup>"

3. SSH工具推荐使用Bitvise SSH Client

4. 发布的时候注意点，稳妥起见
	部署模式：独立
	目标运行时选择对应的环境

5. 框架依赖的可移植模式发布，发布目录仅仅包含项目文件而不包含运行时文件，所以出现错误，可能是需要进行其他配置才能启动。（出错原本是版本过低）