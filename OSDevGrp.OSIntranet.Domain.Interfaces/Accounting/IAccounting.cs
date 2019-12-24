using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccounting : IAuditable, IDeletable
    {
        int Number { get; }

        string Name { get; }

        ILetterHead LetterHead { get; set; }

        BalanceBelowZeroType BalanceBelowZero { get; set; }

        int BackDating { get; set; }

        bool DefaultForPrincipal { get; }

        void ApplyDefaultForPrincipal(int? defaultAccountingNumber);
    }
}