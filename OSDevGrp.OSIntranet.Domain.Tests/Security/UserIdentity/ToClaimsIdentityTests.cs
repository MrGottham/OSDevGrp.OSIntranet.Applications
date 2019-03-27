using System.Security.Claims;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserIdentity
{
    [TestFixture]
    public class ToClaimsIdentityTests : UserIdentityTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaimsIdentity_WhenCalled_ReturnsClaimsIdentity()
        {
            IUserIdentity sut = CreateSut();

            ClaimsIdentity result = sut.ToClaimsIdentity();

            Assert.That(result, Is.EqualTo(sut));
        }
    }
}
