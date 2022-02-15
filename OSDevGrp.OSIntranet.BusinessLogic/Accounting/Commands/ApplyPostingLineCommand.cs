using System;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public class ApplyPostingLineCommand : IApplyPostingLineCommand
    {
        #region Private variables

        private DateTime _postingDate = DateTime.Today;
        private string _accountNumber;
        private string _budgetAccountNumber;
        private string _contactAccountNumber;

        #endregion

        #region Properties

        public Guid? Identifier { get; set; }

        public DateTime PostingDate
        {
            get => _postingDate.Date;
            set => _postingDate = value.Date;
        }

        public string Reference { get; set; }

        public string AccountNumber
        {
            get => _accountNumber;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _accountNumber = value.Trim().ToUpper();
            }
        }

        public string Details { get; set; }

        public string BudgetAccountNumber
        {
            get => _budgetAccountNumber;
            set => _budgetAccountNumber = string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpper();
        }

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }

        public string ContactAccountNumber
        {
            get => _contactAccountNumber;
            set => _contactAccountNumber = string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpper();
        }

        public int SortOrder { get; set; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator, IAccounting accounting)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accounting, nameof(accounting));

            int backDating = accounting.BackDating;
            IAccountCollection accountCollection = accounting.AccountCollection;
            IBudgetAccountCollection budgetAccountCollection = accounting.BudgetAccountCollection;
            IContactAccountCollection contactAccountCollection = accounting.ContactAccountCollection;

            IValidator result = validator
                .DateTime.ShouldBePastDateWithinDaysFromOffsetDate(PostingDate, backDating, DateTime.Today, GetType(), nameof(PostingDate))
                .DateTime.ShouldBePastDateOrToday(PostingDate, GetType(), nameof(PostingDate))
                .String.ShouldHaveMinLength(Reference, 1, GetType(), nameof(Reference), true)
                .String.ShouldHaveMaxLength(Reference, 16, GetType(), nameof(Reference), true)
                .ValidateAccountIdentifier(AccountNumber, GetType(), nameof(AccountNumber))
                .Object.ShouldBeKnownValue(AccountNumber, accountNumber => IsKnown(accountNumber, accountCollection), GetType(), nameof(AccountNumber))
                .String.ShouldNotBeNullOrWhiteSpace(Details, GetType(), nameof(Details))
                .String.ShouldHaveMinLength(Details, 1, GetType(), nameof(Details))
                .String.ShouldHaveMaxLength(Details, 256, GetType(), nameof(Details));

            if (BudgetAccountNumber != null)
            {
                result = result.ValidateAccountIdentifier(BudgetAccountNumber, GetType(), nameof(BudgetAccountNumber), true)
                    .Object.ShouldBeKnownValue(BudgetAccountNumber, budgetAccountNumber => IsKnown(budgetAccountNumber, budgetAccountCollection), GetType(), nameof(BudgetAccountNumber), true);
            }

            if (Debit != null)
            {
                result = result.Decimal.ShouldBeGreaterThanOrEqualToZero(Debit.Value, GetType(), nameof(Debit));
            }

            if (Credit != null)
            {
                result = result.Decimal.ShouldBeGreaterThanOrEqualToZero(Credit.Value, GetType(), nameof(Credit));
            }

            if (Debit == null && Credit == null)
            {
                result = result.Object.ShouldNotBeNull(Debit, GetType(), nameof(Debit));
            }

            if (ContactAccountNumber != null)
            {
                result = result.ValidateAccountIdentifier(ContactAccountNumber, GetType(), nameof(ContactAccountNumber), true)
                    .Object.ShouldBeKnownValue(ContactAccountNumber, contactAccountNumber => IsKnown(contactAccountNumber, contactAccountCollection), GetType(), nameof(ContactAccountNumber), true);
            }

            return result.Integer.ShouldBeGreaterThanOrEqualToZero(SortOrder, GetType(), nameof(SortOrder));
        }

        public IPostingLine ToDomain(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            IAccountCollection accountCollection = accounting.AccountCollection;
            IBudgetAccountCollection budgetAccountCollection = accounting.BudgetAccountCollection;
            IContactAccountCollection contactAccountCollection = accounting.ContactAccountCollection;

            return new PostingLine(
                Identifier ?? Guid.NewGuid(),
                PostingDate,
                string.IsNullOrWhiteSpace(Reference) ? null : Reference,
                Resolve(AccountNumber, accountCollection).GetAwaiter().GetResult(),
                Details,
                string.IsNullOrWhiteSpace(BudgetAccountNumber) ? null : Resolve(BudgetAccountNumber, budgetAccountCollection).GetAwaiter().GetResult(),
                Debit ?? 0M,
                Credit ?? 0M,
                string.IsNullOrWhiteSpace(ContactAccountNumber) ? null : Resolve(ContactAccountNumber, contactAccountCollection).GetAwaiter().GetResult(),
                SortOrder);
        }

        private static async Task<bool> IsKnown<TAccount>(string accountNumber, IAccountCollectionBase<TAccount> accountCollection) where TAccount : IAccountBase<TAccount>
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber))
                .NotNull(accountCollection, nameof(accountCollection));

            TAccount account = await Resolve(accountNumber, accountCollection);

            return account != null;
        }

        private static Task<TAccount> Resolve<TAccount>(string accountNumber, IAccountCollectionBase<TAccount> accountCollection) where TAccount : IAccountBase<TAccount>
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber))
                .NotNull(accountCollection, nameof(accountCollection));

            return Task.FromResult(accountCollection.SingleOrDefault(account => account.AccountNumber == accountNumber));
        }

        #endregion
    }
}