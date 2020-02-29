using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using Sut=OSDevGrp.OSIntranet.Domain.Security.Token;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.Token
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
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.Create<Sut>((byte[]) null));

            Assert.That(result.ParamName, Is.EqualTo("byteArray"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithByteArray_AssertTokenIsDeserialized()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            DateTime expires = _fixture.Create<DateTime>().ToUniversalTime();
            byte[] byteArray = CreateByteArray(tokenType, accessToken, expires);

            IToken result = Sut.Create<Sut>(byteArray);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TokenType, Is.EqualTo(tokenType));
            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenBase64StringIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.Create<Sut>((string) null));

            Assert.That(result.ParamName, Is.EqualTo("base64String"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenBase64StringIsEmpty_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.Create<Sut>(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("base64String"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenBase64StringIsWhiteSpace_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.Create<Sut>(" "));

            Assert.That(result.ParamName, Is.EqualTo("base64String"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithBase64String_AssertTokenIsDeserialized()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            DateTime expires = _fixture.Create<DateTime>().ToUniversalTime();
            string base64String = CreateBase64String(tokenType, accessToken, expires);

            IToken result = Sut.Create<Sut>(base64String);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TokenType, Is.EqualTo(tokenType));
            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenTokenBasedQueryIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.Create((ITokenBasedQuery) null));

            Assert.That(result.ParamName, Is.EqualTo("tokenBasedQuery"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithTokenBasedQuery_AssertTokenTypeWasCalledOnTokenBasedQuery()
        {
            Mock<ITokenBasedQuery> tokenBasedQueryMock = _fixture.BuildTokenBasedQueryMock();

            Sut.Create(tokenBasedQueryMock.Object);

            tokenBasedQueryMock.Verify(m => m.TokenType, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithTokenBasedQuery_AssertAccessTokenWasCalledOnTokenBasedQuery()
        {
            Mock<ITokenBasedQuery> tokenBasedQueryMock = _fixture.BuildTokenBasedQueryMock();

            Sut.Create(tokenBasedQueryMock.Object);

            tokenBasedQueryMock.Verify(m => m.AccessToken, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithTokenBasedQuery_AssertExpiresWasCalledOnTokenBasedQuery()
        {
            Mock<ITokenBasedQuery> tokenBasedQueryMock = _fixture.BuildTokenBasedQueryMock();

            Sut.Create(tokenBasedQueryMock.Object);

            tokenBasedQueryMock.Verify(m => m.Expires, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithTokenBasedQuery_ReturnsTokenWithValuesFromTokenBasedQuery()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            DateTime expires = _fixture.Create<DateTime>();
            ITokenBasedQuery tokenBasedQuery = _fixture.BuildTokenBasedQueryMock(tokenType, accessToken, expires).Object;

            IToken result = Sut.Create(tokenBasedQuery);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TokenType, Is.EqualTo(tokenType));
            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenTokenBasedCommandIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.Create((ITokenBasedCommand) null));

            Assert.That(result.ParamName, Is.EqualTo("tokenBasedCommand"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithTokenBasedCommand_AssertTokenTypeWasCalledOnTokenBasedCommand()
        {
            Mock<ITokenBasedCommand> tokenBasedCommandMock = _fixture.BuildTokenBasedCommandMock();

            Sut.Create(tokenBasedCommandMock.Object);

            tokenBasedCommandMock.Verify(m => m.TokenType, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithTokenBasedCommand_AssertAccessTokenWasCalledOnTokenBasedCommand()
        {
            Mock<ITokenBasedCommand> tokenBasedCommandMock = _fixture.BuildTokenBasedCommandMock();

            Sut.Create(tokenBasedCommandMock.Object);

            tokenBasedCommandMock.Verify(m => m.AccessToken, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithTokenBasedCommand_AssertExpiresWasCalledOnTokenBasedCommand()
        {
            Mock<ITokenBasedCommand> tokenBasedCommandMock = _fixture.BuildTokenBasedCommandMock();

            Sut.Create(tokenBasedCommandMock.Object);

            tokenBasedCommandMock.Verify(m => m.Expires, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalledWithTokenBasedCommand_ReturnsTokenWithValuesFromTokenBasedCommand()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            DateTime expires = _fixture.Create<DateTime>();
            ITokenBasedCommand tokenBasedCommand = _fixture.BuildTokenBasedCommandMock(tokenType, accessToken, expires).Object;

            IToken result = Sut.Create(tokenBasedCommand);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TokenType, Is.EqualTo(tokenType));
            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        private byte[] CreateByteArray(string tokenType = null, string accessToken = null, DateTime? expires = null)
        {
            return new Domain.Security.Token(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), expires ?? _fixture.Create<DateTime>().ToUniversalTime()).ToByteArray();
        }

        private string CreateBase64String(string tokenType = null, string accessToken = null, DateTime? expires = null)
        {
            return new Domain.Security.Token(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), expires ?? _fixture.Create<DateTime>().ToUniversalTime()).ToBase64();
        }
    }
}