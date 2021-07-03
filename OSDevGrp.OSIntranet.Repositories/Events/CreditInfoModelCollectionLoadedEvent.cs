using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Events
{
    internal class CreditInfoModelCollectionLoadedEvent : ModelCollectionLoadedEventBase<CreditInfoModel>
    {
        #region Constructor

        public CreditInfoModelCollectionLoadedEvent(DbContext dbContext, IReadOnlyCollection<CreditInfoModel> creditInfoModelCollection, DateTime statusDate)
            : base(dbContext, creditInfoModelCollection)
        {
            StatusDate = statusDate;
        }

        #endregion

        #region Properties

        internal DateTime StatusDate { get; }

        #endregion
    }
}