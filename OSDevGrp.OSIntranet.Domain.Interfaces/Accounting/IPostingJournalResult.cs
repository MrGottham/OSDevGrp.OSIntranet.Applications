namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IPostingJournalResult : IPostingJournal, ICalculable<IPostingJournalResult>
    {
        IPostingWarningCollection PostingWarningCollection { get; }
    }
}