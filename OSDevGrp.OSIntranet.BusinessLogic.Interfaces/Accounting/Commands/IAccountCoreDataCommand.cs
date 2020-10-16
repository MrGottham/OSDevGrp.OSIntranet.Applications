using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IAccountCoreDataCommand<out TAccount> : IAccountIdentificationCommand where TAccount : IAccountBase
    {
        string AccountName { get; set; }

        string Description { get; set; }

        string Note { get; set; }

        TAccount ToDomain(IAccountingRepository accountingRepository);
    }
}