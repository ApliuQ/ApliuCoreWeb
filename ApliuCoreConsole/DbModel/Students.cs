using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApliuCoreConsole.DbModel
{
    public partial class Students
    {
        [StringLength(50)]
        public string ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Content { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }
    }
}
