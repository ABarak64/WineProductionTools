using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.DtoModels;

namespace WineProdTools.Data.Managers
{
    public interface IAccountManager
    {
        void Create(string userName);
        AccountDto GetAccount(Int64 accountId);
        void UpdateAccount(AccountDto accountDto);
    }
}
