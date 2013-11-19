using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WineProdTools.Data.Migrations;

namespace WineProdTools.Data
{
    public static class WineProdToolsDataInitializer
    {
        public static void Init()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WineProdToolsContext, Configuration>());
            using (var db = new WineProdToolsContext())
            {
                db.Database.Initialize(true);
            }
        }
    }
}
