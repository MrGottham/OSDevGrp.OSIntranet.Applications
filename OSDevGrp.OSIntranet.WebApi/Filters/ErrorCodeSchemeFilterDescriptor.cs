using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.WebApi.Models.Core;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OSDevGrp.OSIntranet.WebApi.Filters
{
    internal class ErrorCodeSchemeFilterDescriptor : ISchemaFilter
    {
        #region Methods

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            NullGuard.NotNull(schema, nameof(schema))
                .NotNull(schema, nameof(schema));

            if (context.Type != typeof(ErrorModel))
            {
                return;
            }

            schema.AddExtension("x-error-codes", new ErrorCodeMetadataCollection(new CoreModelConverter()));
        }

        #endregion
    }

    internal class ErrorCodeMetadataCollection : IOpenApiExtension
    {
        #region Private variables

        private readonly IConverter _coreModelConverter;

        #endregion

        #region Constructor

        public ErrorCodeMetadataCollection(IConverter coreModelConverter)
        {
            NullGuard.NotNull(coreModelConverter, nameof(coreModelConverter));

            _coreModelConverter = coreModelConverter;
        }

        #endregion

        #region Properties

        public IEnumerable<IOpenApiExtension> ErrorCodes
        {
            get
            {
                return Enum.GetValues(typeof(ErrorCode))
                    .OfType<ErrorCode>()
                    .Select(errorCode =>
                    {
                        FieldInfo fieldInfo = errorCode.GetType().GetField(errorCode.ToString());
                        if (fieldInfo == null)
                        {
                            return null;
                        }

                        return new ErrorCodeMetadata((int) errorCode, fieldInfo.GetCustomAttributes(typeof(ErrorCodeAttribute)).OfType<ErrorCodeAttribute>().Single(), _coreModelConverter);
                    })
                    .Where(errorCodeMetadata => errorCodeMetadata != null)
                    .ToArray();
            }
        }

        #endregion

        #region Methods

        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            NullGuard.NotNull(writer, nameof(writer))
                .NotNull(specVersion, nameof(specVersion));

            writer.WriteStartArray();
            foreach (IOpenApiExtension errorCode in ErrorCodes)
            {
                errorCode.Write(writer, specVersion);
            }
            writer.WriteEndArray();
        }

        #endregion
    }

    internal class ErrorCodeMetadata : IOpenApiExtension
    {
        #region Constructor

        public ErrorCodeMetadata(int errorCode, ErrorCodeAttribute errorCodeAttribute, IConverter coreModelConverter)
        {
            NullGuard.NotNull(errorCodeAttribute, nameof(errorCodeAttribute))
                .NotNull(coreModelConverter, nameof(coreModelConverter));

            ErrorCode = errorCode;
            ErrorType = coreModelConverter.Convert<Type, string>(errorCodeAttribute.ExceptionType);
            ErrorMessage = errorCodeAttribute.Message;
        }

        #endregion

        #region Properties

        public int ErrorCode { get; }

        public string ErrorType { get; }

        public string ErrorMessage { get; }

        #endregion

        #region Methods

        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            NullGuard.NotNull(writer, nameof(writer))
                .NotNull(specVersion, nameof(specVersion));

            writer.WriteStartObject();
            writer.WriteProperty(nameof(ErrorCode), ErrorCode.ToString());
            if (string.IsNullOrWhiteSpace(ErrorType) == false)
            {
                writer.WriteProperty(nameof(ErrorType), ErrorType);
            }
            if (string.IsNullOrWhiteSpace(ErrorMessage) == false)
            {
                writer.WriteProperty(nameof(ErrorMessage), ErrorMessage);
            }
            writer.WriteEndObject();
        }

        #endregion
    }
}