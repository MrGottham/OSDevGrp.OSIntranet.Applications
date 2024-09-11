using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class UserAddress(ClaimsPrincipal claimsPrincipal) : UserInfoBase(claimsPrincipal), IUserAddress
    {
        #region Properties

        [JsonPropertyName("formatted")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string MailingAddress => IsEmpty() == false ? BuildMailingAddress() : null;

        [JsonPropertyName("street_address")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string StreetAddress => GetStringValue(ClaimTypes.StreetAddress);

        [JsonPropertyName("locality")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string City => GetStringValue(ClaimTypes.Locality);

        [JsonPropertyName("region")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Region => GetStringValue(ClaimTypes.StateOrProvince);

        [JsonPropertyName("postal_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PostalCode => GetStringValue(ClaimTypes.PostalCode);

        [JsonPropertyName("country")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Country => GetStringValue(ClaimTypes.Country);

        #endregion

        #region Methods

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(StreetAddress) &&
                   string.IsNullOrWhiteSpace(City) &&
                   string.IsNullOrWhiteSpace(Region) &&
                   string.IsNullOrWhiteSpace(PostalCode) &&
                   string.IsNullOrWhiteSpace(Country);
        }

        public override string ToJson()
        {
            return IsEmpty() ? null : base.ToJson();
        }

        private string BuildMailingAddress()
        {
            StringBuilder mailingAddressBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(StreetAddress) == false)
            {
                mailingAddressBuilder.AppendLine(StreetAddress);
            }
            if (string.IsNullOrWhiteSpace(City) == false || string.IsNullOrWhiteSpace(Region) == false || string.IsNullOrWhiteSpace(PostalCode) == false)
            {
                mailingAddressBuilder.AppendLine(Combine(City, Region, PostalCode));
            }
            if (string.IsNullOrWhiteSpace(Country) == false)
            {
                mailingAddressBuilder.AppendLine(Country);
            }
            return mailingAddressBuilder.ToString();
        }

        private static string Combine(params string[] values)
        {
            NullGuard.NotNull(values, nameof(values));

            return string.Join(", ", values.Where(value => string.IsNullOrWhiteSpace(value) == false));
        }

        #endregion
    }
}