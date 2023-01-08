using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccounting : IAuditable, ICalculable<IAccounting>, IProtectable
    {
        int Number { get; }

        string Name { get; }

        ILetterHead LetterHead { get; set; }

        BalanceBelowZeroType BalanceBelowZero { get; set; }

        int BackDating { get; set; }

        bool DefaultForPrincipal { get; }

        IAccountCollection AccountCollection { get; }

        IBudgetAccountCollection BudgetAccountCollection { get; }

        IContactAccountCollection ContactAccountCollection { get; }

        void ApplyDefaultForPrincipal(int? defaultAccountingNumber);

        Task<IPostingLineCollection> GetPostingLinesAsync(DateTime statusDate);
    }
}