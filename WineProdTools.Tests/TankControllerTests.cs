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
    public class TankControllerTests
    {
        [TestMethod]
        public void EnsureThatWhenGettingAnExistingTankThatWeReturnATank()
        {
            var dto = new TankAndContentsDto { Id = 1 };
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.GetTankForAccount(dto.Id, user.AccountId))
                .Returns(dto);

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.GetTank(dto.Id);
            Assert.AreEqual(dto, actual);
        }

        [TestMethod]
        public void EnsureThatWhenGettingANonexistantTankThatWeThrowANotFoundResponse()
        {
            var dto = new TankAndContentsDto { Id = 1 };
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.GetTankForAccount(dto.Id, user.AccountId))
                .Returns<TankAndContentsDto>(null);

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            try
            {
                var actual = ctrl.GetTank(dto.Id);
                Assert.Fail("An Http response exception should have been thrown.");
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, e.Response.StatusCode);
            }
        }

        [TestMethod]
        public void EnsureThatPostingAValidTankReturnsACreatedResponse()
        {
            var dto = new TankDto { Id = 1 };
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.AddTankForAccount(dto, user.AccountId))
                .Returns(1);

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            var config = new HttpConfiguration();
            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, "http://localhost");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/");
            var routeData = new System.Web.Http.Routing.HttpRouteData(route, new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "blah" } });
            ctrl.ControllerContext = new System.Web.Http.Controllers.HttpControllerContext(config, routeData, request);
            ctrl.Request = request;
            ctrl.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            ctrl.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

            var actual = ctrl.PostTank(dto);
            Assert.AreEqual(HttpStatusCode.Created, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatPostingAValidTankCausesTheTankToBeAdded()
        {
            var dto = new TankDto { Id = 1 };
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.AddTankForAccount(dto, user.AccountId))
                .Returns(1);

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            var config = new HttpConfiguration();
            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, "http://localhost");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/");
            var routeData = new System.Web.Http.Routing.HttpRouteData(route, new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "blah" } });
            ctrl.ControllerContext = new System.Web.Http.Controllers.HttpControllerContext(config, routeData, request);
            ctrl.Request = request;
            ctrl.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            ctrl.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

            var actual = ctrl.PostTank(dto);
            mock.Verify(m => m.AddTankForAccount(dto, user.AccountId));
        }

        [TestMethod]
        public void EnsureThatPostingAnInvalidTankReturnsABadRequest()
        {
            var dto = new TankDto { Id = 1 };
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.AddTankForAccount(dto, user.AccountId))
                .Returns(1);

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            var config = new HttpConfiguration();
            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, "http://localhost");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/");
            var routeData = new System.Web.Http.Routing.HttpRouteData(route, new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "blah" } });
            ctrl.ControllerContext = new System.Web.Http.Controllers.HttpControllerContext(config, routeData, request);
            ctrl.Request = request;
            ctrl.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            ctrl.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            ctrl.ModelState.AddModelError("blah", "blah");

            var actual = ctrl.PostTank(dto);
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatDeletingATankThatIsPartOfTheirAccountCausesTheTankToBeDeleted()
        {
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.DeleteTankForAccount(1, user.AccountId));

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.DeleteTank(1);
            mock.Verify(m => m.DeleteTankForAccount(1, user.AccountId));
        }

        [TestMethod]
        public void EnsureThatDeletingATankThatIsPartOfTheirAccountReturnsAnOkResponse()
        {
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.DeleteTankForAccount(1, user.AccountId));

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.DeleteTank(1);
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatDeletingATankThatIsNotPartOfTheirAccountReturnsAnUnauthorizedResponse()
        {
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.DeleteTankForAccount(1, user.AccountId))
                .Throws<System.Security.Authentication.AuthenticationException>();

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.DeleteTank(1);
            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatPuttingAValidTankActuallyUpdatesTheTank()
        {
            var dto = new TankDto();
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.UpdateTankForAccount(dto, user.AccountId));

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.PutTank(dto);
            mock.Verify(m => m.UpdateTankForAccount(dto, user.AccountId));
        }

        [TestMethod]
        public void EnsureThatPuttingAValidTankReturnsAnOkResponse()
        {
            var dto = new TankDto();
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.UpdateTankForAccount(dto, user.AccountId));

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.PutTank(dto);
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatPuttingAnInvalidTankReturnsABadRequestResponse()
        {
            var dto = new TankDto();
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.UpdateTankForAccount(dto, user.AccountId));

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            ctrl.ModelState.AddModelError("blah", "blah");

            var actual = ctrl.PutTank(dto);
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatPuttingValidTankContentsActuallyUpdatesTheTankContents()
        {
            var dto = new TankContentsDto();
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.UpdateTankContentsForAccount(dto, user.AccountId));

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.PutTankContents(dto);
            mock.Verify(m => m.UpdateTankContentsForAccount(dto, user.AccountId));
        }

        [TestMethod]
        public void EnsureThatPuttingValidTankContentsReturnsAnOkResponse()
        {
            var dto = new TankContentsDto();
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.UpdateTankContentsForAccount(dto, user.AccountId));

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.PutTankContents(dto);
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatPuttingInvalidTankContentsReturnsABadRequestResponse()
        {
            var dto = new TankContentsDto();
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.UpdateTankContentsForAccount(dto, user.AccountId));

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            ctrl.ModelState.AddModelError("blah", "blah");

            var actual = ctrl.PutTankContents(dto);
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatPuttingValidTankContentsOfATankThatDoesntBelongToTheirAccountReturnsAnUnauthorizedResponse()
        {
            var dto = new TankContentsDto();
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.UpdateTankContentsForAccount(dto, user.AccountId))
                .Throws<System.Security.Authentication.AuthenticationException>();

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.PutTankContents(dto);
            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatPuttingAValidTankTransferActuallyPerformsTheTransfer()
        {
            var dto = new TankTransferDto();
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.TankContentsTransferForAccount(dto, user.AccountId));

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.PutTankTransfer(dto);
            mock.Verify(m => m.TankContentsTransferForAccount(dto, user.AccountId));
        }

        [TestMethod]
        public void EnsureThatPuttingAValidTankTransferReturnsAnOkResponse()
        {
            var dto = new TankTransferDto();
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.TankContentsTransferForAccount(dto, user.AccountId));

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.PutTankTransfer(dto);
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatPuttingAnInvalidTankTransferReturnsABadRequestResponse()
        {
            var dto = new TankTransferDto();
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.TankContentsTransferForAccount(dto, user.AccountId));

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            ctrl.ModelState.AddModelError("blah", "blah");

            var actual = ctrl.PutTankTransfer(dto);
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [TestMethod]
        public void EnsureThatPuttingAValidTankTransferOfATankThatDoesntBelongToTheirAccountReturnsAnUnauthorizedResponse()
        {
            var dto = new TankTransferDto();
            var user = new CustomPrincipal("myusername");
            user.AccountId = 1;
            var mock = new Mock<ITankManager>();
            mock.Setup(m => m.TankContentsTransferForAccount(dto, user.AccountId))
                .Throws<System.Security.Authentication.AuthenticationException>();

            var ctrl = new TankController(mock.Object);
            System.Threading.Thread.CurrentPrincipal = user;
            ctrl.Request = new System.Net.Http.HttpRequestMessage();
            ctrl.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actual = ctrl.PutTankTransfer(dto);
            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.StatusCode);
        }
    }
}
