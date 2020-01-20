using System;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;

namespace OSDevGrp.OSIntranet.Core.Commands
{
    public class TokenBasedCommand : ITokenBasedCommand
    {
        #region Private variables

        private string _tokenType;
        private string _accessToken;

        #endregion

        #region Properties

        public string TokenType
        {
            get => _tokenType;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _tokenType = value;
            }
        }

        public string AccessToken
        {
            get => _accessToken;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _accessToken = value;
            }
        }

        public DateTime Expires { get; set; }

        #endregion
    }
}