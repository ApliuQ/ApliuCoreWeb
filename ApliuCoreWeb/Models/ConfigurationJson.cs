using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;

namespace ApliuCoreWeb.Models
{
    /// <summary>
    /// Json配置文件信息
    /// </summary>
    public class ConfigurationJson
    {
        /// <summary>
        /// 网站域名
        /// </summary>
        public static string Domain { get; set; }
        /// <summary>
        /// 业务数据库类型: SqlServer / Oracle / MySql
        /// </summary>
        public static string DatabaseType { get; set; }
        /// <summary>
        /// 业务数据库连接字符串
        /// </summary>
        public static string DatabaseConnection { get; set; }

        /// <summary>
        /// userdefined.json 中的Appsetting节点信息
        /// </summary>
        public static Appsettings Appsettings = new Appsettings();

        /// <summary>
        /// 静态对象锁
        /// </summary>
        private readonly static Object objectLock = new Object();
        private static HostUrl _HostUrl;
        /// <summary>
        /// 网站访问URL
        /// </summary>
        public static HostUrl HostUrl
        {
            get
            {
                if (_HostUrl == null)
                {
                    lock (objectLock)
                    {
                        if (_HostUrl == null)
                        {
                            _HostUrl = GetSetting<HostUrl>("HostUrl");
                        }
                    }
                }
                return _HostUrl;
            }
        }

        /// <summary>
        /// 是否启用Https 证书
        /// </summary>
        public static Boolean IsUseHttps
        {
            get
            {
                bool isUseHttps = false;
                foreach (var endpoint in HostUrl.Endpoints.Where(a => a.Value != null && a.Value.IsEnabled))
                {
                    if (endpoint.Value.Certificate != null)//证书不为空使用UserHttps
                    {
                        if (endpoint.Value.Certificate.Source != "File" || File.Exists(endpoint.Value.Certificate?.Path))
                        {
                            isUseHttps = true;
                            break;
                        }
                    }
                }
                return isUseHttps;
            }
        }

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public static void LoadConfig()
        {
            Appsettings = GetSetting<Appsettings>("Appsettings");
            Domain = GetSetting("Domain");
            DatabaseType = GetSetting("DatabaseType");
            DatabaseConnection = GetSetting("DatabaseConnection");
        }

        /// <summary>
        /// 设置并获取配置节点对象 var c =SetConfig<Cad>("Cad", (p => p.b = "123"));
        /// </summary>  
        public static T SetConfig<T>(string key, Action<T> action, string fileName = "userdefined.json") where T : class, new()
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
        public static T GetSetting<T>(string key, string fileName = "userdefined.json") where T : class, new()
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

        /// <summary>
        /// 获取指定Json文件中节点的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static String GetSetting(string key, string fileName = "userdefined.json")
        {
            IConfiguration config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .Add(new JsonConfigurationSource { Path = fileName, Optional = false, ReloadOnChange = true })
               .Build();
            String value = config.GetSection(key).Value?.ToString();

            return value;
        }
    }

    public class Appsettings
    {
        /// <summary>
        /// 演示数据库类型
        /// </summary>
        public string TesDatabaseTypet { get; set; }
        /// <summary>
        /// 演示数据库连接字符串
        /// </summary>
        public string TesDatabaseConnection { get; set; }
        /// <summary>
        /// Session加密密钥
        /// </summary>
        public string SessionSecurityKey { get; set; }
        /// <summary>
        /// 网站域名
        /// </summary>
        public string WxDomain { get; set; }
        /// <summary>
        /// 微信公众号Id
        /// </summary>
        public string WxAppId { get; set; }
        /// <summary>
        /// 微信公众号密钥
        /// </summary>
        public string WxAppSecret { get; set; }
        /// <summary>
        /// 微信公众号Token
        /// </summary>
        public string WxToken { get; set; }
        /// <summary>
        ///已过期，由微信公众号设置决定是否启用加密
        /// </summary>
        public string IsSecurity { get; set; }
        /// <summary>
        /// 微信公众号密文消息密钥
        /// </summary>
        public string WxEncodingAESKey { get; set; }
        /// <summary>
        /// 腾讯云应用Id
        /// </summary>
        public string TcAppId { get; set; }
        /// <summary>
        /// 腾讯云应用密钥Id
        /// </summary>
        public string TcSecretId { get; set; }
        /// <summary>
        /// 腾讯云应用密钥Key
        /// </summary>
        public string TcSecretKey { get; set; }
        /// <summary>
        /// SDK AppID是短信应用的唯一标识，调用短信API接口时需要提供该参数
        /// </summary>
        public string TcSMSAppId { get; set; }
        /// <summary>
        /// 用来校验短信发送请求合法性的密码，与SDK AppID对应
        /// </summary>
        public string TcSMSAppKey { get; set; }
    }

    public class UserConnectionString
    {
        public string SqlServer { get; set; }
        public string Oracle { get; set; }
        public string MySql { get; set; }
    }
}
