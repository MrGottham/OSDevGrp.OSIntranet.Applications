using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class UserInfo : UserInfoBase, IUserInfo
    {
        #region Constructor

        public UserInfo(ClaimsPrincipal claimsPrincipal) 
            : base(claimsPrincipal)
        {
            IUserAddress address = new UserAddress(claimsPrincipal);
            Address = address.IsEmpty() == false ? address : null;
        }

        #endregion

        #region Properties

        [JsonPropertyName("sub")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string Subject => GetStringValue(ClaimTypes.NameIdentifier);

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FullName => GetStringValue(ClaimTypes.Name);

        [JsonPropertyName("given_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string GivenName => GetStringValue(ClaimTypes.GivenName);

        [JsonPropertyName("family_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Surname => GetStringValue(ClaimTypes.Surname);

        [JsonPropertyName("middle_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string MiddleName => null;

        [JsonPropertyName("nickname")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string NickName => null;

        [JsonPropertyName("preferred_username")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PreferredUsername => null;

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Uri Profile => null;

        [JsonPropertyName("profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        // ReSharper disable UnusedMember.Global
        public string ProfileAsString => Profile?.AbsoluteUri;
        // ReSharper restore UnusedMember.Global

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Uri Picture => null;

        [JsonPropertyName("picture")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        // ReSharper disable UnusedMember.Global
        public string PictureAsString => Picture?.AbsoluteUri;
        // ReSharper restore UnusedMember.Global

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Uri Webpage => GetAbsoluteUrlValue(ClaimTypes.Webpage);

        [JsonPropertyName("website")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        // ReSharper disable UnusedMember.Global
        public string WebpageAsString => Webpage?.AbsoluteUri;
        // ReSharper restore UnusedMember.Global

        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Email => GetStringValue(ClaimTypes.Email);

        [JsonPropertyName("email_verified")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EmailVerified => null;

        [JsonPropertyName("gender")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Gender => GetStringValue(ClaimTypes.Gender);

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public DateTimeOffset? Birthdate => GetDateTimeOffsetValue(ClaimTypes.DateOfBirth);

        [JsonPropertyName("birthdate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        // ReSharper disable UnusedMember.Global
        public string BirthdateAsString => Birthdate?.ToString("yyyy-MM-dd", FormatProvider);
        // ReSharper restore UnusedMember.Global

        [JsonPropertyName("zoneinfo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string TimeZone => null;

        [JsonPropertyName("locale")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Locale => null;

        [JsonPropertyName("phone_number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PhoneNumber => GetStringValue(ClaimTypes.MobilePhone) ??
                                     GetStringValue(ClaimTypes.HomePhone) ??
                                     GetStringValue(ClaimTypes.OtherPhone);

        [JsonPropertyName("phone_number_verified")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PhoneNumberVerified => null;

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public IUserAddress Address { get; }

        [JsonPropertyName("address")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        // ReSharper disable UnusedMember.Global
        public string AddressAsString => Address?.ToJson();
        // ReSharper restore UnusedMember.Global

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public DateTimeOffset? UpdatedAt => null;

        [JsonPropertyName("updated_at")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        // ReSharper disable UnusedMember.Global
        public long? UpdatedAtAsString => UpdatedAt?.ToUniversalTime().ToUnixTimeSeconds();
        // ReSharper restore UnusedMember.Global

        #endregion

        #region Methods

        public IEnumerable<Claim> ToClaims()
        {
            return GetClaimProperties()
                .Select(ToClaim)
                .Where(claim => claim != null)
                .ToArray();
        }

        private IEnumerable<PropertyInfo> GetClaimProperties()
        {
            return GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(propertyInfo => propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>() != null &&
                                       propertyInfo.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                .ToArray();
        }

        private Claim ToClaim(PropertyInfo propertyInfo)
        {
            NullGuard.NotNull(propertyInfo, nameof(propertyInfo));

            JsonPropertyNameAttribute jsonPropertyNameAttribute = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
            JsonIgnoreAttribute jsonIgnoreAttribute = propertyInfo.GetCustomAttribute<JsonIgnoreAttribute>();
            if (jsonPropertyNameAttribute == null || string.IsNullOrWhiteSpace(jsonPropertyNameAttribute.Name) || jsonIgnoreAttribute == null || jsonIgnoreAttribute.Condition == JsonIgnoreCondition.Always)
            {
                return null;
            }

            object value = propertyInfo.GetValue(this);
            if (value == null && jsonIgnoreAttribute.Condition == JsonIgnoreCondition.WhenWritingNull)
            {
                return null;
            }

            string claimValue = value?.ToString();

            return new Claim(jsonPropertyNameAttribute.Name, string.IsNullOrWhiteSpace(claimValue) == false ? claimValue : string.Empty);
        }

        #endregion
    }
}