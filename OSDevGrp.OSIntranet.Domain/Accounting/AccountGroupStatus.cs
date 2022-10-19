using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    internal class AccountGroupStatus : AccountGroup, IAccountGroupStatus
    {
        #region Private variables

        private bool _isCalculating;

        #endregion

        #region Constructor

        public AccountGroupStatus(IAccountGroup accountGroup, IAccountCollection accountCollection)
            : base(accountGroup.Number, accountGroup.Name, accountGroup.AccountGroupType, accountGroup.Deletable)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup))
                .NotNull(accountCollection, nameof(accountCollection));

            SetAuditInformation(accountGroup.CreatedDateTime, accountGroup.CreatedByIdentifier, accountGroup.ModifiedDateTime, accountGroup.ModifiedByIdentifier);

            AccountCollection = accountCollection;
        }

        #endregion

        #region Properties

        public IAccountCollection AccountCollection { get; private set; }

        public IAccountCollectionValues ValuesAtStatusDate => AccountCollection.ValuesAtStatusDate;

        public IAccountCollectionValues ValuesAtEndOfLastMonthFromStatusDate => AccountCollection.ValuesAtEndOfLastMonthFromStatusDate;

        public IAccountCollectionValues ValuesAtEndOfLastYearFromStatusDate => AccountCollection.ValuesAtEndOfLastYearFromStatusDate;

        public DateTime StatusDate { get; private set; }

        #endregion

        #region Methods

        public async Task<IAccountGroupStatus> CalculateAsync(DateTime statusDate)
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
                IAccountCollection calculatedAccountCollection = await AccountCollection.CalculateAsync(StatusDate);
                if (calculatedAccountCollection != null)
                {
                    AccountCollection = calculatedAccountCollection;
                }

                return this;
            }
            finally
            {
                _isCalculating = false;
            }
        }

        public override Task<IAccountGroupStatus> CalculateAsync(DateTime statusDate, IAccountCollection accountCollection) => throw new NotSupportedException();

        public override void AddAuditInformation(DateTime createdUtcDateTime, string createdByIdentifier, DateTime modifiedUtcDateTime, string modifiedByIdentifier) => throw new NotSupportedException();

        public override void AllowDeletion() => throw new NotSupportedException();

        public override void DisallowDeletion() => throw new NotSupportedException();

        #endregion
    }
}