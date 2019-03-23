using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserIdentityBuilder
{
    [TestFixture]
    public class WithIdentifierTests : UserIdentityBuilderTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void WithIdentifier_WhenCalled_ReturnsUserIdentityBuilder()
        {
            IUserIdentityBuilder sut = CreateSut();

            IUserIdentityBuilder result = sut.WithIdentifier(Fixture.Create<int>());

            Assert.That(result, Is.EqualTo(sut));
        }
    }
}
