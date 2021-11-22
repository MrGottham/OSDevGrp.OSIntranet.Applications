using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    public abstract class ContactRepositoryTestBase : DatabaseRepositoryTestBase
    {
        protected IContactRepository CreateSut()
        {
            return new Repositories.ContactRepository(CreateTestRepositoryContext(), CreateTestConfiguration(), CreateLoggerFactory());
        }
    }
}