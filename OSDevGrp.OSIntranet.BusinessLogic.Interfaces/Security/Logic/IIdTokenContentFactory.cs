using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface IIdTokenContentFactory
    {
        IIdTokenContentBuilder Create(string subjectIdentifier, IUserInfo userInfo, DateTimeOffset authenticationTime);
    }
}