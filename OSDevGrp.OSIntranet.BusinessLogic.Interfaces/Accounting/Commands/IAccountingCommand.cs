using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IAccountingCommand : IAccountingIdentificationCommand
    {
        string Name { get; set; }

        int LetterHeadNumber { get; set; }

        BalanceBelowZeroType BalanceBelowZero { get; set; }

        int BackDating { get; set; }

        IAccounting ToDomain(ICommonRepository commonRepository);
    }
}