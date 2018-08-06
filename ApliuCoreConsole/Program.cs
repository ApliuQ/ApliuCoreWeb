using Apliu.Core.Database;
using Apliu.Standard.Tools;
using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace ApliuCoreConsole
{
    class Program
    {
        static void Run()
        {
            Logger.WriteLogDesktop("WriteLogDesktop");
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
            Run();
            Console.ReadKey();
        }
    }
}
