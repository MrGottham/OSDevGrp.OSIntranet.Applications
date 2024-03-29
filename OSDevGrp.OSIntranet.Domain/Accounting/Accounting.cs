using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class Accounting : AuditableBase, IAccounting
    {
        #region Private variables

        private ILetterHead _letterHead;
        private bool _isCalculating;

        #endregion

        #region Constructors

        public Accounting(int number, string name, ILetterHead letterHead)
            : this(number, name, letterHead, BalanceBelowZeroType.Creditors, 30)
        {
        }

        public Accounting(int number, string name, ILetterHead letterHead, BalanceBelowZeroType balanceBelowZero, int backDating)
            : this(number, name, letterHead, balanceBelowZero, backDating, new AccountCollection(), new BudgetAccountCollection(), new ContactAccountCollection())
        {
        }

        public Accounting(int number, string name, ILetterHead letterHead, BalanceBelowZeroType balanceBelowZero, int backDating, IAccountCollection accountCollection, IBudgetAccountCollection budgetAccountCollection, IContactAccountCollection contactAccountCollection)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name))
                .NotNull(letterHead, nameof(letterHead))
                .NotNull(accountCollection, nameof(accountCollection))
                .NotNull(budgetAccountCollection, nameof(budgetAccountCollection))
                .NotNull(contactAccountCollection, nameof(contactAccountCollection));

            Number = number;
            Name = name.Trim();
            LetterHead = letterHead;
            BalanceBelowZero = balanceBelowZero;
            BackDating = backDating;
            AccountCollection = accountCollection;
            BudgetAccountCollection = budgetAccountCollection;
            ContactAccountCollection = contactAccountCollection;
        }

        #endregion

        #region Properties

        public int Number { get; }

        public string Name { get; }

        public ILetterHead LetterHead
        {
            get => _letterHead;
            set
            {
                NullGuard.NotNull(value, nameof(value));

                _letterHead = value;
            }
        }

        public BalanceBelowZeroType BalanceBelowZero { get; set; }

        public int BackDating { get; set; }

        public DateTime StatusDate { get; private set; }

        public bool IsProtected { get; private set; }

        public bool Deletable { get; private set; }

        public bool DefaultForPrincipal { get; private set; }

        public IAccountCollection AccountCollection { get; private set; }

        public IBudgetAccountCollection BudgetAccountCollection { get; private set; }

        public IContactAccountCollection ContactAccountCollection { get; private set; }

        #endregion

        #region Methods

        public async Task<IAccounting> CalculateAsync(DateTime statusDate)
        {
            if (statusDate.Date == StatusDate)
            {
                while (_isCalculating)
                {
                    await Task.Delay(250);
                }

                return this;
            }

            StatusDate = statusDate.Date;

            _isCalculating = true;
            try
            {
                await Task.WhenAll(
                    GetAccountCollectionCalculationTask(StatusDate),
                    GetBudgetAccountCollectionCalculationTask(StatusDate),
                    GetContactAccountCollectionCalculationTask(StatusDate));

                foreach (IPostingLineCollection postingLineCollection in AccountCollection.AsParallel().Select(account => account.PostingLineCollection).ToArray())
                {
                    await postingLineCollection.ApplyCalculationAsync(this);
                }

                return this;
            }
            finally
            {
                _isCalculating = false;
            }
        }

        public void ApplyProtection()
        {
            AccountCollection?.ApplyProtection();
            BudgetAccountCollection.ApplyProtection();
            ContactAccountCollection.ApplyProtection();

            DisallowDeletion();

            IsProtected = true;
        }

        public void AllowDeletion()
        {
            Deletable = true;
        }

        public void DisallowDeletion()
        {
            Deletable = false;
        }

        public void ApplyDefaultForPrincipal(int? defaultAccountingNumber)
        {
            DefaultForPrincipal = defaultAccountingNumber.HasValue && defaultAccountingNumber.Value == Number;
        }

        public async Task<IPostingLineCollection> GetPostingLinesAsync(DateTime statusDate)
        {
            IAccountCollection calculatedAccountCollection = await AccountCollection.CalculateAsync(statusDate);

            IPostingLine[] postingLineArray = calculatedAccountCollection.SelectMany(m => m.PostingLineCollection)
                .AsParallel()
                .Where(postingLine => postingLine.PostingDate.Date <= statusDate.Date)
                .OrderByDescending(postingLine => postingLine.PostingDate.Date)
                .ThenByDescending(postingLine => postingLine.SortOrder)
                .ToArray();

            IPostingLineCollection postingLineCollection = new PostingLineCollection
            {
                postingLineArray
            };

            return await postingLineCollection.CalculateAsync(statusDate);
        }

        private async Task GetAccountCollectionCalculationTask(DateTime statusDate)
        {
            AccountCollection = await AccountCollection.CalculateAsync(statusDate);
        }

        private async Task GetBudgetAccountCollectionCalculationTask(DateTime statusDate)
        {
            BudgetAccountCollection = await BudgetAccountCollection.CalculateAsync(statusDate);
        }

        private async Task GetContactAccountCollectionCalculationTask(DateTime statusDate)
        {
            ContactAccountCollection = await ContactAccountCollection.CalculateAsync(statusDate);
        }

        #endregion
    }
}