using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ClientSecretIdentityBuilder
{
    [TestFixture]
    public class WithClientSecretTests : ClientSecretIdentityBuilderTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void WithClientSecret_WhenClientSecretIsNull_ThrowsArgumentNullException()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithClientSecret(null));

            Assert.That(result.ParamName, Is.EqualTo("clientSecret"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithClientSecret_WhenClientSecretIsEmpty_ThrowsArgumentNullException()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithClientSecret(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("clientSecret"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithClientSecret_WhenClientSecretIsWhiteSpace_ThrowsArgumentNullException()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithClientSecret(" "));

            Assert.That(result.ParamName, Is.EqualTo("clientSecret"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithClientSecret_WhenCalled_ReturnsClientSecretIdentityBuilder()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            IClientSecretIdentityBuilder result = sut.WithClientSecret(Fixture.Create<string>());

            Assert.That(result, Is.EqualTo(sut));
        }
    }
}
