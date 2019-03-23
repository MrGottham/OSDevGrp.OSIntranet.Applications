using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    public class UserIdentityBuilder : IUserIdentityBuilder
    {
        #region Private variables

        private int _identifier;
        private readonly string _externalUserIdentifier;

        #endregion

        #region Constructor

        public UserIdentityBuilder(string externalUserIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

            _identifier = default(int);
            _externalUserIdentifier = externalUserIdentifier;
        }

        #endregion

        #region Methods

        public IUserIdentityBuilder WithIdentifier(int identifier)
        {
            _identifier = identifier;

            return this;
        }

        public IUserIdentity Build()
        {
            return new UserIdentity(_identifier, _externalUserIdentifier);
        }

        #endregion
    }
}
