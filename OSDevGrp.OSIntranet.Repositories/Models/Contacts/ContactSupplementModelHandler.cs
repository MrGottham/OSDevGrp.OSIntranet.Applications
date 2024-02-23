using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Models.Contacts
{
	internal class ContactSupplementModelHandler : ModelHandlerBase<IContact, RepositoryContext, ContactSupplementModel, int>
    {
        #region Private variables

        private readonly IConverter _accountingModelConverter;
        private readonly ContactSupplementBindingModelHandler _contactSupplementBindingModelHandler;

        #endregion

        #region Constructor

        public ContactSupplementModelHandler(RepositoryContext dbContext, IConverter modelConverter, IConverter accountingModelConverter) 
            : base(dbContext, modelConverter)
        {
            NullGuard.NotNull(accountingModelConverter, nameof(accountingModelConverter));

            _accountingModelConverter = accountingModelConverter;
            _contactSupplementBindingModelHandler = new ContactSupplementBindingModelHandler(dbContext, modelConverter);
        }

        #endregion

        #region Properties

        protected override DbSet<ContactSupplementModel> Entities => DbContext.ContactSupplements;

        protected override Func<IContact, int> PrimaryKey => throw new NotSupportedException();

        protected override IQueryable<ContactSupplementModel> Reader => Entities
            .Include(contactSupplementModel => contactSupplementModel.ContactGroup)
            .Include(contactSupplementModel => contactSupplementModel.PaymentTerm)
            .Include(contactSupplementModel => contactSupplementModel.ContactSupplementBindings);

        #endregion

        #region Methods

        internal async Task<IContact> ApplyContactSupplementAsync(IContact contact)
        {
            NullGuard.NotNull(contact, nameof(contact));

            ContactSupplementModel contactSupplementModel = await ReadAsync(contact);
            if (contactSupplementModel == null)
            {
                return contact;
            }

            contactSupplementModel.ApplyContactSupplement(contact, ModelConverter, _accountingModelConverter);

            return contact;
        }

        internal Task<IEnumerable<IContact>> ApplyContactSupplementAsync(IEnumerable<IContact> contactCollection)
        {
            NullGuard.NotNull(contactCollection, nameof(contactCollection));

            return Task.FromResult<IEnumerable<IContact>>(contactCollection.Select(contact => ApplyContactSupplementAsync(contact).GetAwaiter().GetResult()).ToArray());
        }

        internal async Task<ContactSupplementModel> ReadAsync(IContact contact, string existingExternalIdentifier = null)
        {
            NullGuard.NotNull(contact, nameof(contact));

            if (string.IsNullOrWhiteSpace(contact.InternalIdentifier) == false && int.TryParse(contact.InternalIdentifier, out int internalIdentifier))
            {
                ContactSupplementModel contactSupplementModel = await Reader.SingleOrDefaultAsync(EntitySelector(internalIdentifier));
                if (contactSupplementModel != null)
                {
                    return contactSupplementModel;
                }
            }

            return await _contactSupplementBindingModelHandler.ReadAsync(contact, existingExternalIdentifier);
        }

        internal async Task<IContact> CreateOrUpdateContactSupplementAsync(IContact contact, string existingExternalIdentifier = null)
        {
            NullGuard.NotNull(contact, nameof(contact));

            ContactSupplementModel contactSupplementModel = await ReadAsync(contact, existingExternalIdentifier);
            if (contactSupplementModel == null)
            {
                contactSupplementModel = ModelConverter.Convert<IContact, ContactSupplementModel>(contact);
                await Entities.AddAsync(await OnCreateAsync(contact, contactSupplementModel));

                await DbContext.SaveChangesAsync();

                return await ApplyContactSupplementAsync(contact);
            }

            await OnUpdateAsync(contact, contactSupplementModel);

            await DbContext.SaveChangesAsync();

            return await ApplyContactSupplementAsync(contact);
        }

        internal async Task<IContact> DeleteAsync(IContact contact)
        {
            NullGuard.NotNull(contact, nameof(contact));

            ContactSupplementModel contactSupplementModel = await ReadAsync(contact);
            if (contactSupplementModel == null)
            {
                return null;
            }

            Entities.Remove(await OnDeleteAsync(contactSupplementModel));

            await DbContext.SaveChangesAsync();

            return null;
        }

        protected override Expression<Func<ContactSupplementModel, bool>> EntitySelector(int primaryKey) => contactSupplementModel => contactSupplementModel.ContactSupplementIdentifier == primaryKey;

        protected override Task<IEnumerable<IContact>> SortAsync(IEnumerable<IContact> domainModelCollection) => throw new NotSupportedException();

        protected override void OnDispose()
        {
            _contactSupplementBindingModelHandler.Dispose();
        }

        protected override async Task<ContactSupplementModel> OnCreateAsync(IContact contact, ContactSupplementModel contactSupplementModel)
        {
            NullGuard.NotNull(contact, nameof(contact))
                .NotNull(contactSupplementModel, nameof(contactSupplementModel));

            contactSupplementModel.ContactGroup = await DbContext.ContactGroups.SingleAsync(contactGroupModel => contactGroupModel.ContactGroupIdentifier == contact.ContactGroup.Number);
            contactSupplementModel.PaymentTerm = await DbContext.PaymentTerms.SingleAsync(paymentTermModel => paymentTermModel.PaymentTermIdentifier == contact.PaymentTerm.Number);

            return contactSupplementModel;
        }

        protected override async Task OnUpdateAsync(IContact contact, ContactSupplementModel contactSupplementModel)
        {
            NullGuard.NotNull(contact, nameof(contact))
                .NotNull(contactSupplementModel, nameof(contactSupplementModel));

            contactSupplementModel.Birthday = contact.Birthday;
            contactSupplementModel.ContactGroupIdentifier = contact.ContactGroup.Number;
            contactSupplementModel.ContactGroup = await DbContext.ContactGroups.SingleAsync(contactGroupModel => contactGroupModel.ContactGroupIdentifier == contact.ContactGroup.Number);
            contactSupplementModel.Acquaintance = contact.Acquaintance;
            contactSupplementModel.PersonalHomePage = contact.PersonalHomePage;
            contactSupplementModel.LendingLimit = contact.LendingLimit;
            contactSupplementModel.PaymentTermIdentifier = contact.PaymentTerm.Number;
            contactSupplementModel.PaymentTerm = await DbContext.PaymentTerms.SingleAsync(paymentTermModel => paymentTermModel.PaymentTermIdentifier == contact.PaymentTerm.Number);

            string[] externalIdentifierCollection = _contactSupplementBindingModelHandler.GetExternalIdentifierCollection(contact);
            foreach (ContactSupplementBindingModel contactSupplementBindingModel in contactSupplementModel.ContactSupplementBindings.Where(m => externalIdentifierCollection.Any(externalIdentifier => externalIdentifier == ValueConverter.ByteArrayToExternalIdentifier(m.ExternalIdentifier)) == false).ToArray())
            {
                contactSupplementModel.ContactSupplementBindings.Remove(contactSupplementBindingModel);
            }
            foreach (string externalIdentifier in externalIdentifierCollection.Where(m => contactSupplementModel.ContactSupplementBindings.Any(contactSupplementBindingModel => ValueConverter.ByteArrayToExternalIdentifier(contactSupplementBindingModel.ExternalIdentifier) == m) == false))
            {
                ContactSupplementBindingModel contactSupplementBindingModel = new ContactSupplementBindingModel
                {
                    ContactSupplementIdentifier = contactSupplementModel.ContactSupplementIdentifier,
                    ContactSupplement = contactSupplementModel,
                    ExternalIdentifier = ValueConverter.ExternalIdentifierToByteArray(externalIdentifier)
                };
                contactSupplementModel.ContactSupplementBindings.Add(contactSupplementBindingModel);
            }
        }

        protected override Task<ContactSupplementModel> OnDeleteAsync(ContactSupplementModel contactSupplementModel)
        {
            NullGuard.NotNull(contactSupplementModel, nameof(contactSupplementModel));

            ContactSupplementBindingModel contactSupplementBindingModel = contactSupplementModel.ContactSupplementBindings.FirstOrDefault();
            while (contactSupplementBindingModel != null)
            {
                contactSupplementModel.ContactSupplementBindings.Remove(contactSupplementBindingModel);
                contactSupplementBindingModel = contactSupplementModel.ContactSupplementBindings.FirstOrDefault();
            }

            return Task.FromResult(contactSupplementModel);
        }

        #endregion
    }
}