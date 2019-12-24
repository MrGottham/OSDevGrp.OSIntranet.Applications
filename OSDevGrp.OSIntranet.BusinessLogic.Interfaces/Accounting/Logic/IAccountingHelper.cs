using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic
{
    public interface IAccountingHelper
    {
        IAccounting ApplyLogicForPrincipal(IAccounting accounting);

        IEnumerable<IAccounting> ApplyLogicForPrincipal(IEnumerable<IAccounting> accountingCollection);
    }
}