using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class ContactAccountCollection : AccountCollectionBase<IContactAccount, IContactAccountCollection>, IContactAccountCollection
    {
        #region Methods

        protected override IContactAccountCollection Calculate(DateTime statusDate)
        {
            return this;
        }

        #endregion
    } 
}