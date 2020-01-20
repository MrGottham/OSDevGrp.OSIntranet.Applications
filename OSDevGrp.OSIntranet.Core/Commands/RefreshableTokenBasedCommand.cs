using OSDevGrp.OSIntranet.Core.Interfaces.Commands;

namespace OSDevGrp.OSIntranet.Core.Commands
{
    public class RefreshableTokenBasedCommand : TokenBasedCommand, IRefreshableTokenBasedCommand
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