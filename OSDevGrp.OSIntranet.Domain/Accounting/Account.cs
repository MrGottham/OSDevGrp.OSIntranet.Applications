using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class Account : AccountBase<IAccount>, IAccount
    {
        #region Private variables

        private IAccountGroup _accountGroup;

        #endregion

        #region Constructors

        public Account(IAccounting accounting, string accountNumber, string accountName, IAccountGroup accountGroup)
            : this(accounting, accountNumber, accountName, accountGroup, new CreditInfoCollection(), new PostingLineCollection())
        {
        }

        public Account(IAccounting accounting, string accountNumber, string accountName, IAccountGroup accountGroup, ICreditInfoCollection creditInfoCollection, IPostingLineCollection postingLineCollection)
            : base(accounting, accountNumber, accountName, postingLineCollection)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup))
                .NotNull(creditInfoCollection, nameof(creditInfoCollection));

            AccountGroup = accountGroup;
            CreditInfoCollection = creditInfoCollection;
        }

        #endregion

        #region Properties

        public IAccountGroup AccountGroup
        { 
            get => _accountGroup;
            set
            {
                NullGuard.NotNull(value, nameof(value));

                _accountGroup = value;
            } 
        }

        public AccountGroupType AccountGroupType => AccountGroup.AccountGroupType;

        public ICreditInfoValues ValuesAtStatusDate => CreditInfoCollection.ValuesAtStatusDate;

        public ICreditInfoValues ValuesAtEndOfLastMonthFromStatusDate => CreditInfoCollection.ValuesAtEndOfLastMonthFromStatusDate;

        public ICreditInfoValues ValuesAtEndOfLastYearFromStatusDate => CreditInfoCollection.ValuesAtEndOfLastYearFromStatusDate;

        public ICreditInfoCollection CreditInfoCollection { get; private set; }

        #endregion

        #region Methods

        public override void ApplyProtection()
        {
            CreditInfoCollection.ApplyProtection();

            base.ApplyProtection();
        }

        protected override Task[] GetCalculationTasks(DateTime statusDate)
        {
            return new[]
            {
                CalculateCreditInfoCollectionAsync(statusDate),
                CalculatePostingLineCollectionAsync(statusDate)
            };
        }

        protected override async Task<IAccount> GetCalculationResultAsync()
        {
            PostingLineCollection = await PostingLineCollection.ApplyCalculationAsync(this);

            return this;
        }

        protected override IAccount AlreadyCalculated() => this;

        private async Task CalculateCreditInfoCollectionAsync(DateTime statusDate)
        {
            CreditInfoCollection = await CreditInfoCollection.CalculateAsync(statusDate);
        }

        #endregion
    }
}