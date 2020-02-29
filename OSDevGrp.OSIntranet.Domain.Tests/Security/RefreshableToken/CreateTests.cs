using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using Sut=OSDevGrp.OSIntranet.Domain.Security.RefreshableToken;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableToken
{
    [TestFixture]
    public class CreateTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenByteArrayIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.Token.Create<Sut>((byte[]) null));

            Assert.That(result.ParamName, Is.EqualTo("byteArray"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithByteArray_AssertRefreshableTokenIsDeserialized()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = _fixture.Create<DateTime>().ToUniversalTime();
            byte[] byteArray = CreateByteArray(tokenType, accessToken, refreshToken, expires);

            IRefreshableToken result = Domain.Security.Token.Create<Sut>(byteArray);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TokenType, Is.EqualTo(tokenType));
            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
            Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenBase64StringIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.Token.Create<Sut>((string) null));

            Assert.That(result.ParamName, Is.EqualTo("base64String"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenBase64StringIsEmpty_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.Token.Create<Sut>(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("base64String"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenBase64StringIsWhiteSpace_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.Token.Create<Sut>(" "));

            Assert.That(result.ParamName, Is.EqualTo("base64String"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithBase64String_AssertRefreshableTokenIsDeserialized()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = _fixture.Create<DateTime>().ToUniversalTime();
            string base64String = CreateBase64String(tokenType, accessToken, refreshToken, expires);

            IRefreshableToken result = Domain.Security.Token.Create<Sut>(base64String);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TokenType, Is.EqualTo(tokenType));
            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
            Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenRefreshableTokenBasedQueryIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.Create((IRefreshableTokenBasedQuery) null));

            Assert.That(result.ParamName, Is.EqualTo("refreshableTokenBasedQuery"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithRefreshableTokenBasedQuery_AssertTokenTypeWasCalledOnRefreshableTokenBasedQuery()
        {
            Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = _fixture.BuildRefreshableTokenBasedQueryMock();

            Sut.Create(refreshableTokenBasedQueryMock.Object);

            refreshableTokenBasedQueryMock.Verify(m => m.TokenType, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithRefreshableTokenBasedQuery_AssertAccessTokenWasCalledOnRefreshableTokenBasedQuery()
        {
            Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = _fixture.BuildRefreshableTokenBasedQueryMock();

            Sut.Create(refreshableTokenBasedQueryMock.Object);

            refreshableTokenBasedQueryMock.Verify(m => m.AccessToken, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithRefreshableTokenBasedQuery_AssertRefreshTokenWasCalledOnRefreshableTokenBasedQuery()
        {
            Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = _fixture.BuildRefreshableTokenBasedQueryMock();

            Sut.Create(refreshableTokenBasedQueryMock.Object);

            refreshableTokenBasedQueryMock.Verify(m => m.RefreshToken, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithRefreshableTokenBasedQuery_AssertExpiresWasCalledOnRefreshableTokenBasedQuery()
        {
            Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = _fixture.BuildRefreshableTokenBasedQueryMock();

            Sut.Create(refreshableTokenBasedQueryMock.Object);

            refreshableTokenBasedQueryMock.Verify(m => m.Expires, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithRefreshableTokenBasedQuery_ReturnsRefreshableTokenWithValuesFromRefreshableTokenBasedQuery()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = _fixture.Create<DateTime>();
            IRefreshableTokenBasedQuery refreshableTokenBasedQuery = _fixture.BuildRefreshableTokenBasedQueryMock(tokenType, accessToken, refreshToken, expires).Object;

            IRefreshableToken result = Sut.Create(refreshableTokenBasedQuery);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TokenType, Is.EqualTo(tokenType));
            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
            Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenRefreshableTokenBasedCommandIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.Create((IRefreshableTokenBasedCommand) null));

            Assert.That(result.ParamName, Is.EqualTo("refreshableTokenBasedCommand"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithRefreshableTokenBasedCommand_AssertTokenTypeWasCalledOnRefreshableTokenBasedCommand()
        {
            Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = _fixture.BuildRefreshableTokenBasedCommandMock();

            Sut.Create(refreshableTokenBasedCommandMock.Object);

            refreshableTokenBasedCommandMock.Verify(m => m.TokenType, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithRefreshableTokenBasedCommand_AssertAccessTokenWasCalledOnRefreshableTokenBasedCommand()
        {
            Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = _fixture.BuildRefreshableTokenBasedCommandMock();

            Sut.Create(refreshableTokenBasedCommandMock.Object);

            refreshableTokenBasedCommandMock.Verify(m => m.AccessToken, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithRefreshableTokenBasedCommand_AssertRefreshTokenWasCalledOnRefreshableTokenBasedCommand()
        {
            Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = _fixture.BuildRefreshableTokenBasedCommandMock();

            Sut.Create(refreshableTokenBasedCommandMock.Object);

            refreshableTokenBasedCommandMock.Verify(m => m.RefreshToken, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithRefreshableTokenBasedCommand_AssertExpiresWasCalledOnRefreshableTokenBasedCommand()
        {
            Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = _fixture.BuildRefreshableTokenBasedCommandMock();

            Sut.Create(refreshableTokenBasedCommandMock.Object);

            refreshableTokenBasedCommandMock.Verify(m => m.Expires, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithRefreshableTokenBasedCommand_ReturnsRefreshableTokenWithValuesFromRefreshableTokenBasedCommand()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = _fixture.Create<DateTime>();
            IRefreshableTokenBasedCommand refreshableTokenBasedCommand = _fixture.BuildRefreshableTokenBasedCommandMock(tokenType, accessToken, refreshToken, expires).Object;

            IRefreshableToken result = Sut.Create(refreshableTokenBasedCommand);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TokenType, Is.EqualTo(tokenType));
            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
            Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        private byte[] CreateByteArray(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
        {
            return new Domain.Security.RefreshableToken(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), refreshToken ?? _fixture.Create<string>(), expires ?? _fixture.Create<DateTime>().ToUniversalTime()).ToByteArray();
        }

        private string CreateBase64String(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
        {
            return new Domain.Security.RefreshableToken(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), refreshToken ?? _fixture.Create<string>(), expires ?? _fixture.Create<DateTime>().ToUniversalTime()).ToBase64();
        }
    }
}