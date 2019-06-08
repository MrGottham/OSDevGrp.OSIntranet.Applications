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
    public class CreateUserIdentityTests
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
        public async Task CreateUserIdentity_WhenCalledWithoutModel_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.CreateUserIdentity();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<Claim>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateUserIdentity_WhenCalledWithoutModel_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CreateUserIdentity();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateUserIdentity_WhenCalledWithoutModel_ReturnsViewResultWhereViewNameIsEqualToCreateUserIdentity()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.CreateUserIdentity();

            Assert.That(result.ViewName, Is.EqualTo("CreateUserIdentity"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateUserIdentity_WhenCalledWithoutModel_ReturnsViewResultWhereModelIsUserIdentityViewModel()
        {
            IList<Claim> claimCollection = _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(claimCollection);

            ViewResult result = (ViewResult) await sut.CreateUserIdentity();

            Assert.That(result.Model, Is.TypeOf<UserIdentityViewModel>());

            UserIdentityViewModel userIdentityViewModel = (UserIdentityViewModel) result.Model;

            Assert.That(userIdentityViewModel, Is.Not.Null);
            Assert.That(userIdentityViewModel.Identifier, Is.EqualTo(default(int)));
            Assert.That(userIdentityViewModel.ExternalUserIdentifier, Is.Null);
            Assert.That(userIdentityViewModel.EditMode, Is.EqualTo(EditMode.Create));
            
            Assert.That(userIdentityViewModel.Claims, Is.Not.Null);
            Assert.That(userIdentityViewModel.Claims, Is.Not.Empty);
            Assert.That(userIdentityViewModel.Claims.Count(), Is.EqualTo(claimCollection.Count));
            foreach (Claim claim in claimCollection)
            {
                Assert.That(userIdentityViewModel.Claims.SingleOrDefault(m => string.CompareOrdinal(m.ClaimType, claim.Type) == 0), Is.Not.Null);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CreateUserIdentity_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateUserIdentity(null));

            Assert.That(result.ParamName, Is.EqualTo("userIdentityViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateUserIdentity_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            UserIdentityViewModel model = CreateModel();
            await sut.CreateUserIdentity(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreateUserIdentityCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateUserIdentity_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            UserIdentityViewModel model = CreateModel();
            IActionResult result = await sut.CreateUserIdentity(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateUserIdentity_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToCreateUserIdentity()
        {
            Controller sut = CreateSut(modelIsValid: false);

            UserIdentityViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateUserIdentity(model);

            Assert.That(result.ViewName, Is.EqualTo("CreateUserIdentity"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateUserIdentity_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            UserIdentityViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateUserIdentity(model);

            Assert.That(result.Model, Is.TypeOf<UserIdentityViewModel>());

            UserIdentityViewModel userIdentityViewModel = (UserIdentityViewModel) result.Model;

            Assert.That(userIdentityViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateUserIdentity_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            UserIdentityViewModel model = CreateModel();
            await sut.CreateUserIdentity(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateUserIdentityCommand>(command => command.Identifier == int.MaxValue && string.CompareOrdinal(command.ExternalUserIdentifier, model.ExternalUserIdentifier) == 0 && command.Claims.Count() == model.Claims.Count(claimViewModel => claimViewModel.IsSelected))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateUserIdentity_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            UserIdentityViewModel model = CreateModel();
            IActionResult result = await sut.CreateUserIdentity(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateUserIdentity_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereContollerNameIsEqualToSecurity()
        {
            Controller sut = CreateSut();

            UserIdentityViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateUserIdentity(model);

            Assert.That(result.ControllerName, Is.EqualTo("Security"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateUserIdentity_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToUserIdentities()
        {
            Controller sut = CreateSut();

            UserIdentityViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateUserIdentity(model);

            Assert.That(result.ActionName, Is.EqualTo("UserIdentities"));
        }

        private Controller CreateSut(IEnumerable<Claim> claimCollection = null, bool modelIsValid = true)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<Claim>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => claimCollection ?? _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList()));

            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<ICreateUserIdentityCommand>()))
                .Returns(Task.Run(() => { }));

            Controller controller = new Mvc.Controllers.SecurityController(_commandBusMock.Object, _queryBusMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private UserIdentityViewModel CreateModel()
        {
            return _fixture.Build<UserIdentityViewModel>()
                .With(m => m.Claims, _fixture.CreateMany<ClaimViewModel>(_random.Next(5, 10)).ToList())
                .Create();
        }
    }
}
