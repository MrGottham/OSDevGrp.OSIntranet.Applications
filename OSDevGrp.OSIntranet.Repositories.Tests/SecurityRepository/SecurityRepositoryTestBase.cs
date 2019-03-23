using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.SecurityRepository
{
    public abstract class SecurityRepositoryTestBase : RepositoryTestBase
    {
        protected ISecurityRepository CreateSut()
        {
            return new Repositories.SecurityRepository(CreateTestConfiguration());
        }
    }
}
