using System;
using System.Collections.Concurrent;
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
        #region Private variables

        private IDictionary<int, IPostingLineCollection> _yearMonthDictionary;
        private IDictionary<int, IPostingLineCollection> _yearDictionary;
        private IDictionary<int, decimal> _postingValueDictionary;
        private readonly object _syncRoot = new object();

        #endregion

        #region Properties

        public DateTime StatusDate { get; private set;  }

        #endregion

        #region Methods

        public new void Add(IPostingLine postingLine)
        {
            NullGuard.NotNull(postingLine, nameof(postingLine));

            if (base.Add(postingLine))
            {
                ClearDictionaries();

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
            return Between(fromDate, toDate, null);
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

        public decimal CalculatePostingValue(DateTime fromDate, DateTime toDate, int? sortOrder = null)
        {
            if (fromDate.Date > toDate.Date)
            {
                return 0M;
            }

            IDictionary<int, decimal> postingValueDictionary = GetPostingValueDictionary();

            if (IsSameYearMonth(fromDate, toDate))
            {
                if (IsFullMonth(fromDate, toDate) == false || sortOrder.HasValue)
                {
                    return Between(fromDate, toDate, sortOrder)
                        .AsParallel()
                        .Sum(postingLine => postingLine.PostingValue);
                }

                return ResolvePostingValue(postingValueDictionary, ToYearMonthKey(toDate));
            }

            decimal postingValue = IsFirstDayInMonth(fromDate)
                ? ResolvePostingValue(postingValueDictionary, ToYearMonthKey(fromDate))
                : Between(fromDate, ToLastDayInMonth(fromDate), null)
                    .AsParallel()
                    .Sum(postingLine => postingLine.PostingValue);

            int fromYearMonthKey = ToYearMonthKey(ToFirstDayInMonth(fromDate).AddMonths(1));
            int toYearMonthKey = ToYearMonthKey(ToFirstDayInMonth(toDate).AddMonths(-1));
            postingValue += postingValueDictionary
                .AsParallel()
                .Where(item => item.Key >= fromYearMonthKey && item.Key <= toYearMonthKey)
                .Sum(item => item.Value);

            postingValue += IsLastDayInMonth(toDate) && sortOrder.HasValue == false
                ? ResolvePostingValue(postingValueDictionary, ToYearMonthKey(toDate))
                : Between(ToFirstDayInMonth(toDate), toDate, sortOrder)
                    .AsParallel()
                    .Sum(postingLine => postingLine.PostingValue);

            return postingValue;
        }

        public async Task<IPostingLineCollection> CalculateAsync(DateTime statusDate)
        {
            if (statusDate.Date == StatusDate)
            {
                return this;
            }

            StatusDate = statusDate.Date;

            await Task.WhenAll(this.GroupBy(postingLine => postingLine.PostingDate.Year)
                .Select(group => CalculateAsync(group.Where(postingLine => postingLine.StatusDate != StatusDate), StatusDate))
                .ToArray());

            return this;
        }

        private IPostingLineCollection Between(DateTime fromDate, DateTime toDate, int? sortOrder)
        {
            if (IsSameYearMonth(fromDate, toDate))
            {
                return GetYearMonthDictionary().TryGetValue(ToYearMonthKey(toDate), out IPostingLineCollection postingLineCollection)
                    ? Between(postingLineCollection, fromDate, toDate, sortOrder)
                    : Between(Array.Empty<IPostingLine>(), fromDate, toDate, sortOrder);
            }

            if (IsSameYear(fromDate, toDate))
            {
                return GetYearDictionary().TryGetValue(toDate.Year, out IPostingLineCollection postingLineCollection)
                    ? Between(postingLineCollection, fromDate, toDate, sortOrder)
                    : Between(Array.Empty<IPostingLine>(), fromDate, toDate, sortOrder);
            }

            return Between(this, fromDate, toDate, sortOrder);
        }

        private IPostingLineCollection Between(IEnumerable<IPostingLine> postingLineCollection, DateTime fromDate, DateTime toDate, int? sortOrder)
        {
            NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection));

            return new PostingLineCollection
            {
                postingLineCollection.AsParallel()
                    .Where(postingLine =>
                    {
                        DateTime postingDate = postingLine.PostingDate.Date;

                        return postingDate >= fromDate.Date &&
                               postingDate <= toDate.Date &&
                               (sortOrder == null || postingDate < toDate.Date || postingDate == toDate.Date && postingLine.SortOrder <= sortOrder.Value);
                    })
            };
        }

        private IDictionary<int, IPostingLineCollection> GetYearMonthDictionary()
        {
            lock (_syncRoot)
            {
                if (_yearMonthDictionary != null)
                {
                    return _yearMonthDictionary;
                }

                return _yearMonthDictionary = new ConcurrentDictionary<int, IPostingLineCollection>(this
                    .GroupBy(postingLine => ToYearMonthKey(postingLine.PostingDate))
                    .ToDictionary(group => group.Key, group => (IPostingLineCollection)new PostingLineCollection { group }));
            }
        }

        private IDictionary<int, IPostingLineCollection> GetYearDictionary()
        {
            lock (_syncRoot)
            {
                if (_yearDictionary != null)
                {
                    return _yearDictionary;
                }
            }

            return _yearDictionary = new ConcurrentDictionary<int, IPostingLineCollection>(this
                .GroupBy(postingLine => postingLine.PostingDate.Year)
                .ToDictionary(group => group.Key, group => (IPostingLineCollection)new PostingLineCollection { group }));
        }

        private IDictionary<int, decimal> GetPostingValueDictionary()
        {
            lock (_syncRoot)
            {
                if (_postingValueDictionary != null)
                {
                    return _postingValueDictionary;
                }

                return _postingValueDictionary = new ConcurrentDictionary<int, decimal>(GetYearMonthDictionary()
                    .ToDictionary(item => item.Key, item => item.Value.AsParallel().Sum(postingLine => postingLine.PostingValue)));
            }
        }

        private void ClearDictionaries()
        {
            lock (_syncRoot)
            {
                _yearMonthDictionary = null;
                _yearDictionary = null;
                _postingValueDictionary = null;
            }
        }

        private static async Task CalculateAsync(IEnumerable<IPostingLine> postingLineCollection, DateTime statusDate)
        {
            NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection));

            foreach (IPostingLine postingLine in postingLineCollection)
            {
                if (postingLine.StatusDate == statusDate)
                {
                    continue;
                }

                await postingLine.CalculateAsync(statusDate);
            }
        }

        private static bool IsFirstDayInMonth(DateTime value)
        {
            return value.Day == 1;
        }

        private static bool IsLastDayInMonth(DateTime value)
        {
            return value.Day == DateTime.DaysInMonth(value.Year, value.Month);
        }

        private static bool IsFullMonth(DateTime fromDate, DateTime toDate)
        {
            return IsSameYearMonth(fromDate, toDate) && IsLastDayInMonth(toDate);
        }

        private static bool IsSameYearMonth(DateTime fromDate, DateTime toDate)
        {
            return IsSameYear(fromDate, toDate) && fromDate.Month == toDate.Month;
        }

        private static bool IsSameYear(DateTime fromDate, DateTime toDate)
        {
            return fromDate.Year == toDate.Year;
        }

        private static int ToYearMonthKey(DateTime value)
        {
            return value.Year * 100 + value.Month;
        }

        private static DateTime ToFirstDayInMonth(DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        private static DateTime ToLastDayInMonth(DateTime value)
        {
            return value.AddDays(DateTime.DaysInMonth(value.Year, value.Month) - value.Day);
        }

        private static decimal ResolvePostingValue(IDictionary<int, decimal> postingValueDictionary, int key)
        {
            NullGuard.NotNull(postingValueDictionary, nameof(postingValueDictionary));

            return postingValueDictionary.TryGetValue(key, out decimal postingValue)
                ? postingValue
                : 0M;
        }

        #endregion
    }
}