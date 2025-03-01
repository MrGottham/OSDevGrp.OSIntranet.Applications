using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.MyFirstCommand;

public class MyFirstCommandRequest : IRequest
{
    #region Properties

    public Guid RequestId => throw new NotImplementedException();

    public ISecurityContext SecurityContext => throw new NotImplementedException();

    #endregion
}