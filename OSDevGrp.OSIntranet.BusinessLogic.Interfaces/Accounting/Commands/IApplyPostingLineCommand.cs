using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IApplyPostingLineCommand
    {
        Guid? Identifier { get; set; }

        DateTime PostingDate { get; set; }

        string Reference { get; set; }

        string AccountNumber { get; set; }

        string Details { get; set; }

        string BudgetAccountNumber { get; set; }

        decimal? Debit { get; set; }

        decimal? Credit { get; set; }

        string ContactAccountNumber { get; set; }

        int SortOrder { get; set; }

        IValidator Validate(IValidator validator, IAccounting accounting);

        IPostingLine ToDomain(IAccounting accounting);
    }
}