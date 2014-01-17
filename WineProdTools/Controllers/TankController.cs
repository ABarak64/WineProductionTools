using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WineProdTools.Data.Managers;
using WineProdTools.Data.DtoModels;
using WineProdTools.Data.EntityModels;
using WineProdTools.Membership;
using System.Security.Authentication;

namespace WineProdTools.Controllers
{
    [Authorize]
    public class TankController : ApiController
    {
        private readonly ITankManager _manager;

        public TankController()
        {
            this._manager = new TankManager();
        }

        public TankController(ITankManager manager)
        {
            this._manager = manager;
        }

        public IEnumerable<TankAndContentsDto> GetTanks()
        {
            return this._manager.GetTanksForAccount(((CustomPrincipal)User).AccountId);
        }

        public IEnumerable<TankContentsState> GetTankStates()
        {
            return this._manager.GetContentStates();
        }

        public TankAndContentsDto GetTank(Int64 tankId)
        {
            var tank = this._manager.GetTankForAccount(tankId, ((CustomPrincipal)User).AccountId);
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

            var id = this._manager.AddTankForAccount(tankDto, ((CustomPrincipal)User).AccountId);
            tankDto.Id = id;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tankDto);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = id }));
            return response;
        }

        public HttpResponseMessage DeleteTank(Int64 tankId)
        {
            try
            {
                this._manager.DeleteTankForAccount(tankId, ((CustomPrincipal)User).AccountId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (AuthenticationException e)
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
            try
            {
                this._manager.UpdateTankForAccount(tankDto, ((CustomPrincipal)User).AccountId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (AuthenticationException e)
            {
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        public HttpResponseMessage PutTankContents(TankContentsDto contentsDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                this._manager.UpdateTankContentsForAccount(contentsDto, ((CustomPrincipal)User).AccountId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (AuthenticationException e)
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
            try
            {
                this._manager.TankContentsTransferForAccount(transferDto, ((CustomPrincipal)User).AccountId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (AuthenticationException e)
            {
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}
