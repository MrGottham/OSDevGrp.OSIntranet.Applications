using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Events
{
    internal class AccountModelCollectionLoadedEvent : ModelCollectionLoadedEventBase<AccountModel>
    {
        #region Constructor

        public AccountModelCollectionLoadedEvent(DbContext dbContext, IReadOnlyCollection<AccountModel> accountModelCollection, DateTime statusDate, bool creditInformationIncluded) 
            : base(dbContext, accountModelCollection)
        {
            StatusDate = statusDate;
            CreditInformationIncluded = creditInformationIncluded;
        }

        #endregion

        #region Properties

        internal DateTime StatusDate { get; }

        internal bool CreditInformationIncluded { get; }

        #endregion
    }
}