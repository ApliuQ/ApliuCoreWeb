using Apliu.Standard.Tools;
using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using System.Text;

namespace ApliuCoreWeb.Models
{
    /// <summary>
    /// 用户登录的Session对象
    /// </summary>
    [Serializable]
    public class UserSession
    {
        public string UserId;
        public string UserName;
        public string MobileNumber;
        public string Openid;
        public string Unionid;
        public string hubConnectionId;
    }

    public static class UserInfo
    {
        /// <summary>
        /// 登录成功后设置用户信息
        /// </summary>
        /// <param name="userSession"></param>
        public static void SetUserInfo(this ISession ISesson, UserSession userSession)
        {
            ISesson.SetValue("UserSession", userSession);
        }

        /// <summary>
        /// 登录成功后设置用户信息
        /// </summary>
        public static void SetUserInfo(this ISession ISesson, string UserId, string UserName, string MobileNumber, string Openid, string Unionid)
        {
            UserSession us = new UserSession();
            us.UserId = UserId;
            us.UserName = UserName;
            us.MobileNumber = MobileNumber;
            us.Openid = Openid;
            us.Unionid = Unionid;

            ISesson.SetValue("UserSession", us);
        }

        /// <summary>
        /// 获取当前登录的用户信息
        /// </summary>
        /// </summary>
        /// <returns></returns>
        public static UserSession GetUserInfo(this ISession ISesson)
        {
            return ISesson.GetValue("UserSession") as UserSession;
        }

        /// <summary>
        /// 退出 清空用户信息
        /// </summary>
        public static void Logout(this ISession ISesson)
        {
            ISesson.Remove("UserSession");
        }

        public static bool UserCheck(string UserId)
        {
            string regs = string.Format(@"select 1 from ApUserInfo where UserId='{0}';", UserId);
            DataSet ds = DataAccess.Instance.GetData(regs);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
            {
                return true;
            }
            else return false;
        }

        public static bool LoginCheck(string UserId, string Password)
        {
            string regs = string.Format(@"select 1 from ApUserInfo where UserId='{0}' and Password='{1}';", UserId, SecurityHelper.MD5Encrypt(Password, Encoding.UTF8));
            DataSet ds = DataAccess.Instance.GetData(regs);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                return true;
            }
            else return false;
        }
    }
}