﻿using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
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
        public void Create_WhenBase64StringIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.Create<Sut>(null));

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
        public void Create_WhenCalled_AssertTokenIsDeserialized()
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

        private string CreateBase64String(string tokenType = null, string accessToken = null, DateTime? expires = null)
        {
            return new Domain.Security.Token(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), expires ?? _fixture.Create<DateTime>().ToUniversalTime()).ToBase64();
        }
    }
}