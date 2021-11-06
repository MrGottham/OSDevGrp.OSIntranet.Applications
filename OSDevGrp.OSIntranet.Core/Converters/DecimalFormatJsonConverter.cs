using System;
using System.Globalization;
using System.Text.Json;

namespace OSDevGrp.OSIntranet.Core.Converters
{
    public sealed class DecimalFormatJsonConverter : CustomJsonConverterBase<decimal>
    {
        #region Methods

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            NullGuard.NotNull(writer, nameof(writer))
                .NotNull(options, nameof(options));

            string valueAsString = value.ToString("N2", FormatProvider);

            writer.WriteNumberValue(decimal.Parse(valueAsString, NumberStyles.Any, FormatProvider));
        }

        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            NullGuard.NotNull(typeToConvert, nameof(typeToConvert))
                .NotNull(options, nameof(options));

            return reader.GetDecimal();
        }

        #endregion
    }
}