using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Events
{
    internal class ContactAccountModelCollectionLoadedEvent : ModelCollectionLoadedEventBase<ContactAccountModel>
    {
        #region Constructor

        public ContactAccountModelCollectionLoadedEvent(DbContext dbContext, IReadOnlyCollection<ContactAccountModel> contactAccountModelCollection, DateTime statusDate)
            : base(dbContext, contactAccountModelCollection)
        {
            StatusDate = statusDate;
        }

        #endregion

        #region Properties

        internal DateTime StatusDate { get; }

        #endregion
    }
}