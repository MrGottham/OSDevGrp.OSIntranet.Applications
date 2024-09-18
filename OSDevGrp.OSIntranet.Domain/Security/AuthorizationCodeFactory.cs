using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class AuthorizationCodeFactory : IAuthorizationCodeFactory
    {
        #region Methods

        public IAuthorizationCodeBuilder Create(string value, DateTimeOffset expires)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return new AuthorizationCodeBuilder(value, expires);
        }

        #endregion
    }
}