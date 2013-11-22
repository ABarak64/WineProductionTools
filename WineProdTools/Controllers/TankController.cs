using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WineProdTools.Data.Managers;
using WineProdTools.Data.DtoModels;
using WineProdTools.Membership;

namespace WineProdTools.Controllers
{
    [Authorize]
    public class TankController : ApiController
    {
        public IEnumerable<TankDto> GetTanks()
        {
            var mgr = new TankManager();
            return mgr.GetTanksForAccount(((CustomPrincipal)User).AccountId);
        }
    }
}
