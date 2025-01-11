using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal class IdTokenContentFactory : IIdTokenContentFactory
    {
        #region Private variables

        private readonly IClaimsSelector _claimsSelector;

        #endregion

        #region Constructor

        public IdTokenContentFactory(IClaimsSelector claimsSelector)
        {
            NullGuard.NotNull(claimsSelector, nameof(claimsSelector));

            _claimsSelector = claimsSelector;
        }

        #endregion

        #region Methods

        public IIdTokenContentBuilder Create(string nameIdentifier, IUserInfo userInfo, DateTimeOffset authenticationTime, IReadOnlyDictionary<string, IScope> supportedScopes, IReadOnlyCollection<string> scopes)
        {
            NullGuard.NotNullOrWhiteSpace(nameIdentifier, nameof(nameIdentifier))
                .NotNull(userInfo, nameof(userInfo))
                .NotNull(supportedScopes, nameof(supportedScopes))
                .NotNull(scopes, nameof(scopes));

            return new IdTokenContentBuilder(nameIdentifier, userInfo, authenticationTime, supportedScopes, scopes, _claimsSelector);
        }

        #endregion
    }
}