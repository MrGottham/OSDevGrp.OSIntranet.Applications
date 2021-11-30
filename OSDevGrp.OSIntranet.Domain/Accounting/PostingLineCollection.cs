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
        private IDictionary<int, decimal> _postingValueForYearMonthDictionary;
        private readonly IDictionary<string, decimal> _postingValueForDateIntervalDictionary = new ConcurrentDictionary<string, decimal>();
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
                Add(postingLine);
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

            decimal postingValue;
            string dateIntervalKey = ToDateIntervalKey(fromDate, toDate);
            if (sortOrder.HasValue == false)
            {
                if (_postingValueForDateIntervalDictionary.TryGetValue(dateIntervalKey, out postingValue))
                {
                    return postingValue;
                }
            }

            IDictionary<int, decimal> postingValueForYearMonthDictionary = GetPostingValueForYearMonthDictionary();

            if (IsSameYearMonth(fromDate, toDate))
            {
                if (IsFullMonth(fromDate, toDate) == false || sortOrder.HasValue)
                {
                    return HandleCalculatedPostingValue(dateIntervalKey, Between(fromDate, toDate, sortOrder).AsParallel().Sum(postingLine => postingLine.PostingValue), sortOrder);
                }

                return HandleCalculatedPostingValue(dateIntervalKey, ResolvePostingValue(postingValueForYearMonthDictionary, ToYearMonthKey(toDate)), null);
            }

            postingValue = IsFirstDayInMonth(fromDate)
                ? ResolvePostingValue(postingValueForYearMonthDictionary, ToYearMonthKey(fromDate))
                : Between(fromDate, ToLastDayInMonth(fromDate), null)
                    .AsParallel()
                    .Sum(postingLine => postingLine.PostingValue);

            int fromYearMonthKey = ToYearMonthKey(ToFirstDayInMonth(fromDate).AddMonths(1));
            int toYearMonthKey = ToYearMonthKey(ToFirstDayInMonth(toDate).AddMonths(-1));
            postingValue += postingValueForYearMonthDictionary
                .AsParallel()
                .Where(item => item.Key >= fromYearMonthKey && item.Key <= toYearMonthKey)
                .Sum(item => item.Value);

            postingValue += IsLastDayInMonth(toDate) && sortOrder.HasValue == false
                ? ResolvePostingValue(postingValueForYearMonthDictionary, ToYearMonthKey(toDate))
                : Between(ToFirstDayInMonth(toDate), toDate, sortOrder)
                    .AsParallel()
                    .Sum(postingLine => postingLine.PostingValue);

            return HandleCalculatedPostingValue(dateIntervalKey, postingValue, sortOrder);
        }

        public async Task<IPostingLineCollection> CalculateAsync(DateTime statusDate)
        {
            if (statusDate.Date == StatusDate)
            {
                return this;
            }

            StatusDate = statusDate.Date;

            await Task.WhenAll(this.GroupBy(postingLine => postingLine.PostingDate.Year)
                .Select(group => CalculateAsync(group.ToArray(), StatusDate))
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

                return _yearMonthDictionary = new ConcurrentDictionary<int, IPostingLineCollection>(this.AsParallel()
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

            return _yearDictionary = new ConcurrentDictionary<int, IPostingLineCollection>(this.AsParallel()
                .GroupBy(postingLine => postingLine.PostingDate.Year)
                .ToDictionary(group => group.Key, group => (IPostingLineCollection)new PostingLineCollection { group }));
        }

        private IDictionary<int, decimal> GetPostingValueForYearMonthDictionary()
        {
            lock (_syncRoot)
            {
                if (_postingValueForYearMonthDictionary != null)
                {
                    return _postingValueForYearMonthDictionary;
                }

                return _postingValueForYearMonthDictionary = new ConcurrentDictionary<int, decimal>(GetYearMonthDictionary().AsParallel()
                    .ToDictionary(item => item.Key, item => item.Value.AsParallel().Sum(postingLine => postingLine.PostingValue)));
            }
        }

        private decimal HandleCalculatedPostingValue(string dateIntervalKey, decimal postingValue, int? sortOrder)
        {
            NullGuard.NotNullOrWhiteSpace(dateIntervalKey, nameof(dateIntervalKey));

            if (sortOrder.HasValue)
            {
                return postingValue;
            }

            lock (_syncRoot)
            {
                if (_postingValueForDateIntervalDictionary.ContainsKey(dateIntervalKey) == false)
                {
                    _postingValueForDateIntervalDictionary.Add(dateIntervalKey, postingValue);
                }
            }

            return postingValue;
        }

        private void ClearDictionaries()
        {
            lock (_syncRoot)
            {
                _yearMonthDictionary = null;
                _yearDictionary = null;
                _postingValueForYearMonthDictionary = null;
                _postingValueForDateIntervalDictionary.Clear();
            }
        }

        private static async Task CalculateAsync(IReadOnlyCollection<IPostingLine> postingLineCollection, DateTime statusDate)
        {
            NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection));

            foreach (IPostingLine postingLine in postingLineCollection)
            {
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

        private static string ToDateIntervalKey(DateTime fromDate, DateTime toDate)
        {
            return $"{fromDate:yyyyMMdd}-{toDate:yyyyMMdd}";
        }

        private static DateTime ToFirstDayInMonth(DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        private static DateTime ToLastDayInMonth(DateTime value)
        {
            return value.AddDays(DateTime.DaysInMonth(value.Year, value.Month) - value.Day);
        }

        private static decimal ResolvePostingValue(IDictionary<int, decimal> postingValueForYearMonthDictionary, int key)
        {
            NullGuard.NotNull(postingValueForYearMonthDictionary, nameof(postingValueForYearMonthDictionary));

            return postingValueForYearMonthDictionary.TryGetValue(key, out decimal postingValue)
                ? postingValue
                : 0M;
        }

        #endregion
    }
}