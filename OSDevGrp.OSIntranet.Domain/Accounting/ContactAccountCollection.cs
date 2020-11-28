using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class ContactAccountCollection : AccountCollectionBase<IContactAccount, IContactAccountCollection>, IContactAccountCollection
    {
        #region Methods

        protected override IContactAccountCollection Calculate(DateTime statusDate, IEnumerable<IContactAccount> calculatedContactAccountCollection)
        {
            NullGuard.NotNull(calculatedContactAccountCollection, nameof(calculatedContactAccountCollection));

            return this;
        }

        #endregion
    } 
}