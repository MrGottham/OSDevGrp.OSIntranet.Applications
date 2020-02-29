using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
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

        #region Methods

        public static IRefreshableToken Create(IRefreshableTokenBasedQuery refreshableTokenBasedQuery)
        {
            NullGuard.NotNull(refreshableTokenBasedQuery, nameof(refreshableTokenBasedQuery));

            return new RefreshableToken(refreshableTokenBasedQuery.TokenType, refreshableTokenBasedQuery.AccessToken, refreshableTokenBasedQuery.RefreshToken, refreshableTokenBasedQuery.Expires);
        }

        public static IRefreshableToken Create(IRefreshableTokenBasedCommand refreshableTokenBasedCommand)
        {
            NullGuard.NotNull(refreshableTokenBasedCommand, nameof(refreshableTokenBasedCommand));

            return new RefreshableToken(refreshableTokenBasedCommand.TokenType, refreshableTokenBasedCommand.AccessToken, refreshableTokenBasedCommand.RefreshToken, refreshableTokenBasedCommand.Expires);
        }

        #endregion
    }
}