using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Security
{
	internal class RefreshableToken : Token, IRefreshableToken
    {
		#region Constructor

		public RefreshableToken(string tokenType, string accessToken, string refreshToken, DateTime expires)
            : base(tokenType, accessToken, expires)
        {
            NullGuard.NotNullOrWhiteSpace(refreshToken, nameof(refreshToken));

            RefreshToken = refreshToken;
        }

        #endregion

        #region Properties

        public string RefreshToken { get; }

        #endregion
    }
}