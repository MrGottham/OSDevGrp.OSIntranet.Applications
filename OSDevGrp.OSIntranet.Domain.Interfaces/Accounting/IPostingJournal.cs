namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IPostingJournal
    {
        IPostingLineCollection PostingLineCollection { get; }
    }
}