using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using WineProdTools.Data.EntityModels;

namespace WineProdTools.Data
{
    internal class WineProdToolsContext : DbContext
    {
        public WineProdToolsContext() : base("name=DefaultConnection") { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}
