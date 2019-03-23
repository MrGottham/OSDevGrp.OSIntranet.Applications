using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ClientSecretIdentityBuilder
{
    [TestFixture]
    public class WithClientIdTests : ClientSecretIdentityBuilderTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void WithClientId_WhenClientIdIsNull_ThrowsArgumentNullException()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithClientId(null));

            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithClientId_WhenClientIdIsEmpty_ThrowsArgumentNullException()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithClientId(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithClientId_WhenClientIdIsWhiteSpace_ThrowsArgumentNullException()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithClientId(" "));

            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithClientId_WhenCalled_ReturnsClientSecretIdentityBuilder()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            IClientSecretIdentityBuilder result = sut.WithClientId(Fixture.Create<string>());

            Assert.That(result, Is.EqualTo(sut));
        }
    }
}
