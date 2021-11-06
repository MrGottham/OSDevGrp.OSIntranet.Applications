using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class PostingWarningCollection : List<IPostingWarning>, IPostingWarningCollection
    {
        #region Methods

        public new void Add(IPostingWarning postingWarning)
        {
            NullGuard.NotNull(postingWarning, nameof(postingWarning));

            base.Add(postingWarning);
        }

        public void Add(IEnumerable<IPostingWarning> postingWarningCollection)
        {
            NullGuard.NotNull(postingWarningCollection, nameof(postingWarningCollection));

            foreach (IPostingWarning postingWarning in postingWarningCollection)
            {
                Add(postingWarning);
            }
        }

        public IPostingWarningCollection Ordered()
        {
            IPostingWarningCollection postingWarningCollection = new PostingWarningCollection
            {
                this.AsParallel().OrderByDescending(postingWarning => postingWarning.PostingLine.PostingDate.Date).ThenByDescending(postingWarning => postingWarning.PostingLine.SortOrder).ThenBy(postingWarning => (int) postingWarning.Reason)
            };

            return postingWarningCollection;
        }

        #endregion
    }
}