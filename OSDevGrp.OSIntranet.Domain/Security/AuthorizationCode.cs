using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class AuthorizationCode : IAuthorizationCode
    {
        #region Constructor

        public AuthorizationCode(string value, DateTimeOffset expires)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            Value = value.Trim();
            Expires = expires.UtcDateTime;
        }

        #endregion

        #region Properties

        public string Value { get; }

        public DateTime Expires { get; }

        public bool Expired => Expires <= DateTime.UtcNow;

        #endregion
    }
}