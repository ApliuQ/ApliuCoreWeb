using Apliu.Standard.Tools.Web;
using System;
using System.Xml;

namespace ApliuCoreWeb.Config
{
    public class SiteConfig
    {
        private static SiteConfig config = new SiteConfig();

        public static SiteConfig Instance
        {
            get
            {
                //如果当前配置文件的修改时间小于上次的修改时间，则认为配置文件已经被修改，需重新加载
                if (configFileModifyDate < System.IO.File.GetLastWriteTime(configFilePath))
                    LoadConfig();

                return config;
            }
        }

        private readonly static string configFilePath = ServerInfo.SitePath + @"\Config\BaseConfig.config";
        private static DateTime configFileModifyDate = DateTime.Now; //配置文件最后一次修改时间

        #region ==属性字段==
        private string _Domain;
        /// <summary>
        /// 网站域名 格式 https://www.xxxxxx.com/
        /// </summary>
        public string Domain
        {
            get
            {
                if (_Domain.EndsWith("/"))
                    return _Domain;

                return _Domain + "/";
            }
        }

        private string _DatabaseConnection;
        /// <summary>
        /// SQL Server数据库链接字符串
        /// </summary>
        public string DatabaseConnection
        {
            get { return _DatabaseConnection; }
        }

        private string _DatabaseType;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DatabaseType
        {
            get { return _DatabaseType; }
        }

        private string _TcAppId;
        /// <summary>
        /// 腾讯云账号的APPID
        /// </summary>
        public string TcAppId
        {
            get { return _TcAppId; }
        }

        private string _TcSecretId;
        /// <summary>
        /// 腾讯云API密钥上申请的标识身份的 TcSecretId
        /// </summary>
        public string TcSecretId
        {
            get { return _TcSecretId; }
        }

        private string _TcSecretKey;
        /// <summary>
        /// TcSecretId 对应唯一的 TcSecretKey , 而 TcSecretKey 会用来生成请求签名 Signature
        /// </summary>
        public string TcSecretKey
        {
            get { return _TcSecretKey; }
        }
        #endregion

        /// <summary>
        /// 加载配置信息
        /// </summary>
        public static void LoadConfig()
        {
            config = new SiteConfig();
            XmlDocument doc = new XmlDocument();
            doc.Load(configFilePath);

            XmlNode node = null;
            node = doc.SelectSingleNode("//configuration/Domain");
            if (node != null)
            {
                config._Domain = node.InnerText.Trim();
            }

            node = doc.SelectSingleNode("//configuration/DatabaseType");
            if (node != null)
            {
                config._DatabaseType = node.InnerText.Trim();
            }

            node = doc.SelectSingleNode("//configuration/DatabaseConnection");
            if (node != null)
            {
                config._DatabaseConnection = node.InnerText.Trim();
            }

            node = doc.SelectSingleNode("//configuration/appSettings/TcAppId");
            if (node != null)
            {
                config._TcAppId = node.InnerText.Trim();
            }

            node = doc.SelectSingleNode("//configuration/appSettings/TcSecretId");
            if (node != null)
            {
                config._TcSecretId = node.InnerText.Trim();
            }

            node = doc.SelectSingleNode("//configuration/appSettings/TcSecretKey");
            if (node != null)
            {
                config._TcSecretKey = node.InnerText.Trim();
            }
            configFileModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 获取BaseConfig.config文件appSettings中指定节点的值
        /// </summary>
        /// <param name="NodeName"></param>
        /// <returns></returns>
        public static string GetConfigAppSettingsValue(string NodeName)
        {
            string value = null;
            XmlDocument doc = new XmlDocument();
            doc.Load(configFilePath);

            XmlNode node = doc.SelectSingleNode("//configuration/appSettings");

            XmlElement xElem = (XmlElement)node.SelectSingleNode("//add[@key='" + NodeName + "']");

            if (xElem != null)
            {
                value = xElem.GetAttribute("value");
            }
            return value;
        }

        /// <summary>
        /// 获取BaseConfig.config文件指定根节点的值
        /// </summary>
        /// <param name="NodeName"></param>
        /// <returns></returns>
        public static string GetConfigRootValue(string NodeName)
        {
            string value = null;
            XmlDocument doc = new XmlDocument();
            doc.Load(configFilePath);

            XmlNode node = doc.SelectSingleNode("//configuration/" + NodeName);
            if (node != null)
            {
                value = node.InnerText.Trim();
            }
            return value;
        }
    }
}
