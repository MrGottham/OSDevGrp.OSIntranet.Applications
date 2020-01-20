using System;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Queries
{
    public interface ITokenBasedQuery : IQuery
    {
        string TokenType { get; set; }

        string AccessToken { get; set; }

        DateTime Expires { get; set; }
    }
}