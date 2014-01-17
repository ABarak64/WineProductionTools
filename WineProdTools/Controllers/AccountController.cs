using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WineProdTools.Data.DtoModels;
using WineProdTools.Data.Managers;
using WineProdTools.Membership;

namespace WineProdTools.Controllers
{
    [Authorize]
    public class AccountController : ApiController
    {
        private readonly IAccountManager _manager;

        public AccountController()
        {
            this._manager = new AccountManager();
        }

        public AccountController(IAccountManager manager)
        {
            this._manager = manager;
        }

        public AccountDto GetAccount()
        {
            return this._manager.GetAccount(((CustomPrincipal)User).AccountId);
        }

        public HttpResponseMessage PutAccount(AccountDto accountDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            accountDto.Id = ((CustomPrincipal)User).AccountId;
            this._manager.UpdateAccount(accountDto);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
