using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IInfo : IAuditable, ICalculable, IProtectable
    {
        short Year { get; }

        short Month { get; }

        DateTime FromDate { get; }

        DateTime ToDate { get; }

        bool IsMonthOfStatusDate { get; }

        bool IsLastMonthOfStatusDate { get; }

        bool IsYearToDateOfStatusDate { get; }

        bool IsLastYearOfStatusDate { get; }
    }

    public interface IInfo<T> : IInfo, ICalculable<T> where T : IInfo
    {
    }
}