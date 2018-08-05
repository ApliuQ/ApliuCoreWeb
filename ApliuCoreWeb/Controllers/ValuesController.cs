using ApliuCoreWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data;

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
            String result=String.Empty;
            String databaseConnection = AppsettingJson.GetSetting("ConnectionString").ToString();
            ApliuCoreDatabase.DatabaseHelper databaseHelper = new ApliuCoreDatabase.DatabaseHelper(databaseConnection);

            string sql01 = "insert into test values('" + Guid.NewGuid().ToString().ToLower() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
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

            //  <DatabaseConnection>Data Source=APLIUDELL\SQLEXPRESS;Database=ApliuWeb;User ID=sa;Password=sa</DatabaseConnection>
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
