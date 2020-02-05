using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public abstract class ContactCommandBase : RefreshableTokenBasedCommand, IContactCommand
    {
        #region Methods

        public virtual IValidator Validate(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(microsoftGraphRepository, nameof(microsoftGraphRepository))
                .NotNull(contactRepository, nameof(contactRepository))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return validator.ValidateTokenType(TokenType, GetType(), nameof(TokenType))
                .ValidateAccessToken(AccessToken, GetType(), nameof(AccessToken))
                .ValidateRefreshToken(RefreshToken, GetType(), nameof(RefreshToken))
                .ValidateExpires(Expires, GetType(), nameof(Expires));
        }

        public IRefreshableToken ToToken()
        {
            return new RefreshableToken(TokenType, AccessToken, RefreshToken, Expires);
        }

        #endregion
    }
}