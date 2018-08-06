using System;

namespace Apliu.Standard.Tools
{
    public static class DataConvert
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <param name="decimals">保留小数位（四舍五入）</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string stringtemp, int decimals)
        {
            decimal temp = 0;
            decimal.TryParse(stringtemp, out temp);

            temp = Math.Round(temp, decimals);

            return temp;
        }

        /// <summary>
        /// 转换为decimal，如果不能转换，返回NULL
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static decimal? ToDecimalN(this string stringtemp)
        {
            decimal temp = 0;
            if (decimal.TryParse(stringtemp, out temp))
                return temp;
            else
                return null;
        }

        /// <summary>
        /// 转换为decimal，如果不能转换，返回0
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string stringtemp)
        {
            decimal temp = 0;
            decimal.TryParse(stringtemp, out temp);
            return temp;
        }

        /// <summary>
        /// 不能转换则返回0
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static int ToInt(this string stringtemp)
        {
            int temp = 0;
            int.TryParse(stringtemp, out temp);
            return temp;
        }

        /// <summary>
        /// 转换为Boolean，如果不能转换，返回false
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static Boolean ToBoolean(this string stringtemp)
        {
            Boolean temp = false;
            Boolean.TryParse(stringtemp, out temp);
            return temp;
        }

        /// <summary>
        /// 不能转换则返回0
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static Byte ToByte(this string stringtemp)
        {
            Byte temp = 0;
            Byte.TryParse(stringtemp, out temp);
            return temp;
        }

        /// <summary>
        /// 不能转换则返回0
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static double ToDouble(this string stringtemp)
        {
            double temp = 0;
            double.TryParse(stringtemp, out temp);
            return temp;
        }

        /// <summary>
        /// 将字符串转换为日期，不符合的返回NULL
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeN(this string stringtemp)
        {
            DateTime temp = new DateTime();
            if (DateTime.TryParse(stringtemp, out temp))//如果能够转换成功
                return temp;
            else
                return null;
        }

        /// <summary>
        /// 将字符串转换为日期，不符合的转换为1900
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string stringtemp)
        {
            DateTime temp = new DateTime(1900, 1, 1);
            DateTime.TryParse(stringtemp, out temp);
            return temp;
        }
    }
}
