using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Apliu.Standard.Tools
{
    public class JsonHelper
    {
        /// <summary>
        /// 将json转成url参数
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string JsonConvertUrl(string json)
        {
            string urlparams = string.Empty;
            JArray jo = (JArray)JsonConvert.DeserializeObject(json);
            foreach (JToken child in jo)
            {
                JProperty jPro = child as JProperty;
                if (!string.IsNullOrEmpty(urlparams)) urlparams += "&";
                urlparams += jPro.Name + "=" + jPro.Value;
            }
            return urlparams;
        }

        /// <summary>
        /// 将实体类转换成XML格式的字符串
        /// </summary>
        /// <typeparam name="EveryType"></typeparam>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static String XmlSerialize<EveryType>(EveryType objType)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //ns.Add("", "");
                XmlSerializer serializer = new XmlSerializer(objType.GetType());
                serializer.Serialize(sw, objType, ns);
                sw.Close();
                String strXml = sw.ToString();
                return strXml;
            }
        }

        /// <summary>
        /// 将XML格式的字符串转换换成实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public static EveryType XmlDeserialize<EveryType>(String strXML) where EveryType : class
        {
            try
            {
                using (StringReader sr = new StringReader(strXML))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(EveryType));
                    return serializer.Deserialize(sr) as EveryType;
                }
            }
            catch (Exception)
            {
                return default(EveryType);
            }
        }

        /// <summary>
        /// Json序列化
        /// </summary>
        /// <typeparam name="EveryType"></typeparam>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static String JsonSerialize<EveryType>(EveryType objType)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string Json = JsonConvert.SerializeObject(objType, setting);
            return Json;
        }

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="EveryType"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static EveryType JsonDeserialize<EveryType>(String strJson)
        {
            return JsonConvert.DeserializeObject<EveryType>(strJson);
        }
        /// <summary>
        /// Json数组转实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static List<T> JsonDeserializeList<T>(string strJson)
        {
            List<T> objs = JsonConvert.DeserializeObject<List<T>>(strJson);
            return objs;
        }

        /// <summary>  
        /// 把对象序列化为字节数组  
        /// </summary>  
        public static byte[] SerializeObject(object obj)
        {
            if (obj == null)
                return null;
            //内存实例
            MemoryStream ms = new MemoryStream();
            //创建序列化的实例
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);//序列化对象，写入ms流中  
            ms.Position = 0;
            //byte[] bytes = new byte[ms.Length];//这个有错误
            byte[] bytes = ms.GetBuffer();
            ms.Read(bytes, 0, bytes.Length);
            ms.Close();
            return bytes;
        }

        /// <summary>  
        /// 把字节数组反序列化成对象  
        /// </summary>  
        public static object DeserializeObject(byte[] bytes)
        {
            object obj = null;
            if (bytes == null)
                return obj;
            //利用传来的byte[]创建一个内存流
            MemoryStream ms = new MemoryStream(bytes)
            {
                Position = 0
            };
            BinaryFormatter formatter = new BinaryFormatter();
            obj = formatter.Deserialize(ms);//把内存流反序列成对象  
            ms.Close();
            return obj;
        }
        /// <summary>
        /// 把字典序列化
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static byte[] SerializeDic(Dictionary<string, object> dic)
        {
            if (dic.Count == 0)
                return null;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, dic);//把字典序列化成流

            byte[] bytes = new byte[ms.Length];//从流中读出byte[]
            ms.Read(bytes, 0, bytes.Length);

            return bytes;
        }
        /// <summary>
        /// 反序列化返回字典
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Dictionary<string, object> DeserializeDic(byte[] bytes)
        {
            Dictionary<string, object> dic = null;
            if (bytes == null)
                return dic;
            //利用传来的byte[]创建一个内存流
            MemoryStream ms = new MemoryStream(bytes)
            {
                Position = 0
            };
            BinaryFormatter formatter = new BinaryFormatter();
            //把流中转换为Dictionary
            dic = (Dictionary<string, object>)formatter.Deserialize(ms);
            return dic;
        }
    }
}
