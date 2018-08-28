using Apliu.Standard.Tools;
using log4net;
using System;

namespace ApliuCoreWeb.Models
{
    public class Common
    {
        /// <summary>
        /// 程序跟目录 需要先初始化
        /// </summary>
        private static string _RootDirectory = String.Empty;
        public static string RootDirectory
        {
            get
            {
                if (String.IsNullOrEmpty(_RootDirectory))
                {
                    throw new Exception("程序跟目录未初始化");
                }
                else return _RootDirectory;
            }
            set
            {
                _RootDirectory = value;
                //同时初始化日志路径
                Logger.RootDirectory = _RootDirectory;
            }
        }

        /// <summary>
        /// Session加密秘钥
        /// </summary>
        private static readonly string SessionSecurityKey = ConfigurationJson.Appsettings.SessionSecurityKey;

        /// <summary>
        /// Log4Net日志实例
        /// </summary>
        public static ILog Log4Net { get; set; }

        #region 身份证号验证
        public static bool CheckIDCard(string idNumber)
        {
            if (idNumber.Length == 18)
            {
                bool check = CheckIDCard18(idNumber);
                return check;
            }
            else if (idNumber.Length == 15)
            {
                bool check = CheckIDCard15(idNumber);
                return check;
            }
            else return false;
        }
        private static bool CheckIDCard18(string idNumber)
        {
            if (long.TryParse(idNumber.Remove(17), out long n) == false || n < Math.Pow(10, 16)
                || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证  
            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");

            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');

            char[] Ai = idNumber.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
            {
                return false;//校验码验证  
            }
            return true;//符合GB11643-1999标准  
        }
        private static bool CheckIDCard15(string idNumber)
        {
            if (long.TryParse(idNumber, out long n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证  
            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 6).Insert(4, "-").Insert(2, "-");

            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            return true;
        }
        #endregion

        //手机号校验
        public static bool CheckMobileNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number)) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(number, @"^[1]+\d{10}");
        }

        /// <summary>
        /// 解析身份证
        /// </summary>
        /// <param name="CertId"></param>
        /// <param name="Birthday"></param>
        /// <param name="Sex"></param>
        /// <param name="Age"></param>
        /// <returns></returns>
        public static bool CertIdTryParse(string CertId, out string Birthday, out string Sex, out int Age)
        {
            Birthday = string.Empty; Sex = string.Empty; Age = 0;
            if (CertId.Length == 15)
            {
                Birthday = "19" + CertId.Substring(6, 2) + "-" + CertId.Substring(8, 2) + "-" + CertId.Substring(10, 2);

                TimeSpan ts = DateTime.Now.Subtract(Convert.ToDateTime(Birthday));
                int.TryParse(CertId.Substring(CertId.Length - 3, 1), out int tempsex);
                if (tempsex == 0)
                {
                    Sex = "1";
                }
                else if (tempsex == 1)
                {
                    Sex = "2";
                }

                Age = ts.Days / 365;
                return true;
            }
            else if (CertId.Length == 18)
            {
                Birthday = CertId.Substring(6, 4) + "-" + CertId.Substring(10, 2) + "-" + CertId.Substring(12, 2);

                string Sub_str = CertId.Substring(6, 8).Insert(4, "-").Insert(7, "-");
                TimeSpan ts = DateTime.Now.Subtract(Convert.ToDateTime(Sub_str));
                int.TryParse(CertId.Substring(CertId.Length - 3, 1), out int tempsex);
                if (tempsex == 0)
                {
                    Sex = "1";
                }
                else if (tempsex == 1)
                {
                    Sex = "2";
                }

                Age = ts.Days / 365;
                return true;
            }
            return false;
        }
    }
}