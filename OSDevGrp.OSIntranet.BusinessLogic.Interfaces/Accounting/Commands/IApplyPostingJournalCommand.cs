using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IApplyPostingJournalCommand : IAccountingIdentificationCommand
    {
        IEnumerable<IApplyPostingLineCommand> PostingLineCollection { get; set; }

        IPostingJournal ToDomain(IAccountingRepository accountingRepository);
    }
}