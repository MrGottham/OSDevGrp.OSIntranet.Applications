using System;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OSDevGrp.OSIntranet.BusinessLogic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;
using OSDevGrp.OSIntranet.WebApi.Helpers.Security;

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
                opt.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                opt.KnownNetworks.Clear();
                opt.KnownProxies.Clear();
            });

            services.Configure<CookiePolicyOptions>(opt =>
            {
                opt.MinimumSameSitePolicy = SameSiteMode.None;
                opt.Secure = CookieSecurePolicy.SameAsRequest;
            });

            services.AddControllers().AddJsonOptions(opt => 
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddApiVersioning(opt => opt.ApiVersionReader = new HeaderApiVersionReader());

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(Configuration["Security:JWT:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("SecurityAdmin", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimHelper.SecurityAdminClaimType);
                });
                opt.AddPolicy("Accounting", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimHelper.AccountingClaimType);
                });
                opt.AddPolicy("CommonData", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
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

                OpenApiSecurityScheme basicOpenApiSecurityScheme = CreateBasicOpenApiSecurityScheme();
                OpenApiSecurityScheme bearerOpenApiSecurityScheme = CreateBearerOpenApiSecurityScheme();
                
                options.AddSecurityDefinition(basicOpenApiSecurityScheme.Reference.Id, basicOpenApiSecurityScheme);
                options.AddSecurityDefinition(bearerOpenApiSecurityScheme.Reference.Id, bearerOpenApiSecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {basicOpenApiSecurityScheme, Array.Empty<string>()},
                    {bearerOpenApiSecurityScheme, Array.Empty<string>()},
                });
            });

            services.AddHealthChecks();

            services.AddCommandBus().AddCommandHandlers(typeof(AuthenticateCommandHandlerBase<,>).Assembly);
            services.AddQueryBus().AddQueryHandlers(typeof(AuthenticateCommandHandlerBase<,>).Assembly);
            services.AddResolvers();
            services.AddRepositories();
            services.AddBusinessLogicValidators();
            services.AddBusinessLogicHelpers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipalResolver, PrincipalResolver>();
            services.AddTransient<ISecurityContextReader, SecurityContextReader>();
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

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", WebApiName);
            });

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHealthChecks("/health");
            });
        }

        private OpenApiSecurityScheme CreateBasicOpenApiSecurityScheme()
        {
            return new OpenApiSecurityScheme
            {
                Name = "Basic Authorization",
                Description = $"Basic Authorization header using the Basic scheme.{Environment.NewLine}{Environment.NewLine}Example: 'Basic NGIyNGRiZGQtZGQ4NS00MTM3LWJjNzgtMzQ4YjBmMjhlMWFl'",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Basic",
                Reference = new OpenApiReference
                {
                    Id = "Basic",
                    Type = ReferenceType.SecurityScheme
                }
            };
        }

        private OpenApiSecurityScheme CreateBearerOpenApiSecurityScheme()
        {
            return new OpenApiSecurityScheme
            {
                Name = "JWT Authorization header",
                Description = $"JWT Authorization header using the Bearer scheme.{Environment.NewLine}{Environment.NewLine}Example: '{JwtBearerDefaults.AuthenticationScheme} NGI4YTg0MWEtMzNiZi00MTYyLTk5MGMtY2M5OTFjOWU2MmRi'",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
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