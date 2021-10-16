using System;
using System.Text.Json;

namespace OSDevGrp.OSIntranet.Core.Converters
{
    public sealed class NullableDecimalFormatJsonConverter : CustomJsonConverterBase<decimal?>
    {
        #region Private variables

        private readonly DecimalFormatJsonConverter _decimalFormatJsonConverter = new DecimalFormatJsonConverter();

        #endregion

        #region Properties

        public override bool HandleNull => true;

        #endregion

        #region Methods

        public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
        {
            NullGuard.NotNull(writer, nameof(writer))
                .NotNull(options, nameof(options));

            if (value.HasValue)
            {
                _decimalFormatJsonConverter.Write(writer, value.Value, options);

                return;
            }

            writer.WriteNullValue();
        }

        public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            NullGuard.NotNull(typeToConvert, nameof(typeToConvert))
                .NotNull(options, nameof(options));

            return _decimalFormatJsonConverter.Read(ref reader, typeToConvert, options);
        }

        #endregion
    }
}