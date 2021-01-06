using System.Globalization;
using System.Text.Json;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Converters
{
    internal sealed class DecimalFormatJsonConverter : CustomJsonConverterBase<decimal>
    {
        #region Methods

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            NullGuard.NotNull(writer, nameof(writer))
                .NotNull(options, nameof(options));

            string valueAsString = value.ToString("N2", FormatProvider);

            writer.WriteNumberValue(decimal.Parse(valueAsString, NumberStyles.Any, FormatProvider));
        }

        #endregion
    }
}