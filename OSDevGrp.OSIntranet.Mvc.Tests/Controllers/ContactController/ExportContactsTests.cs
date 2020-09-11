using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class ExportContactsTests
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
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContacts_WhenCalled_AssertGetTokenAsyncWasCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.ExportContacts();

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContacts_WhenNoTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasNotCalledOnQueryBus()
        {
            Controller sut = CreateSut(false);

            await sut.ExportContacts();

            _queryBusMock.Verify(m => m.QueryAsync<IExportContactCollectionQuery, byte[]>(It.IsAny<IExportContactCollectionQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContacts_WhenNoTokenWasReturnedFromTokenHelperFactory_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.ExportContacts();

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContacts_WhenTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasCalledOnQueryBus()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = DateTime.Now.AddMinutes(_random.Next(5, 60));
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(tokenType, accessToken, refreshToken, expires).Object;
            Controller sut = CreateSut(refreshableToken: refreshableToken);

            await sut.ExportContacts();

            _queryBusMock.Verify(m => m.QueryAsync<IExportContactCollectionQuery, byte[]>(It.Is<IExportContactCollectionQuery>(value => value != null && string.CompareOrdinal(value.TokenType, tokenType) == 0 && string.CompareOrdinal(value.AccessToken, accessToken) == 0 && string.CompareOrdinal(value.RefreshToken, refreshToken) == 0 && value.Expires == expires)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContacts_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsFileContentResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ExportContacts();

            Assert.That(result, Is.TypeOf<FileContentResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContacts_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsFileContentResultWhereContentTypeIsEqualToApplicationCsv()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult) await sut.ExportContacts();

            Assert.That(result.ContentType, Is.EqualTo("application/csv"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContacts_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsFileContentResultWhereFileDownloadNameIsEqualToContactsCsv()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult) await sut.ExportContacts();

            Assert.That(result.FileDownloadName, Is.EqualTo("Contacts.csv"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExportContacts_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsFileContentResultWhereFileContentsIsEqualToByteArrayFromQueryBus()
        {
            byte[] byteCollection = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
            Controller sut = CreateSut(byteCollection: byteCollection);

            FileContentResult result = (FileContentResult) await sut.ExportContacts();

            Assert.That(result.FileContents, Is.EqualTo(byteCollection));
        }

        private Controller CreateSut(bool hasRefreshableToken = true, IRefreshableToken refreshableToken = null, byte[] byteCollection = null)
        {
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.FromResult(hasRefreshableToken ? refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object : null));

            _queryBusMock.Setup(m => m.QueryAsync<IExportContactCollectionQuery, byte[]>(It.IsAny<IExportContactCollectionQuery>()))
                .Returns(Task.FromResult(byteCollection ?? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}