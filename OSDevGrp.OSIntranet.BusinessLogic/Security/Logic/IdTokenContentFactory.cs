using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal class IdTokenContentFactory : IIdTokenContentFactory
    {
        #region Methods

        public IIdTokenContentBuilder Create(string subjectIdentifier, IUserInfo userInfo, DateTimeOffset authenticationTime)
        {
            NullGuard.NotNullOrWhiteSpace(subjectIdentifier, nameof(subjectIdentifier))
                .NotNull(userInfo, nameof(userInfo));

            return new IdTokenContentBuilder(subjectIdentifier, userInfo, authenticationTime);
        }

        #endregion
    }
}