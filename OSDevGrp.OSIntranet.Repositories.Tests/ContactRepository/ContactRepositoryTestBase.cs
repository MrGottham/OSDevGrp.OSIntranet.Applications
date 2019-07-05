using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    public abstract class ContactRepositoryTestBase : RepositoryTestBase
    {
        protected IContactRepository CreateSut()
        {
            return new Repositories.ContactRepository(CreateTestConfiguration(), CreatePrincipalResolverMock().Object, CreateLoggerFactory());
        }
    }
}
