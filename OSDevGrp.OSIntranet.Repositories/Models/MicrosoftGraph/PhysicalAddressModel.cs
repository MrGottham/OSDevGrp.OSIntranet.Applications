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
        [DataMember(Name = "street", IsRequired = false, EmitDefaultValue = false)]
        public string Street { get; set; }

        [DataMember(Name = "postalCode", IsRequired = false, EmitDefaultValue = false)]
        public string PostalCode { get; set; }

        [DataMember(Name = "city", IsRequired = false, EmitDefaultValue = false)]
        public string City { get; set; }

        [DataMember(Name = "state", IsRequired = false, EmitDefaultValue = false)]
        public string State { get; set; }

        [DataMember(Name = "countryOrRegion", IsRequired = false, EmitDefaultValue = false)]
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

        internal static PhysicalAddressModel ToChangedOnlyModel(this PhysicalAddressModel targetPhysicalAddressModel, PhysicalAddressModel sourcePhysicalAddressModel)
        {
            return targetPhysicalAddressModel.CalculateChange(
                sourcePhysicalAddressModel,
                physicalAddressModel => physicalAddressModel == null,
                physicalAddressModel => physicalAddressModel == null || physicalAddressModel.IsEmpty(),
                new PhysicalAddressModel
                {
                    Street = string.Empty,
                    PostalCode = string.Empty,
                    City = string.Empty,
                    State = string.Empty,
                    CountryOrRegion = string.Empty
                },
                (t, s) =>
                {
                    t.Street = t.Street.CalculateChange(s.Street);
                    t.PostalCode = t.PostalCode.CalculateChange(s.PostalCode);
                    t.City = t.City.CalculateChange(s.City);
                    t.State = t.State.CalculateChange(s.State);
                    t.CountryOrRegion = t.CountryOrRegion.CalculateChange(s.CountryOrRegion);

                    if (t.IsEmpty())
                    {
                        return null;
                    }

                    t.Street ??= s.Street;
                    t.PostalCode ??= s.PostalCode;
                    t.City ??= s.City;
                    t.State ??= s.State;
                    t.CountryOrRegion ??= s.CountryOrRegion;

                    return t;
                });
        }

        internal static bool IsEmpty(this PhysicalAddressModel physicalAddressModel)
        {
            NullGuard.NotNull(physicalAddressModel, nameof(physicalAddressModel));

            return physicalAddressModel.Street == null && physicalAddressModel.PostalCode == null && physicalAddressModel.City == null && physicalAddressModel.State == null && physicalAddressModel.CountryOrRegion == null;
        }
    }
}
