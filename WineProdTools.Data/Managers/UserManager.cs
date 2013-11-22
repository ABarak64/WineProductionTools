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
        public UserProfile GetUserFromUserId(string userId)
        {
            using (var db = new WineProdToolsContext())
            {
                return db.UserProfiles
                    .Single(u => u.UserName == userId);
            }
        }
    }
}
