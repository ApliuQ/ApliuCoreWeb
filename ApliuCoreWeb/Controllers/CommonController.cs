using Apliu.Standard.Tools;
using Apliu.Standard.Tools.Web;
using ApliuCoreWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace ApliuCoreWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        [HttpPost]
        public string ChangePassword()
        {
            string username = HttpContext.Request.Form["username"];
            string smscode = HttpContext.Request.Form["smscode"];
            string password = HttpContext.Request.Form["password"];
            string passwordag = HttpContext.Request.Form["passwordag"];
            ResponseMessage result = new ResponseMessage
            {
                code = "-1",
                msg = "发生异常"
            };

            if (string.IsNullOrEmpty(username))
            {
                result.msg = "用户名不能为空";
                return result.ToString();
            }
            if (!(HttpContext.Session.GetValue(CodeType.ChangePassword.ToString()) is CodeCase cc) || (DateTimeHelper.DataTimeNow - cc.CreateTime).Seconds > cc.Timeout)
            {
                result.msg = "请重新获取短信验证码";
                return result.ToString();
            }
            if (cc.User != username || cc.Code != smscode)
            {
                result.msg = "短信验证码错误";
                return result.ToString();
            }
            if (string.IsNullOrEmpty(password))
            {
                result.msg = "密码不能为空";
                return result.ToString();
            }
            if (password.Length < 3)
            {
                result.msg = "密码长度必须大于等于3";
                return result.ToString();
            }
            if (string.IsNullOrEmpty(passwordag))
            {
                result.msg = "请再次输入密码";
                return result.ToString();
            }
            if (password != passwordag)
            {
                result.msg = "两次密码输入不一致";
                return result.ToString();
            }

            string id = Guid.NewGuid().ToString().ToUpper();
            string regs = string.Format(@"update ApUserInfo set Password='{1}' where UserId='{0}';", username, SecurityHelper.MD5Encrypt(password, Encoding.UTF8));
            if (DataAccess.Instance.PostData(regs))
            {
                result.code = "0";
                result.msg = "修改成功";
            }
            else
            {
                result.msg = "修改失败";
            }

            return result.ToString();
        }

        [HttpPost]
        public string Register()
        {
            string username = HttpContext.Request.Form["username"];
            string smscode = HttpContext.Request.Form["smscode"];
            string password = HttpContext.Request.Form["password"];
            string passwordag = HttpContext.Request.Form["passwordag"];
            ResponseMessage result = new ResponseMessage
            {
                code = "-1",
                msg = "发生异常"
            };

            if (string.IsNullOrEmpty(username))
            {
                result.msg = "用户名不能为空";
                return result.ToString();
            }
            if (!(HttpContext.Session.GetValue(CodeType.Register.ToString()) is CodeCase cc) || (DateTimeHelper.DataTimeNow - cc.CreateTime).Seconds > cc.Timeout)
            {
                result.msg = "请重新获取短信验证码";
                return result.ToString();
            }
            if (cc.User != username || cc.Code != smscode)
            {
                result.msg = "短信验证码错误";
                return result.ToString();
            }
            if (string.IsNullOrEmpty(password))
            {
                result.msg = "密码不能为空";
                return result.ToString();
            }
            if (password.Length < 3)
            {
                result.msg = "密码长度必须大于等于3";
                return result.ToString();
            }
            if (string.IsNullOrEmpty(passwordag))
            {
                result.msg = "请再次输入密码";
                return result.ToString();
            }
            if (password != passwordag)
            {
                result.msg = "两次密码输入不一致";
                return result.ToString();
            }

            if (!UserInfo.UserCheck(username))
            {
                result.msg = "该用户已注册";
                return result.ToString();
            }

            string id = Guid.NewGuid().ToString().ToUpper();
            string regs = string.Format(@"insert into ApUserInfo (ID,UserId,UserName,RealName,MobileNumber,UserType,Password ,CreateTime,Project,IsLocked)
     VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}');", id, username, username, "", username, "Normal", SecurityHelper.MD5Encrypt(password, Encoding.UTF8), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "ApliuWeb", "0");
            if (DataAccess.Instance.PostData(regs))
            {
                result.code = "0";
                result.msg = "注册成功";
            }
            else
            {
                result.msg = "注册失败";
            }

            return result.ToString();
        }

        [HttpPost]
        public string Login()
        {
            string username = HttpContext.Request.Form["username"];
            string password = HttpContext.Request.Form["password"];
            ResponseMessage result = new ResponseMessage
            {
                code = "-1",
                msg = "发生异常"
            };

            if (string.IsNullOrEmpty(username))
            {
                result.msg = "用户名不能为空";
                return result.ToString();
            }
            if (string.IsNullOrEmpty(password))
            {
                result.msg = "密码不能为空";
                return result.ToString();
            }

            if (UserInfo.LoginCheck(username, password))
            {
                HttpContext.Session.SetUserInfo(username, username, username, "", "");
                result.code = "0";
                result.msg = "登录成功";
            }
            else
            {
                result.msg = "用户名或密码错误";
            }

            return result.ToString();
        }

        [HttpGet]
        public new string User()
        {
            ResponseMessage result = new ResponseMessage
            {
                code = "-1",
                msg = "用户未登录"
            };
            string userid = HttpContext.Session.GetUserInfo()?.UserId;
            string username = HttpContext.Session.GetUserInfo()?.UserName;
            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(username))
            {
                result.code = "0";
                result.msg = username;
            }
            return result.ToString();
        }

        [HttpPost]
        public string Logout()
        {
            ResponseMessage result = new ResponseMessage
            {
                code = "0",
                msg = ""
            };
            HttpContext.Session.Logout();
            return result.ToString();
        }

        [HttpPost]
        public string SetGameData()
        {
            ResponseMessage result = new ResponseMessage
            {
                code = "-1",
                msg = "设置失败"
            };

            string gamename = HttpContext.Request.Form["gamename"];
            string score = HttpContext.Request.Form["score"];
            string usetime = HttpContext.Request.Form["usetime"];
            string stage = HttpContext.Request.Form["stage"];

            string userid = HttpContext.Session.GetUserInfo()?.UserId;
            if (string.IsNullOrEmpty(userid))
            {
                result.code = "1";
                result.msg = "用户未登录";
                if (stage.ToInt() > 6) userid = "Everyone";
                else return result.ToString();
            }

            if (string.IsNullOrEmpty(gamename) || string.IsNullOrEmpty(score) || string.IsNullOrEmpty(usetime) || string.IsNullOrEmpty(stage))
            {
                result.msg = "用户输入非法";
                return result.ToString();
            }

            string id = Guid.NewGuid().ToString().ToUpper();
            string insertgame = string.Format(@"INSERT INTO GameData (ID ,UserId ,GameName ,Score,Stage ,UseTime
,Remark,CreateTime, IP)  VALUES ('{0}','{1}','{2}',{3},{4},{5},'{6}','{7}','{8}')",
id, userid, gamename, score, stage, usetime, "",
DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), HttpContext.GetClientIP());
            if (DataAccess.Instance.PostData(insertgame))
            {
                if (result.code == "-1") result.code = "0";
                result.msg = "保存成绩成功";
            }
            return result.ToString();
        }
    }
}
