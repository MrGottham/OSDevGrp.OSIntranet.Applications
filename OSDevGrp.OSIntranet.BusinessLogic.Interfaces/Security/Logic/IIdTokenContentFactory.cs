using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface IIdTokenContentFactory
    {
        IIdTokenContentBuilder Create(string nameIdentifier, IUserInfo userInfo, DateTimeOffset authenticationTime, IReadOnlyDictionary<string, IScope> supportedScopes, IReadOnlyCollection<string> scopes);
    }
}