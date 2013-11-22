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
        public IEnumerable<TankDto> GetTanksForAccount(Int64 accountId)
        {
            using (var db = new WineProdToolsContext())
            {
                return db.Tanks
                    .Where(t => t.AccountId == accountId && t.DateDeleted == null)
                    .AsEnumerable()
                    .Select(t => new TankDto(t))
                    .ToList();
            }
        }
    }
}
