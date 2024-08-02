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

        public Task<IKeyValueEntry> ToKeyValueEntryAsync(IAuthorizationCode authorizationCode, IEnumerable<Claim> claims)
        {
            NullGuard.NotNull(authorizationCode, nameof(authorizationCode))
                .NotNull(claims, nameof(claims));

            return Task.FromResult(KeyValueEntry.Create(authorizationCode.Value, AuthorizationData.Create(authorizationCode, claims)));
        }

        public Task<IAuthorizationCode> ToAuthorizationCodeAsync(IKeyValueEntry keyValueEntry, out IEnumerable<Claim> claims)
        {
            NullGuard.NotNull(keyValueEntry, nameof(keyValueEntry));

            AuthorizationData authorizationData = keyValueEntry.ToObject<AuthorizationData>();
            IAuthorizationCode authorizationCode = _authorizationCodeFactory.Create(keyValueEntry.Key, authorizationData.Expires).Build();

            claims = authorizationData.ToClaims();

            return Task.FromResult(authorizationCode);
        }

        // ReSharper disable MemberCanBePrivate.Local
        private class AuthorizationData
        {
            public DateTimeOffset Expires { get; init; }

            public IReadOnlyCollection<AuthorizationDataClaim> Claims { get; init; }

            public IEnumerable<Claim> ToClaims()
            {
                return (Claims ?? Array.Empty<AuthorizationDataClaim>())
                    .Select(authorizationDataClaim => authorizationDataClaim.ToClaim())
                    .ToArray();
            }

            public static AuthorizationData Create(IAuthorizationCode authorizationCode, IEnumerable<Claim> claims)
            {
                NullGuard.NotNull(authorizationCode, nameof(authorizationCode))
                    .NotNull(claims, nameof(claims));

                return new AuthorizationData
                {
                    Expires = authorizationCode.Expires,
                    Claims = claims.Select(AuthorizationDataClaim.Create).ToArray()
                };
            }
        }

        private class AuthorizationDataClaim
        {
            public string Type { get; init; }

            public string Value { get; init; }

            public string ValueType { get; init; }

            public string Issuer { get; init; }

            public Claim ToClaim()
            {
                return new Claim(Type, Value, ValueType, Issuer);
            }

            public static AuthorizationDataClaim Create(Claim claim)
            {
                NullGuard.NotNull(claim, nameof(claim));

                return new AuthorizationDataClaim
                {
                    Type = claim.Type,
                    Value = claim.Value,
                    ValueType = claim.ValueType,
                    Issuer = claim.Issuer
                };
            }
        }
        // ReSharper restore MemberCanBePrivate.Local

        #endregion
    }
}