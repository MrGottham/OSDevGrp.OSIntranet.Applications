using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers
{
    public class GetContactWithBirthdayCollectionQueryHandler : ContactQueryHandlerBase<IGetContactWithBirthdayCollectionQuery, IEnumerable<IContact>>
    {
        #region Constructor

        public GetContactWithBirthdayCollectionQueryHandler(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository) 
            : base(validator, microsoftGraphRepository, contactRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IEnumerable<IContact>> GetDataAsync(IGetContactWithBirthdayCollectionQuery query, IRefreshableToken token)
        {
            NullGuard.NotNull(query, nameof(query))
                .NotNull(token, nameof(token));

            int birthdayWithinDays = query.BirthdayWithinDays;

            IEnumerable<IContact> contacts = await MicrosoftGraphRepository.GetContactsAsync(token);
            if (contacts == null)
            {
                return new List<IContact>(0);
            }

            IEnumerable<IContact> appliedSupplementContacts = await ContactRepository.ApplyContactSupplementAsync(contacts);
            if (appliedSupplementContacts == null)
            {
                return new List<IContact>(0);
            }

            return appliedSupplementContacts
                .Where(contact => contact.HasBirthdayWithinDays(birthdayWithinDays))
                .ToList();
        }

        #endregion
    }
}