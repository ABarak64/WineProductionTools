using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data;
using WineProdTools.Data.EntityModels;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace WineProdTools.Data.Tests.Mocks
{
    public class FakeWineProdToolsContext : IWineProdToolsContext
    {
        public IDbSet<Account> Accounts { get; set; }
        public IDbSet<UserProfile> UserProfiles { get; set; }
        public IDbSet<Tank> Tanks { get; set; }
        public IDbSet<Note> Notes { get; set; }
        public IDbSet<TankContents> TankContents { get; set; }
        public IDbSet<TankContentsState> TankContentsStates { get; set; }
        public bool SaveChangesCalled = false;

        public FakeWineProdToolsContext()
        {
            this.Accounts = new FakeDbSet<Account>();
            this.UserProfiles = new FakeDbSet<UserProfile>();
            this.Tanks = new FakeDbSet<Tank>();
            this.Notes = new FakeDbSet<Note>();
            this.TankContents = new FakeDbSet<TankContents>();
            this.TankContentsStates = new FakeDbSet<TankContentsState>();
        }

        public int SaveChanges()
        {
            this.SaveChangesCalled = true;
            return 0;
        }

        public void Dispose() { }

    }
}
