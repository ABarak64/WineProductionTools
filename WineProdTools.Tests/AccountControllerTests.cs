using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WineProdTools.Controllers;
using Moq;
using WineProdTools.Data.Managers;
using WineProdTools.Data.DtoModels;
using WineProdTools.Membership;
using System.Net;

namespace WineProdTools.Tests
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void EnsureThatAValidAccountDtoCausesTheAccountToBeUpdated()
        {
            var accountDto = new AccountDto { };
            var mock = new Mock<IAccountManager>();
            mock.Setup(m => m.UpdateAccount(accountDto));

            var ctrl = new AccountController(mock.Object);
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();

            var actual = ctrl.PutAccount(accountDto);
            mock.Verify(m => m.UpdateAccount(accountDto));
        }

        [TestMethod]
        public void EnsureThatAValidAccountDtoReturnsAnOkResponse()
        {
            var accountDto = new AccountDto { };
            var mock = new Mock<IAccountManager>();
            mock.Setup(m => m.UpdateAccount(accountDto));

            var ctrl = new AccountController(mock.Object);
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();

            var actual = ctrl.PutAccount(accountDto);
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatAnInvalidAccountDtoReturnsABadRequestResponse()
        {
            var accountDto = new AccountDto { };
            var mock = new Mock<IAccountManager>();
            mock.Setup(m => m.UpdateAccount(accountDto));

            var ctrl = new AccountController(mock.Object);
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.ModelState.AddModelError("blah", "blah");

            var actual = ctrl.PutAccount(accountDto);
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }
    }
}
