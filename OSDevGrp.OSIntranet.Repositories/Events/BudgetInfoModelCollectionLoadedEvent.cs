using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Events
{
    internal class BudgetInfoModelCollectionLoadedEvent : ModelCollectionLoadedEventBase<BudgetInfoModel>
    {
        #region Constructor

        public BudgetInfoModelCollectionLoadedEvent(DbContext dbContext, IReadOnlyCollection<BudgetInfoModel> budgetInfoModelCollection, DateTime statusDate)
            : base(dbContext, budgetInfoModelCollection)
        {
            StatusDate = statusDate;
        }

        #endregion

        #region Properties

        internal DateTime StatusDate { get; }

        #endregion
    }
}