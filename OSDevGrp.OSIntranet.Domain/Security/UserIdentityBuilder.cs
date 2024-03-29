﻿using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Security
{
	internal class UserIdentityBuilder : IUserIdentityBuilder
    {
        #region Private variables

        private int _identifier;
        private readonly string _externalUserIdentifier;
        private readonly List<Claim> _claims = new List<Claim>();

        #endregion

        #region Constructor

        internal UserIdentityBuilder(string externalUserIdentifier, IEnumerable<Claim> claims = null)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

            _identifier = default;
            _externalUserIdentifier = externalUserIdentifier;

            if (claims == null)
            {
                return;
            }

            _claims.AddRange(claims);
        }

        #endregion

        #region Methods

        public IUserIdentityBuilder WithIdentifier(int identifier)
        {
            _identifier = identifier;

            return this;
        }

        public IUserIdentityBuilder AddClaims(IEnumerable<Claim> claims)
        {
            NullGuard.NotNull(claims, nameof(claims));

            _claims.AddRange(claims);

            return this;
        }

        public IUserIdentity Build()
        {
            return new UserIdentity(_identifier, _externalUserIdentifier, _claims);
        }

        #endregion
    }
}