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
    public class NoteController : ApiController
    {
        private readonly INoteManager _manager;

        public NoteController()
        {
            this._manager = new NoteManager();
        }

        public NoteController(INoteManager manager)
        {
            this._manager = manager;
        }

        public IEnumerable<NoteDto> GetSomeNotesAfterThisMany(int count)
        {
            return this._manager.GetSomeNotesAfterThisManyForAccount(count, ((CustomPrincipal)User).AccountId);
        }

        public HttpResponseMessage PostNote(NoteDto noteDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            this._manager.AddNoteForAccount(noteDto, ((CustomPrincipal)User).AccountId);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, noteDto);
            return response;
        }
    }
}
