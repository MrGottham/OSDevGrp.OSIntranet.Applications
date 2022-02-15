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
                creditInfoCollection.Add(BuildCreditInfo(account, (short) fromDate.Year, (short) fromDate.Month));
                creditInfoCollection.Add(BuildCreditInfo(account, (short) statusDateForCreditInfos.Year, (short) statusDateForCreditInfos.Month));

                creditInfoCollection.EnsurePopulation(account);

                return;
            }

            ICreditInfo creditInfoForFromDate = creditInfos.FindInfoForFromDate(fromDate,
                (year, month, creditInfoBeforeFromDate) => BuildCreditInfo(account, year, month, creditInfoBeforeFromDate),
                (year, month) => BuildCreditInfo(account, year, month));
            creditInfoCollection.Add(creditInfoForFromDate);

            creditInfoCollection.Add(creditInfos.Between(creditInfoForFromDate.ToDate.AddDays(1), statusDateForCreditInfos));

            ICreditInfo lastCreditInfo = creditInfoCollection.Last();
            if (lastCreditInfo == null || lastCreditInfo.Year > (short) statusDateForCreditInfos.Year || lastCreditInfo.Year == (short) statusDateForCreditInfos.Year && lastCreditInfo.Month >= (short) statusDateForCreditInfos.Month)
            {
                creditInfoCollection.EnsurePopulation(account);

                return;
            }

            creditInfoCollection.Add(BuildCreditInfo(account, (short) statusDateForCreditInfos.Year, (short) statusDateForCreditInfos.Month, lastCreditInfo));

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

        private static ICreditInfo BuildCreditInfo(IAccount account, short year, short month)
        {
            NullGuard.NotNull(account, nameof(account));

            ICreditInfo creditInfo = new CreditInfo(account, year, month, 0M);
            creditInfo.AddAuditInformation(account.CreatedDateTime.ToUniversalTime(), account.CreatedByIdentifier, account.ModifiedDateTime.ToUniversalTime(), account.ModifiedByIdentifier);
            return creditInfo;
        }

        private static ICreditInfo BuildCreditInfo(IAccount account, short year, short month, ICreditInfo copyFromCreditInfo)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(copyFromCreditInfo, nameof(copyFromCreditInfo));

            ICreditInfo creditInfo = new CreditInfo(account, year, month, copyFromCreditInfo.Credit);
            creditInfo.AddAuditInformation(copyFromCreditInfo.CreatedDateTime.ToUniversalTime(), copyFromCreditInfo.CreatedByIdentifier, copyFromCreditInfo.ModifiedDateTime.ToUniversalTime(), copyFromCreditInfo.ModifiedByIdentifier);
            return creditInfo;
        }
    }
}