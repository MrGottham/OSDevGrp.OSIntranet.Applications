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

        protected override IQueryable<ContactAccountModel> Reader => Entities
            .Include(accountModel => accountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead)
            .Include(accountModel => accountModel.BasicAccount)
            .Include(accountModel => accountModel.PaymentTerm);

        protected override IQueryable<ContactAccountModel> DeleteReader => Reader; // TODO: Include posting lines.

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

        protected override async Task<ContactAccountModel> OnReadAsync(ContactAccountModel contactAccountModel)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel));

            contactAccountModel = await base.OnReadAsync(contactAccountModel);

            if (IncludePostingLines)
            {
                // TODO: Include all posting lines for the given status date.
            }

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

            contactAccountModel = await base.OnDeleteAsync(contactAccountModel);

            // TODO: Delete all posting lines.

            return contactAccountModel;
        }

        protected override Task<bool> CanDeleteAsync(ContactAccountModel contactAccountModel)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel));

            // TODO: Validate the existence of posting lines.

            return Task.FromResult(false);
        }

        #endregion
    }
}