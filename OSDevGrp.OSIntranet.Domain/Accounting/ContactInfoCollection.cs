using System;
using System.Linq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class ContactInfoCollection : InfoCollectionBase<IContactInfo, IContactInfoCollection>, IContactInfoCollection
    {
        #region Properties

        public IContactInfoValues ValuesAtStatusDate { get; private set; } = new ContactInfoValues(0M);

        public IContactInfoValues ValuesAtEndOfLastMonthFromStatusDate { get; private set; } = new ContactInfoValues(0M);

        public IContactInfoValues ValuesAtEndOfLastYearFromStatusDate { get; private set; } = new ContactInfoValues(0M);

        #endregion

        #region Methods

        protected override IContactInfoCollection Calculate(DateTime statusDate, IContactInfo[] calculatedContactInfoCollection)
        {
            NullGuard.NotNull(calculatedContactInfoCollection, nameof(calculatedContactInfoCollection));

            IContactInfo contactInfoAtStatusDate = calculatedContactInfoCollection
                .AsParallel()
                .SingleOrDefault(contactInfo => contactInfo.IsMonthOfStatusDate);
            IContactInfo contactInfoAtEndOfLastMonthFromStatusDate = calculatedContactInfoCollection
                .AsParallel()
                .SingleOrDefault(contactInfo => contactInfo.IsLastMonthOfStatusDate);
            IContactInfo contactInfoAtEndOfLastYearFromStatusDate = calculatedContactInfoCollection
                .AsParallel()
                .Where(contactInfo => contactInfo.IsLastYearOfStatusDate)
                .OrderByDescending(contactInfo => contactInfo.Year)
                .ThenByDescending(contactInfo => contactInfo.Month)
                .FirstOrDefault();

            ValuesAtStatusDate = ToContactInfoValues(contactInfoAtStatusDate);
            ValuesAtEndOfLastMonthFromStatusDate = ToContactInfoValues(contactInfoAtEndOfLastMonthFromStatusDate);
            ValuesAtEndOfLastYearFromStatusDate = ToContactInfoValues(contactInfoAtEndOfLastYearFromStatusDate);

            return this;
        }

        private IContactInfoValues ToContactInfoValues(IContactInfo contactInfo)
        {
            return contactInfo == null ? new ContactInfoValues(0M) : new ContactInfoValues(contactInfo.Balance);
        }

        #endregion
    }
}