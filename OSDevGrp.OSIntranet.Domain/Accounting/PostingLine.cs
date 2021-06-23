using System;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class PostingLine : AuditableBase, IPostingLine
    {
        #region Constructor

        public PostingLine(Guid identifier, DateTime postingDate, string reference, IAccount account, string details, IBudgetAccount budgetAccount, decimal debit, decimal credit, IContactAccount contactAccount, int sortOrder)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNullOrWhiteSpace(details, nameof(details));

            if (postingDate.Year < InfoBase<ICreditInfo>.MinYear || postingDate.Year > InfoBase<ICreditInfo>.MaxYear)
            {
                throw new ArgumentException($"Year for the posting data should be between {InfoBase<ICreditInfo>.MinYear} and {InfoBase<ICreditInfo>.MaxYear}.", nameof(postingDate));
            }

            if (postingDate.Month < InfoBase<ICreditInfo>.MinMonth || postingDate.Month > InfoBase<ICreditInfo>.MaxMonth)
            {
                throw new ArgumentException($"Month for the posting data should be between {InfoBase<ICreditInfo>.MinMonth} and {InfoBase<ICreditInfo>.MaxMonth}.", nameof(postingDate));
            }

            if (budgetAccount != null && budgetAccount.Accounting.Number != account.Accounting.Number)
            {
                throw new ArgumentException("Accounting on the given budget account does not match the accounting on the given account.", nameof(budgetAccount));
            }

            if (debit < 0M)
            {
                throw new ArgumentException("Debit cannot be lower than 0.", nameof(debit));
            }

            if (credit < 0M)
            {
                throw new ArgumentException("Credit cannot be lower than 0.", nameof(credit));
            }

            if (contactAccount != null && contactAccount.Accounting.Number != account.Accounting.Number)
            {
                throw new ArgumentException("Accounting on the given contact account does not match the accounting on the given account.", nameof(contactAccount));
            }

            if (sortOrder < 0)
            {
                throw new ArgumentException("Sort order cannot be lower than 0.", nameof(sortOrder));
            }

            Identifier = identifier;
            Accounting = account.Accounting;
            PostingDate = postingDate;
            Reference = string.IsNullOrWhiteSpace(reference) ? null : reference.Trim();
            Account = account;
            AccountValuesAtPostingDate = new CreditInfoValues(0M, 0M);
            Details = details.Trim();
            BudgetAccount = budgetAccount;
            BudgetAccountValuesAtPostingDate = budgetAccount != null ? new BudgetInfoValues(0M, 0M) : null;
            Debit = debit;
            Credit = credit;
            ContactAccount = contactAccount;
            ContactAccountValuesAtPostingDate = contactAccount != null ? new ContactInfoValues(0M) : null;
            SortOrder = sortOrder;
        }

        #endregion

        #region Properties

        public Guid Identifier { get; }

        public IAccounting Accounting { get; private set; }

        public DateTime PostingDate { get; }

        public string Reference { get; }

        public IAccount Account { get; private set; }

        public ICreditInfoValues AccountValuesAtPostingDate { get; private set; }

        public string Details { get; }

        public IBudgetAccount BudgetAccount { get; private set; }

        public IBudgetInfoValues BudgetAccountValuesAtPostingDate { get; private set; }

        public decimal Debit { get; }

        public decimal Credit { get; }

        public decimal PostingValue => Debit - Credit;

        public IContactAccount ContactAccount { get; private set; }

        public IContactInfoValues ContactAccountValuesAtPostingDate { get; private set; }

        public int SortOrder { get; }

        public DateTime StatusDate { get; private set; }

        #endregion

        #region Methods

        public async Task<IPostingLine> CalculateAsync(DateTime statusDate)
        {
            if (statusDate.Date == StatusDate)
            {
                return this;
            }

            StatusDate = statusDate.Date;

            Task[] calculationTasks =
            {
                CalculateAccountingAsync(StatusDate),
                CalculateAccountAsync(StatusDate),
                CalculateBudgetAccountAsync(StatusDate),
                CalculateContactAccountAsync(StatusDate)
            };
            await Task.WhenAll(calculationTasks);

            return this;
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is IPostingLine postingLine)
            {
                return postingLine.Identifier == Identifier;
            }

            return false;
        }

        private async Task CalculateAccountingAsync(DateTime statusDate)
        {
            Accounting = await Accounting.CalculateAsync(statusDate);
        }

        private async Task CalculateAccountAsync(DateTime statusDate)
        {
            Account = await Account.CalculateAsync(statusDate);

            ICreditInfo creditInfo = FindInfo<ICreditInfo, ICreditInfoCollection>(Account.CreditInfoCollection, PostingDate);
            decimal balance = CalculatePostingValue(Account.PostingLineCollection, DateTime.MinValue, PostingDate, SortOrder);

            AccountValuesAtPostingDate = new CreditInfoValues(creditInfo?.Credit ?? 0M, balance);
        }

        private async Task CalculateBudgetAccountAsync(DateTime statusDate)
        {
            if (BudgetAccount == null)
            {
                BudgetAccountValuesAtPostingDate = null;
                return;
            }

            BudgetAccount = await BudgetAccount.CalculateAsync(statusDate);

            IBudgetInfo budgetInfo = FindInfo<IBudgetInfo, IBudgetInfoCollection>(BudgetAccount.BudgetInfoCollection, PostingDate);
            decimal posted = CalculatePostingValue(BudgetAccount.PostingLineCollection, new DateTime(PostingDate.Year, PostingDate.Month, 1), PostingDate, SortOrder);

            BudgetAccountValuesAtPostingDate = new BudgetInfoValues(budgetInfo?.Budget ?? 0M, posted);
        }

        private async Task CalculateContactAccountAsync(DateTime statusDate)
        {
            if (ContactAccount == null)
            {
                ContactAccountValuesAtPostingDate = null;
                return;
            }

            ContactAccount = await ContactAccount.CalculateAsync(statusDate);

            decimal balance = CalculatePostingValue(ContactAccount.PostingLineCollection, DateTime.MinValue, PostingDate, SortOrder);

            ContactAccountValuesAtPostingDate = new ContactInfoValues(balance);
        }

        private static TInfo FindInfo<TInfo, TInfoCollection>(TInfoCollection infoCollection, DateTime postingDate) where TInfo : IInfo<TInfo> where TInfoCollection : IInfoCollection<TInfo, TInfoCollection>
        {
            NullGuard.NotNull(infoCollection, nameof(infoCollection));

            return infoCollection.SingleOrDefault(info => info.Year == (short) postingDate.Year && info.Month == (short) postingDate.Month);
        }

        private static decimal CalculatePostingValue(IPostingLineCollection postingLineCollection, DateTime fromDate, DateTime toDate, int sortOrder)
        {
            NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection));

            return postingLineCollection
                .AsParallel()
                .Where(postingLine => postingLine.PostingDate.Date >= fromDate.Date &&
                                      postingLine.PostingDate.Date <= toDate.Date &&
                                      postingLine.SortOrder <= sortOrder)
                .Sum(postingLine => postingLine.PostingValue);
        }

        #endregion
    }
}