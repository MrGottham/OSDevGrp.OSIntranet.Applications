using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public abstract class ValidationValueRuleDtoBase : ValidationRuleDtoBase
{
    [Required]
    [MinLength(ValidationValues.ValidationValueMinLength)]
    public required string Value { get; init; }

    protected static string Map<TValue>(TValue value) where TValue : struct, IComparable<TValue>
    {
        if (typeof(TValue) == typeof(DateTime))
        {
            DateTime dateTime = (DateTime)(object)value;
            TimeSpan offset = dateTime.Kind == DateTimeKind.Utc 
                ? TimeSpan.Zero 
                : TimeZoneInfo.Local.GetUtcOffset(dateTime);
            return Map(new DateTimeOffset(dateTime, offset));
        }

        if (typeof(TValue) == typeof(DateTimeOffset))
        {
            DateTimeOffset dateTimeOffset = (DateTimeOffset)(object)value;
            return dateTimeOffset.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);
        }

        return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
    }
}