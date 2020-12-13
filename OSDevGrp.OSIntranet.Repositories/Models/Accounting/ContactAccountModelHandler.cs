using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class ContactAccountModelHandler : AccountModelHandlerBase<IContactAccount, ContactAccountModel>
    {
        #region Constructor

        public ContactAccountModelHandler(RepositoryContext dbContext, IConverter modelConverter, DateTime statusDate, bool includePostingLines) 
            : base(dbContext, modelConverter, statusDate, includePostingLines)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<ContactAccountModel> Entities => DbContext.ContactAccounts;

        protected override IQueryable<ContactAccountModel> Reader => CreateReader(IncludePostingLines);

        protected override IQueryable<ContactAccountModel> UpdateReader => CreateReader(false);

        protected override IQueryable<ContactAccountModel> DeleteReader => CreateReader(true);

        #endregion

        #region Methods

        protected override async Task<ContactAccountModel> OnCreateAsync(IContactAccount contactAccount, ContactAccountModel contactAccountModel)
        {
            NullGuard.NotNull(contactAccount, nameof(contactAccount))
                .NotNull(contactAccountModel, nameof(contactAccountModel));

            contactAccountModel = await base.OnCreateAsync(contactAccount, contactAccountModel);
            contactAccountModel.PaymentTerm = await DbContext.PaymentTerms.SingleAsync(paymentTermModel => paymentTermModel.PaymentTermIdentifier == contactAccount.PaymentTerm.Number);

            return contactAccountModel;
        }

        protected override async Task OnUpdateAsync(IContactAccount contactAccount, ContactAccountModel contactAccountModel)
        {
            NullGuard.NotNull(contactAccount, nameof(contactAccount))
                .NotNull(contactAccountModel, nameof(contactAccountModel));

            await base.OnUpdateAsync(contactAccount, contactAccountModel);

            contactAccountModel.PaymentTermIdentifier = contactAccount.PaymentTerm.Number;
            contactAccountModel.PaymentTerm = await DbContext.PaymentTerms.SingleAsync(paymentTermModel => paymentTermModel.PaymentTermIdentifier == contactAccount.PaymentTerm.Number);
            contactAccountModel.MailAddress = contactAccount.MailAddress;
            contactAccountModel.PrimaryPhone = contactAccount.PrimaryPhone;
            contactAccountModel.SecondaryPhone = contactAccount.SecondaryPhone;
        }

        protected override async Task<ContactAccountModel> OnDeleteAsync(ContactAccountModel contactAccountModel)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel));

            // TODO: Delete all posting lines.

            return await base.OnDeleteAsync(contactAccountModel);
        }

        protected override Task<bool> CanDeleteAsync(ContactAccountModel contactAccountModel)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel));

            // TODO: Validate the existence of posting lines.

            return Task.FromResult(false);
        }

        private IQueryable<ContactAccountModel> CreateReader(bool includePostingLines)
        {
            IQueryable<ContactAccountModel> reader = Entities
                .Include(contactAccountModel => contactAccountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead)
                .Include(contactAccountModel => contactAccountModel.BasicAccount)
                .Include(contactAccountModel => contactAccountModel.PaymentTerm);

            if (includePostingLines)
            {
                // TODO: Include posting lines.
            }

            return reader;
        }

        #endregion
    }
}