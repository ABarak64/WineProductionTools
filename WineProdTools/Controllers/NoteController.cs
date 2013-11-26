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
        public IEnumerable<NoteDto> GetNotes()
        {
            var mgr = new NoteManager();
            return mgr.GetRecentNotesForAccount(((CustomPrincipal)User).AccountId);
        }

        public HttpResponseMessage PostNote(NoteDto noteDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var mgr = new NoteManager();
            mgr.AddNoteForAccount(noteDto, ((CustomPrincipal)User).AccountId);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, noteDto);
            return response;
        }
    }
}
