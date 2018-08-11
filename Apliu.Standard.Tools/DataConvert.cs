using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
            decimal.TryParse(stringtemp, out var temp);
            temp = Math.Round(temp, decimals);
            return temp;
        }

        /// <summary>
        /// 转换为decimal，如果不能转换，返回0
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string stringtemp)
        {
            decimal.TryParse(stringtemp, out var temp);
            return temp;
        }

        /// <summary>
        /// 不能转换则返回0
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static int ToInt(this string stringtemp)
        {
            int.TryParse(stringtemp, out var temp);
            return temp;
        }

        /// <summary>
        /// 转换为Boolean，如果不能转换，返回false
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static Boolean ToBoolean(this string stringtemp)
        {
            Boolean.TryParse(stringtemp, out var temp);
            return temp;
        }

        /// <summary>
        /// 不能转换则返回0
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static Byte ToByte(this string stringtemp)
        {
            Byte.TryParse(stringtemp, out var temp);
            return temp;
        }

        /// <summary>
        /// 不能转换则返回0
        /// </summary>
        /// <param name="stringtemp"></param>
        /// <returns></returns>
        public static double ToDouble(this string stringtemp)
        {
            double.TryParse(stringtemp, out var temp);
            return temp;
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

        /// <summary>
        /// 字符串切割
        /// </summary>
        /// <param name="content"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static String[] Split(this string content, String separator)
        {
            string[] sArray = System.Text.RegularExpressions.Regex.Split(content, separator);
            return sArray;
        }

        /// <summary>
        /// 将文件路径更正成兼容Linux和Windows系统，主要因为Linux系统 '/' 可做文件夹名称
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static String ToLinuxOrWinPath(this String filePath)
        {
            if (String.IsNullOrEmpty(filePath)) return default;
            if (filePath.Contains(@":/") || filePath.Contains(@"./") || filePath.StartsWith(@"/")) return filePath.Replace(@"\", @"/");
            else if (filePath.Contains(@":\") || filePath.Contains(@".\") || filePath.StartsWith(@"/")) return filePath.Replace(@"/", @"\");
            else return filePath;
        }

        /// <summary>
        /// 在其他平台系统上写入文本并且换行
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task WriteLineAsyncOnLinuxOrWin(this StreamWriter streamWriter, String content)
        {
            await streamWriter.WriteAsync(content);
            await streamWriter.WriteAsync(Encoding.UTF8.GetChars(new byte[] { 13, 10 }));
        }

        /// <summary>
        /// 获取Bitmap字节流
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Byte[] GetBytes(this Bitmap bitmap)
        {
            Byte[] content = null;
            try
            {
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                content = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(content, 0, (int)ms.Length);
                ms.Close();
                ms.Dispose();
            }
            catch (Exception)
            {
            }
            return content;
        }
    }
}
