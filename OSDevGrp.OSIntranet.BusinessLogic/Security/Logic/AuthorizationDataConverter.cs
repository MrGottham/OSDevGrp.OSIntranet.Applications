using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal class AuthorizationDataConverter : IAuthorizationDataConverter
    {
        #region Internal constants

        internal const string ClientIdKey = "urn:osdevgrp:osintranet:authorizationdata:client:id";
        internal const string ClientSecretKey = "urn:osdevgrp:osintranet:authorizationdata:client:secret";
        internal const string RedirectUriKey = "urn:osdevgrp:osintranet:authorizationdata:redirecturi";
        internal const string IdTokenKey = "urn:osdevgrp:osintranet:authorizationdata:idtoken";

        #endregion

        #region Private variables

        private readonly IAuthorizationCodeFactory _authorizationCodeFactory;

        #endregion

        #region Constructor

        public AuthorizationDataConverter(IAuthorizationCodeFactory authorizationCodeFactory)
        {
            NullGuard.NotNull(authorizationCodeFactory, nameof(authorizationCodeFactory));

            _authorizationCodeFactory = authorizationCodeFactory;
        }

        #endregion

        #region Methods

        public Task<IKeyValueEntry> ToKeyValueEntryAsync(IAuthorizationCode authorizationCode, IReadOnlyCollection<Claim> claims, IReadOnlyDictionary<string, string> authorizationData)
        {
            NullGuard.NotNull(authorizationCode, nameof(authorizationCode))
                .NotNull(claims, nameof(claims))
                .NotNull(authorizationData, nameof(authorizationData));

            return Task.FromResult(KeyValueEntry.Create(authorizationCode.Value, InternalAuthorizationData.Create(authorizationCode, claims, authorizationData)));
        }

        public Task<IAuthorizationCode> ToAuthorizationCodeAsync(IKeyValueEntry keyValueEntry, out IReadOnlyCollection<Claim> claims, out IReadOnlyDictionary<string, string> authorizationData)
        {
            NullGuard.NotNull(keyValueEntry, nameof(keyValueEntry));

            InternalAuthorizationData internalAuthorizationData = keyValueEntry.ToObject<InternalAuthorizationData>();
            IAuthorizationCode authorizationCode = _authorizationCodeFactory.Create(keyValueEntry.Key, internalAuthorizationData.Expires).Build();

            claims = internalAuthorizationData.ToClaims();
            authorizationData = internalAuthorizationData.ToAuthorizationData();

            return Task.FromResult(authorizationCode);
        }

        // ReSharper disable MemberCanBePrivate.Local
        private class InternalAuthorizationData
        {
            public DateTimeOffset Expires { get; init; }

            public IReadOnlyCollection<InternalAuthorizationDataClaim> Claims { get; init; }

            public IReadOnlyCollection<InternalAuthorizationDataEntry> Entries { get; init; } 

            public IReadOnlyCollection<Claim> ToClaims()
            {
                return (Claims ?? Array.Empty<InternalAuthorizationDataClaim>())
                    .Select(internalAuthorizationDataClaim => internalAuthorizationDataClaim.ToClaim())
                    .ToArray();
            }

            public IReadOnlyDictionary<string, string> ToAuthorizationData()
            {
                return (Entries ?? Array.Empty<InternalAuthorizationDataEntry>())
                    .Select(internalAuthorizationDataEntry => internalAuthorizationDataEntry.ToKeyValuePair())
                    .ToDictionary();
            }

            public static InternalAuthorizationData Create(IAuthorizationCode authorizationCode, IEnumerable<Claim> claims, IReadOnlyDictionary<string, string> authorizationData)
            {
                NullGuard.NotNull(authorizationCode, nameof(authorizationCode))
                    .NotNull(claims, nameof(claims))
                    .NotNull(authorizationData, nameof(authorizationData));

                return new InternalAuthorizationData
                {
                    Expires = authorizationCode.Expires,
                    Claims = claims.Select(InternalAuthorizationDataClaim.Create).ToArray(),
                    Entries = authorizationData.Select(InternalAuthorizationDataEntry.Create).ToArray()
                };
            }
        }

        private class InternalAuthorizationDataClaim
        {
            public string Type { get; init; }

            public string Value { get; init; }

            public string ValueType { get; init; }

            public string Issuer { get; init; }

            public Claim ToClaim()
            {
                return new Claim(Type, Value, ValueType, Issuer);
            }

            public static InternalAuthorizationDataClaim Create(Claim claim)
            {
                NullGuard.NotNull(claim, nameof(claim));

                return new InternalAuthorizationDataClaim
                {
                    Type = claim.Type,
                    Value = claim.Value,
                    ValueType = claim.ValueType,
                    Issuer = claim.Issuer
                };
            }
        }

        private class InternalAuthorizationDataEntry
        {
            public string Key { get; init; }

            public string Value { get; init; }

            public KeyValuePair<string, string> ToKeyValuePair()
            {
                return new KeyValuePair<string, string>(Key, Value);
            }

            public static InternalAuthorizationDataEntry Create(KeyValuePair<string, string> keyValuePair)
            {
                NullGuard.NotNull(keyValuePair, nameof(keyValuePair));

                return new InternalAuthorizationDataEntry
                {
                    Key = keyValuePair.Key,
                    Value = keyValuePair.Value
                };
            }
        }
        // ReSharper restore MemberCanBePrivate.Local

        #endregion
    }
}