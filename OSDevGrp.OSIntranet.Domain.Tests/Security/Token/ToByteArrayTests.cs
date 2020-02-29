using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.Token
{
    [TestFixture]
    public class ToByteArrayTests
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
        public void ToByteArray_WhenCalled_AssertTokenIsSerialized()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            DateTime expires = _fixture.Create<DateTime>().ToUniversalTime();
            IToken sut = CreateSut(tokenType, accessToken, expires);

            byte[] byteArray = sut.ToByteArray();

            IToken result = Domain.Security.Token.Create<Domain.Security.Token>(byteArray);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TokenType, Is.EqualTo(tokenType));
            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        [Test]
        [Category("UnitTest")]
        public void ToByteArray_WhenCalled_ReturnsByteArray()
        {
            IToken sut = CreateSut();

            byte[] result = sut.ToByteArray();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        private IToken CreateSut(string tokenType = null, string accessToken = null, DateTime? expires = null)
        {
            return new Domain.Security.Token(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), expires ?? _fixture.Create<DateTime>().ToUniversalTime());
        }
    }
}