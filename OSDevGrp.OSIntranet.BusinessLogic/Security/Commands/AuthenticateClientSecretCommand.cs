using System.Collections.Generic;
using System.Security.Claims;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public class AuthenticateClientSecretCommand : IAuthenticateClientSecretCommand
    {
        #region Constructors

        public AuthenticateClientSecretCommand(string clientId, string clientSecret)
            : this(clientId, clientSecret, new List<Claim>(0))
        {
        }

        public AuthenticateClientSecretCommand(string clientId, string clientSecret, IEnumerable<Claim> claims)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNullOrWhiteSpace(clientSecret, nameof(clientSecret))
                .NotNull(claims, nameof(claims));

            ClientId = clientId;
            ClientSecret = clientSecret;
            Claims = claims;
        }

        #endregion

        #region Properties

        public string ClientId { get; }

        public string ClientSecret { get; }

        public IEnumerable<Claim> Claims { get; }

        #endregion
    }
}
