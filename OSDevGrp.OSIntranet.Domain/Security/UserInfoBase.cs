using OSDevGrp.OSIntranet.Core;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal abstract class UserInfoBase
    {
        #region Private variables

        private readonly ClaimsPrincipal _claimsPrincipal;

        #endregion

        #region Constructor

        protected UserInfoBase(ClaimsPrincipal claimsPrincipal)
        {
            NullGuard.NotNull(claimsPrincipal, nameof(claimsPrincipal));

            _claimsPrincipal = claimsPrincipal;
        }

        #endregion

        #region Properties

        protected IFormatProvider FormatProvider => CultureInfo.InvariantCulture;

        #endregion

        #region Methods

        public override string ToString()
        {
            return ToJson();
        }

        public override int GetHashCode()
        {
            return ToString()?.GetHashCode() ?? 0;
        }

        public virtual string ToJson()
        {
            return Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(this, GetType(), GetJsonSerializerOptions()));
        }

        protected string GetStringValue(string claimType)
        {
            NullGuard.NotNullOrWhiteSpace(claimType, nameof(claimType));

            Claim claim = _claimsPrincipal.GetClaim(claimType);
            if (claim == null || string.IsNullOrWhiteSpace(claim.Value))
            {
                return null;
            }

            return claim.Value;
        }

        protected Uri GetAbsoluteUrlValue(string claimType)
        {
            NullGuard.NotNullOrWhiteSpace(claimType, nameof(claimType));

            string claimValue = GetStringValue(claimType);
            if (string.IsNullOrWhiteSpace(claimValue))
            {
                return null;
            }

            if (Uri.TryCreate(claimValue, UriKind.Absolute, out Uri absoluteUrl) == false)
            {
                return null;
            }

            return absoluteUrl;
        }

        protected DateTimeOffset? GetDateTimeOffsetValue(string claimType)
        {
            NullGuard.NotNullOrWhiteSpace(claimType, nameof(claimType));

            string claimValue = GetStringValue(claimType);
            if (string.IsNullOrWhiteSpace(claimValue))
            {
                return null;
            }

            if (DateTimeOffset.TryParse(claimValue, FormatProvider, out DateTimeOffset dateTimeOffset) == false)
            {
                return null;
            }

            return dateTimeOffset;
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = false
            };
        }

        #endregion
    }
}