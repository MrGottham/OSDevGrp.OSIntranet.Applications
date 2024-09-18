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
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Converters;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.Options;
using OSDevGrp.OSIntranet.WebApi.Filters;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;
using OSDevGrp.OSIntranet.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        private IConfiguration Configuration { get; }

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
                opt.DefaultScheme = GetInternalScheme();
                opt.DefaultSignInScheme = GetInternalScheme();
                opt.DefaultSignOutScheme = GetInternalScheme();
                opt.DefaultAuthenticateScheme = GetBearerAuthenticationScheme();
                opt.DefaultChallengeScheme = GetBearerAuthenticationScheme();
            })
            .AddCookie(GetInternalScheme(), opt =>
            {
                opt.ExpireTimeSpan = new TimeSpan(0, 0, 10);
                opt.Cookie.SameSite = SameSiteMode.None;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.WebApi");
            })
            .AddMicrosoftAccount(opt =>
            {
                MicrosoftSecurityOptions microsoftSecurityOptions = Configuration.GetMicrosoftSecurityOptions();
                opt.ClientId = microsoftSecurityOptions.ClientId ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.MicrosoftClientId).Build();
                opt.ClientSecret = microsoftSecurityOptions.ClientSecret ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.MicrosoftClientSecret).Build();
                opt.SignInScheme = GetInternalScheme();
                opt.CorrelationCookie.SameSite = SameSiteMode.None;
                opt.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.SaveTokens = true;
                opt.Scope.Clear();
                opt.Scope.Add("User.Read");
                opt.Scope.Add("Contacts.ReadWrite");
                opt.Scope.Add("offline_access");
                opt.Events.OnCreatingTicket += o => o.Properties.Items.PrepareAsync(ClaimHelper.MicrosoftTokenClaimType, o.TokenType, o.AccessToken, o.RefreshToken, o.ExpiresIn);
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.WebApi");
            })
            .AddGoogle(opt =>
            {
                GoogleSecurityOptions googleSecurityOptions = Configuration.GetGoogleSecurityOptions();
                opt.ClientId = googleSecurityOptions.ClientId ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.GoogleClientId).Build();
                opt.ClientSecret = googleSecurityOptions.ClientSecret ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.GoogleClientSecret).Build();
                opt.SignInScheme = GetInternalScheme();
                opt.CorrelationCookie.SameSite = SameSiteMode.None;
                opt.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.SaveTokens = true;
                opt.Scope.Clear();
                opt.Scope.Add("openid");
                opt.Scope.Add("profile");
                opt.Scope.Add("email");
                opt.Events.OnCreatingTicket += o => o.Properties.Items.PrepareAsync(ClaimHelper.GoogleTokenClaimType, o.TokenType, o.AccessToken, o.RefreshToken, o.ExpiresIn);
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.WebApi");
            })
            .AddJwtBearer(opt =>
            {
                TokenGeneratorOptions tokenGeneratorOptions = Configuration.GetTokenGeneratorOptions();
                opt.IncludeErrorDetails = false;
                opt.RequireHttpsMetadata = true;
                opt.SaveToken = true;
                opt.TokenValidationParameters = tokenGeneratorOptions.ToTokenValidationParameters();
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(Policies.UserInfoPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(GetBearerAuthenticationScheme());
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimTypes.NameIdentifier);
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
#pragma warning disable ASP0000
                using ServiceProvider serviceProvider = services.BuildServiceProvider();
                using IServiceScope serviceScope = serviceProvider.CreateScope();
#pragma warning restore ASP0000

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

                OpenApiSecurityScheme oAuth2AuthenticationWithAuthorizationCodeFlowSecurityScheme = CreateOAuth2AuthenticationWithAuthorizationCodeFlowSecurityScheme(GetAuthorizationUrl(), GetTokenUrl(), GetScopes(serviceScope.ServiceProvider.GetRequiredService<ISupportedScopesProvider>()));
                OpenApiSecurityScheme oAuth2AuthenticationWithClientCredentialsFlowSecurityScheme = CreateOAuth2AuthenticationWithClientCredentialsFlowSecurityScheme(GetTokenUrl());
                OpenApiSecurityScheme bearerSecurityScheme = CreateBearerSecurityScheme();

                options.AddSecurityDefinition(oAuth2AuthenticationWithAuthorizationCodeFlowSecurityScheme.Reference.Id, oAuth2AuthenticationWithAuthorizationCodeFlowSecurityScheme);
                options.AddSecurityDefinition(oAuth2AuthenticationWithClientCredentialsFlowSecurityScheme.Reference.Id, oAuth2AuthenticationWithClientCredentialsFlowSecurityScheme);
                options.AddSecurityDefinition(bearerSecurityScheme.Reference.Id, bearerSecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {{oAuth2AuthenticationWithAuthorizationCodeFlowSecurityScheme, Array.Empty<string>()}});
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {{oAuth2AuthenticationWithClientCredentialsFlowSecurityScheme, Array.Empty<string>()}});
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {{bearerSecurityScheme, Array.Empty<string>()}});
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddHealthChecks()
                .AddSecurityHealthChecks(opt =>
                {
                    opt.WithMicrosoftValidation(Configuration, false);
                    opt.WithGoogleValidation(Configuration);
                    opt.WithTrustedDomainCollectionValidation(Configuration);
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

        private static Uri GetAuthorizationUrl()
        {
            return new Uri("/api/oauth/authorize", UriKind.Relative);
        }

        private static Uri GetTokenUrl()
        {
            return new Uri("/api/oauth/token", UriKind.Relative);
        }

        private static string GetInternalScheme()
        {
            return Schemes.Internal;
        }

        private static OpenApiSecurityScheme CreateOAuth2AuthenticationWithAuthorizationCodeFlowSecurityScheme(Uri authorizationUrl, Uri tokenUrl, IDictionary<string, string> scopes)
        {
            NullGuard.NotNull(authorizationUrl, nameof(authorizationUrl))
                .NotNull(tokenUrl, nameof(tokenUrl))
                .NotNull(scopes, nameof(scopes));

            return new OpenApiSecurityScheme
            {
                Name = "OAuth Authorization with Authorization Code Flow",
                Description = "OAuth Authorization 2.0 with the Authorization Code Flow.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Scheme = GetOAuth2AuthenticationWithAuthorizationCodeFlowScheme(),
                Reference = new OpenApiReference
                {
                    Id = GetOAuth2AuthenticationWithAuthorizationCodeFlowScheme(),
                    Type = ReferenceType.SecurityScheme
                },
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = authorizationUrl,
                        TokenUrl = tokenUrl,
                        Scopes = scopes
                    }
                }
            };
        }

        private static string GetOAuth2AuthenticationWithAuthorizationCodeFlowScheme()
        {
            return "OIDC";
        }

        private static OpenApiSecurityScheme CreateOAuth2AuthenticationWithClientCredentialsFlowSecurityScheme(Uri tokenUri)
        {
            NullGuard.NotNull(tokenUri, nameof(tokenUri));

            return new OpenApiSecurityScheme
            {
                Name = "OAuth Authorization with Client Credentials Flow",
                Description = $"OAuth Authorization 2.0 with the Client Credentials Flow.{Environment.NewLine}{Environment.NewLine}Example: 'Basic YzQwMGUxY2UtNzQyOC00NjBmLTg5ZTYtOGFmMTZkMWFiN2Ni'.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Scheme = GetOAuth2AuthenticationWithClientCredentialsFlowScheme(),
                Reference = new OpenApiReference
                {
                    Id = GetOAuth2AuthenticationWithClientCredentialsFlowScheme(),
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

        private static string GetOAuth2AuthenticationWithClientCredentialsFlowScheme()
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

        private static IDictionary<string, string> GetScopes(ISupportedScopesProvider supportedScopesProvider)
        {
            NullGuard.NotNull(supportedScopesProvider, nameof(supportedScopesProvider));

            return supportedScopesProvider.SupportedScopes.ToDictionary(supportedScope => supportedScope.Value.Name, supportedScope => supportedScope.Value.Description);
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