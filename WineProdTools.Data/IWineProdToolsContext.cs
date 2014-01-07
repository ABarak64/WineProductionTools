using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WineProdTools.Data.EntityModels;
using System.Data.Entity.Infrastructure;

namespace WineProdTools.Data
{
    public interface IWineProdToolsContext : IDisposable
    {
        IDbSet<Account> Accounts { get; set; }
        IDbSet<UserProfile> UserProfiles { get; set; }
        IDbSet<Tank> Tanks { get; set; }
        IDbSet<Note> Notes { get; set; }
        IDbSet<TankContents> TankContents { get; set; }
        IDbSet<TankContentsState> TankContentsStates { get; set; }
        int SaveChanges();
    }
}
