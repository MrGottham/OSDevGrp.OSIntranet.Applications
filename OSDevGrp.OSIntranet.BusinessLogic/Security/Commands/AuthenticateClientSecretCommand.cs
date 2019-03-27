using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public class AuthenticateClientSecretCommand : IAuthenticateClientSecretCommand
    {
        #region Constructor

        public AuthenticateClientSecretCommand(string clientId, string clientSecret)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNullOrWhiteSpace(clientSecret, nameof(clientSecret));

            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        #endregion

        #region Properties

        public string ClientId { get; }

        public string ClientSecret { get; }

        #endregion
    }
}
