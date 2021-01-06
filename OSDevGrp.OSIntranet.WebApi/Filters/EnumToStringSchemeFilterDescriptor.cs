using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using OSDevGrp.OSIntranet.Core;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OSDevGrp.OSIntranet.WebApi.Filters
{
    internal class EnumToStringSchemeFilterDescriptor : ISchemaFilter
    {
        #region Methods

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            NullGuard.NotNull(schema, nameof(schema))
                .NotNull(context, nameof(context));

            if (context.Type.IsEnum == false)
            {
                return;
            }

            schema.Type = "string";
            schema.Format = "string";
            schema.Enum = GetEnumValues(context.Type)
                .Select(enumValue => (IOpenApiAny) new OpenApiString(enumValue))
                .ToList();
        }

        private static IEnumerable<string> GetEnumValues(Type enumType)
        {
            NullGuard.NotNull(enumType, nameof(enumType));

            return Enum.GetValues(enumType).OfType<object>()
                .Select(obj => enumType.GetField(obj.ToString() ?? string.Empty))
                .Where(fieldInfo => fieldInfo != null)
                .Select(fieldInfo =>
                {
                    EnumMemberAttribute enumMemberAttribute = fieldInfo.GetCustomAttributes(typeof(EnumMemberAttribute))
                        .OfType<EnumMemberAttribute>()
                        .SingleOrDefault();
                    if (enumMemberAttribute == null)
                    {
                        return fieldInfo.Name;
                    }

                    return string.IsNullOrWhiteSpace(enumMemberAttribute.Value)
                        ? fieldInfo.Name
                        : enumMemberAttribute.Value;
                })
                .ToArray();
        }

        #endregion
    }
}