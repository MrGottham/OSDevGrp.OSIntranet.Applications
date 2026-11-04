using Microsoft.OpenApi;
using OSDevGrp.OSIntranet.Core;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.WebApi.Filters
{
    internal class OperationApiVersionFilterDescriptor : IOperationFilter
    {
        #region Methods

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            NullGuard.NotNull(operation, nameof(operation))
                .NotNull(context, nameof(context));

            // Check if this operation should exclude the api-version header from the Swagger contract
            bool excludeApiVersionRequirement = context.MethodInfo.GetCustomAttributes(typeof(ExcludeApiVersionRequirementAttribute), false).Any() || context.MethodInfo.DeclaringType?.GetCustomAttributes(typeof(ExcludeApiVersionRequirementAttribute), false).Any() == true;

            // If excluded, return without adding the header parameter
            if (excludeApiVersionRequirement)
                return;

            if (operation.Parameters is null)
                operation.Parameters = new List<IOpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "api-version",
                In = ParameterLocation.Header,
                Description = "API version (default: 1.0)",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = JsonSchemaType.String
                }
            });
        }

        #endregion
    }
}