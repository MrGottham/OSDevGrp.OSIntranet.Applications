using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class PostingJournalResult : PostingJournal, IPostingJournalResult
    {
        #region Private variables

        private bool _isCalculating;
        private readonly IPostingWarningCalculator _postingWarningCalculator;

        #endregion

        #region Constructor

        public PostingJournalResult(IPostingLineCollection postingLineCollection, IPostingWarningCalculator postingWarningCalculator) 
            : base(postingLineCollection)
        {
            NullGuard.NotNull(postingWarningCalculator, nameof(postingWarningCalculator));

            _postingWarningCalculator = postingWarningCalculator;
        }

        #endregion

        #region Properties

        public IPostingWarningCollection PostingWarningCollection { get; private set; } = new PostingWarningCollection();

        public DateTime StatusDate { get; private set; }

        #endregion

        #region Methods

        public async Task<IPostingJournalResult> CalculateAsync(DateTime statusDate)
        {
            if (statusDate.Date == StatusDate)
            {
                while (_isCalculating)
                {
                    await Task.Delay(250);
                }

                return this;
            }

            StatusDate = statusDate.Date;

            _isCalculating = true;
            try
            {
                PostingLineCollection = await PostingLineCollection.CalculateAsync(StatusDate);
                PostingWarningCollection = await _postingWarningCalculator.CalculateAsync(PostingLineCollection);

                return this;
            }
            finally
            {
                _isCalculating = false;
            }
        }

        #endregion
    }
}