using System.Security.Principal;
using System.Threading;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;

namespace OSDevGrp.OSIntranet.Core.Tests.Resolvers.ThreadPrincipalResolver
{
    [TestFixture]
    public class GetCurrentPrincipalTests
    {
        [Test]
        [Category("UnitTest")]
        public void GetCurrentPrincipal_WhenCalledAndCurrentPrincipalOnThreadIsNull_ReturnsNull()
        {
            IPrincipalResolver sut = CreateSut();

            IPrincipal currentPrincipal = Thread.CurrentPrincipal;
            try
            {
                Thread.CurrentPrincipal = null;

                IPrincipal result = sut.GetCurrentPrincipal();

                Assert.That(result, Is.Null);
            }
            finally
            {
                Thread.CurrentPrincipal = currentPrincipal;
            }
        }

        [Test]
        [Category("UnitTest")]
        public void GetCurrentPrincipal_WhenCalledAndCurrentPrincipalOnThreadIsNotNull_ReturnsPrincipalFromThread()
        {
            IPrincipalResolver sut = CreateSut();

            IPrincipal currentPrincipal = Thread.CurrentPrincipal;
            try
            {
                IPrincipal principal = CreatePrincipalMock().Object;
                Thread.CurrentPrincipal = principal;

                IPrincipal result = sut.GetCurrentPrincipal();

                Assert.That(result, Is.EqualTo(principal));
            }
            finally
            {
                Thread.CurrentPrincipal = currentPrincipal;
            }
        }

        private IPrincipalResolver CreateSut()
        {
            return new Core.Resolvers.ThreadPrincipalResolver();
        }

        private Mock<IPrincipal> CreatePrincipalMock()
        {
            return new Mock<IPrincipal>();
        }
    }
}