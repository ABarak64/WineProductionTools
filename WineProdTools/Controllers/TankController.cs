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
        public IEnumerable<TankAndContentsDto> GetTanks()
        {
            var mgr = new TankManager();
            return mgr.GetTanksForAccount(((CustomPrincipal)User).AccountId);
        }

        public TankAndContentsDto GetTank(Int64 tankId)
        {
            var mgr = new TankManager();
            var tank = mgr.GetTankForAccount(tankId, ((CustomPrincipal)User).AccountId);
            if (tank == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return tank;
        }

        public HttpResponseMessage PostTank(TankDto tankDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var mgr = new TankManager();
            var id = mgr.AddTankForAccount(tankDto, ((CustomPrincipal)User).AccountId);
            tankDto.Id = id;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tankDto);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = id }));
            return response;
        }

        public HttpResponseMessage DeleteTank(Int64 tankId)
        {
            var mgr = new TankManager();
            try
            {
                mgr.DeleteTankForAccount(tankId, ((CustomPrincipal)User).AccountId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Security.Authentication.AuthenticationException e)
            {
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        public HttpResponseMessage PutTank(TankDto tankDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            var mgr = new TankManager();
            try
            {
                mgr.UpdateTankForAccount(tankDto, ((CustomPrincipal)User).AccountId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Security.Authentication.AuthenticationException e)
            {
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        public HttpResponseMessage PutTankTransfer(TankTransferDto transferDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            var mgr = new TankManager();
            try
            {
                mgr.TankContentsTransferForAccount(transferDto, ((CustomPrincipal)User).AccountId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Security.Authentication.AuthenticationException e)
            {
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}
