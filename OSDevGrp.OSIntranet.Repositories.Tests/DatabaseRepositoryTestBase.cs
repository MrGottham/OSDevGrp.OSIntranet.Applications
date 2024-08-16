using Moq;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.Repositories.Tests
{
    public abstract class DatabaseRepositoryTestBase : RepositoryTestBase
    {
        #region Methods

        protected RepositoryContext CreateTestRepositoryContext(IPrincipal principal = null)
        {
            RepositoryContext repositoryContext = RepositoryContext.Create(
                CreateTestConfiguration(),
                CreatePrincipalResolverMock(principal).Object,
                CreateLoggerFactory());

            RegisterDisposable(repositoryContext);

            return repositoryContext;
        }

        private Mock<IPrincipalResolver> CreatePrincipalResolverMock(IPrincipal principal = null)
        {
            Mock<IPrincipalResolver> principalResolverMock = new Mock<IPrincipalResolver>();
            principalResolverMock.Setup(m => m.GetCurrentPrincipal())
                .Returns(principal ?? CreateClaimsPrincipal());
            return principalResolverMock;
        }

        private ClaimsPrincipal CreateClaimsPrincipal()
        {
            Claim nameClaim = new Claim(ClaimTypes.Name, "OSDevGrp.OSIntranet.Repositories.Tests");

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { nameClaim });

            return new ClaimsPrincipal(claimsIdentity);
        }

        #endregion
    }
}