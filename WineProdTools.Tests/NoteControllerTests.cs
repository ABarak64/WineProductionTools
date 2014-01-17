using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WineProdTools.Controllers;
using Moq;
using WineProdTools.Data.Managers;
using WineProdTools.Data.DtoModels;
using WineProdTools.Membership;
using System.Net;
using System.Web.Http.Hosting;
using System.Web.Http;

namespace WineProdTools.Tests
{
    [TestClass]
    public class NoteControllerTests
    {
        [TestMethod]
        public void EnsureThatAValidNoteDtoCausesTheNoteToBeAdded()
        {
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var noteDto = new NoteDto { };
            var mock = new Mock<INoteManager>();
            mock.Setup(m => m.AddNoteForAccount(noteDto, user.AccountId));

            var ctrl = new NoteController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.PostNote(noteDto);
            mock.Verify(m => m.AddNoteForAccount(noteDto, user.AccountId));
        }

        [TestMethod]
        public void EnsureThatAValidNoteDtoReturnsACreatedResponse()
        {
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var noteDto = new NoteDto { };
            var mock = new Mock<INoteManager>();
            mock.Setup(m => m.AddNoteForAccount(noteDto, user.AccountId));

            var ctrl = new NoteController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.PostNote(noteDto);
            Assert.AreEqual(HttpStatusCode.Created, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatAnInvalidNoteDtoReturnsABadRequestResponse()
        {
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var noteDto = new NoteDto { };
            var mock = new Mock<INoteManager>();
            mock.Setup(m => m.AddNoteForAccount(noteDto, user.AccountId));

            var ctrl = new NoteController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            ctrl.ModelState.AddModelError("blah", "blah");

            var actual = ctrl.PostNote(noteDto);
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }
    }
}
