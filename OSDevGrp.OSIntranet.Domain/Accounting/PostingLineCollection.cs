using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class PostingLineCollection : HashSet<IPostingLine>, IPostingLineCollection
    {
        #region Properties

        public DateTime StatusDate { get; private set;  }

        #endregion

        #region Methods

        public new void Add(IPostingLine postingLine)
        {
            NullGuard.NotNull(postingLine, nameof(postingLine));

            if (base.Add(postingLine))
            {
                return;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ObjectAlreadyExists, postingLine.GetType().Name).Build();
        }

        public void Add(IEnumerable<IPostingLine> postingLineCollection)
        {
            NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection));

            foreach (IPostingLine postingLine in postingLineCollection)
            {
                this.Add(postingLine);
            }
        }

        public IPostingLineCollection Between(DateTime fromDate, DateTime toDate)
        {
            IPostingLineCollection postingLineCollection = new PostingLineCollection
            {
                this.AsParallel().Where(postingLine => postingLine.PostingDate.Date >= fromDate.Date && postingLine.PostingDate.Date <= toDate)
            };

            return postingLineCollection;
        }

        public IPostingLineCollection Ordered()
        {
            IPostingLineCollection postingLineCollection = new PostingLineCollection
            {
                this.AsParallel().OrderByDescending(postingLine => postingLine.PostingDate.Date).ThenByDescending(postingLine => postingLine.SortOrder)
            };

            return postingLineCollection;
        }

        public IPostingLineCollection Top(int numberOfPostingLines)
        {
            IPostingLineCollection postingLineCollection = new PostingLineCollection
            {
                Ordered().Take(numberOfPostingLines)
            };

            return postingLineCollection;
        }

        public async Task<IPostingLineCollection> CalculateAsync(DateTime statusDate)
        {
            if (statusDate.Date == StatusDate)
            {
                return this;
            }

            StatusDate = statusDate.Date;

            await Task.WhenAll(this.Select(postingLine=> postingLine.CalculateAsync(StatusDate)).ToArray());

            return this;
        }

        #endregion
    }
}