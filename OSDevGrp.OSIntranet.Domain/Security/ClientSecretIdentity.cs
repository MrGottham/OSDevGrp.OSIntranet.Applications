﻿using System;
using System.Linq;
using System.Security.Claims;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    public class ClientSecretIdentity : ClaimsIdentity, IClientSecretIdentity
    {
        #region Constructor

        public ClientSecretIdentity(int identifier, string friendlyName, string clientId, string clientSecret)
        {
            NullGuard.NotNullOrWhiteSpace(friendlyName, nameof(friendlyName))
                .NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNullOrWhiteSpace(clientSecret, nameof(clientSecret));

            Identifier = identifier;
            ClientSecret = clientSecret;

            base.AddClaim(ClaimHelper.CreateFriendlyNameClaim(friendlyName));
            base.AddClaim(ClaimHelper.CreateClientIdClaim(clientId));
        }

        #endregion

        #region Properties

        public int Identifier { get; }

        public string FriendlyName => Claims.Single(m => string.Compare(m.Type, ClaimHelper.FriendlyNameClaimType, StringComparison.Ordinal) == 0).Value;

        public string ClientId => Claims.Single(m => string.Compare(m.Type, ClaimHelper.ClientIdClaimType, StringComparison.Ordinal) == 0).Value;

        public string ClientSecret { get; }

        #endregion

        #region Methods

        public ClaimsIdentity ToClaimsIdentity()
        {
            return this;
        }

        #endregion
    }
}
