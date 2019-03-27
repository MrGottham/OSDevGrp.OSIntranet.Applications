using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ClientSecretIdentity
{
    [TestFixture]
    public class AddTokenTests : ClientSecretIdentityTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void AddToken_WhenTokenIsNull_ThrowsArgumentNullException()
        {
            IClientSecretIdentity sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddToken(null));

            Assert.That(result.ParamName, Is.EqualTo("token"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddToken_WhenTokenIsEmpty_ThrowsArgumentNullException()
        {
            IClientSecretIdentity sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddToken(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("token"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddToken_WhenTokenIsWhiteSpace_ThrowsArgumentNullException()
        {
            IClientSecretIdentity sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddToken(" "));

            Assert.That(result.ParamName, Is.EqualTo("token"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddToken_WhenCalled_AddsToken()
        {
            IClientSecretIdentity sut = CreateSut();

            string token = Fixture.Create<string>();
            sut.AddToken(token);

            Assert.That(sut.Token, Is.EqualTo(token));
        }
    }
}
