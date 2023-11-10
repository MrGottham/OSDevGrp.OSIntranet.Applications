using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Security
{
	internal class Token : IToken
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

        public string ToBase64String()
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

        #endregion
    }
}