using System;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries
{
    public interface IGetAuthorizeUriForMicrosoftGraphQuery : IQuery
    {
        Uri RedirectUri { get; }

        Guid StateIdentifier { get; }
    }
}
