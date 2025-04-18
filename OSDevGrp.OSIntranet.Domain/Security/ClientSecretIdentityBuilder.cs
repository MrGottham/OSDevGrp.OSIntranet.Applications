﻿using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Extensions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class ClientSecretIdentityBuilder : IClientSecretIdentityBuilder
    {
        #region Private variables

        private int _identifier;
        private string _clientId;
        private string _clientSecret;
        private readonly string _friendlyName;
        private readonly List<Claim> _claims = new();

		#endregion

		#region Constructor

		internal ClientSecretIdentityBuilder(string friendlyName, IEnumerable<Claim> claims = null)
        {
            NullGuard.NotNullOrWhiteSpace(friendlyName, nameof(friendlyName));

            _identifier = 0;
            _friendlyName = friendlyName;

            if (claims == null)
            {
                return;
            }

            _claims.AddRange(claims);
        }

        #endregion

        #region Methods

        public IClientSecretIdentityBuilder WithIdentifier(int identifier)
        {
            _identifier = identifier;

            return this;
        }

        public IClientSecretIdentityBuilder WithClientId(string clientId)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId));

            _clientId = clientId;

            return this;
        }

        public IClientSecretIdentityBuilder WithClientSecret(string clientSecret)
        {
            NullGuard.NotNullOrWhiteSpace(clientSecret, nameof(clientSecret));

            _clientSecret = clientSecret;

            return this;
        }

        public IClientSecretIdentityBuilder AddClaims(IEnumerable<Claim> claims)
        {
            NullGuard.NotNull(claims, nameof(claims));

            _claims.AddRange(claims);

            return this;
       }

        public IClientSecretIdentity Build()
        {
            return new ClientSecretIdentity(_identifier, _friendlyName, _clientId ?? ComputeHash(_friendlyName), _clientSecret ?? ComputeHash(_friendlyName), _claims);
        }

        private static string ComputeHash(string friendlyName)
        {
            NullGuard.NotNull(friendlyName, nameof(friendlyName));

            return $"{Guid.NewGuid():N}:{friendlyName}:{DateTime.UtcNow.Ticks}".ComputeMd5Hash();
        }

        #endregion
    }
}