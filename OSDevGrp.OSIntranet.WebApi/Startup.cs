using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OSDevGrp.OSIntranet.BusinessLogic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Converters;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.WebApi.Filters;
using OSDevGrp.OSIntranet.WebApi.Handlers;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;
using OSDevGrp.OSIntranet.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi
{
    public class Startup
    {
        private const string WebApiVersion = "v1";
        private const string WebApiName = "OS Development Group Web API";
        private const string WebApiDescription = "Web API supporting OS Intranet by OS Development Group";
        private const string DotnetRunningInContainerEnvironmentVariable = "DOTNET_RUNNING_IN_CONTAINER";

        public Startup(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            NullGuard.NotNull(services, nameof(services));

            services.Configure<ForwardedHeadersOptions>(opt => 
            {
                opt.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto;
                opt.KnownNetworks.Clear();
                opt.KnownProxies.Clear();
            });

            services.Configure<CookiePolicyOptions>(opt =>
            {
                opt.MinimumSameSitePolicy = SameSiteMode.None;
                opt.Secure = CookieSecurePolicy.SameAsRequest;
            });

            services.AddDataProtection()
                .SetApplicationName("OSDevGrp.OSIntranet.WebApi")
                .UseEphemeralDataProtectionProvider()
                .SetDefaultKeyLifetime(new TimeSpan(30, 0, 0, 0));

            services.AddRazorPages();
            services.AddControllers(opt => 
            {
                opt.Filters.Add<ErrorHandlerFilter>();
                opt.Filters.Add<SchemaValidationFilter>();
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.Converters.Add(new DecimalFormatJsonConverter());
                opt.JsonSerializerOptions.Converters.Add(new NullableDecimalFormatJsonConverter());
                opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            services.AddApiVersioning(opt => opt.ApiVersionReader = new HeaderApiVersionReader());

            services.AddAuthentication(opt => 
            {
                opt.DefaultAuthenticateScheme = GetBearerAuthenticationScheme();
                opt.DefaultChallengeScheme = GetBearerAuthenticationScheme();
            })
            .AddJwtBearer(opt =>
            {
	            TokenGeneratorOptions tokenGeneratorOptions = Configuration.GetTokenGeneratorOptions();
                opt.IncludeErrorDetails = false;
                opt.RequireHttpsMetadata = true;
                opt.SaveToken = true;
                opt.TokenValidationParameters = tokenGeneratorOptions.ToTokenValidationParameters();
            })
            .AddClientSecret(GetBasicAuthenticationScheme());

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(Policies.AcquireTokenPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(GetBasicAuthenticationScheme());
                    policy.RequireAuthenticatedUser();
                });
                opt.AddPolicy(Policies.AccountingPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(GetBearerAuthenticationScheme());
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.AccountingClaimType);
                });
                opt.AddPolicy(Policies.AccountingModifierPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(GetBearerAuthenticationScheme());
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.AccountingClaimType);
                    policy.RequireClaim(ClaimHelper.AccountingModifierClaimType);
                });
                opt.AddPolicy(Policies.AccountingViewerPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(GetBearerAuthenticationScheme());
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.AccountingClaimType);
                    policy.RequireClaim(ClaimHelper.AccountingViewerClaimType);
                });
                opt.AddPolicy(Policies.CommonDataPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(GetBearerAuthenticationScheme());
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.CommonDataClaimType);
                });
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(WebApiVersion, new OpenApiInfo
                {
                    Version = WebApiVersion,
                    Title = WebApiName,
                    Description = WebApiDescription
                });

                options.OperationFilter<OperationAuthorizeFilterDescriptor>();
                options.OperationFilter<OperationResponseFilterDescriptor>();
                options.SchemaFilter<EnumToStringSchemeFilterDescriptor>();
                options.SchemaFilter<ErrorCodeSchemeFilterDescriptor>();

                OpenApiSecurityScheme oAuthWithClientCredentialsFlowSecurityScheme = CreateOAuthWithClientCredentialsFlowSecurityScheme(new Uri("/api/oauth/token", UriKind.Relative));
                OpenApiSecurityScheme bearerSecurityScheme = CreateBearerSecurityScheme();

                options.AddSecurityDefinition(oAuthWithClientCredentialsFlowSecurityScheme.Reference.Id, oAuthWithClientCredentialsFlowSecurityScheme);
                options.AddSecurityDefinition(bearerSecurityScheme.Reference.Id, bearerSecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {{ oAuthWithClientCredentialsFlowSecurityScheme, Array.Empty<string>()}});
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {{bearerSecurityScheme, Array.Empty<string>()}});
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddHealthChecks()
                .AddSecurityHealthChecks(opt =>
                {
                    opt.WithJwtValidation(Configuration);
                    opt.WithAcmeChallengeValidation(Configuration);
                })
                .AddRepositoryHealthChecks(opt => 
                {
                    opt.WithRepositoryContextValidation();
                    opt.WithConnectionStringsValidation(Configuration);
                });

            services.AddCommandBus().AddCommandHandlers(typeof(CreateUserIdentityCommandHandler).Assembly);
            services.AddQueryBus().AddQueryHandlers(typeof(CreateUserIdentityCommandHandler).Assembly);
            services.AddEventPublisher();
            services.AddResolvers(Configuration);
            services.AddDomainLogic();
            services.AddRepositories(Configuration);
            services.AddBusinessLogicConfiguration(Configuration);
            services.AddBusinessLogicValidators();
            services.AddBusinessLogicHelpers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipalResolver, PrincipalResolver>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            NullGuard.NotNull(app, nameof(app))
                .NotNull(env, nameof(env));

            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            if (RunningInDocker() == false)
            {
                app.UseHttpsRedirection();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseCors("default");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "/api/swagger/{documentName}/swagger.json";
                options.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
                {
                    const string forwardedProtoHeader = "X-Forwarded-Proto";
                    const string forwardedHostHeader = "X-Forwarded-Host";

                    if (httpRequest.Headers.ContainsKey(forwardedProtoHeader) == false || string.IsNullOrWhiteSpace(httpRequest.Headers[forwardedProtoHeader]))
                    {
                        return;
                    }

                    if (httpRequest.Headers.ContainsKey(forwardedHostHeader) == false || string.IsNullOrWhiteSpace(httpRequest.Headers[forwardedHostHeader]))
                    {
                        return;
                    }

                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new()
                        {
                            Url = $"{httpRequest.Headers[forwardedProtoHeader]}://{httpRequest.Headers[forwardedHostHeader]}"
                        }
                    };
                });
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/api/swagger/{WebApiVersion}/swagger.json", WebApiName);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
	            endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/health");
                endpoints.MapHealthChecks("/api/health");
            });
        }

        private static OpenApiSecurityScheme CreateOAuthWithClientCredentialsFlowSecurityScheme(Uri tokenUri)
        {
            NullGuard.NotNull(tokenUri, nameof(tokenUri));

            return new OpenApiSecurityScheme
            {
                Name = "OAuth Authorization with client credentials grant flow",
                Description = $"OAuth Authorization 2.0 with the client credentials grant flow.{Environment.NewLine}{Environment.NewLine}Example: '{GetBasicAuthenticationScheme()} YzQwMGUxY2UtNzQyOC00NjBmLTg5ZTYtOGFmMTZkMWFiN2Ni'.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Scheme = GetBasicAuthenticationScheme(),
                Reference = new OpenApiReference
                {
                    Id = GetBasicAuthenticationScheme(),
                    Type = ReferenceType.SecurityScheme
                },
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = tokenUri,
                        Scopes = new Dictionary<string, string>(0)
                    }
                }
            };
        }

        private static string GetBasicAuthenticationScheme()
        {
            return "Basic";
        }

        private static OpenApiSecurityScheme CreateBearerSecurityScheme()
        {
            return new OpenApiSecurityScheme
            {
                Name = "JWT Authorization header",
                Description = $"JWT Authorization header using the Bearer scheme.{Environment.NewLine}{Environment.NewLine}Example: '{GetBearerAuthenticationScheme()} NGI4YTg0MWEtMzNiZi00MTYyLTk5MGMtY2M5OTFjOWU2MmRi'.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = GetBearerAuthenticationScheme().ToLower(),
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = GetBearerAuthenticationScheme(),
                    Type = ReferenceType.SecurityScheme
                }
            };
        }

        private static string GetBearerAuthenticationScheme()
        {
            return JwtBearerDefaults.AuthenticationScheme;
        }

        private static bool RunningInDocker()
        {
            return RunningInDocker(Environment.GetEnvironmentVariable(DotnetRunningInContainerEnvironmentVariable));
        }

        private static bool RunningInDocker(string environmentVariable)
        {
            if (string.IsNullOrWhiteSpace(environmentVariable) || bool.TryParse(environmentVariable, out bool result) == false)
            {
                return false;
            }

            return result;
        }
    }
}