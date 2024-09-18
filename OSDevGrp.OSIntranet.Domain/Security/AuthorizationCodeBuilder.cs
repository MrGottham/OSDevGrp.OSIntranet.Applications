using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class AuthorizationCodeBuilder : IAuthorizationCodeBuilder
    {
        #region Private variables

        private readonly string _value;
        private readonly DateTimeOffset _expires;

        #endregion

        #region Constructor

        public AuthorizationCodeBuilder(string value, DateTimeOffset expires)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            _value = value;
            _expires = expires;
        }

        #endregion

        #region Methods

        public IAuthorizationCode Build()
        {
            return new AuthorizationCode(_value, _expires);
        }

        #endregion
    }
}