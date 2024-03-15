using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.SecurityKeyBuilder
{
    [TestFixture]
    public class DisposeTests : SecurityKeyBuilderTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void Dispose_WhenCalled_ExpertNoError()
        {
            ISecurityKeyBuilder sut = CreateSut();

            sut.Dispose();
        }

        [Test]
        [Category("UnitTest")]
        public void Dispose_WhenCalledMultipleTimes_ExpertNoError()
        {
            ISecurityKeyBuilder sut = CreateSut();

            sut.Dispose();
            sut.Dispose();
            sut.Dispose();
        }
    }
}