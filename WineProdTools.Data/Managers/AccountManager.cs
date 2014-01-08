using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.EntityModels;
using WineProdTools.Data.DtoModels;

namespace WineProdTools.Data.Managers
{
    public class AccountManager
    {
        private readonly Func<IWineProdToolsContext> _getNewContext;

        public AccountManager()
        {
            this._getNewContext = () => { return new WineProdToolsContext(); };
        }

        public AccountManager(Func<IWineProdToolsContext> contextFactory)
        {
            this._getNewContext = contextFactory;
        }
        /// <summary>
        /// Creates a new account and adds the account id to the user passed in.
        /// </summary>
        /// <returns></returns>
        public void Create(string userName)
        {
            using (var db = this._getNewContext())
            {
                var newAccount = new Account { Name = "My Winery", Active = true };
                db.Accounts.Add(newAccount);
                db.SaveChanges();  // We need the db-generated account id.
                var user = db.UserProfiles.Single(u => u.UserName == userName);
                user.AccountId = newAccount.Id;
                db.SaveChanges();
            }
        }

        public AccountDto GetAccount(Int64 accountId)
        {
            using (var db = this._getNewContext())
            {
                return db.Accounts
                    .Where(a => a.Id == accountId)
                    .AsEnumerable()
                    .Select(a => new AccountDto(a))
                    .SingleOrDefault();
            }
        }

        public void UpdateAccount(AccountDto accountDto)
        {
            using (var db = this._getNewContext())
            {
                var acct = db.Accounts.Single(a => a.Id == accountDto.Id);
                acct.Name = accountDto.Name;
                db.SaveChanges();
            }
        }
    }
}
