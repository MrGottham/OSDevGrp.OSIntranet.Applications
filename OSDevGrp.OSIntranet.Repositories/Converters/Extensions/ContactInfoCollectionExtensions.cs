using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Converters.Extensions
{
    internal static class ContactInfoCollectionExtensions
    {
        internal static void Populate(this IContactInfoCollection contactInfoCollection, IContactAccount contactAccount, DateTime statusDate, DateTime statusDateForContactInfos)
        {
            NullGuard.NotNull(contactInfoCollection, nameof(contactInfoCollection))
                .NotNull(contactAccount, nameof(contactAccount));

            DateTime fromDate = new DateTime(statusDate.AddYears(-1).Year, 1, 1);

            contactInfoCollection.Add(BuildContactInfo(contactAccount, (short) fromDate.Year, (short) fromDate.Month));
            contactInfoCollection.Add(BuildContactInfo(contactAccount, (short) statusDateForContactInfos.Year, (short) statusDateForContactInfos.Month));

            contactInfoCollection.EnsurePopulation(contactAccount);
        }

        private static void EnsurePopulation(this IContactInfoCollection contactInfoCollection, IContactAccount contactAccount)
        {
            NullGuard.NotNull(contactInfoCollection, nameof(contactInfoCollection))
                .NotNull(contactAccount, nameof(contactAccount));

            contactInfoCollection.EnsurePopulation<IContactInfo, IContactInfoCollection>(contactInfo =>
            {
                DateTime nextContactInfoFromDate = contactInfo.ToDate.AddDays(1);
                return new ContactInfo(contactAccount, (short) nextContactInfoFromDate.Year, (short) nextContactInfoFromDate.Month);
            });
        }

        private static IContactInfo BuildContactInfo(IContactAccount contactAccount, short year, short month)
        {
            NullGuard.NotNull(contactAccount, nameof(contactAccount));

            IContactInfo contactInfo = new ContactInfo(contactAccount, year, month);
            contactInfo.AddAuditInformation(contactAccount.CreatedDateTime.ToUniversalTime(), contactAccount.CreatedByIdentifier, contactAccount.ModifiedDateTime.ToUniversalTime(), contactAccount.ModifiedByIdentifier);
            return contactInfo;
        }
    }
}