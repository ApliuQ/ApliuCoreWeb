using System;
using System.Collections.Generic;
using System.Text;

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
}
