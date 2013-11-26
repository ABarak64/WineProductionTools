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
        public AccountDto GetAccount()
        {
            var mgr = new AccountManager();
            return mgr.GetAccount(((CustomPrincipal)User).AccountId);
        }

        public HttpResponseMessage PutAccount(AccountDto accountDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            accountDto.Id = ((CustomPrincipal)User).AccountId;
            var mgr = new AccountManager();
            mgr.UpdateAccount(accountDto);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
