using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public class UpdateContactCommand : ContactDataCommandBase, IUpdateContactCommand
    {
        #region Private variables

        private IContact _contact;

        #endregion

        #region Properties

        public string ExternalIdentifier { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(microsoftGraphRepository, nameof(microsoftGraphRepository))
                .NotNull(contactRepository, nameof(contactRepository))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return base.Validate(validator, microsoftGraphRepository, contactRepository, accountingRepository)
                .ValidateExternalIdentifier(ExternalIdentifier, GetType(), nameof(ExternalIdentifier))
                .Object.ShouldBeKnownValue(ExternalIdentifier, externalIdentifier => Task.Run(async () => await GetContactAsync(microsoftGraphRepository, contactRepository) != null), GetType(), nameof(ExternalIdentifier));
        }

        public override IContact ToDomain(IContactRepository contactRepository, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(contactRepository, nameof(contactRepository))
                .NotNull(accountingRepository, nameof(accountingRepository));

            IContact contact = base.ToDomain(contactRepository, accountingRepository);
            
            contact.ExternalIdentifier = string.IsNullOrWhiteSpace(ExternalIdentifier) ? null : ExternalIdentifier;

            return contact;
        }

        public Task<IContact> GetExistingContactAsync(IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository)
        {
            NullGuard.NotNull(microsoftGraphRepository, nameof(microsoftGraphRepository))
                .NotNull(contactRepository, nameof(contactRepository));

            return GetContactAsync(microsoftGraphRepository, contactRepository);
        }

        protected Task<IContact> GetContactAsync(IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository)
        {
            NullGuard.NotNull(microsoftGraphRepository, nameof(microsoftGraphRepository))
                .NotNull(contactRepository, nameof(contactRepository));

            return Task.Run(() => ExternalIdentifier.GetContact(ToToken(), microsoftGraphRepository, contactRepository, ref _contact));
        }

        #endregion
    }
}