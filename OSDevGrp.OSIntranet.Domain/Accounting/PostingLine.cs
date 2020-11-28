using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class PostingLine : AuditableBase, IPostingLine
    {
        #region Constructor

        public PostingLine(Guid identifier, DateTime postingDate)
        {
            if (postingDate.Year < InfoBase<ICreditInfo>.MinYear || postingDate.Year > InfoBase<ICreditInfo>.MaxYear)
            {
                throw new ArgumentException($"Year for the posting data should be between {InfoBase<ICreditInfo>.MinYear} and {InfoBase<ICreditInfo>.MaxYear}.", nameof(postingDate));
            }

            if (postingDate.Month < InfoBase<ICreditInfo>.MinMonth || postingDate.Month > InfoBase<ICreditInfo>.MaxMonth)
            {
                throw new ArgumentException($"Month for the posting data should be between {InfoBase<ICreditInfo>.MinMonth} and {InfoBase<ICreditInfo>.MaxMonth}.", nameof(postingDate));
            }

            Identifier = identifier;
            PostingDate = postingDate;
        }

        #endregion

        #region Properties

        public Guid Identifier { get; }

        public DateTime PostingDate { get; }

        public decimal PostingValue => 0M;

        public DateTime StatusDate { get; private set; }

        #endregion

        #region Methods

        public Task<IPostingLine> CalculateAsync(DateTime statusDate)
        {
            return Task.Run<IPostingLine>(() =>
            {
                StatusDate = statusDate.Date;

                return this;
            });
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is IPostingLine postingLine)
            {
                return postingLine.Identifier == Identifier;
            }

            return false;
        }

        #endregion
    }
}