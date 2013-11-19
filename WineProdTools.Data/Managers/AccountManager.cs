using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.EntityModels;

namespace WineProdTools.Data.Managers
{
    public class AccountManager
    {
        /// <summary>
        /// Creates a new account and adds the account id to the user passed in.
        /// </summary>
        /// <returns></returns>
        public void Create(string userName)
        {
            using (var db = new WineProdToolsContext())
            {
                var newAccount = new Account { Name = "My Winery", Active = true };
                db.Accounts.Add(newAccount);
                db.SaveChanges();
                var user = db.UserProfiles.Single(u => u.UserName == userName);
                user.AccountId = newAccount.Id;
                db.SaveChanges();
            }
        }
    }
}
