using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries
{
    public interface IIdentityIdentificationQuery : IQuery
    {
        int Identifier { get; set; }

        IValidator Validate(IValidator validator, ISecurityRepository securityRepository);
    }
}
