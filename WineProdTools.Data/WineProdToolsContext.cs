using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using WineProdTools.Data.EntityModels;

namespace WineProdTools.Data
{
    internal class WineProdToolsContext : DbContext, IWineProdToolsContext
    {
        public WineProdToolsContext() : base("name=DefaultConnection") { }

        public IDbSet<Account> Accounts { get; set; }
        public IDbSet<UserProfile> UserProfiles { get; set; }
        public IDbSet<Tank> Tanks { get; set; }
        public IDbSet<Note> Notes { get; set; }
        public IDbSet<TankContents> TankContents { get; set; }
        public IDbSet<TankContentsState> TankContentsStates { get; set; }
    }
}
