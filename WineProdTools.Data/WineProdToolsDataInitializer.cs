using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WineProdTools.Data.Migrations;
using WineProdTools.Data.EntityModels;

namespace WineProdTools.Data
{
    public static class WineProdToolsDataInitializer
    {
        public static void Init()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WineProdToolsContext, Configuration>());
            Database.SetInitializer(new WineProdToolsSeeder());
            using (var db = new WineProdToolsContext())
            {
                db.Database.Initialize(true);
            }
        }
    }

    internal class WineProdToolsSeeder : CreateDatabaseIfNotExists<WineProdToolsContext>
    {
        protected override void Seed(WineProdToolsContext context)
        {
            context.TankContentsStates.Add(new TankContentsState { Name = "None" });
            context.TankContentsStates.Add(new TankContentsState { Name = "Primary Fermentation" });
            context.TankContentsStates.Add(new TankContentsState { Name = "Malolactic Fermentation" });
            context.TankContentsStates.Add(new TankContentsState { Name = "Finished" });
            context.TankContentsStates.Add(new TankContentsState { Name = "Previous Vintage" });
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
