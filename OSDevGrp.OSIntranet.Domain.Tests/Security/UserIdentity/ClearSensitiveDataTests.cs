using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserIdentity
{
    [TestFixture]
    public class ClearSensitiveDataTests : UserIdentityTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ClearSensitiveData_WhenCalled_NoError()
        {
            IUserIdentity sut = CreateSut();

            sut.ClearSensitiveData();
        }
    }
}
