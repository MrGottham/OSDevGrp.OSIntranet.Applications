using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.AccountingController
{
    public abstract class ApplyPostingJournalAsyncTestBase
    {
        protected abstract Fixture Fixture { get; }

        protected abstract Random Random { get; }

        protected ApplyPostingJournalModel CreateApplyPostingJournalModel(int? accountingNumber = null, ApplyPostingLineCollectionModel applyPostingLineCollectionModel = null)
        {
            return Fixture.Build<ApplyPostingJournalModel>()
                .With(m => m.AccountingNumber, accountingNumber ?? Fixture.Create<int>())
                .With(m => m.ApplyPostingLines, applyPostingLineCollectionModel ?? CreateApplyPostingLineCollectionModel())
                .Create();
        }

        protected ApplyPostingLineCollectionModel CreateApplyPostingLineCollectionModel(IEnumerable<ApplyPostingLineModel> applyPostingLineModels = null)
        {
            ApplyPostingLineCollectionModel applyPostingLineCollection = new ApplyPostingLineCollectionModel();
            foreach (ApplyPostingLineModel applyPostingLineModel in applyPostingLineModels ?? Fixture.CreateMany<ApplyPostingLineModel>(Random.Next(5, 10)).ToArray())
            {
                applyPostingLineCollection.Add(applyPostingLineModel);
            }

            return applyPostingLineCollection;
        }

        protected static bool IsMatch(IApplyPostingLineCommand applyPostingLineCommand, ApplyPostingLineModel applyPostingLineModel)
        {
            NullGuard.NotNull(applyPostingLineCommand, nameof(applyPostingLineCommand))
                .NotNull(applyPostingLineModel, nameof(applyPostingLineModel));

            return applyPostingLineCommand.Identifier == applyPostingLineModel.Identifier &&
                   applyPostingLineCommand.PostingDate == applyPostingLineModel.PostingDate.Date &&
                   applyPostingLineCommand.Reference == (string.IsNullOrWhiteSpace(applyPostingLineModel.Reference) ? null : applyPostingLineModel.Reference) &&
                   applyPostingLineCommand.AccountNumber == applyPostingLineModel.AccountNumber.ToUpper() &&
                   applyPostingLineCommand.Details == applyPostingLineModel.Details &&
                   applyPostingLineCommand.BudgetAccountNumber == (string.IsNullOrWhiteSpace(applyPostingLineModel.BudgetAccountNumber) ? null : applyPostingLineModel.BudgetAccountNumber.ToUpper()) &&
                   applyPostingLineCommand.Debit == applyPostingLineModel.Debit &&
                   applyPostingLineCommand.Credit == applyPostingLineModel.Credit &&
                   applyPostingLineCommand.ContactAccountNumber == (string.IsNullOrWhiteSpace(applyPostingLineModel.ContactAccountNumber) ? null : applyPostingLineModel.ContactAccountNumber.ToUpper()) &&
                   applyPostingLineCommand.SortOrder == (applyPostingLineModel.SortOrder ?? 0);
        }

        protected static bool IsMatch(PostingLineModel postingLineModel, IPostingLine postingLine)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel))
                .NotNull(postingLine, nameof(postingLine));

            return postingLineModel.Identifier == postingLine.Identifier &&
                   postingLineModel.PostingDate.Date == postingLine.PostingDate &&
                   postingLineModel.Reference == postingLine.Reference &&
                   postingLineModel.Account != null &&
                   postingLineModel.Account.AccountNumber == postingLine.Account.AccountNumber &&
                   postingLineModel.Details == postingLine.Details &&
                   postingLineModel.BudgetAccount?.AccountNumber == postingLine.BudgetAccount?.AccountNumber &&
                   postingLineModel.Debit == (postingLine.Debit == 0M ? null : postingLine.Debit) &&
                   postingLineModel.Credit == (postingLine.Credit == 0M ? null : postingLine.Credit) &&
                   postingLineModel.ContactAccount?.AccountNumber == postingLine.ContactAccount?.AccountNumber &&
                   postingLineModel.SortOrder == postingLine.SortOrder;
        }

        protected static bool IsMatch(PostingWarningModel postingWarningModel, IPostingWarning postingWarning)
        {
            NullGuard.NotNull(postingWarningModel, nameof(postingWarningModel))
                .NotNull(postingWarning, nameof(postingWarning));

            return postingWarningModel.Reason.ToString() == postingWarning.Reason.ToString() &&
                   postingWarningModel.Account.AccountNumber == postingWarning.Account.AccountNumber &&
                   postingWarningModel.Amount == postingWarning.Amount &&
                   IsMatch(postingWarningModel.PostingLine, postingWarning.PostingLine);
        }
    }
}