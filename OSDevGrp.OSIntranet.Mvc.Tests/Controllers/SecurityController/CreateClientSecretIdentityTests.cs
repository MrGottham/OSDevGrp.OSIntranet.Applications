using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using OSDevGrp.OSIntranet.Mvc.Models.Security;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class CreateClientSecretIdentityTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();

            _fixture = new Fixture();
            _fixture.Customize<Claim>(builder => builder.FromFactory(() => new Claim(_fixture.Create<string>(), _fixture.Create<string>())));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateClientSecretIdentity_WhenCalledWithoutModel_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.CreateClientSecretIdentity();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<Claim>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateClientSecretIdentity_WhenCalledWithoutModel_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CreateClientSecretIdentity();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateClientSecretIdentity_WhenCalledWithoutModel_ReturnsViewResultWhereViewNameIsEqualToCreateClientSecretIdentity()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.CreateClientSecretIdentity();

            Assert.That(result.ViewName, Is.EqualTo("CreateClientSecretIdentity"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateClientSecretIdentity_WhenCalledWithoutModel_ReturnsViewResultWhereModelIsClientSecretIdentityViewModel()
        {
            IList<Claim> claimCollection = _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(claimCollection);

            ViewResult result = (ViewResult) await sut.CreateClientSecretIdentity();

            Assert.That(result.Model, Is.TypeOf<ClientSecretIdentityViewModel>());

            ClientSecretIdentityViewModel clientSecretIdentityViewModel = (ClientSecretIdentityViewModel) result.Model;

            Assert.That(clientSecretIdentityViewModel, Is.Not.Null);
            Assert.That(clientSecretIdentityViewModel.Identifier, Is.EqualTo(default(int)));
            Assert.That(clientSecretIdentityViewModel.FriendlyName, Is.Null);
            Assert.That(clientSecretIdentityViewModel.ClientId, Is.Null);
            Assert.That(clientSecretIdentityViewModel.ClientSecret, Is.Null);
            Assert.That(clientSecretIdentityViewModel.EditMode, Is.EqualTo(EditMode.Create));
            
            Assert.That(clientSecretIdentityViewModel.Claims, Is.Not.Null);
            Assert.That(clientSecretIdentityViewModel.Claims, Is.Not.Empty);
            Assert.That(clientSecretIdentityViewModel.Claims.Count(), Is.EqualTo(claimCollection.Count));
            foreach (Claim claim in claimCollection)
            {
                Assert.That(clientSecretIdentityViewModel.Claims.SingleOrDefault(m => string.CompareOrdinal(m.ClaimType, claim.Type) == 0), Is.Not.Null);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CreateClientSecretIdentity_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateClientSecretIdentity(null));

            Assert.That(result.ParamName, Is.EqualTo("clientSecretIdentityViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateClientSecretIdentity_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            ClientSecretIdentityViewModel model = CreateModel();
            await sut.CreateClientSecretIdentity(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreateClientSecretIdentityCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateClientSecretIdentity_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            ClientSecretIdentityViewModel model = CreateModel();
            IActionResult result = await sut.CreateClientSecretIdentity(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateClientSecretIdentity_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToCreateClientSecretIdentity()
        {
            Controller sut = CreateSut(modelIsValid: false);

            ClientSecretIdentityViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateClientSecretIdentity(model);

            Assert.That(result.ViewName, Is.EqualTo("CreateClientSecretIdentity"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateClientSecretIdentity_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            ClientSecretIdentityViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateClientSecretIdentity(model);

            Assert.That(result.Model, Is.TypeOf<ClientSecretIdentityViewModel>());

            ClientSecretIdentityViewModel clientSecretIdentityViewModel = (ClientSecretIdentityViewModel) result.Model;

            Assert.That(clientSecretIdentityViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateClientSecretIdentity_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            ClientSecretIdentityViewModel model = CreateModel();
            await sut.CreateClientSecretIdentity(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateClientSecretIdentityCommand>(command => command.Identifier == int.MaxValue && string.CompareOrdinal(command.FriendlyName, model.FriendlyName) == 0 && command.Claims.Count() == model.Claims.Count(claimViewModel => claimViewModel.IsSelected))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateClientSecretIdentity_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            ClientSecretIdentityViewModel model = CreateModel();
            IActionResult result = await sut.CreateClientSecretIdentity(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateClientSecretIdentity_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereContollerNameIsEqualToSecurity()
        {
            Controller sut = CreateSut();

            ClientSecretIdentityViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateClientSecretIdentity(model);

            Assert.That(result.ControllerName, Is.EqualTo("Security"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateClientSecretIdentity_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToClientSecretIdentities()
        {
            Controller sut = CreateSut();

            ClientSecretIdentityViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateClientSecretIdentity(model);

            Assert.That(result.ActionName, Is.EqualTo("ClientSecretIdentities"));
        }

        private Controller CreateSut(IEnumerable<Claim> claimCollection = null, bool modelIsValid = true)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<Claim>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => claimCollection ?? _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList()));

            Controller controller = new Mvc.Controllers.SecurityController(_commandBusMock.Object, _queryBusMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private ClientSecretIdentityViewModel CreateModel()
        {
            return _fixture.Build<ClientSecretIdentityViewModel>()
                .With(m => m.Claims, _fixture.CreateMany<ClaimViewModel>(_random.Next(5, 10)).ToList())
                .Create();
        }
    }
}
