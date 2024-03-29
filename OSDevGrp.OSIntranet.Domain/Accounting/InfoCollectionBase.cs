﻿using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public abstract class InfoCollectionBase<TInfo, TInfoCollection> : HashSet<TInfo>, IInfoCollection<TInfo, TInfoCollection> where TInfo : IInfo<TInfo> where TInfoCollection : IInfoCollection<TInfo>
    {
        #region Private variables

        private bool _isCalculating;
        private IDictionary<int, TInfo> _infoDictionary;
        private readonly object _syncRoot = new object();

        #endregion

        #region Properties

        public DateTime StatusDate { get; private set; }

        public bool IsProtected { get; private set; }

        public bool Deletable => IsProtected == false && this.All(info => info.Deletable);

        #endregion

        #region Methods

        public new void Add(TInfo info)
        {
            NullGuard.NotNull(info, nameof(info));

            if (base.Add(info))
            {
                ClearDictionaries();

                return;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ObjectAlreadyExists, info.GetType().Name).Build();
        }

        public void Add(IEnumerable<TInfo> infoCollection)
        {
            NullGuard.NotNull(infoCollection, nameof(infoCollection));

            foreach (TInfo info in infoCollection)
            {
                Add(info);
            }
        }

        public TInfo First()
        {
            return GetInfoDictionary()
                .AsParallel()
                .OrderBy(item => item.Key)
                .Select(item => item.Value)
                .FirstOrDefault();
        }

        public TInfo Prev(TInfo info)
        {
            NullGuard.NotNull(info, nameof(info));

            return Find(info.FromDate.AddDays(-1));
        }

        public TInfo Next(TInfo info)
        {
            NullGuard.NotNull(info, nameof(info));

            return Find(info.ToDate.AddDays(1));
        }

        public TInfo Last()
        {
            return GetInfoDictionary()
                .AsParallel()
                .OrderByDescending(item => item.Key)
                .Select(item => item.Value)
                .FirstOrDefault();
        }

        public TInfo Find(DateTime matchingDate)
        {
            return GetInfoDictionary().TryGetValue(ToYearMonthKey(matchingDate), out TInfo info)
                ? info
                : default;
        }

        public async Task<TInfoCollection> CalculateAsync(DateTime statusDate)
        {
            if (statusDate.Date == StatusDate)
            {
                while (_isCalculating)
                {
                    await Task.Delay(250);
                }

                return AlreadyCalculated();
            }

            StatusDate = statusDate.Date;

            _isCalculating = true;
            try
            {
                List<TInfo> calculatedInfoCollection = new List<TInfo>();
                foreach (TInfo info in this)
                {
                    calculatedInfoCollection.Add(await info.CalculateAsync(StatusDate));
                }

                return Calculate(StatusDate, calculatedInfoCollection.AsReadOnly());
            }
            finally
            {
                _isCalculating = false;
            }
        }

        public void ApplyProtection()
        {
            foreach (TInfo info in this)
            {
                info.ApplyProtection();
            }

            IsProtected = true;
        }

        public void AllowDeletion() => throw new NotSupportedException();

        public void DisallowDeletion() => throw new NotSupportedException();

        protected abstract TInfoCollection Calculate(DateTime statusDate, IReadOnlyCollection<TInfo> calculatedInfoCollection);

        protected abstract TInfoCollection AlreadyCalculated();

        private IDictionary<int, TInfo> GetInfoDictionary()
        {
            lock (_syncRoot)
            {
                if (_infoDictionary != null)
                {
                    return _infoDictionary;
                }

                return _infoDictionary = new ConcurrentDictionary<int, TInfo>(this.AsParallel()
                    .GroupBy(ToYearMonthKey)
                    .ToDictionary(item => item.Key, item => item.Single()));
            }
        }

        private void ClearDictionaries()
        {
            lock (_syncRoot)
            {
                _infoDictionary = null;
            }
        }

        private static int ToYearMonthKey(TInfo info)
        {
            NullGuard.NotNull(info, nameof(info));

            return info.Year * 100 + info.Month;
        }

        private static int ToYearMonthKey(DateTime value)
        {
            return value.Year * 100 + value.Month;
        }

        #endregion
    }
}