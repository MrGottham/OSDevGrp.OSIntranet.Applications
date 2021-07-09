using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Events
{
    internal class PostingLineModelCollectionLoadedEvent : ModelCollectionLoadedEventBase<PostingLineModel>
    {
        #region Constructor

        public PostingLineModelCollectionLoadedEvent(DbContext dbContext, IReadOnlyCollection<PostingLineModel> postingLineModelCollection, DateTime fromDate, DateTime toDate)
            : base(dbContext, postingLineModelCollection)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        #endregion

        #region Properties

        internal DateTime FromDate { get; }

        internal DateTime ToDate { get; }

        #endregion

        #region Methods

        internal bool ContainsMorePostingLines(IReadOnlyCollection<PostingLineModel> postingLineModelCollection)
        {
            NullGuard.NotNull(postingLineModelCollection, nameof(postingLineModelCollection));

            return ModelCollection.Count > postingLineModelCollection.Count;
        }

        #endregion
    }
}