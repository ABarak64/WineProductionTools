using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.EntityModels;

namespace WineProdTools.Data.Managers
{
    public class UserManager
    {
        private readonly Func<IWineProdToolsContext> _getNewContext;

        public UserManager()
        {
            this._getNewContext = () => { return new WineProdToolsContext(); };
        }

        public UserManager(Func<IWineProdToolsContext> contextFactory)
        {
            this._getNewContext = contextFactory;
        }

        public UserProfile GetUserFromUserId(string userId)
        {
            using (var db = this._getNewContext())
            {
                return db.UserProfiles
                    .Single(u => u.UserName == userId);
            }
        }
    }
}
