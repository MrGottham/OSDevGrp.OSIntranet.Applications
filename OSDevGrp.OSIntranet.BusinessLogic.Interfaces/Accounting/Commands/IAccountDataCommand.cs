using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IAccountDataCommand : IAccountCoreDataCommand<IAccount>
    {
        int AccountGroupNumber { get; set; }
        
        IEnumerable<ICreditInfoCommand> CreditInfoCollection { get; set; }
    }
}