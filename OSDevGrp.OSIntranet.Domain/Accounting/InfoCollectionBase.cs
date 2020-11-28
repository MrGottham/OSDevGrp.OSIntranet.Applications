using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public abstract class InfoCollectionBase<TInfo, TInfoCollection> : HashSet<TInfo>, IInfoCollection<TInfo, TInfoCollection> where TInfo : IInfo<TInfo> where TInfoCollection : IInfoCollection<TInfo>
    {
        #region Properties

        public DateTime StatusDate { get; private set; }

        #endregion

        #region Methods

        public new void Add(TInfo info)
        {
            NullGuard.NotNull(info, nameof(info));

            if (base.Add(info))
            {
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
            return this.OrderBy(info => info.Year).ThenBy(info => info.Month).FirstOrDefault();
        }

        public TInfo Prev(TInfo info)
        {
            NullGuard.NotNull(info, nameof(info));

            return this.SingleOrDefault(m => m.Year == info.FromDate.AddDays(-1).Year && m.Month == info.FromDate.AddDays(-1).Month);
        }

        public TInfo Next(TInfo info)
        {
            NullGuard.NotNull(info, nameof(info));

            return this.SingleOrDefault(m => m.Year == info.ToDate.AddDays(1).Year && m.Month == info.ToDate.AddDays(1).Month);
        }

        public TInfo Last()
        {
            return this.OrderBy(info => info.Year).ThenBy(info => info.Month).LastOrDefault();
        }

        public async Task<TInfoCollection> CalculateAsync(DateTime statusDate)
        {
            StatusDate = statusDate.Date;

            TInfo[] calculatedInfoCollection = await Task.WhenAll(this.Select(info => info.CalculateAsync(StatusDate)).ToArray());

            return Calculate(StatusDate, calculatedInfoCollection);
        }

        protected abstract TInfoCollection Calculate(DateTime statusDate, TInfo[] calculatedInfoCollection);

        #endregion
    }
}