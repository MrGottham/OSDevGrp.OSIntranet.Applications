using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IInfoCollection<TInfo> : IEnumerable<TInfo>, ICalculable where TInfo : IInfo<TInfo>
    {
        void Add(TInfo info);

        void Add(IEnumerable<TInfo> infoCollection);

        TInfo First();

        TInfo Prev(TInfo info);

        TInfo Next(TInfo info);

        TInfo Last();

        TInfo Find(DateTime matchingDate);
    }

    public interface IInfoCollection<TInfo, TInfoCollection> : IInfoCollection<TInfo>, ICalculable<TInfoCollection> where TInfo : IInfo<TInfo> where TInfoCollection : IInfoCollection<TInfo>
    {
    }
}