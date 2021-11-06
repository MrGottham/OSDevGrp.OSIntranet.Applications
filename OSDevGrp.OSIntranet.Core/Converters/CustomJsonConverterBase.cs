using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.Core.Converters
{
    public abstract class CustomJsonConverterBase<T> : JsonConverter<T>
    {
        #region Properties

        public IFormatProvider FormatProvider => CultureInfo.InvariantCulture;

        #endregion

        #region Methods

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            NullGuard.NotNull(typeToConvert, nameof(typeToConvert))
                .NotNull(options, nameof(options));

            throw new NotSupportedException();
        }

        #endregion
    }
}