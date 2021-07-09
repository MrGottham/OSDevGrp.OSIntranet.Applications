using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class PaymentTermModelHandler : ModelHandlerBase<IPaymentTerm, RepositoryContext, PaymentTermModel, int>
    {
        #region Constructor

        public PaymentTermModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<PaymentTermModel> Entities => DbContext.PaymentTerms;

        protected override Func<IPaymentTerm, int> PrimaryKey => paymentTerm => paymentTerm.Number;

        #endregion

        #region Methods

        protected override Expression<Func<PaymentTermModel, bool>> EntitySelector(int primaryKey) => paymentTermModel => paymentTermModel.PaymentTermIdentifier == primaryKey;

        protected override Task<IEnumerable<IPaymentTerm>> SortAsync(IEnumerable<IPaymentTerm> paymentTermCollection)
        {
            NullGuard.NotNull(paymentTermCollection, nameof(paymentTermCollection));

            return Task.FromResult(paymentTermCollection.OrderBy(paymentTerm => paymentTerm.Number).AsEnumerable());
        }

        protected override async Task<PaymentTermModel> OnReadAsync(PaymentTermModel paymentTermModel)
        {
            NullGuard.NotNull(paymentTermModel, nameof(paymentTermModel));

            paymentTermModel.Deletable = await CanDeleteAsync(paymentTermModel);

            return paymentTermModel;
        }

        protected override Task OnUpdateAsync(IPaymentTerm paymentTerm, PaymentTermModel paymentTermModel)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm))
                .NotNull(paymentTermModel, nameof(paymentTermModel));

            paymentTermModel.Name = paymentTerm.Name;

            return Task.CompletedTask;
        }

        protected override async Task<bool> CanDeleteAsync(PaymentTermModel paymentTermModel)
        {
            NullGuard.NotNull(paymentTermModel, nameof(paymentTermModel));

            if (paymentTermModel.ContactAccounts != null && paymentTermModel.ContactAccounts.Any())
            {
                return false;
            }

            if (paymentTermModel.ContactSupplements != null && paymentTermModel.ContactSupplements.Any())
            {
                return false;
            }

            bool usedOnContactAccount = await DbContext.ContactAccounts.FirstOrDefaultAsync(contactAccountModel => contactAccountModel.PaymentTermIdentifier == paymentTermModel.PaymentTermIdentifier) != null;
            bool usedOnContactSupplement = await DbContext.ContactSupplements.FirstOrDefaultAsync(contactSupplementModel => contactSupplementModel.PaymentTermIdentifier == paymentTermModel.PaymentTermIdentifier) != null;

            return usedOnContactAccount == false && usedOnContactSupplement == false;
        }

        #endregion
    }
}