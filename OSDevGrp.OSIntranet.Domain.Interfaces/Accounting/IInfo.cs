using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IInfo : IAuditable, ICalculable, IDeletable
    {
        short Year { get; }

        short Month { get; }

        DateTime FromDate { get; }

        DateTime ToDate { get; }

        bool IsMonthOfStatusDate { get; }

        bool IsLastMonthOfStatusDate { get; }

        bool IsYearOfStatusDate { get; }

        bool IsLastYearOfStatusDate { get; }
    }

    public interface IInfo<T> : IInfo, ICalculable<T> where T : IInfo
    {
    }
}