using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Repositories.Models.MicrosoftGraph
{
    [DataContract]
    internal class PhysicalAddressModel
    {
        [DataMember(Name = "street", IsRequired = false)]
        public string Street { get; set; }

        [DataMember(Name = "postalCode", IsRequired = false)]
        public string PostalCode { get; set; }

        [DataMember(Name = "city", IsRequired = false)]
        public string City { get; set; }

        [DataMember(Name = "state", IsRequired = false)]
        public string State { get; set; }

        [DataMember(Name = "countryOrRegion", IsRequired = false)]
        public string CountryOrRegion { get; set; }
    }

    internal static class PhysicalAddressModelExtensions
    {
        internal static IAddress ToDomain(this PhysicalAddressModel physicalAddressModel)
        {
            NullGuard.NotNull(physicalAddressModel, nameof(physicalAddressModel));

            string[] streetLines = physicalAddressModel.Street == null ? new string[0] : physicalAddressModel.Street.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

            return new Address
            {
                StreetLine1 = streetLines.Length > 0 ? streetLines[0] : null,
                StreetLine2 = streetLines.Length > 1 ? streetLines[1] : null,
                PostalCode = physicalAddressModel.PostalCode,
                City = physicalAddressModel.City,
                State = physicalAddressModel.State,
                Country = physicalAddressModel.CountryOrRegion
            };
        }
    }
}
