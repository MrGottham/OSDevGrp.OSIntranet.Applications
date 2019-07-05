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
    public class UpdateClientSecretIdentityTests
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
        public async Task UpdateClientSecretIdentity_WhenCalledWithoutModel_AssertQueryAsyncWasCalledOnQueryBusToGetClaims()
        {
            Controller sut = CreateSut();

            await sut.UpdateClientSecretIdentity(_fixture.Create<int>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<Claim>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithoutModel_AssertQueryAsyncWasCalledOnQueryBusToGetClientSecretIdentity()
        {
            Controller sut = CreateSut();

            int identifier = _fixture.Create<int>();
            await sut.UpdateClientSecretIdentity(identifier);

            _queryBusMock.Verify(m => m.QueryAsync<IGetClientSecretIdentityQuery, IClientSecretIdentity>(It.Is<IGetClientSecretIdentityQuery>(query => query != null && query.Identifier == identifier)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithoutModelAndIdentifierIsUnknown_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut(hasClientSecretIdentity: false);

            IActionResult result = await sut.UpdateClientSecretIdentity(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithoutModelAndIdentifierIsUnknown_ReturnsRedirectToActionResultWhereContollerNameIsEqualToSecurity()
        {
            Controller sut = CreateSut(hasClientSecretIdentity: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateClientSecretIdentity(_fixture.Create<int>());

            Assert.That(result.ControllerName, Is.EqualTo("Security"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithoutModelAndIdentifierIsUnknown_ReturnsRedirectToActionResultWhereActionNameIsEqualToClientSecretIdentities()
        {
            Controller sut = CreateSut(hasClientSecretIdentity: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateClientSecretIdentity(_fixture.Create<int>());

            Assert.That(result.ActionName, Is.EqualTo("ClientSecretIdentities"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithoutModelAndIdentifierIsKnown_AssertToClaimsIdentityWasCalledOnClientSecretIdentity()
        {
            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
            Controller sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object);

            IActionResult result = await sut.UpdateClientSecretIdentity(_fixture.Create<int>());

            clientSecretIdentityMock.Verify(m => m.ToClaimsIdentity(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithoutModelAndIdentifierIsKnown_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateClientSecretIdentity(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithoutModelAndIdentifierIsKnown_ReturnsViewResultWhereViewNameIsEqualToUpdateClientSecretIdentity()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.UpdateClientSecretIdentity(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("UpdateClientSecretIdentity"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithoutModelAndIdentifierIsKnown_ReturnsViewResultWhereModelIsClientSecretIdentityViewModel()
        {
            IList<Claim> claimCollection = _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList();
            IList<Claim> clientSecretIdentityClaimCollection = claimCollection.Take(2).ToList();
            IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(claims: clientSecretIdentityClaimCollection).Object;
            Controller sut = CreateSut(claimCollection, clientSecretIdentity: clientSecretIdentity);

            ViewResult result = (ViewResult) await sut.UpdateClientSecretIdentity(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<ClientSecretIdentityViewModel>());

            ClientSecretIdentityViewModel clientSecretIdentityViewModel = (ClientSecretIdentityViewModel) result.Model;

            Assert.That(clientSecretIdentityViewModel, Is.Not.Null);
            Assert.That(clientSecretIdentityViewModel.Identifier, Is.EqualTo(clientSecretIdentity.Identifier));
            Assert.That(clientSecretIdentityViewModel.FriendlyName, Is.EqualTo(clientSecretIdentity.FriendlyName));
            Assert.That(clientSecretIdentityViewModel.ClientId, Is.EqualTo(clientSecretIdentity.ClientId));
            Assert.That(clientSecretIdentityViewModel.ClientSecret, Is.EqualTo(clientSecretIdentity.ClientSecret.Substring(0, Math.Min(4, clientSecretIdentity.ClientSecret.Length)) + new string('*', clientSecretIdentity.ClientSecret.Length - Math.Min(4, clientSecretIdentity.ClientSecret.Length))));
            Assert.That(clientSecretIdentityViewModel.EditMode, Is.EqualTo(EditMode.Edit));
            
            Assert.That(clientSecretIdentityViewModel.Claims, Is.Not.Null);
            Assert.That(clientSecretIdentityViewModel.Claims, Is.Not.Empty);
            Assert.That(clientSecretIdentityViewModel.Claims.Count(), Is.EqualTo(claimCollection.Count));
            foreach (Claim claim in claimCollection)
            {
                Assert.That(clientSecretIdentityViewModel.Claims.SingleOrDefault(m => string.CompareOrdinal(m.ClaimType, claim.Type) == 0), Is.Not.Null);
            }
            Assert.That(clientSecretIdentityViewModel.Claims.Count(m => m.IsSelected), Is.EqualTo(clientSecretIdentityClaimCollection.Count));
            foreach (Claim claim in clientSecretIdentityClaimCollection)
            {
                Assert.That(clientSecretIdentityViewModel.Claims.SingleOrDefault(m => m.IsSelected && string.CompareOrdinal(m.ClaimType, claim.Type) == 0 && string.Compare(m.ActualValue, claim.Value) == 0), Is.Not.Null);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateClientSecretIdentity_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateClientSecretIdentity(null));

            Assert.That(result.ParamName, Is.EqualTo("clientSecretIdentityViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            ClientSecretIdentityViewModel model = CreateModel();
            await sut.UpdateClientSecretIdentity(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IUpdateClientSecretIdentityCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            ClientSecretIdentityViewModel model = CreateModel();
            IActionResult result = await sut.UpdateClientSecretIdentity(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToUpdateClientSecretIdentity()
        {
            Controller sut = CreateSut(modelIsValid: false);

            ClientSecretIdentityViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateClientSecretIdentity(model);

            Assert.That(result.ViewName, Is.EqualTo("UpdateClientSecretIdentity"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            ClientSecretIdentityViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateClientSecretIdentity(model);

            Assert.That(result.Model, Is.TypeOf<ClientSecretIdentityViewModel>());

            ClientSecretIdentityViewModel clientSecretIdentityViewModel = (ClientSecretIdentityViewModel) result.Model;

            Assert.That(clientSecretIdentityViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            ClientSecretIdentityViewModel model = CreateModel();
            await sut.UpdateClientSecretIdentity(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateClientSecretIdentityCommand>(command => command.Identifier == model.Identifier && string.CompareOrdinal(command.FriendlyName, model.FriendlyName) == 0 && command.Claims.Count() == model.Claims.Count(claimViewModel => claimViewModel.IsSelected))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            ClientSecretIdentityViewModel model = CreateModel();
            IActionResult result = await sut.UpdateClientSecretIdentity(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereContollerNameIsEqualToSecurity()
        {
            Controller sut = CreateSut();

            ClientSecretIdentityViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateClientSecretIdentity(model);

            Assert.That(result.ControllerName, Is.EqualTo("Security"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateClientSecretIdentity_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToClientSecretIdentities()
        {
            Controller sut = CreateSut();

            ClientSecretIdentityViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateClientSecretIdentity(model);

            Assert.That(result.ActionName, Is.EqualTo("ClientSecretIdentities"));
        }

        private Controller CreateSut(IEnumerable<Claim> claimCollection = null, bool hasClientSecretIdentity = true, IClientSecretIdentity clientSecretIdentity = null, bool modelIsValid = true)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<Claim>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => claimCollection ?? _fixture.CreateMany<Claim>(_random.Next(5, 10)).ToList()));
            _queryBusMock.Setup(m => m.QueryAsync<IGetClientSecretIdentityQuery, IClientSecretIdentity>(It.IsAny<IGetClientSecretIdentityQuery>()))
                .Returns(Task.Run(() => hasClientSecretIdentity ? clientSecretIdentity ?? _fixture.BuildClientSecretIdentityMock().Object : null));

            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IUpdateClientSecretIdentityCommand>()))
                .Returns(Task.Run(() => { }));

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
