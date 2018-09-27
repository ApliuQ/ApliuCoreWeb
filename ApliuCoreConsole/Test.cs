using Apliu.Core.Database;
using Apliu.Standard.ORM;
using Apliu.Standard.Tools;
using ApliuCoreConsole.DbModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ApliuCoreConsole
{
    public class Test
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Msg { get; set; }
        public String CreateTime { get; set; }

        public Test() { }
        public Test(String id, String name) { this.Id = id; this.Name = name; }
    }
    public static class RunFuction
    {
        public static void testFun(object obj)
        {
            // AutoResetEvent

            Console.WriteLine(string.Format("{0}:第{1}个线程", DateTime.Now.ToString(), obj.ToString()));
            Thread.Sleep(10000);
        }

        /// <summary>
        /// 小Z专属知识产权 @copyright zz
        /// </summary>
        /// <param name="n">a个数</param>
        /// <param name="m">z个数</param>
        /// <param name="x">第几个</param>
        /// <returns></returns>
        private static String RunAsResult(Int32 n, Int32 m, Int32 x)
        {
            StringBuilder mmm = new StringBuilder();
            for (int i = 0; i < m; i++) mmm.Append("1");
            StringBuilder nnn = new StringBuilder();
            for (int i = 0; i < n; i++) nnn.Append("0");
            Int32 min = Convert.ToInt32(mmm.ToString(), 2);
            Int32 max = Convert.ToInt32(mmm.ToString() + nnn.ToString(), 2);
            Int32 result = min, forX = 0;
            for (int i = min; i < max; i++)
            {
                if (Regex.Matches(Convert.ToString(i, 2), @"1").Count == m)
                {
                    result = i;
                    forX++;
                    //Console.WriteLine(Convert.ToString(result, 2).Replace("0", "a").Replace("1", "z").PadLeft(n + m, 'a'));
                }
                if (forX >= x) break;
            }
            return Convert.ToString(result, 2).Replace("0", "a").Replace("1", "z").PadLeft(n + m, 'a');
        }

        public static void EFCoreRun()
        {
            Students students = new Students()
            {
                ID = Guid.NewGuid().ToString().ToUpper(),
                Name = "students",
                Content = DateTime.Now.ToLongTimeString(),
                Remark = "Remark"
            };
            using (DbHelper dbHelper = new DbHelper())
            {
                //dbHelper.Database.BeginTransaction();
                //var tran = new TransactionScope();
                dbHelper.Students.Add(students);
                //dbHelper.Students.Remove(students);


                var p_name = new SqlParameter("@Name", "萝莉");
                var p_age = new SqlParameter("@Remark", "Remark");
                //如果使用的是MySql数据库 需要SqlParameter把替换为MySqlParameter
                //var p_name = new MySqlParameter("@name", "萝莉");
                //var p_age = new MySqlParameter("@age", 13);
                //更改学生年龄
                int stu01 = dbHelper.Database.ExecuteSqlCommand(@"UPDATE students
                                           SET Name = @Name
                                           WHERE Remark = @Remark;", p_age, p_name);

                dbHelper.SaveChanges();

                Students stu02 = (from t in dbHelper.Students where t.Remark == null select t).First();

                dbHelper.Entry<Students>(stu02).State = EntityState.Modified;
            }
        }

        public static void Run()
        {
            SecurityHelper.GenerateRSAKeys(out string publicKey, out String privateKey);
            String enc = SecurityHelper.RSAEncrypt(publicKey, "1111111111111");
            String dec = SecurityHelper.RSADecrypt(privateKey, enc);
            return;
            EFCoreRun();
            return;
            Console.WriteLine(RunAsResult(7, 7, 20));
            return;
            //ThreadPool.SetMinThreads(1, 1);
            //ThreadPool.SetMaxThreads(5, 5);
            for (int i = 1; i <= 10; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(testFun), i.ToString());
            }
            return;
            Console.WriteLine("开始获取");
            HttpClient httpClient = new HttpClient();
            try
            {
                Stream stream = httpClient.GetStreamAsync("https://t11.baidu.com/it/u=1895047213,2045297736&amp;fm=173&amp;app=25&amp;f=JPEG?w=218&amp;h=146&amp;s=20145D93D4AB10AF710181F10300E032").Result;
                Console.WriteLine("获取完成：" + stream.Length);

            }
            catch (Exception ex)
            {
                Console.WriteLine("获取完成：" + ex.Message);
            }

            return;

            ModelClass modelClass = new ModelClass()
            {
                Id = Guid.NewGuid().ToString().ToUpper(),
                Name = "modelname",
                Count = 8
            };
            String insertSql = new WeChatMsg().GetInsertSql();
            String updateSql = modelClass.GetUpdateSql();
            String deleteSql = modelClass.GetDeleteSql();

            Console.WriteLine(insertSql);
            Console.WriteLine(updateSql);
            Console.WriteLine(deleteSql);
            return;

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
            DataTable dataTable = new DataTable
            {
                TableName = "Test"
            };
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

            String ddd = HttpGet("http://apliu.xyz/api/wx");
            String dddA = HttpGetA("http://apliu.xyz/api/wx");
            Console.WriteLine("HttpWebRequest:" + ddd);
            Console.WriteLine("HttpClient:" + dddA);
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
