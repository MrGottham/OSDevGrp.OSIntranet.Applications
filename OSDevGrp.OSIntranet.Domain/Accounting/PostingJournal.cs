using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class PostingJournal : IPostingJournal
    {
        #region Constructor

        public PostingJournal(IPostingLineCollection postingLineCollection)
        {
            NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection));

            PostingLineCollection = postingLineCollection;
        }

        #endregion

        #region Properties

        public IPostingLineCollection PostingLineCollection { get; protected set; }

        #endregion
    }
}