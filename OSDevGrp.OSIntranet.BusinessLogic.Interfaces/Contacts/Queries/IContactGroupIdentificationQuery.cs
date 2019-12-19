using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries
{
    public interface IContactGroupIdentificationQuery : IQuery
    {
        int Number { get; set; }

        IValidator Validate(IValidator validator, IContactRepository contactRepository);
    }
}
