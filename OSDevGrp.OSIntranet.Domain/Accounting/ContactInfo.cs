using System;
using System.Linq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class ContactInfo : InfoBase<IContactInfo>, IContactInfo
    {
        #region Constructor

        public ContactInfo(IContactAccount contactAccount, short year, short month) 
            : base(year, month)
        {
            NullGuard.NotNull(contactAccount, nameof(contactAccount));

            ContactAccount = contactAccount;
        }

        #endregion

        #region Properties

        public IContactAccount ContactAccount { get; }

        public decimal Balance { get; private set; }

        #endregion

        #region Methods

        protected override IContactInfo Calculate(DateTime statusDate)
        {
            DateTime calculationToDate = ResolveCalculationToDate(statusDate);

            Balance = ContactAccount.PostingLineCollection.AsParallel()
                .Where(postingLine => postingLine.PostingDate.Date <= calculationToDate)
                .Sum(postingLine => postingLine.PostingValue);

            return this;
        }

        protected override IContactInfo AlreadyCalculated() => this;

        #endregion
    }
}