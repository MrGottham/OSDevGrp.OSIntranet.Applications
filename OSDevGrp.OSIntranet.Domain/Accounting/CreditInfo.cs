using System;
using System.Linq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class CreditInfo : InfoBase<ICreditInfo>, ICreditInfo
    {
        #region Private variables

        private decimal _credit;

        #endregion

        #region Constructor

        public CreditInfo(IAccount account, short year, short month, decimal credit) 
            : base(year, month)
        {
            NullGuard.NotNull(account, nameof(account));

            if (credit < 0M)
            {
                throw new ArgumentException("The credit value cannot be below 0.", nameof(credit));
            }

            Account = account;
            Credit = credit;
        }

        #endregion

        #region Properties

        public IAccount Account { get; }

        public decimal Credit
        {
            get => _credit;
            set
            {
                if (value < 0M)
                {
                    throw new ArgumentException("The credit value cannot be below 0.", nameof(value));
                }

                _credit = value;
            }
        }

        public decimal Balance { get; private set; }

        #endregion

        #region Methods

        protected override ICreditInfo Calculate(DateTime statusDate)
        {
            DateTime calculationToDate = ResolveCalculationToDate(statusDate);

            Balance = Account.PostingLineCollection.AsParallel()
                .Where(postingLine => postingLine.PostingDate.Date <= calculationToDate)
                .Sum(postingLine => postingLine.PostingValue);

            return this;
        }

        #endregion
    }
}