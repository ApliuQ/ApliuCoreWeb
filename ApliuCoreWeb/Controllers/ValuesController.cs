using System;
using System.Data;
using ApliuCoreWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApliuCoreWeb.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public string Get()
        {
            String result = String.Empty;
            Apliu.Core.Database.DatabaseType databaseType = (Apliu.Core.Database.DatabaseType)Enum.Parse(typeof(Apliu.Core.Database.DatabaseType), AppsettingJson.GetuUserDefinedSetting("DatabaseType")); 
            UserConnectionString userConnectionString = AppsettingJson.GetSetting<UserConnectionString>("ConnectionString", "userdefined.json");
            String databaseConnection = String.Empty;
            switch (databaseType)
            {
                case Apliu.Core.Database.DatabaseType.SqlServer:
                    databaseConnection = userConnectionString.SqlServer;
                    break;
                case Apliu.Core.Database.DatabaseType.Oracle:
                    databaseConnection = userConnectionString.Oracle;
                    break;
                case Apliu.Core.Database.DatabaseType.MySql:
                    databaseConnection = userConnectionString.MySql;
                    break;
                case Apliu.Core.Database.DatabaseType.OleDb:
                    databaseConnection = userConnectionString.OleDb;
                    break;
                default:
                    break;
            }

            Apliu.Core.Database.DatabaseHelper databaseHelper = 
                new Apliu.Core.Database.DatabaseHelper(databaseType, databaseConnection);

            string sql01 = "insert into test (ID,NAME) values('" + Guid.NewGuid().ToString().ToLower() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
            int p1 = databaseHelper.PostData(sql01);

            DataSet ds = databaseHelper.GetData("select * from test ");
            if (ds != null && ds.Tables.Count > 0)
            {
                JsonSerializerSettings setting = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                result = JsonConvert.SerializeObject(ds.Tables[0], setting);
            }

            return result;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
