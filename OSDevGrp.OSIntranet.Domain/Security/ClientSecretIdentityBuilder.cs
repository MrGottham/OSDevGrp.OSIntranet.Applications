using System;
using System.Security.Cryptography;
using System.Text;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    public class ClientSecretIdentityBuilder : IClientSecretIdentityBuilder
    {
        #region Private variables

        private int _identifier;
        private string _clientId;
        private string _clientSecret;
        private readonly string _friendlyName;

        #endregion

        #region Constructor

        public ClientSecretIdentityBuilder(string friendlyName)
        {
            NullGuard.NotNullOrWhiteSpace(friendlyName, nameof(friendlyName));

            _identifier = default(int);
            _friendlyName = friendlyName;
        }

        #endregion

        #region Methods

        public IClientSecretIdentityBuilder WithIdentifier(int identifier)
        {
            _identifier = identifier;

            return this;
        }

        public IClientSecretIdentityBuilder WithClientId(string clientId)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId));

            _clientId = clientId;

            return this;
        }

        public IClientSecretIdentityBuilder WithClientSecret(string clientSecret)
        {
            NullGuard.NotNullOrWhiteSpace(clientSecret, nameof(clientSecret));

            _clientSecret = clientSecret;

            return this;
        }

        public IClientSecretIdentity Build()
        {
            return new ClientSecretIdentity(_identifier, _friendlyName, _clientId ?? ComputeHash(_friendlyName), _clientSecret ?? ComputeHash(_friendlyName));
        }

        private static string ComputeHash(string friendlyName)
        {
            NullGuard.NotNull(friendlyName, nameof(friendlyName));

            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes($"{Guid.NewGuid():N}:{friendlyName}:{DateTime.UtcNow.Ticks}"));

                StringBuilder resultBuilder = new StringBuilder();
                foreach (byte b in data)
                {
                    resultBuilder.Append(b.ToString("x2"));
                }

                return resultBuilder.ToString();
            }
        }

        #endregion
    }
}
