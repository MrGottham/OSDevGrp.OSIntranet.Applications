using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.WebApi.Models.Core;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OSDevGrp.OSIntranet.WebApi.Filters
{
    internal class OperationResponseFilterDescriptor : IOperationFilter
    {
        #region Private variables

        private readonly static Type[] EmptyResponseTypeCollection = new[] 
        {
            typeof(void),
            typeof(IActionResult),
            typeof(ActionResult),
            typeof(EmptyResult),
            typeof(OkResult),
            typeof(Task)
        };
        private readonly static Type[] GenericResponseTypeCollection = new[]
        {
            typeof(ActionResult<>),
            typeof(Task<>)
        };

        #endregion

        #region Methods

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            NullGuard.NotNull(operation, nameof(operation))
                .NotNull(context, nameof(context));

            AddResponse(operation, context, HttpStatusCode.OK, "Success", GetResponseType(context.MethodInfo));
            AddResponse(operation, context, HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorModel));
            AddResponse(operation, context, HttpStatusCode.InternalServerError, "Internal Server Error", typeof(ErrorModel));
        }

        private static void AddResponse(OpenApiOperation operation, OperationFilterContext context, HttpStatusCode httpStatusCode, string description, Type responseType)
        {
            NullGuard.NotNull(operation, nameof(operation))
                .NotNull(context, nameof(context))
                .NotNullOrWhiteSpace(description, nameof(description));

            string key = Convert.ToString((int) httpStatusCode);
            if (operation.Responses.ContainsKey(key))
            {
                return;
            }

            OpenApiResponse response = new OpenApiResponse
            {
                Description = description,
            };

            if (responseType != null && response.Content.ContainsKey("application/json") == false)
            {
                response.Content.Add("application/json", new OpenApiMediaType { Schema = context.SchemaGenerator.GenerateSchema(responseType, context.SchemaRepository) });
            }

            operation.Responses.Add(key, response);
        }

        private static Type GetResponseType(MethodInfo methodInfo)
        {
            NullGuard.NotNull(methodInfo, nameof(methodInfo));

            return GetResponseType(methodInfo.ReturnType);
        }

        private static Type GetResponseType(Type type)
        {
            if (type == null || EmptyResponseTypeCollection.Contains(type))
            {
                return null;
            }

            if (type.IsGenericType == false)
            {
                return type;
            }

            Type genericTypeDefinition = type.GetGenericTypeDefinition();
            if (genericTypeDefinition != null && GenericResponseTypeCollection.Contains(genericTypeDefinition))
            {
                return GetResponseType(type.GetGenericArguments().Single());
            }

            return type;
        }

        #endregion
    }
}