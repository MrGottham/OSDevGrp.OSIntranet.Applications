using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    public class Token : IToken
    {
        #region Constructors

        public Token(string tokenType, string accessToken, DateTime expires)
        {
            NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
                .NotNullOrWhiteSpace(accessToken, nameof(accessToken));

            TokenType = tokenType;
            AccessToken = accessToken;
            Expires = expires;
        }

        #endregion

        #region Properties

        public string TokenType { get; }

        public string AccessToken { get; }

        public DateTime Expires { get; }

        public bool HasExpired => (Expires.Kind == DateTimeKind.Utc ? Expires : Expires.ToUniversalTime()) < DateTime.UtcNow;

        #endregion

        #region Methods

        public byte[] ToByteArray()
        {
            return DomainHelper.ToByteArray(this);
        }

        public string ToBase64()
        {
            return Convert.ToBase64String(ToByteArray());
        }

        public bool WillExpireWithin(TimeSpan timeSpan)
        {
            if (HasExpired)
            {
                return true;
            }

            return (Expires.Kind == DateTimeKind.Utc ? Expires : Expires.ToUniversalTime()) < DateTime.UtcNow.Add(timeSpan);
        }

        public static TToken Create<TToken>(byte[] byteArray) where TToken : class, IToken
        {
            NullGuard.NotNull(byteArray, nameof(byteArray));

            if (typeof(TToken) == typeof(IRefreshableToken))
            {
                return DomainHelper.FromByteArray<RefreshableToken>(byteArray) as TToken;
            }

            if (typeof(TToken) == typeof(IToken))
            {
                return DomainHelper.FromByteArray<Token>(byteArray) as TToken;
            }

            return DomainHelper.FromByteArray<TToken>(byteArray);
        }

        public static TToken Create<TToken>(string base64String) where TToken : class, IToken
        {
            NullGuard.NotNullOrWhiteSpace(base64String, nameof(base64String));

            return Create<TToken>(Convert.FromBase64String(base64String));
        }

        public static IToken Create(ITokenBasedQuery tokenBasedQuery)
        {
            NullGuard.NotNull(tokenBasedQuery, nameof(tokenBasedQuery));

            return new Token(tokenBasedQuery.TokenType, tokenBasedQuery.AccessToken, tokenBasedQuery.Expires);
        }

        public static IToken Create(ITokenBasedCommand tokenBasedCommand)
        {
            NullGuard.NotNull(tokenBasedCommand, nameof(tokenBasedCommand));

            return new Token(tokenBasedCommand.TokenType, tokenBasedCommand.AccessToken, tokenBasedCommand.Expires);
        }

        #endregion
    }
}