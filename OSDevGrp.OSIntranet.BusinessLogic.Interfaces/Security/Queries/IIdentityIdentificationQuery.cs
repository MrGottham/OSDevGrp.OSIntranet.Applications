﻿using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries
{
    public interface IIdentityIdentificationQuery : IQuery
    {
        int IdentityIdentifier { get; set; }
    }
}