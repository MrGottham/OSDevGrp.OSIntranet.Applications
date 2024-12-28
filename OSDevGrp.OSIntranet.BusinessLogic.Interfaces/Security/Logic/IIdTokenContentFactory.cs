using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface IIdTokenContentFactory
    {
        IIdTokenContentBuilder Create(string subjectIdentifier, DateTimeOffset authenticationTime);
    }
}