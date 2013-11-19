using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using WineProdTools.Data.EntityModels;
using WineProdTools.Data.Migrations;

namespace WineProdTools.Data
{
    internal class WineProdToolsContext : DbContext
    {
        public WineProdToolsContext() : base("name=DefaultConnection") { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WineProdToolsContext, Configuration>());
            base.OnModelCreating(modelBuilder);
        }
    }
}
