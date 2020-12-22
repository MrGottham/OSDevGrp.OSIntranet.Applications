using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OSDevGrp.OSIntranet.Core;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OSDevGrp.OSIntranet.WebApi.Filters
{
    internal class OperationAuthorizeFilterDescriptor : IOperationFilter
    {
        #region Private variables

        private readonly AuthorizationOptions _authorizationOptions;
        private readonly IDictionary<string, OpenApiSecurityScheme> _securitySchemes;
        private readonly IList<OpenApiSecurityRequirement> _securityRequirements;

        #endregion

        #region Constructor

        public OperationAuthorizeFilterDescriptor(IOptions<AuthorizationOptions> authorizationOptions, IOptions<SwaggerGenOptions> swaggerGenOptions)
        {
            NullGuard.NotNull(authorizationOptions, nameof(authorizationOptions))
                .NotNull(swaggerGenOptions, nameof(swaggerGenOptions));

            _authorizationOptions = authorizationOptions.Value;
            _securitySchemes = swaggerGenOptions.Value.SwaggerGeneratorOptions.SecuritySchemes;
            _securityRequirements = swaggerGenOptions.Value.SwaggerGeneratorOptions.SecurityRequirements;
        }

        #endregion

        #region Methods

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            NullGuard.NotNull(operation, nameof(operation))
                .NotNull(context, nameof(context));

            if (GetAllowAnonymousAttribute(context.MethodInfo) != null)
            {
                return;
            }

            string primaryAuthenticationScheme = GetPrimaryAuthenticationScheme(GetAuthorizeAttribute(context.MethodInfo));
            if (string.IsNullOrWhiteSpace(primaryAuthenticationScheme) == false)
            {
                Apply(operation, primaryAuthenticationScheme);
                return;
            }

            if (GetAllowAnonymousAttribute(context.MethodInfo.DeclaringType) != null)
            {
                return;
            }

            primaryAuthenticationScheme = GetPrimaryAuthenticationScheme(GetAuthorizeAttribute(context.MethodInfo.DeclaringType));
            if (string.IsNullOrWhiteSpace(primaryAuthenticationScheme))
            {
                return;
            }

            Apply(operation, primaryAuthenticationScheme);
        }

        private string GetPrimaryAuthenticationScheme(IAuthorizeData authorizeData)
        {
            if (authorizeData == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(authorizeData.Policy) == false)
            {
                AuthorizationPolicy authorizationPolicy = _authorizationOptions.GetPolicy(authorizeData.Policy);

                return authorizationPolicy?.AuthenticationSchemes.FirstOrDefault();
            }

            return string.IsNullOrWhiteSpace(authorizeData.AuthenticationSchemes) ? null : authorizeData.AuthenticationSchemes.Split(',').First();
        }

        private void Apply(OpenApiOperation operation, string primaryAuthenticationScheme)
        {
            NullGuard.NotNull(operation, nameof(operation))
                .NotNullOrWhiteSpace(primaryAuthenticationScheme, nameof(primaryAuthenticationScheme));

            if (_securitySchemes.TryGetValue(primaryAuthenticationScheme, out OpenApiSecurityScheme securityScheme) == false)
            {
                return;
            }

            if (operation.Responses.ContainsKey(Convert.ToString((int) HttpStatusCode.Unauthorized)) == false)
            {
                operation.Responses.Add(Convert.ToString((int) HttpStatusCode.Unauthorized), new OpenApiResponse {Description = "Unauthorized"});
            }

            if (operation.Responses.ContainsKey(Convert.ToString((int) HttpStatusCode.Forbidden)) == false)
            {
                operation.Responses.Add(Convert.ToString((int) HttpStatusCode.Forbidden), new OpenApiResponse {Description = "Forbidden"});
            }

            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(_securityRequirements.Single(securityRequirement => securityRequirement.ContainsKey(securityScheme)));
        }

        private static AllowAnonymousAttribute GetAllowAnonymousAttribute(MemberInfo memberInfo)
        {
            NullGuard.NotNull(memberInfo, nameof(memberInfo));

            return GetCustomAttribute<AllowAnonymousAttribute>(memberInfo);
        }

        private static AuthorizeAttribute GetAuthorizeAttribute(MemberInfo memberInfo)
        {
            NullGuard.NotNull(memberInfo, nameof(memberInfo));

            return GetCustomAttribute<AuthorizeAttribute>(memberInfo);
        }

        private static T GetCustomAttribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            NullGuard.NotNull(memberInfo, nameof(memberInfo));

            return memberInfo.GetCustomAttributes(typeof(T), true)
                .OfType<T>()
                .SingleOrDefault();
        }

        #endregion
    }
}