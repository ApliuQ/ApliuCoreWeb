using Apliu.Standard.ORM;
using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace ApliuCoreConsole
{
    class Program
    {
        static void Run()
        {
            ModelClass modelClass = new ModelClass()
            {
                Id = Guid.NewGuid().ToString().ToUpper(),
                Name = "modelname",
                Count = 8
            };
            String insertSql = modelClass.GetInsertSql();
            String updateSql = modelClass.GetUpdateSql();
            String deleteSql = modelClass.GetDeleteSql();

            Console.WriteLine(insertSql);
            Console.WriteLine(updateSql);
            Console.WriteLine(deleteSql);
            return;
            String ddd = HttpGet("http://apliu.xyz/api/wx");
            String dddA = HttpGetA("http://apliu.xyz/api/wx");
            Console.WriteLine("HttpWebRequest:" + ddd);
            Console.WriteLine("HttpClient:" + dddA);
        }

        static void Main(string[] args)
        {
            //DatabaseType databaseType = DatabaseType.SqlServer;
            //String databaseConnection = "Data Source=140.143.5.141;Initial Catalog=ApliuWeb;User ID=sa;Password=apliu@2018";

            //DatabaseHelper databaseHelper = new DatabaseHelper(databaseType, databaseConnection);

            //string sql01 = "insert into test (ID,NAME) values('" + Guid.NewGuid().ToString().ToLower() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
            //int p1 = databaseHelper.PostData(sql01);

            //DataSet ds = databaseHelper.GetData("select * from test ");

            Console.WriteLine("Apliu Core Console Hello World!");
            try
            {
                Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Run：" + ex.Message);
            }
            Console.ReadKey();
        }

        public static string HttpGetA(string getUrl)
        {
            string result = string.Empty;
            try
            {
                HttpClient httpClient = new HttpClient();

                var ddd = httpClient.GetAsync(getUrl).Result;
                result = ddd.Content.ReadAsStringAsync().Result;

            }
            catch (Exception)
            {
            }
            return result;
        }


        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="getUrl"></param>
        /// <returns></returns>
        public static string HttpGet(string getUrl)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(getUrl);
                wbRequest.Method = "GET";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
                    {
                        result = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}
