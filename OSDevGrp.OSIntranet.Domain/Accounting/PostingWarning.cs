using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class PostingWarning : IPostingWarning
    {
        #region Constructor

        public PostingWarning(PostingWarningReason reason, IAccountBase account, decimal amount, IPostingLine postingLine)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(postingLine, nameof(postingLine));

            Reason = reason;
            Account = account;
            Amount = amount;
            PostingLine = postingLine;
        }

        #endregion

        #region Properties

        public PostingWarningReason Reason { get; }

        public IAccountBase Account { get; }

        public decimal Amount { get; }

        public IPostingLine PostingLine { get; }

        #endregion
    }
}