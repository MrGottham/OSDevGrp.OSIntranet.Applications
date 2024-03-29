﻿using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IBudgetAccountDataCommand : IAccountCoreDataCommand<IBudgetAccount>
    {
        int BudgetAccountGroupNumber { get; set; }
        
        IEnumerable<IBudgetInfoCommand> BudgetInfoCollection { get; set; }
    }
}