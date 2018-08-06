using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ApliuTools.Web
{
    /// <summary>
    /// 获取当前 HTTP 请求的 System.Web.HttpRequest Url对象
    /// </summary>
    public class QueryString
    {
        public QueryString() { }
        public string this[string queryName]
        {
            get
            {
                string queryValue = string.Empty;
                if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.Count > 0)
                {
                    queryValue = HttpContext.Current.Request.QueryString[queryName];
                }

                //过虑一些特殊的请求状态值,主要是一些有关页面视图状态的参数
                if (!HttpContextRequest.QueryNameIsKey(queryName))
                {
                    if (!string.IsNullOrEmpty(queryValue))
                    {
                        queryValue = HttpContextRequest.MyEncodeInputString(queryValue);
                    }
                }

                return queryValue;
            }
        }
    }

    /// <summary>
    /// 获取当前 HTTP 请求的 System.Web.HttpRequest Form对象
    /// </summary>
    public class Form
    {
        public Form() { }
        public string this[string queryName]
        {
            get
            {
                string queryValue = string.Empty;
                if (HttpContext.Current.Request.Form != null && HttpContext.Current.Request.Form.Count > 0)
                {
                    queryValue = HttpContext.Current.Request.Form[queryName];
                }

                //过虑一些特殊的请求状态值,主要是一些有关页面视图状态的参数
                if (!HttpContextRequest.QueryNameIsKey(queryName))
                {
                    if (!string.IsNullOrEmpty(queryValue))
                    {
                        queryValue = HttpContextRequest.MyEncodeInputString(queryValue);
                    }
                }

                return queryValue;
            }
        }
    }

    public static class HttpContextRequest
    {
        /// <summary>
        /// 获取当前 HTTP 请求的 System.Web.HttpRequest Url对象
        /// </summary>
        public static QueryString QueryString
        {
            get { return new QueryString(); }
        }

        /// <summary>
        /// 获取当前 HTTP 请求的 System.Web.HttpRequest Form对象
        /// </summary>
        public static Form Form
        {
            get { return new Form(); }
        }

        /// <summary>
        /// 过虑一些特殊的请求状态值,主要是一些有关页面视图状态的参数
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns></returns>
        public static bool QueryNameIsKey(string queryName)
        {
            List<string> NameKey = new List<string>() { "__VIEWSTATE", "__EVENTVALIDATION", "access_token", "editorcontent", "editApplication", "editMaterials", "editProcesses", "editQuestions" };
            foreach (string key in NameKey)
            {
                if (queryName == key)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 将sql关键字替换成全角字母
        /// </summary>
        /// <param name="queryValue"></param>
        /// <returns></returns>
        public static string MyEncodeInputString(string queryValue)
        {
            //有些参数固定的此项的两个值，所以不做过滤
            if (queryValue == "asc" || queryValue == "desc") return queryValue;
            string SqlStr = queryValue;

            SqlStr = ReplaceNoCase(SqlStr, "'", "＇");
            //SqlStr = ReplaceNoCase(SqlStr, "%", "％");
            SqlStr = ReplaceNoCase(SqlStr, ";", "；");
            SqlStr = ReplaceNoCase(SqlStr, "and", "ａnd");
            SqlStr = ReplaceNoCase(SqlStr, "or", "ｏr");
            SqlStr = ReplaceNoCase(SqlStr, "exec", "ｅxec");
            SqlStr = ReplaceNoCase(SqlStr, "execute", "ｅxecute");
            SqlStr = ReplaceNoCase(SqlStr, "insert", "ｉnsert");
            SqlStr = ReplaceNoCase(SqlStr, "select", "ｓelect");
            SqlStr = ReplaceNoCase(SqlStr, "delete", "ｄelete");
            SqlStr = ReplaceNoCase(SqlStr, "update", "ｕpdate");
            SqlStr = ReplaceNoCase(SqlStr, "alter", "ａlter");
            SqlStr = ReplaceNoCase(SqlStr, "create", "ｃreate");
            SqlStr = ReplaceNoCase(SqlStr, "drop", "ｄrop");
            SqlStr = ReplaceNoCase(SqlStr, "count", "ｃount");
            SqlStr = ReplaceNoCase(SqlStr, "\\*", "＊");
            SqlStr = ReplaceNoCase(SqlStr, "chr", "ｃhr");
            SqlStr = ReplaceNoCase(SqlStr, "char", "ｃhar");
            SqlStr = ReplaceNoCase(SqlStr, "asc", "ａsc");
            SqlStr = ReplaceNoCase(SqlStr, "mid", "ｍid");
            SqlStr = ReplaceNoCase(SqlStr, "substring", "ｓubstring");
            SqlStr = ReplaceNoCase(SqlStr, "master", "ｍaster");
            SqlStr = ReplaceNoCase(SqlStr, "truncate", "ｔruncate");
            SqlStr = ReplaceNoCase(SqlStr, "declare", "ｄeclare");
            SqlStr = ReplaceNoCase(SqlStr, "xp_cmdshell", "ｘp_cmdshell");
            SqlStr = ReplaceNoCase(SqlStr, "restore", "ｒestore");
            SqlStr = ReplaceNoCase(SqlStr, "backup", "ｂackup");
            SqlStr = ReplaceNoCase(SqlStr, "net", "ｎet");
            SqlStr = ReplaceNoCase(SqlStr, "user", "ｕser");
            SqlStr = ReplaceNoCase(SqlStr, "localgroup", "ｌocalgroup");

            return SqlStr;
        }

        private static string ReplaceNoCase(string replaceValue, string oldValue, string newValue)
        {
            return System.Text.RegularExpressions.Regex.Replace(replaceValue, oldValue, newValue, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }
    }
}
