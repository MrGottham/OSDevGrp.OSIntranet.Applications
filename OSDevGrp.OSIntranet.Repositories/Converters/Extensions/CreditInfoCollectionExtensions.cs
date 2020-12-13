using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Converters.Extensions
{
    internal static class CreditInfoCollectionExtensions
    {
        internal static void Populate(this ICreditInfoCollection creditInfoCollection, IAccount account, ICreditInfo[] creditInfos, DateTime statusDate, DateTime statusDateForCreditInfos)
        {
            NullGuard.NotNull(creditInfoCollection, nameof(creditInfoCollection))
                .NotNull(account, nameof(account))
                .NotNull(creditInfos, nameof(creditInfos));

            DateTime fromDate = new DateTime(statusDate.AddYears(-1).Year, 1, 1);

            if (creditInfos.Length == 0)
            {
                creditInfoCollection.Add(new CreditInfo(account, (short) fromDate.Year, (short) fromDate.Month, 0M));
                creditInfoCollection.Add(new CreditInfo(account, (short) statusDateForCreditInfos.Year, (short) statusDateForCreditInfos.Month, 0M));

                creditInfoCollection.EnsurePopulation(account);

                return;
            }

            ICreditInfo creditInfoForFromDate = creditInfos.FindInfoForFromDate(fromDate,
                (year, month, creditInfoBeforeFromDate) => new CreditInfo(account, year, month, creditInfoBeforeFromDate.Credit),
                (year, month) => new CreditInfo(account, year, month, 0M));
            creditInfoCollection.Add(creditInfoForFromDate);
            
            creditInfoCollection.Add(creditInfos.Between(creditInfoForFromDate.ToDate.AddDays(1), statusDateForCreditInfos));

            ICreditInfo lastCreditInfo = creditInfoCollection.Last();
            if (lastCreditInfo == null || lastCreditInfo.Year > (short) statusDateForCreditInfos.Year || lastCreditInfo.Year == (short) statusDateForCreditInfos.Year && lastCreditInfo.Month >= (short) statusDateForCreditInfos.Month)
            {
                creditInfoCollection.EnsurePopulation(account);

                return;
            }

            creditInfoCollection.Add(new CreditInfo(account, (short) statusDateForCreditInfos.Year, (short) statusDateForCreditInfos.Month, lastCreditInfo.Credit));

            creditInfoCollection.EnsurePopulation(account);
        }

        internal static void EnsurePopulation(this ICreditInfoCollection creditInfoCollection, IAccount account)
        {
            NullGuard.NotNull(creditInfoCollection, nameof(creditInfoCollection))
                .NotNull(account, nameof(account));

            creditInfoCollection.EnsurePopulation<ICreditInfo, ICreditInfoCollection>(creditInfo =>
            {
                DateTime nextCreditInfoFromDate = creditInfo.ToDate.AddDays(1);
                return new CreditInfo(account, (short) nextCreditInfoFromDate.Year, (short) nextCreditInfoFromDate.Month, creditInfo.Credit);
            });
        }
    }
}