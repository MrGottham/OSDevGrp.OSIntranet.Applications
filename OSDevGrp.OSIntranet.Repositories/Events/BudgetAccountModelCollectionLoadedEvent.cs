using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Events
{
    internal class BudgetAccountModelCollectionLoadedEvent : ModelCollectionLoadedEventBase<BudgetAccountModel>
    {
        #region Constructor

        public BudgetAccountModelCollectionLoadedEvent(DbContext dbContext, IReadOnlyCollection<BudgetAccountModel> budgetAccountModelCollection, DateTime statusDate, bool budgetInformationIncluded)
            : base(dbContext, budgetAccountModelCollection)
        {
            StatusDate = statusDate;
            BudgetInformationIncluded = budgetInformationIncluded;
        }

        #endregion

        #region Properties

        internal DateTime StatusDate { get; }

        internal bool BudgetInformationIncluded { get; }

        #endregion
    }
}