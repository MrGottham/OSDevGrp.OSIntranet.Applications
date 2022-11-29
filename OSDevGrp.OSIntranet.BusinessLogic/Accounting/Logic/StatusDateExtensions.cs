using System;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal static class StatusDateExtensions
    {
        internal static DateTime GetEndDateOfLastMonth(this DateTime statusDate)
        {
            return statusDate.AddDays(statusDate.Day * -1).Date;
        }

        internal static DateTime GetFirstDateOfMonth(this DateTime statusDate)
        {
            return new DateTime(statusDate.Year, statusDate.Month, 1);
        }

        internal static DateTime GetFirstDateOfYear(this DateTime statusDateTime)
        {
            return new DateTime(statusDateTime.Year, 1, 1);
        }

        internal static DateTime GetEndDateOfLastYear(this DateTime statusDateTime)
        {
            return new DateTime(statusDateTime.Year - 1, 12, 31);
        }

        internal static string ToDateText(this DateTime statusDateTime, IFormatProvider formatProvider)
        {
            NullGuard.NotNull(formatProvider, nameof(formatProvider));

            return statusDateTime.ToString("d. MMMM yyyy", formatProvider).ToLower();
        }

        internal static string ToMonthText(this DateTime statusDate, IFormatProvider formatProvider)
        {
            NullGuard.NotNull(formatProvider, nameof(formatProvider));

            return statusDate.ToString("MMMM", formatProvider).ToLower();
        }

        internal static string ToYearText(this DateTime statusDate, IFormatProvider formatProvider)
        {
            NullGuard.NotNull(formatProvider, nameof(formatProvider));

            return statusDate.ToString("yyyy", formatProvider).ToLower();
        }

        internal static string ToMonthYearText(this DateTime statusDate, IFormatProvider formatProvider)
        {
            NullGuard.NotNull(formatProvider, nameof(formatProvider));

            return $"{statusDate.ToMonthText(formatProvider)} {statusDate.ToYearText(formatProvider)}";
        }
    }
}