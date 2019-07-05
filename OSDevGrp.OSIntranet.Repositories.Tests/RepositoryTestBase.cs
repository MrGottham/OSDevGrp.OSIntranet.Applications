using System.Security.Principal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;

namespace OSDevGrp.OSIntranet.Repositories.Tests
{
    public abstract class RepositoryTestBase
    {
        #region Methods

        protected IConfiguration CreateTestConfiguration()
        {
            return new ConfigurationBuilder()
                .AddUserSecrets<RepositoryTestBase>()
                .Build();
        }

        protected Mock<IPrincipalResolver> CreatePrincipalResolverMock(IPrincipal principal = null)
        {
            Mock<IPrincipalResolver> principalResolverMock = new Mock<IPrincipalResolver>();
            principalResolverMock.Setup(m => m.GetCurrentPrincipal())
                .Returns(principal ?? new Mock<IPrincipal>().Object);
            return principalResolverMock;
        }

        protected ILoggerFactory CreateLoggerFactory()
        {
            return NullLoggerFactory.Instance;
        }

        #endregion
    }
}
