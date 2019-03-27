using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ClientSecretIdentity
{
    [TestFixture]
    public class ClearSensitiveDataTests : ClientSecretIdentityTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ClearSensitiveData_WhenCalled_AssertClientSecretIsNull()
        {
            IClientSecretIdentity sut = CreateSut();

            sut.ClearSensitiveData();

            Assert.That(sut.ClientSecret, Is.Null);
        }
    }
}
