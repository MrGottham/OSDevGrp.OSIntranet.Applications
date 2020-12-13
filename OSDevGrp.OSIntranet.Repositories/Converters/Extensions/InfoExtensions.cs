using System;
using System.Linq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Converters.Extensions
{
    internal static class InfoExtensions
    {
        internal static TInfo FindInfoForFromDate<TInfo>(this TInfo[] infoCollection, DateTime fromDate, Func<short, short, TInfo, TInfo> infoBeforeFromDateBuilder, Func<short, short, TInfo> defaultInfoBuilder) where TInfo : IInfo<TInfo>
        {
            NullGuard.NotNull(infoCollection, nameof(infoCollection))
                .NotNull(infoBeforeFromDateBuilder, nameof(infoBeforeFromDateBuilder))
                .NotNull(defaultInfoBuilder, nameof(defaultInfoBuilder));

            TInfo infoForFromDate = infoCollection.AsParallel().SingleOrDefault(info => info.Year == (short) fromDate.Year && info.Month == (short) fromDate.Month);
            if (infoForFromDate != null)
            {
                return infoForFromDate;
            }

            TInfo infoBeforeFromDate = infoCollection.AsParallel()
                .Where(info => info.Year < (short) fromDate.Year || info.Year == (short) fromDate.Year && info.Month < (short) fromDate.Month)
                .OrderByDescending(info => info.Year)
                .ThenByDescending(info => info.Month)
                .FirstOrDefault();
            if (infoBeforeFromDate != null)
            {
                return infoBeforeFromDateBuilder((short) fromDate.Year, (short) fromDate.Month, infoBeforeFromDate);
            }

            return defaultInfoBuilder((short) fromDate.Year, (short) fromDate.Month);
        }

        internal static TInfo[] Between<TInfo>(this TInfo[] infoCollection, DateTime fromDate, DateTime toDate) where TInfo : IInfo<TInfo>
        {
            NullGuard.NotNull(infoCollection, nameof(infoCollection));

            return infoCollection.AsParallel()
                .Where(info =>
                    (info.Year > (short) fromDate.Year || info.Year == (short) fromDate.Year && info.Month >= (short) fromDate.Month) &&
                    (info.Year < (short) toDate.Year || info.Year == (short) toDate.Year && info.Month <= (short) toDate.Month))
                .ToArray();
        }
    }
}