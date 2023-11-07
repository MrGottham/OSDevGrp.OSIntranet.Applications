using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OSDevGrp.OSIntranet.BusinessLogic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Converters;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Resolvers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Filters;
using OSDevGrp.OSIntranet.Mvc.Security;
using OSDevGrp.OSIntranet.Repositories;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc
{
	public class Startup
    {
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
                // ReSharper disable UnusedParameter.Local
                opt.CheckConsentNeeded = context => true;
                // ReSharper restore UnusedParameter.Local
                opt.MinimumSameSitePolicy = SameSiteMode.None;
                opt.Secure = CookieSecurePolicy.SameAsRequest;
            });

            services.AddDataProtection()
                .SetApplicationName("OSDevGrp.OSIntranet.Mvc")
                .UseEphemeralDataProtectionProvider()
                .SetDefaultKeyLifetime(new TimeSpan(30, 0, 0, 0));

            services.AddAntiforgery();

            services.AddControllersWithViews(opt => opt.Filters.Add(typeof(AcquireTokenActionFilter)))
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    opt.JsonSerializerOptions.Converters.Add(new DecimalFormatJsonConverter());
                    opt.JsonSerializerOptions.Converters.Add(new NullableDecimalFormatJsonConverter());
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });
            services.AddRazorPages();

            services.AddAuthentication(opt => 
            {
                opt.DefaultScheme = Schemas.InternalAuthenticationSchema;
                opt.DefaultSignInScheme = Schemas.ExternalAuthenticationSchema;
            })
            .AddCookie(Schemas.InternalAuthenticationSchema, opt => 
            {
                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logoff";
                opt.AccessDeniedPath = "/Account/AccessDenied";
				opt.ExpireTimeSpan = new TimeSpan(0, 60, 0);
                opt.Cookie.SameSite = SameSiteMode.None;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            })
            .AddCookie(Schemas.ExternalAuthenticationSchema, opt =>
            {
                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logoff";
                opt.AccessDeniedPath = "/Account/AccessDenied";
                opt.ExpireTimeSpan = new TimeSpan(0, 0, 10);
                opt.Cookie.SameSite = SameSiteMode.None;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            })
            .AddMicrosoftAccount(opt => 
            {
                opt.ClientId = Configuration[SecurityConfigurationKeys.MicrosoftClientId] ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.MicrosoftClientId).Build();
                opt.ClientSecret = Configuration[SecurityConfigurationKeys.MicrosoftClientSecret] ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.MicrosoftClientSecret).Build();
                opt.SignInScheme = Schemas.ExternalAuthenticationSchema;
                opt.CorrelationCookie.SameSite = SameSiteMode.None;
                opt.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.SaveTokens = true;
                opt.Scope.Clear();
                opt.Scope.Add("User.Read");
                opt.Scope.Add("Contacts.ReadWrite");
                opt.Scope.Add("offline_access");
                opt.Events.OnCreatingTicket += o =>
                {
                    double seconds = o.ExpiresIn?.TotalSeconds ?? 0;
                    IRefreshableToken refreshableToken = new RefreshableToken(o.TokenType, o.AccessToken, o.RefreshToken, DateTime.UtcNow.AddSeconds(seconds));
                    o.Properties.Items.Add($".{TokenType.MicrosoftGraphToken}", refreshableToken.ToBase64());
                    return Task.CompletedTask;
                };
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            })
            .AddGoogle(opt =>
            {
                opt.ClientId = Configuration[SecurityConfigurationKeys.GoogleClientId] ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.GoogleClientId).Build();
				opt.ClientSecret = Configuration[SecurityConfigurationKeys.GoogleClientSecret] ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.GoogleClientSecret).Build();
				opt.SignInScheme = Schemas.ExternalAuthenticationSchema;
                opt.CorrelationCookie.SameSite = SameSiteMode.None;
                opt.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            });
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(Policies.ContactPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemas.InternalAuthenticationSchema);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.ContactsClaimType);
                });
                opt.AddPolicy(Policies.AccountingPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemas.InternalAuthenticationSchema);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.AccountingClaimType);
                });
                opt.AddPolicy(Policies.AccountingAdministratorPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemas.InternalAuthenticationSchema);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.AccountingClaimType);
                    policy.RequireClaim(ClaimHelper.AccountingAdministratorClaimType);
                });
                opt.AddPolicy(Policies.AccountingCreatorPolicy, policy =>
                {
	                policy.AddAuthenticationSchemes(Schemas.InternalAuthenticationSchema);
	                policy.RequireAuthenticatedUser();
	                policy.RequireClaim(ClaimHelper.AccountingClaimType);
	                policy.RequireClaim(ClaimHelper.AccountingCreatorClaimType);
                });
                opt.AddPolicy(Policies.AccountingModifierPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemas.InternalAuthenticationSchema);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.AccountingClaimType);
                    policy.RequireClaim(ClaimHelper.AccountingModifierClaimType);
                });
                opt.AddPolicy(Policies.AccountingViewerPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemas.InternalAuthenticationSchema);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.AccountingClaimType);
                    policy.RequireClaim(ClaimHelper.AccountingViewerClaimType);
                });
                opt.AddPolicy(Policies.MediaLibraryPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemas.InternalAuthenticationSchema);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.MediaLibraryClaimType);
                });
                opt.AddPolicy(Policies.MediaLibraryModifierPolicy, policy =>
                {
	                policy.AddAuthenticationSchemes(Schemas.InternalAuthenticationSchema);
	                policy.RequireAuthenticatedUser();
	                policy.RequireClaim(ClaimHelper.MediaLibraryClaimType);
	                policy.RequireClaim(ClaimHelper.MediaLibraryModifierClaimType);
                });
                opt.AddPolicy(Policies.MediaLibraryLenderPolicy, policy =>
                {
	                policy.AddAuthenticationSchemes(Schemas.InternalAuthenticationSchema);
	                policy.RequireAuthenticatedUser();
	                policy.RequireClaim(ClaimHelper.MediaLibraryClaimType);
	                policy.RequireClaim(ClaimHelper.MediaLibraryLenderClaimType);
                });
                opt.AddPolicy(Policies.CommonDataPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemas.InternalAuthenticationSchema);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.CommonDataClaimType);
                });
                opt.AddPolicy(Policies.SecurityAdminPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemas.InternalAuthenticationSchema);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.SecurityAdminClaimType);
                });
            });

            services.AddHealthChecks()
                .AddSecurityHealthChecks(opt =>
                {
                    opt.WithMicrosoftValidation(Configuration);
                    opt.WithGoogleValidation(Configuration);
                    opt.WithTrustedDomainCollectionValidation(Configuration);
                    opt.WithAcmeChallengeValidation(Configuration);
                })
                .AddRepositoryHealthChecks(opt =>
                {
                    opt.WithRepositoryContextValidation();
                    opt.WithConnectionStringsValidation(Configuration);
                    opt.WithExternalDataDashboardValidation(Configuration);
                });

            services.AddCommandBus().AddCommandHandlers(typeof(AuthenticateCommandHandlerBase<,>).Assembly);
            services.AddQueryBus().AddQueryHandlers(typeof(AuthenticateCommandHandlerBase<,>).Assembly);
            services.AddEventPublisher();
            services.AddResolvers();
            services.AddDomainLogic();
            services.AddRepositories();
            services.AddBusinessLogicValidators();
            services.AddBusinessLogicHelpers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipalResolver, PrincipalResolver>();
            services.AddTransient<ITrustedDomainHelper, TrustedDomainHelper>();
            services.AddTransient<ITokenHelperFactory, TokenHelperFactory>();
            services.AddTransient<ITokenHelper, MicrosoftGraphTokenHelper>();
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            if (RunningInDocker() == false)
            {
                app.UseHttpsRedirection();
            }
            app.UseStaticFiles();

            app.UseRequestLocalization(options => 
            {
                options.AddSupportedCultures("da-DK", "da");
                options.AddSupportedUICultures("da-DK", "da");
                options.SetDefaultCulture("da-DK");
            });

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("default");

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/health");
            });

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