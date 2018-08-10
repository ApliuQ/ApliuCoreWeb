using Apliu.Standard.Tools;
using Apliu.Standard.Tools.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApliuCoreWeb.Models
{
    /// <summary>
    /// HostUrl Json配置访问URL
    /// </summary>
    public class HostUrl
    {
        /// <summary>
        /// appsettings.json字典
        /// </summary>
        public Dictionary<string, Endpoint> Endpoints { get; set; }
    }

    /// <summary>
    /// 终结点
    /// </summary>
    public class Endpoint
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 证书
        /// </summary>
        public Certificate Certificate { get; set; }
    }

    /// <summary>
    /// 证书类
    /// </summary>
    public class Certificate
    {
        /// <summary>
        /// 源
        /// </summary>
        public string Source { get; set; }

        private string _Path;
        /// <summary>
        /// 证书路径()
        /// </summary>
        public string Path
        {
            get
            {
                if (!String.IsNullOrEmpty(_Path?.Trim()))
                {
                    String pathLine = ServerInfo.SitePath + "/" + _Path;
                    return pathLine.ToLinuxOrWinPath();
                }
                else return _Path;
            }
            set { _Path = value; }
        }

        /// <summary>
        /// 证书密钥
        /// </summary>
        public string Password { get; set; }
    }
}
