using OSDevGrp.OSIntranet.Core.Interfaces.Queries;

namespace OSDevGrp.OSIntranet.Core.Queries
{
    public class RefreshableTokenBasedQuery : TokenBasedQuery, IRefreshableTokenBasedQuery
    {
        #region Private variables

        private string _refreshToken;

        #endregion

        #region Properties

        public string RefreshToken
        {
            get => _refreshToken;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _refreshToken = value;
            }
        }

        #endregion
    }
}