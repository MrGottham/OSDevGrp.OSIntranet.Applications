using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries
{
	public abstract class ContactQueryBase : RefreshableTokenBasedQuery, IContactQuery
    {
        #region Methods

        public virtual IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return validator.ValidateTokenType(TokenType, GetType(), nameof(TokenType))
                .ValidateAccessToken(AccessToken, GetType(), nameof(AccessToken))
                .ValidateRefreshToken(RefreshToken, GetType(), nameof(RefreshToken))
                .ValidateExpires(Expires, GetType(), nameof(Expires));
        }

        public IRefreshableToken ToToken()
        {
	        return RefreshableTokenFactory.Create().FromTokenBasedQuery(this);
        }

        #endregion
    }
}