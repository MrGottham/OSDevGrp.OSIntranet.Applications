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

        public async Task<IPostingLineCollection> CalculateAsync(DateTime statusDate)
        {
            StatusDate = statusDate.Date;

            await Task.WhenAll(this.Select(postingLine=> postingLine.CalculateAsync(StatusDate)).ToArray());

            return this;
        }

        #endregion
    }
}