using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Converters.Extensions
{
    internal static class InfoCollectionExtensions
    {
        internal static void EnsurePopulation<TInfo, TInfoCollection>(this TInfoCollection infoCollection, Func<TInfo, TInfo> nextInfoBuilder) where TInfo : IInfo<TInfo> where TInfoCollection : IInfoCollection<TInfo, TInfoCollection>
        {
            NullGuard.NotNull(infoCollection, nameof(infoCollection))
                .NotNull(nextInfoBuilder, nameof(nextInfoBuilder));

            TInfo info = infoCollection.First();
            if (info == null)
            {
                return;
            }

            TInfo lastInfo = infoCollection.Last();
            while (info.Equals(lastInfo) == false)
            {
                TInfo nextInfo = infoCollection.Next(info);
                if (nextInfo == null)
                {
                    TInfo infoToAdd = nextInfoBuilder(info);
                    if (string.IsNullOrWhiteSpace(info.CreatedByIdentifier) == false && string.IsNullOrWhiteSpace(info.ModifiedByIdentifier) == false)
                    {
                        infoToAdd.AddAuditInformation(info.CreatedDateTime.ToUniversalTime(), info.CreatedByIdentifier, info.ModifiedDateTime.ToUniversalTime(), info.ModifiedByIdentifier);
                    }
                    infoCollection.Add(infoToAdd);
                }

                info = infoCollection.Next(info);
            }
        }
    }
}