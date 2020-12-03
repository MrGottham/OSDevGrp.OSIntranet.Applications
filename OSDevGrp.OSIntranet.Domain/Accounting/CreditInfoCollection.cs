using System;
using System.Linq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class CreditInfoCollection : InfoCollectionBase<ICreditInfo, ICreditInfoCollection>, ICreditInfoCollection
    {
        #region Properties

        public ICreditInfoValues ValuesAtStatusDate { get; private set; } = new CreditInfoValues(0M, 0M);

        public ICreditInfoValues ValuesAtEndOfLastMonthFromStatusDate { get; private set; } = new CreditInfoValues(0M, 0M);

        public ICreditInfoValues ValuesAtEndOfLastYearFromStatusDate { get; private set; } = new CreditInfoValues(0M, 0M);

        #endregion

        #region Methods

        protected override ICreditInfoCollection Calculate(DateTime statusDate, ICreditInfo[] calculatedCreditInfoCollection)
        {
            NullGuard.NotNull(calculatedCreditInfoCollection, nameof(calculatedCreditInfoCollection));

            ICreditInfo creditInfoAtStatusDate = calculatedCreditInfoCollection
                .AsParallel()
                .SingleOrDefault(creditInfo => creditInfo.IsMonthOfStatusDate);
            ICreditInfo creditInfoAtEndOfLastMonthFromStatusDate = calculatedCreditInfoCollection
                .AsParallel()
                .SingleOrDefault(creditInfo => creditInfo.IsLastMonthOfStatusDate);
            ICreditInfo creditInfoAtEndOfLastYearFromStatusDate = calculatedCreditInfoCollection
                .AsParallel()
                .Where(creditInfo => creditInfo.IsLastYearOfStatusDate)
                .OrderByDescending(creditInfo => creditInfo.Year)
                .ThenByDescending(creditInfo => creditInfo.Month)
                .FirstOrDefault();

            ValuesAtStatusDate = ToCreditInfoValues(creditInfoAtStatusDate);
            ValuesAtEndOfLastMonthFromStatusDate = ToCreditInfoValues(creditInfoAtEndOfLastMonthFromStatusDate);
            ValuesAtEndOfLastYearFromStatusDate = ToCreditInfoValues(creditInfoAtEndOfLastYearFromStatusDate);

            return this;
        }

        protected override ICreditInfoCollection AlreadyCalculated() => this;

        private ICreditInfoValues ToCreditInfoValues(ICreditInfo creditInfo)
        {
            return creditInfo == null ? new CreditInfoValues(0M, 0M) : new CreditInfoValues(creditInfo.Credit, creditInfo.Balance);
        }

        #endregion
    }
}