using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Contacts
{
    internal class ContactSupplementBindingModelHandler : ModelHandlerBase<IContact, ContactContext, ContactSupplementBindingModel, int>
    {
        #region Constructor

        public ContactSupplementBindingModelHandler(ContactContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<ContactSupplementBindingModel> Entities => DbContext.ContactSupplementBindings;

        protected override Func<IContact, int> PrimaryKey => throw new NotSupportedException();

        protected override IQueryable<ContactSupplementBindingModel> Reader => Entities
            .Include(contactSupplementBindingModel => contactSupplementBindingModel.ContactSupplement).ThenInclude(contactSupplementModel => contactSupplementModel.ContactGroup)
            .Include(contactSupplementBindingModel => contactSupplementBindingModel.ContactSupplement).ThenInclude(contactSupplementModel => contactSupplementModel.PaymentTerm)
            .Include(contactSupplementBindingModel => contactSupplementBindingModel.ContactSupplement).ThenInclude(contactSupplementModel => contactSupplementModel.ContactSupplementBindings);

        #endregion

        #region Methods

        internal async Task<ContactSupplementModel> ReadAsync(IContact contact, string existingExternalIdentifier = null)
        {
            NullGuard.NotNull(contact, nameof(contact));

            foreach (string externalIdentifier in GetExternalIdentifierCollection(contact, existingExternalIdentifier))
            {
                ContactSupplementBindingModel contactSupplementBindingModel = await Reader.SingleOrDefaultAsync(m => m.ExternalIdentifier == externalIdentifier);
                if (contactSupplementBindingModel != null)
                {
                    return contactSupplementBindingModel.ContactSupplement;
                }
            }

            return null;
        }

        internal string[] GetExternalIdentifierCollection(IContact contact, string existingExternalIdentifier = null)
        {
            NullGuard.NotNull(contact, nameof(contact));

            return new[] {contact.CalculateIdentifier(), existingExternalIdentifier ?? contact.ExternalIdentifier}
                .Where(m => string.IsNullOrWhiteSpace(m) == false)
                .ToArray();
        }

        protected override Expression<Func<ContactSupplementBindingModel, bool>> EntitySelector(int primaryKey) => contactSupplementBindingModel => contactSupplementBindingModel.ContactSupplementBindingIdentifier == primaryKey;

        protected override Task<IEnumerable<IContact>> SortAsync(IEnumerable<IContact> domainModelCollection) => throw new NotSupportedException();

        protected override Task<ContactSupplementBindingModel> OnCreateAsync(IContact domainModel, ContactSupplementBindingModel entityModel) => throw new NotSupportedException();

        protected override Task OnUpdateAsync(IContact domainModel, ContactSupplementBindingModel entityModel) => throw new NotSupportedException();

        protected override Task<ContactSupplementBindingModel> OnDeleteAsync(ContactSupplementBindingModel entityModel) => throw new NotSupportedException();

        #endregion
    }
}