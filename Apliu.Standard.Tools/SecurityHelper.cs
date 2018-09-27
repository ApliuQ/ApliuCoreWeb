using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Apliu.Standard.Tools
{
    public class SecurityHelper
    {
        #region 哈希加密算法

        /// <summary>
        /// 哈希加密算法
        /// </summary>
        /// <param name="hashAlgorithm"> 所有加密哈希算法实现均必须从中派生的基类 </param>
        /// <param name="input"> 待加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        private static string HashEncrypt(HashAlgorithm hashAlgorithm, string input, Encoding encoding)
        {
            var data = hashAlgorithm.ComputeHash(encoding.GetBytes(input));

            return BitConverter.ToString(data).Replace("-", "");
        }

        /// <summary>
        /// 验证哈希值
        /// </summary>
        /// <param name="hashAlgorithm"> 所有加密哈希算法实现均必须从中派生的基类 </param>
        /// <param name="unhashedText"> 未加密的字符串 </param>
        /// <param name="hashedText"> 经过加密的哈希值 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        private static bool VerifyHashValue(HashAlgorithm hashAlgorithm, string unhashedText, string hashedText,
            Encoding encoding)
        {
            return string.Equals(HashEncrypt(hashAlgorithm, unhashedText, encoding), hashedText,
                StringComparison.OrdinalIgnoreCase);
        }

        #region MD5 算法

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="input"> 待加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static string MD5Encrypt(string input, Encoding encoding)
        {
            return HashEncrypt(MD5.Create(), input, encoding);
        }

        /// <summary>
        /// 验证 MD5 值
        /// </summary>
        /// <param name="input"> 未加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static bool VerifyMD5Value(string input, Encoding encoding)
        {
            return VerifyHashValue(MD5.Create(), input, MD5Encrypt(input, encoding), encoding);
        }

        #endregion MD5 算法

        #region SHA1 算法

        /// <summary>
        /// SHA1 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static string SHA1Encrypt(string input, Encoding encoding)
        {
            return HashEncrypt(SHA1.Create(), input, encoding);
        }

        /// <summary>
        /// 验证 SHA1 值
        /// </summary>
        /// <param name="input"> 未加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static bool VerifySHA1Value(string input, Encoding encoding)
        {
            return VerifyHashValue(SHA1.Create(), input, SHA1Encrypt(input, encoding), encoding);
        }

        #endregion SHA1 算法

        #region SHA256 算法

        /// <summary>
        /// SHA256 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static string SHA256Encrypt(string input, Encoding encoding)
        {
            return HashEncrypt(SHA256.Create(), input, encoding);
        }

        /// <summary>
        /// 验证 SHA256 值
        /// </summary>
        /// <param name="input"> 未加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static bool VerifySHA256Value(string input, Encoding encoding)
        {
            return VerifyHashValue(SHA256.Create(), input, SHA256Encrypt(input, encoding), encoding);
        }

        #endregion SHA256 算法

        #region SHA384 算法

        /// <summary>
        /// SHA384 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static string SHA384Encrypt(string input, Encoding encoding)
        {
            return HashEncrypt(SHA384.Create(), input, encoding);
        }

        /// <summary>
        /// 验证 SHA384 值
        /// </summary>
        /// <param name="input"> 未加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static bool VerifySHA384Value(string input, Encoding encoding)
        {
            return VerifyHashValue(SHA256.Create(), input, SHA384Encrypt(input, encoding), encoding);
        }

        #endregion SHA384 算法

        #region SHA512 算法

        /// <summary>
        /// SHA512 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static string SHA512Encrypt(string input, Encoding encoding)
        {
            return HashEncrypt(SHA512.Create(), input, encoding);
        }

        /// <summary>
        /// 验证 SHA512 值
        /// </summary>
        /// <param name="input"> 未加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static bool VerifySHA512Value(string input, Encoding encoding)
        {
            return VerifyHashValue(SHA512.Create(), input, SHA512Encrypt(input, encoding), encoding);
        }

        #endregion SHA512 算法

        #region HMAC-MD5 加密

        /// <summary>
        /// HMAC-MD5 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="key"> 密钥 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static string HMACSMD5Encrypt(string input, string key, Encoding encoding)
        {
            return HashEncrypt(new HMACMD5(encoding.GetBytes(key)), input, encoding);
        }

        #endregion HMAC-MD5 加密

        #region HMAC-SHA1 加密

        /// <summary>
        /// HMAC-SHA1 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="key"> 密钥 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static string HMACSHA1Encrypt(string input, string key, Encoding encoding)
        {
            return HashEncrypt(new HMACSHA1(encoding.GetBytes(key)), input, encoding);
        }

        #endregion HMAC-SHA1 加密

        #region HMAC-SHA256 加密

        /// <summary>
        /// HMAC-SHA256 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="key"> 密钥 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static string HMACSHA256Encrypt(string input, string key, Encoding encoding)
        {
            return HashEncrypt(new HMACSHA256(encoding.GetBytes(key)), input, encoding);
        }

        #endregion HMAC-SHA256 加密

        #region HMAC-SHA384 加密

        /// <summary>
        /// HMAC-SHA384 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="key"> 密钥 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static string HMACSHA384Encrypt(string input, string key, Encoding encoding)
        {
            return HashEncrypt(new HMACSHA384(encoding.GetBytes(key)), input, encoding);
        }

        #endregion HMAC-SHA384 加密

        #region HMAC-SHA512 加密

        /// <summary>
        /// HMAC-SHA512 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="key"> 密钥 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static string HMACSHA512Encrypt(string input, string key, Encoding encoding)
        {
            return HashEncrypt(new HMACSHA512(encoding.GetBytes(key)), input, encoding);
        }

        #endregion HMAC-SHA512 加密

        #endregion 哈希加密算法

        #region 对称加密算法

        #region Des 加解密

        private static readonly byte[] IvBytes = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="content"> 待加密的字符串 </param>
        /// <param name="skey">密钥（8位）</param>
        /// <returns></returns>
        public static string DESEncrypt(string encryptString, string skey)
        {
            try
            {
                if (String.IsNullOrEmpty(encryptString)) return encryptString;
                byte[] rgbKey = Encoding.UTF8.GetBytes(skey);                 //用于对称算法的初始化向量（默认值）。       
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, IvBytes), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="content"> 待解密的字符串 </param>
        /// <param name="key"> 密钥（8位） </param>
        /// <returns></returns>
        public static string DESDecrypt(string decryptString, string sKey)
        {
            try
            {
                if (String.IsNullOrEmpty(decryptString)) return decryptString;
                byte[] rgbKey = Encoding.UTF8.GetBytes(sKey);
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, IvBytes), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length); cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return decryptString;
            }
        }

        #endregion Des 加解密

        #endregion 对称加密算法

        #region 非对称加解密算法

        /// <summary>
        /// 生成 RSA 公钥和私钥
        /// </summary>
        /// <param name="publicKey"> 公钥 </param>
        /// <param name="privateKey"> 私钥 </param>
        public static void GenerateRSAKeys(out string publicKey, out string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                publicKey = rsa.ToXmlString(false);
                privateKey = rsa.ToXmlString(true);
            }
        }

        /// <summary>
        /// RSA 加密
        /// </summary>
        /// <param name="publickey"> 公钥 </param>
        /// <param name="content"> 待加密的内容 </param>
        /// <returns> 经过加密的字符串 </returns>
        public static string RSAEncrypt(string publickey, string content)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publickey);
            var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// RSA 解密
        /// </summary>
        /// <param name="privatekey"> 私钥 </param>
        /// <param name="content"> 待解密的内容 </param>
        /// <returns> 解密后的字符串 </returns>
        public static string RSADecrypt(string privatekey, string content)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privatekey);
            var cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);

            return Encoding.UTF8.GetString(cipherbytes);
        }

        #endregion 非对称加密算法

        #region Base64加解密算法
        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result)
        {
            return Base64Decode(Encoding.UTF8, result);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(Encoding encodeType, string result)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
        #endregion

        #region 网页请求加解密算法
        /// <summary>
        /// URL解密
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="Encoding"></param>
        /// <returns></returns>
        public static string UrlDecode(string Content, Encoding encoding)
        {
            return HttpUtility.UrlDecode(Content, encoding);
        }

        /// <summary>
        /// URL 加密
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="Encoding"></param>
        /// <returns></returns>
        public static string UrlEncode(string Content, Encoding encoding)
        {
            return HttpUtility.UrlEncode(Content, encoding);
        }

        /// <summary>
        /// HTML 解密
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string HtmlDecode(string Content)
        {
            return HttpUtility.HtmlDecode(Content);
        }

        /// <summary>
        /// HTML 加密
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string HtmlEncode(string Content)
        {
            return HttpUtility.HtmlEncode(Content);
        }
        #endregion

        public static string Base64FromImage(Image image)
        {
            string strbaser64 = String.Empty;
            try
            {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                strbaser64 = Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strbaser64;
        }

        /// <summary>
        /// 字符串转Unicode码
        /// </summary>
        /// <returns>The to unicode.</returns>
        /// <param name="value">Value.</param>
        public static string ToUnicode(string value)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(value);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                // 取两个字符，每个字符都是右对齐。
                stringBuilder.AppendFormat(@"\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <returns>The to string.</returns>
        /// <param name="unicode">Unicode.</param>
        public static string UnicodeTo(string unicode)
        {
            string resultStr = "";
            string[] strList = unicode.Replace(@"\u", "&&").Split("&&");
            for (int i = 1; i < strList.Length; i++)
            {
                resultStr += (char)int.Parse(strList[i], System.Globalization.NumberStyles.HexNumber);
            }
            return resultStr;
        }
    }
}