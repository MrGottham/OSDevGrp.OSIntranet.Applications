using System.Security.Principal;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;

namespace OSDevGrp.OSIntranet.Core.Tests.Resolvers.GenericPrincipalResolver
{
    [TestFixture]
    public class GetCurrentPrincipalTests
    {
        [Test]
        [Category("UnitTest")]
        public void GetCurrentPrincipal_WhenCalledAndCurrentPrincipalIsNull_ReturnsNull()
        {
            const IPrincipal currentPrincipal = null;
            IPrincipalResolver sut = CreateSut(currentPrincipal);
            
            IPrincipal result = sut.GetCurrentPrincipal();
            
            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetCurrentPrincipal_WhenCalledAndCurrentPrincipalIsNotNull_ReturnsCurrentPrincipal()
        {
            IPrincipal currentPrincipal = CreatePrincipalMock().Object;
            IPrincipalResolver sut = CreateSut(currentPrincipal);
            
            IPrincipal result = sut.GetCurrentPrincipal();
            
            Assert.That(result, Is.EqualTo(currentPrincipal));
        }

        private IPrincipalResolver CreateSut(IPrincipal currentPrincipal)
        {
            return new Core.Resolvers.GenericPrincipalResolver(currentPrincipal);
        }

        private Mock<IPrincipal> CreatePrincipalMock()
        {
            return new Mock<IPrincipal>();
        }
    }
}