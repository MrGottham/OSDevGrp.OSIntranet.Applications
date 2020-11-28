using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Core;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class Accounting : AuditableBase, IAccounting
    {
        #region Private variables

        private ILetterHead _letterHead;

        #endregion

        #region Constructors

        public Accounting(int number, string name, ILetterHead letterHead)
            : this(number, name, letterHead, BalanceBelowZeroType.Creditors, 30)
        {
        }

        public Accounting(int number, string name, ILetterHead letterHead, BalanceBelowZeroType balanceBelowZero, int backDating)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name))
                .NotNull(letterHead, nameof(letterHead));

            Number = number;
            Name = name.Trim();
            LetterHead = letterHead;
            BalanceBelowZero = balanceBelowZero;
            BackDating = backDating;
            AccountCollection = new AccountCollection();
            BudgetAccountCollection = new BudgetAccountCollection();
            ContactAccountCollection = new ContactAccountCollection();
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

        public bool Deletable { get; private set; }

        public bool DefaultForPrincipal { get; private set; }

        public IAccountCollection AccountCollection { get; protected set; }

        public IBudgetAccountCollection BudgetAccountCollection { get; protected set; }

        public IContactAccountCollection ContactAccountCollection { get; protected set; }

        #endregion

        #region Methods

        public async Task<IAccounting> CalculateAsync(DateTime statusDate)
        {
            StatusDate = statusDate.Date;

            Task<IAccountCollection> calculateAccountCollectionTask = AccountCollection.CalculateAsync(StatusDate);
            Task<IBudgetAccountCollection> calculateBudgetAccountCollectionTask = BudgetAccountCollection.CalculateAsync(StatusDate);
            Task<IContactAccountCollection> calculateContactAccountCollectionTask = ContactAccountCollection.CalculateAsync(StatusDate);
            await Task.WhenAll(calculateAccountCollectionTask, calculateBudgetAccountCollectionTask, calculateContactAccountCollectionTask);

            AccountCollection = calculateAccountCollectionTask.GetAwaiter().GetResult();
            BudgetAccountCollection = calculateBudgetAccountCollectionTask.GetAwaiter().GetResult();
            ContactAccountCollection = calculateContactAccountCollectionTask.GetAwaiter().GetResult();

            return this;
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

        #endregion
    }
}