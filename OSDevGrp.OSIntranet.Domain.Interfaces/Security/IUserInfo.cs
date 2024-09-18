using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IUserInfo
    {
        string Subject { get; }

        string FullName { get; }

        string GivenName { get; }

        string Surname { get; }

        string MiddleName { get; }

        string NickName { get; }

        string PreferredUsername { get; }

        Uri Profile { get; }

        Uri Picture { get; }

        Uri Webpage { get; }

        string Email { get; }

        bool? EmailVerified { get; }

        string Gender { get; }

        DateTimeOffset? Birthdate { get; }

        string TimeZone { get; }

        string Locale { get; }

        string PhoneNumber { get; }

        bool? PhoneNumberVerified { get; }

        IUserAddress Address { get; }

        DateTimeOffset? UpdatedAt { get; }

        string ToJson();

        IEnumerable<Claim> ToClaims();
    }
}