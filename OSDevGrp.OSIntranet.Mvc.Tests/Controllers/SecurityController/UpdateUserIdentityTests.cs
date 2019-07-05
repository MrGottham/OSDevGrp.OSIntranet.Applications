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
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using OSDevGrp.OSIntranet.Mvc.Models.Security;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class UpdateUserIdentityTests
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
        public async Task UpdateUserIdentity_WhenCalledWithoutModel_AssertQueryAsyncWasCalledOnQueryBusToGetClaims()
        {
            Controller sut = CreateSut();

            await sut.UpdateUserIdentity(_fixture.Create<int>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<Claim>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithoutModel_AssertQueryAsyncWasCalledOnQueryBusToGetUserIdentity()
        {
            Controller sut = CreateSut();

            int identifier = _fixture.Create<int>();
            await sut.UpdateUserIdentity(identifier);

            _queryBusMock.Verify(m => m.QueryAsync<IGetUserIdentityQuery, IUserIdentity>(It.Is<IGetUserIdentityQuery>(query => query != null && query.Identifier == identifier)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithoutModelAndIdentifierIsUnknown_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut(hasUserIdentity: false);

            IActionResult result = await sut.UpdateUserIdentity(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithoutModelAndIdentifierIsUnknown_ReturnsRedirectToActionResultWhereContollerNameIsEqualToSecurity()
        {
            Controller sut = CreateSut(hasUserIdentity: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateUserIdentity(_fixture.Create<int>());

            Assert.That(result.ControllerName, Is.EqualTo("Security"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithoutModelAndIdentifierIsUnknown_ReturnsRedirectToActionResultWhereActionNameIsEqualToUserIdentities()
        {
            Controller sut = CreateSut(hasUserIdentity: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateUserIdentity(_fixture.Create<int>());

            Assert.That(result.ActionName, Is.EqualTo("UserIdentities"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithoutModelAndIdentifierIsKnown_AssertToClaimsIdentityWasCalledOnUserIdentity()
        {
            Mock<IUserIdentity> userIdentityMock = _fixture.BuildUserIdentityMock();
            Controller sut = CreateSut(userIdentity: userIdentityMock.Object);

            IActionResult result = await sut.UpdateUserIdentity(_fixture.Create<int>());

            userIdentityMock.Verify(m => m.ToClaimsIdentity(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithoutModelAndIdentifierIsKnown_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateUserIdentity(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithoutModelAndIdentifierIsKnown_ReturnsViewResultWhereViewNameIsEqualToUpdateUserIdentity()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.UpdateUserIdentity(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("UpdateUserIdentity"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithoutModelAndIdentifierIsKnown_ReturnsViewResultWhereModelIsUserIdentityViewModel()
        {
            IList<Claim> claimCollection = _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList();
            IList<Claim> userIdentityClaimCollection = claimCollection.Take(2).ToList();
            IUserIdentity userIdentity = _fixture.BuildUserIdentityMock(claims: userIdentityClaimCollection).Object;
            Controller sut = CreateSut(claimCollection, userIdentity: userIdentity);

            ViewResult result = (ViewResult) await sut.UpdateUserIdentity(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<UserIdentityViewModel>());

            UserIdentityViewModel userIdentityViewModel = (UserIdentityViewModel) result.Model;

            Assert.That(userIdentityViewModel, Is.Not.Null);
            Assert.That(userIdentityViewModel.Identifier, Is.EqualTo(userIdentity.Identifier));
            Assert.That(userIdentityViewModel.ExternalUserIdentifier, Is.EqualTo(userIdentity.ExternalUserIdentifier));
            Assert.That(userIdentityViewModel.EditMode, Is.EqualTo(EditMode.Edit));
            
            Assert.That(userIdentityViewModel.Claims, Is.Not.Null);
            Assert.That(userIdentityViewModel.Claims, Is.Not.Empty);
            Assert.That(userIdentityViewModel.Claims.Count(), Is.EqualTo(claimCollection.Count));
            foreach (Claim claim in claimCollection)
            {
                Assert.That(userIdentityViewModel.Claims.SingleOrDefault(m => string.CompareOrdinal(m.ClaimType, claim.Type) == 0), Is.Not.Null);
            }
            Assert.That(userIdentityViewModel.Claims.Count(m => m.IsSelected), Is.EqualTo(userIdentityClaimCollection.Count));
            foreach (Claim claim in userIdentityClaimCollection)
            {
                Assert.That(userIdentityViewModel.Claims.SingleOrDefault(m => m.IsSelected && string.CompareOrdinal(m.ClaimType, claim.Type) == 0 && string.Compare(m.ActualValue, claim.Value) == 0), Is.Not.Null);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateUserIdentity_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateUserIdentity(null));

            Assert.That(result.ParamName, Is.EqualTo("userIdentityViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            UserIdentityViewModel model = CreateModel();
            await sut.UpdateUserIdentity(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IUpdateUserIdentityCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            UserIdentityViewModel model = CreateModel();
            IActionResult result = await sut.UpdateUserIdentity(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToUpdateUserIdentity()
        {
            Controller sut = CreateSut(modelIsValid: false);

            UserIdentityViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateUserIdentity(model);

            Assert.That(result.ViewName, Is.EqualTo("UpdateUserIdentity"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            UserIdentityViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateUserIdentity(model);

            Assert.That(result.Model, Is.TypeOf<UserIdentityViewModel>());

            UserIdentityViewModel userIdentityViewModel = (UserIdentityViewModel) result.Model;

            Assert.That(userIdentityViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            UserIdentityViewModel model = CreateModel();
            await sut.UpdateUserIdentity(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateUserIdentityCommand>(command => command.Identifier == model.Identifier && string.CompareOrdinal(command.ExternalUserIdentifier, model.ExternalUserIdentifier) == 0 && command.Claims.Count() == model.Claims.Count(claimViewModel => claimViewModel.IsSelected))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            UserIdentityViewModel model = CreateModel();
            IActionResult result = await sut.UpdateUserIdentity(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereContollerNameIsEqualToSecurity()
        {
            Controller sut = CreateSut();

            UserIdentityViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateUserIdentity(model);

            Assert.That(result.ControllerName, Is.EqualTo("Security"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateUserIdentity_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToUserIdentities()
        {
            Controller sut = CreateSut();

            UserIdentityViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateUserIdentity(model);

            Assert.That(result.ActionName, Is.EqualTo("UserIdentities"));
        }

        private Controller CreateSut(IEnumerable<Claim> claimCollection = null, bool hasUserIdentity = true, IUserIdentity userIdentity = null, bool modelIsValid = true)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<Claim>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => claimCollection ?? _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList()));
            _queryBusMock.Setup(m => m.QueryAsync<IGetUserIdentityQuery, IUserIdentity>(It.IsAny<IGetUserIdentityQuery>()))
                .Returns(Task.Run(() => hasUserIdentity ? userIdentity ?? _fixture.BuildUserIdentityMock().Object : null));

            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IUpdateUserIdentityCommand>()))
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
