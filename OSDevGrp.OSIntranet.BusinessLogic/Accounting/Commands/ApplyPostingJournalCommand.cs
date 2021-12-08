using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public class ApplyPostingJournalCommand : AccountingIdentificationCommandBase, IApplyPostingJournalCommand
    {
        #region Properties

        public IEnumerable<IApplyPostingLineCommand> PostingLineCollection { get; set; } = Array.Empty<IApplyPostingLineCommand>();

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));

            IValidator result = base.Validate(validator, accountingRepository, commonRepository)
                .Object.ShouldBeKnownValue(AccountingNumber, accountingNumber => AccountingExistsAsync(accountingRepository), GetType(), nameof(AccountingNumber))
                .Object.ShouldNotBeNull(PostingLineCollection, GetType(), nameof(PostingLineCollection))
                .Enumerable.ShouldContainItems(PostingLineCollection, GetType(), nameof(PostingLineCollection));

            IAccounting accounting = GetAccountingAsync(accountingRepository).GetAwaiter().GetResult();
            if (accounting == null)
            {
                return result;
            }

            foreach (IApplyPostingLineCommand applyPostingLineCommand in PostingLineCollection)
            {
                result = applyPostingLineCommand.Validate(result, accounting);
            }

            return result;
        }

        public IPostingJournal ToDomain(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            IAccounting accounting = GetAccountingAsync(accountingRepository).GetAwaiter().GetResult();

            IPostingLineCollection postingLineCollection = new PostingLineCollection
            {
                PostingLineCollection.AsParallel()
                    .Select(applyPostingLineCommand => applyPostingLineCommand.ToDomain(accounting))
                    .OrderBy(postingLine => postingLine.PostingDate)
                    .ThenBy(postingLine=> postingLine.SortOrder)
                    .ToArray()
            };

            return new PostingJournal(postingLineCollection);
        }

        #endregion
    }
}