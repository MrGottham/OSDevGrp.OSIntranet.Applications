using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class AccountCollection : AccountCollectionBase<IAccount, IAccountCollection>, IAccountCollection
    {
        #region Methods

        protected override IAccountCollection Calculate(DateTime statusDate)
        {
            return this;
        }

        #endregion
    } 
}