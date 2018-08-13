using Apliu.Core.Database;
using Apliu.Standard.ORM;
using Apliu.Standard.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace ApliuCoreConsole
{
    class Program
    {
        static void Run()
        {
            List<Test> list = new List<Test>() { new Test("3", "w"), new Test("1", "q"), new Test("1", null), new Test("2", "w") };


            var query = from a in list
                        let b = a.Name
                        group a by b into c
                        where c.Key is null
                        orderby c ascending
                        select c;

            var temp02 = from a in list
                         where a.Name is null
                         orderby a.Id ascending
                         select a;


            return;
            Thread thread01 = new Thread(exSql01);
            Thread thread02 = new Thread(exSql02);
            thread01.Start();
            thread02.Start();
            return;
            DatabaseType databaseType = DatabaseType.SqlServer;
            String databaseConnection = @"Data Source=APLIUDELL\SQLEXPRESS;Database=ApliuWeb;User ID=sa;Password=sa";
            DatabaseHelper databaseHelper = new DatabaseHelper(databaseType, databaseConnection);
            string sql01 = "insert into test (ID,NAME) values('" + Guid.NewGuid().ToString().ToLower() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
            databaseHelper.BeginTransaction(60);
            int p1 = databaseHelper.PostData(sql01);
            databaseHelper.Complete();

            DataSet ds = databaseHelper.GetData("select * from test where name like '%2018-08-13%'");
            return;
            DataTable dataTable = new DataTable();
            dataTable.TableName = "Test";
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("NAME");
            DataRow dataRow01 = dataTable.NewRow();
            dataRow01["ID"] = Guid.NewGuid().ToString().ToUpper();
            dataRow01["NAME"] = DateTimeHelper.DataTimeNow.ToString("yyyy-MM-dd HH:mm:ss");
            dataTable.Rows.Add(dataRow01);

            DataRow dataRow02 = dataTable.NewRow();
            dataRow02["ID"] = Guid.NewGuid().ToString().ToUpper();
            dataRow02["NAME"] = DateTimeHelper.DataTimeNow.ToString("yyyy-MM-dd HH:mm:ss");
            dataTable.Rows.Add(dataRow02);

            databaseHelper.BeginTransaction(60);
            int affected = databaseHelper.InsertTableAsync(dataTable, 60).Result;
            databaseHelper.Dispose();

            DataSet temp = databaseHelper.GetData("select * from test where name like '%2018-08-13%'");

            return;

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

        static void exSql01()
        {
            DatabaseType databaseType = DatabaseType.SqlServer;
            String databaseConnection = @"Data Source=APLIUDELL\SQLEXPRESS;Database=ApliuWeb;User ID=sa;Password=sa";
            DatabaseHelper databaseHelper = new DatabaseHelper(databaseType, databaseConnection);
            string sql01 = "insert into test (ID,NAME,MSG) values('" + Guid.NewGuid().ToString().ToLower() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','0001');";
            databaseHelper.BeginTransaction(60);
            int p1 = databaseHelper.PostData(sql01);
            Thread.Sleep(1000);
            databaseHelper.Complete();
        }
        static void exSql02()
        {
            DatabaseType databaseType = DatabaseType.SqlServer;
            String databaseConnection = @"Data Source=APLIUDELL\SQLEXPRESS;Database=ApliuWeb;User ID=sa;Password=sa";
            DatabaseHelper databaseHelper = new DatabaseHelper(databaseType, databaseConnection);
            string sql01 = "insert into test (ID,NAME,MSG) values('" + Guid.NewGuid().ToString().ToLower() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','0002');";
            databaseHelper.BeginTransaction(60);
            int p1 = databaseHelper.PostData(sql01);
            Thread.Sleep(1000);
            databaseHelper.Dispose();
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
