using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Events;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class ContactAccountModelHandler : AccountModelHandlerBase<IContactAccount, ContactAccountModel>
    {
        #region Constructor

        public ContactAccountModelHandler(RepositoryContext dbContext, IConverter modelConverter, IEventPublisher eventPublisher, DateTime statusDate, bool includePostingLines, bool fromPostingLineModelHandler = false) 
            : base(dbContext, modelConverter, eventPublisher, statusDate, includePostingLines, includePostingLines ? new PostingLineModelHandler(dbContext, modelConverter, eventPublisher, DateTime.MinValue, statusDate, false, false) : null, fromPostingLineModelHandler)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<ContactAccountModel> Entities => DbContext.ContactAccounts;

        protected override IQueryable<ContactAccountModel> Reader => CreateReader(false);

        protected override IQueryable<ContactAccountModel> UpdateReader => CreateReader(true);

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

        protected override async Task<bool> CanDeleteAsync(ContactAccountModel contactAccountModel)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel));

            if (contactAccountModel.PostingLines == null || contactAccountModel.PostingLines.Any() || FromPostingLineModelHandler)
            {
                return false;
            } 

            return await DbContext.PostingLines.FirstOrDefaultAsync(postingLineModel => postingLineModel.ContactAccountIdentifier != null && postingLineModel.ContactAccountIdentifier.Value == contactAccountModel.ContactAccountIdentifier) == null;
        }

        protected override async Task<ContactAccountModel> OnDeleteAsync(ContactAccountModel contactAccountModel)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel));

            await PostingLineModelHandler.DeleteAsync(contactAccountModel.PostingLines);

            return await base.OnDeleteAsync(contactAccountModel);
        }

        protected override Task PublishModelCollectionLoadedEvent(IReadOnlyCollection<ContactAccountModel> contactAccountModelCollection)
        {
            NullGuard.NotNull(contactAccountModelCollection, nameof(contactAccountModelCollection));

            lock (SyncRoot)
            {
                EventPublisher.PublishAsync(new ContactAccountModelCollectionLoadedEvent(DbContext, contactAccountModelCollection, StatusDate))
                    .GetAwaiter()
                    .GetResult();
            }

            return Task.CompletedTask;
        }

        protected override void ExtractPostingLines(ContactAccountModel contactAccountModel, IReadOnlyCollection<PostingLineModel> postingLineCollection)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel))
                .NotNull(postingLineCollection, nameof(postingLineCollection));

            contactAccountModel.ExtractPostingLines(postingLineCollection);
        }

        private IQueryable<ContactAccountModel> CreateReader(bool includeAccounting)
        {
            IQueryable<ContactAccountModel> reader = Entities
                .Include(contactAccountModel => contactAccountModel.BasicAccount)
                .Include(contactAccountModel => contactAccountModel.PaymentTerm);

            return includeAccounting == false ? reader : reader.Include(contactAccountModel => contactAccountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead);
        }

        #endregion
    }
}