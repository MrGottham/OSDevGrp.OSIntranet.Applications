using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal class IdTokenContentFactory : IIdTokenContentFactory
    {
        #region Methods

        public IIdTokenContentBuilder Create(string subjectIdentifier, DateTimeOffset authenticationTime)
        {
            NullGuard.NotNullOrWhiteSpace(subjectIdentifier, nameof(subjectIdentifier));

            return new IdTokenContentBuilder(subjectIdentifier, authenticationTime);
        }

        #endregion
    }
}