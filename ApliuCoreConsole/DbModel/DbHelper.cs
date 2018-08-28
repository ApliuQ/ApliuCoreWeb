using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ApliuCoreConsole.DbModel
{
    public partial class DbHelper : DbContext
    {
        public DbHelper() { }

        public virtual DbSet<Students> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=APLIUDELL\SQLEXPRESS;initial catalog=ApliuWeb;integrated security=True;min pool size=1;MultipleActiveResultSets=True;");
        }
    }
}
