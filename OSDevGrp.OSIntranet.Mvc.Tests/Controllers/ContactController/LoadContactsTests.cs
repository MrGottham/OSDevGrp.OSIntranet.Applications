using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
	[TestFixture]
    public class LoadContactsTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();

            _fixture = new Fixture();
            _fixture.Customize<ICompany>(builder => builder.FromFactory(() => _fixture.BuildCompanyMock().Object));
            _fixture.Customize<IContactGroup>(builder => builder.FromFactory(() => _fixture.BuildContactGroupMock().Object));
            _fixture.Customize<IPaymentTerm>(builder => builder.FromFactory(() => _fixture.BuildPaymentTermMock().Object));
            _fixture.Customize<IContact>(builder => builder.FromFactory(() => _fixture.BuildContactMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenCalled_AssertGetTokenAsyncWasCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.LoadContacts();

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenNoTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasNotCalledOnQueryBusWithGetContactCollectionQuery()
        {
            Controller sut = CreateSut(false);

            await sut.LoadContacts();

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetContactCollectionQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenNoTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMatchingContactCollectionQuery()
        {
            Controller sut = CreateSut(false);

            await sut.LoadContacts();

            _queryBusMock.Verify(m => m.QueryAsync<IGetMatchingContactCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetMatchingContactCollectionQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenNoTokenWasReturnedFromTokenHelperFactory_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.LoadContacts();

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactory_AssertTokenTypeWasCalledOnTokenFromTokenHelperFactory()
        {
            Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock();
            Controller sut = CreateSut(refreshableToken: refreshableTokenMock.Object);

            await sut.LoadContacts();

            refreshableTokenMock.Verify(m => m.TokenType, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactory_AssertAccessTokenWasCalledOnTokenFromTokenHelperFactory()
        {
            Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock();
            Controller sut = CreateSut(refreshableToken: refreshableTokenMock.Object);

            await sut.LoadContacts();

            refreshableTokenMock.Verify(m => m.AccessToken, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactory_AssertRefreshTokenWasCalledOnTokenFromTokenHelperFactory()
        {
            Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock();
            Controller sut = CreateSut(refreshableToken: refreshableTokenMock.Object);

            await sut.LoadContacts();

            refreshableTokenMock.Verify(m => m.RefreshToken, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactory_AssertExpiresWasCalledOnTokenFromTokenHelperFactory()
        {
            Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock();
            Controller sut = CreateSut(refreshableToken: refreshableTokenMock.Object);

            await sut.LoadContacts();

            refreshableTokenMock.Verify(m => m.Expires, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsNull_AssertQueryAsyncWasCalledOnQueryBusWithGetContactCollectionQuery()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = DateTime.Now.AddMinutes(_random.Next(5, 60));
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(tokenType, accessToken, refreshToken, expires).Object;
            Controller sut = CreateSut(refreshableToken: refreshableToken);

            await sut.LoadContacts();

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactCollectionQuery, IEnumerable<IContact>>(It.Is<IGetContactCollectionQuery>(value => value != null && string.CompareOrdinal(value.TokenType, tokenType) == 0 && string.CompareOrdinal(value.AccessToken, accessToken) == 0 && string.CompareOrdinal(value.RefreshToken, refreshToken) == 0 && value.Expires == expires)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsNull_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMatchingContactCollectionQuery()
        {
            Controller sut = CreateSut();

            await sut.LoadContacts();

            _queryBusMock.Verify(m => m.QueryAsync<IGetMatchingContactCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetMatchingContactCollectionQuery>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsNull_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.LoadContacts();

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsNull_ReturnsPartialViewResultWhereViewNameIsEqualToContactCollectionPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadContacts();

            Assert.That(result.ViewName, Is.EqualTo("_ContactCollectionPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsNull_ReturnsPartialViewResultWhereModelIsCollectionOfContactInfoViewModel()
        {
            IEnumerable<IContact> contactCollection = _fixture.CreateMany<IContact>(_random.Next(25, 50)).ToList();
            Controller sut = CreateSut(contactCollection: contactCollection);

            PartialViewResult result = (PartialViewResult) await sut.LoadContacts();

            Assert.That(result.Model, Is.TypeOf<List<ContactInfoViewModel>>());

            List<ContactInfoViewModel> contactInfoViewModelCollection = (List<ContactInfoViewModel>) result.Model;

            Assert.That(contactInfoViewModelCollection, Is.Not.Null);
            Assert.That(contactInfoViewModelCollection.All(contactInfoViewModel => contactCollection.Any(contact => string.CompareOrdinal(contact.ExternalIdentifier, contactInfoViewModel.ExternalIdentifier) == 0)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsEmpty_AssertQueryAsyncWasCalledOnQueryBusWithGetContactCollectionQuery()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = DateTime.Now.AddMinutes(_random.Next(5, 60));
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(tokenType, accessToken, refreshToken, expires).Object;
            Controller sut = CreateSut(refreshableToken: refreshableToken);

            await sut.LoadContacts(string.Empty);

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactCollectionQuery, IEnumerable<IContact>>(It.Is<IGetContactCollectionQuery>(value => value != null && string.CompareOrdinal(value.TokenType, tokenType) == 0 && string.CompareOrdinal(value.AccessToken, accessToken) == 0 && string.CompareOrdinal(value.RefreshToken, refreshToken) == 0 && value.Expires == expires)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsEmpty_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMatchingContactCollectionQuery()
        {
            Controller sut = CreateSut();

            await sut.LoadContacts(string.Empty);

            _queryBusMock.Verify(m => m.QueryAsync<IGetMatchingContactCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetMatchingContactCollectionQuery>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsEmpty_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.LoadContacts(string.Empty);

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsEmpty_ReturnsPartialViewResultWhereViewNameIsEqualToContactCollectionPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadContacts(string.Empty);

            Assert.That(result.ViewName, Is.EqualTo("_ContactCollectionPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsEmpty_ReturnsPartialViewResultWhereModelIsCollectionOfContactInfoViewModel()
        {
            IEnumerable<IContact> contactCollection = _fixture.CreateMany<IContact>(_random.Next(25, 50)).ToList();
            Controller sut = CreateSut(contactCollection: contactCollection);

            PartialViewResult result = (PartialViewResult) await sut.LoadContacts(string.Empty);

            Assert.That(result.Model, Is.TypeOf<List<ContactInfoViewModel>>());

            List<ContactInfoViewModel> contactInfoViewModelCollection = (List<ContactInfoViewModel>) result.Model;

            Assert.That(contactInfoViewModelCollection, Is.Not.Null);
            Assert.That(contactInfoViewModelCollection.All(contactInfoViewModel => contactCollection.Any(contact => string.CompareOrdinal(contact.ExternalIdentifier, contactInfoViewModel.ExternalIdentifier) == 0)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsWhiteSpace_AssertQueryAsyncWasCalledOnQueryBusWithGetContactCollectionQuery()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = DateTime.Now.AddMinutes(_random.Next(5, 60));
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(tokenType, accessToken, refreshToken, expires).Object;
            Controller sut = CreateSut(refreshableToken: refreshableToken);

            await sut.LoadContacts(" ");

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactCollectionQuery, IEnumerable<IContact>>(It.Is<IGetContactCollectionQuery>(value => value != null && string.CompareOrdinal(value.TokenType, tokenType) == 0 && string.CompareOrdinal(value.AccessToken, accessToken) == 0 && string.CompareOrdinal(value.RefreshToken, refreshToken) == 0 && value.Expires == expires)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsWhiteSpace_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMatchingContactCollectionQuery()
        {
            Controller sut = CreateSut();

            await sut.LoadContacts(" ");

            _queryBusMock.Verify(m => m.QueryAsync<IGetMatchingContactCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetMatchingContactCollectionQuery>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsWhiteSpace_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.LoadContacts(" ");

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsWhiteSpace_ReturnsPartialViewResultWhereViewNameIsEqualToContactCollectionPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadContacts(" ");

            Assert.That(result.ViewName, Is.EqualTo("_ContactCollectionPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsWhiteSpace_ReturnsPartialViewResultWhereModelIsCollectionOfContactInfoViewModel()
        {
            IEnumerable<IContact> contactCollection = _fixture.CreateMany<IContact>(_random.Next(25, 50)).ToList();
            Controller sut = CreateSut(contactCollection: contactCollection);

            PartialViewResult result = (PartialViewResult) await sut.LoadContacts(" ");

            Assert.That(result.Model, Is.TypeOf<List<ContactInfoViewModel>>());

            List<ContactInfoViewModel> contactInfoViewModelCollection = (List<ContactInfoViewModel>) result.Model;

            Assert.That(contactInfoViewModelCollection, Is.Not.Null);
            Assert.That(contactInfoViewModelCollection.All(contactInfoViewModel => contactCollection.Any(contact => string.CompareOrdinal(contact.ExternalIdentifier, contactInfoViewModel.ExternalIdentifier) == 0)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsNotNullEmptyOrWhiteSpace_AssertQueryAsyncWasNotCalledOnQueryBusWithGetContactCollectionQuery()
        {
            Controller sut = CreateSut();

            string filter = _fixture.Create<string>();
            await sut.LoadContacts(filter);

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetContactCollectionQuery>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsNotNullEmptyOrWhiteSpace_AssertQueryAsyncWasCalledOnQueryBusWithGetMatchingContactCollectionQuery()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = DateTime.Now.AddMinutes(_random.Next(5, 60));
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(tokenType, accessToken, refreshToken, expires).Object;
            Controller sut = CreateSut(refreshableToken: refreshableToken);

            string filter = _fixture.Create<string>();
            await sut.LoadContacts(filter);

            _queryBusMock.Verify(m => m.QueryAsync<IGetMatchingContactCollectionQuery, IEnumerable<IContact>>(It.Is<IGetMatchingContactCollectionQuery>(value => value != null && string.CompareOrdinal(value.TokenType, tokenType) == 0 && string.CompareOrdinal(value.AccessToken, accessToken) == 0 && string.CompareOrdinal(value.RefreshToken, refreshToken) == 0 && value.Expires == expires && string.CompareOrdinal(value.SearchFor, filter) == 0 && value.SearchWithinName && value.SearchWithinMailAddress && value.SearchWithinPrimaryPhone == false && value.SearchWithinSecondaryPhone == false && value.SearchWithinHomePhone && value.SearchWithinMobilePhone)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            string filter = _fixture.Create<string>();
            IActionResult result = await sut.LoadContacts(filter);

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereViewNameIsEqualToContactCollectionPartial()
        {
            Controller sut = CreateSut();

            string filter = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult) await sut.LoadContacts(filter);

            Assert.That(result.ViewName, Is.EqualTo("_ContactCollectionPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereModelIsCollectionOfContactInfoViewModel()
        {
            IEnumerable<IContact> contactCollection = _fixture.CreateMany<IContact>(_random.Next(25, 50)).ToList();
            Controller sut = CreateSut(contactCollection: contactCollection);

            string filter = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult) await sut.LoadContacts(filter);

            Assert.That(result.Model, Is.TypeOf<List<ContactInfoViewModel>>());

            List<ContactInfoViewModel> contactInfoViewModelCollection = (List<ContactInfoViewModel>) result.Model;

            Assert.That(contactInfoViewModelCollection, Is.Not.Null);
            Assert.That(contactInfoViewModelCollection.All(contactInfoViewModel => contactCollection.Any(contact => string.CompareOrdinal(contact.ExternalIdentifier, contactInfoViewModel.ExternalIdentifier) == 0)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndExternalIdentifierIsNull_ReturnsPartialViewResultWhereViewDataDoesNotContainExternalIdentifier()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadContacts();

            Assert.That(result.ViewData.ContainsKey("ExternalIdentifier"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndExternalIdentifierIsEmpty_ReturnsPartialViewResultWhereViewDataDoesNotContainExternalIdentifier()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadContacts(externalIdentifier: string.Empty);

            Assert.That(result.ViewData.ContainsKey("ExternalIdentifier"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndExternalIdentifierIsWhiteSpace_ReturnsPartialViewResultWhereViewDataDoesNotContainExternalIdentifier()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadContacts(externalIdentifier: " ");

            Assert.That(result.ViewData.ContainsKey("ExternalIdentifier"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndExternalIdentifierIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereViewDataContainingExternalIdentifier()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.LoadContacts(externalIdentifier: _fixture.Create<string>());

            Assert.That(result.ViewData.ContainsKey("ExternalIdentifier"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndExternalIdentifierIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereViewDataContainingExternalIdentifierWithValueEqualToExternalIdentifierArgument()
        {
            Controller sut = CreateSut();

            string externalIdentifier = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult) await sut.LoadContacts(externalIdentifier: externalIdentifier);

            Assert.That(result.ViewData["ExternalIdentifier"], Is.EqualTo(externalIdentifier));
        }

        private Controller CreateSut(bool hasRefreshableToken = true, IRefreshableToken refreshableToken = null, IEnumerable<IContact> contactCollection = null)
        {
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.Run(() => hasRefreshableToken ? refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object : null));
            _queryBusMock.Setup(m => m.QueryAsync<IGetContactCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetContactCollectionQuery>()))
                .Returns(Task.Run(() => contactCollection ?? _fixture.CreateMany<IContact>(_random.Next(15, 25)).ToList()));
            _queryBusMock.Setup(m => m.QueryAsync<IGetMatchingContactCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetMatchingContactCollectionQuery>()))
                .Returns(Task.Run(() => contactCollection ?? _fixture.CreateMany<IContact>(_random.Next(15, 25)).ToList()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}