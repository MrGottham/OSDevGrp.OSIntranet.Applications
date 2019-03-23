using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ClientSecretIdentityBuilder
{
    [TestFixture]
    public class WithIdentifierTests : ClientSecretIdentityBuilderTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void WithIdentifier_WhenCalled_ReturnsClientSecretIdentityBuilder()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            IClientSecretIdentityBuilder result = sut.WithIdentifier(Fixture.Create<int>());

            Assert.That(result, Is.EqualTo(sut));
        }
    }
}
