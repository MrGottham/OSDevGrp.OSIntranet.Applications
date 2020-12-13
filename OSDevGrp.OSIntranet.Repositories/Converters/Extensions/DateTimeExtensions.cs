using System;

namespace OSDevGrp.OSIntranet.Repositories.Converters.Extensions
{
    internal static class DateTimeExtensions
    {
        internal static DateTime GetStatusDateForInfos(this DateTime statusDate) => statusDate.AddMonths(11).Date;
    }
}