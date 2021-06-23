using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IPostingWarning
    {
        PostingWarningReason Reason { get; }

        IAccountBase Account { get; }

        decimal Amount { get; }

        IPostingLine PostingLine { get; }
    }
}