using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    public abstract class AccountingRepositoryTestBase : RepositoryTestBase
    {
        protected IAccountingRepository CreateSut()
        {
            return new Repositories.AccountingRepository(CreateTestConfiguration(), CreatePrincipalResolverMock().Object);
        }
    }
}
