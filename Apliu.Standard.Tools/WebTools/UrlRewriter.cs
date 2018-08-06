using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ApliuTools.Web
{
    public class UrlRewriter
    {
        public static void Rewrite(HttpContext context)
        {
            string requestPath = context.Request.Path.ToLower();
            //主站通用重写部分
            Dictionary<string, SiteUrl> urls = GetSiteUrls();
            foreach (SiteUrl url in urls.Values)
            {
                if (Regex.IsMatch(requestPath, url.Pattern))
                {
                    context.RewritePath(Regex.Replace(requestPath, url.Pattern, url.ToPage));
                }
            }
        }

        protected static Dictionary<string, SiteUrl> GetSiteUrls()
        {
            #region ==code==
            Dictionary<string, SiteUrl> urls = new Dictionary<string, SiteUrl>();
            SiteUrl url = null;

            url = new SiteUrl();
            url.Name = "helptopic";
            url.Description = "帮助文档文章URL";
            url.Path = "helptopic-{0}.aspx";
            url.Pattern = @"helptopic-(\d+).aspx";
            url.ToPage = "helptopic.aspx?articleid=$1";
            urls.Add(url.Name, url);

            url = new SiteUrl();
            url.Name = "newstopic";
            url.Description = "最新动态文章URL";
            url.Path = "newstopic-{0}.aspx";
            url.Pattern = @"newstopic-(\d+).aspx";
            url.ToPage = "newstopic.aspx?articleid=$1";
            urls.Add(url.Name, url);

            //newstopic.aspx
            return urls;
            #endregion
        }
    }

    public class SiteUrl
    {
        private string _name = string.Empty;
        /// <summary>
        /// url名称，名称必须唯一
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _description = string.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _path = string.Empty;
        /// <summary>
        /// 路径
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private string _pattern = string.Empty;
        /// <summary>
        /// 正则
        /// </summary>
        public string Pattern
        {
            get { return _pattern; }
            set { _pattern = value; }
        }

        private string _topage = string.Empty;
        /// <summary>
        /// 跳转页面
        /// </summary>
        public string ToPage
        {
            get { return _topage; }
            set { _topage = value; }
        }
    }
}
