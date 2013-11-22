using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.DtoModels;

namespace WineProdTools.Data.Managers
{
    public class TankManager
    {
        public IEnumerable<TankDto> GetTanksForUser(string userId)
        {
            using (var db = new WineProdToolsContext())
            {
                var accountId = db.UserProfiles
                    .Where(u => u.UserName == userId)
                    .Select(u => u.AccountId)
                    .SingleOrDefault() ?? 0;

                return db.Tanks
                    .Where(t => t.AccountId == accountId && t.DateDeleted == null)
                    .AsEnumerable()
                    .Select(t => new TankDto(t))
                    .ToList();
            }
        }
    }
}
