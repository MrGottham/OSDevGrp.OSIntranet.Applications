using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    [Serializable]
    public class RefreshableToken : Token, IRefreshableToken
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
