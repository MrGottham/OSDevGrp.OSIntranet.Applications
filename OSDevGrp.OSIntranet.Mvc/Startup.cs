using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OSDevGrp.OSIntranet.BusinessLogic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Converters;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Resolvers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Filters;
using OSDevGrp.OSIntranet.Mvc.Security;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.Options;
using System;
using System.Text.Json.Serialization;

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

        private IConfiguration Configuration { get; }

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
                opt.CheckConsentNeeded = _ => true;
                opt.ConsentCookie.Name = $"{GetType().Namespace}.Consent";
                opt.MinimumSameSitePolicy = SameSiteMode.Lax;
                opt.Secure = CookieSecurePolicy.Always;
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = GetLoginPath();
                opt.LogoutPath = GetLogoffPath();
                opt.AccessDeniedPath = GetAccessDeniedPath();
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.Cookie.Name = $"{GetType().Namespace}.Application";
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            });

            services.AddAntiforgery(opt =>
            {
                opt.FormFieldName = "__CSRF";
                opt.HeaderName = $"X-{GetType().Namespace}-CSRF-TOKEN";
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.Cookie.Name = $"{GetType().Namespace}.Antiforgery";
            });

            services.AddDataProtection()
                .SetApplicationName("OSDevGrp.OSIntranet.Mvc")
                .UseEphemeralDataProtectionProvider()
                .SetDefaultKeyLifetime(new TimeSpan(30, 0, 0, 0));

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
                opt.DefaultScheme = Schemes.Internal;
                opt.DefaultSignInScheme = Schemes.Internal;
                opt.DefaultSignOutScheme = Schemes.Internal;
                opt.DefaultAuthenticateScheme = Schemes.Internal;
                opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(Schemes.Internal, opt =>
            {
                opt.LoginPath = GetLoginPath();
                opt.LogoutPath = GetLogoffPath();
                opt.AccessDeniedPath = GetAccessDeniedPath();
                opt.ExpireTimeSpan = new TimeSpan(0, 60, 0);
                opt.Cookie.SameSite = SameSiteMode.Lax;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.Cookie.Name = $"{GetType().Namespace}.Authentication.{Schemes.Internal}";
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            })
            .AddCookie(Schemes.External, opt =>
            {
                opt.LoginPath = GetLoginPath();
                opt.LogoutPath = GetLogoffPath();
                opt.AccessDeniedPath = GetAccessDeniedPath();
                opt.ExpireTimeSpan = new TimeSpan(0, 0, 10);
                opt.Cookie.SameSite = SameSiteMode.Lax;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.Cookie.Name = $"{GetType().Namespace}.Authentication.{Schemes.External}";
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            })
            .AddOpenIdConnect(opt =>
            {
                Repositories.Options.OpenIdConnectOptions openIdConnectOptions = Configuration.GetOpenIdConnectOptions();
                opt.Authority = openIdConnectOptions.Authority ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.OpenIdConnectAuthority).Build();
                opt.ClientId = openIdConnectOptions.ClientId ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.OpenIdConnectClientId).Build();
                opt.ClientSecret = openIdConnectOptions.ClientSecret ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.OpenIdConnectClientSecret).Build();
                opt.SignInScheme = Schemes.Internal;
                opt.SignOutScheme = Schemes.Internal;
                opt.AccessDeniedPath = GetAccessDeniedPath();
                opt.CorrelationCookie.SameSite = SameSiteMode.Lax;
                opt.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.CorrelationCookie.Name = $"{GetType().Namespace}.Authentication.{OpenIdConnectDefaults.AuthenticationScheme}";
                opt.SaveTokens = true;
                opt.ResponseType = OpenIdConnectResponseType.Code;
                opt.UsePkce = true;
                opt.GetClaimsFromUserInfoEndpoint = true; 
                opt.MapInboundClaims = true;
                opt.Scope.Clear();
                opt.Scope.Add("openid");
                opt.Scope.Add("email");
                opt.Scope.Add("profile");
                opt.Scope.Add("webapi");
                opt.ClaimActions.Add(new DeleteClaimAction("name"));
                opt.ClaimActions.Add(new DeleteClaimAction("auth_time"));
                opt.Events.OnTokenValidated += async context =>
                {
                    ITokenHelperFactory tokenHelperFactory = context.HttpContext.RequestServices.GetRequiredService<ITokenHelperFactory>();
                    await tokenHelperFactory.StoreExternalTokensInSecurityToken(context.HttpContext, context.SecurityToken);
                };
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            })
            .AddMicrosoftAccount(opt =>
            {
                MicrosoftSecurityOptions microsoftSecurityOptions = Configuration.GetMicrosoftSecurityOptions();
                opt.ClientId = microsoftSecurityOptions.ClientId ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.MicrosoftClientId).Build();
                opt.ClientSecret = microsoftSecurityOptions.ClientSecret ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.MicrosoftClientSecret).Build();
                opt.SignInScheme = Schemes.External;
                opt.AccessDeniedPath = GetAccessDeniedPath();
                opt.CorrelationCookie.SameSite = SameSiteMode.Lax;
                opt.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.CorrelationCookie.Name = $"{GetType().Namespace}.Authentication.{MicrosoftAccountDefaults.AuthenticationScheme}";
                opt.SaveTokens = true;
                opt.Scope.Clear();
                opt.Scope.Add("User.Read");
                opt.Scope.Add("Contacts.ReadWrite");
                opt.Scope.Add("offline_access");
                opt.Events.OnCreatingTicket += o => o.Properties.Items.PrepareAsync(ClaimHelper.MicrosoftTokenClaimType, o.TokenType, o.AccessToken, o.RefreshToken, o.ExpiresIn);
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            })
            .AddGoogle(opt =>
            {
                GoogleSecurityOptions googleSecurityOptions = Configuration.GetGoogleSecurityOptions();
                opt.ClientId = googleSecurityOptions.ClientId ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.GoogleClientId).Build();
                opt.ClientSecret = googleSecurityOptions.ClientSecret ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.GoogleClientSecret).Build();
                opt.SignInScheme = Schemes.External;
                opt.AccessDeniedPath = GetAccessDeniedPath();
                opt.CorrelationCookie.SameSite = SameSiteMode.Lax;
                opt.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.CorrelationCookie.Name = $"{GetType().Namespace}.Authentication.{GoogleDefaults.AuthenticationScheme}";
                opt.Events.OnCreatingTicket += o => o.Properties.Items.PrepareAsync(ClaimHelper.GoogleTokenClaimType, o.TokenType, o.AccessToken, o.RefreshToken, o.ExpiresIn);
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            });
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(Policies.ContactPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemes.Internal);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.ContactsClaimType);
                });
                opt.AddPolicy(Policies.AccountingPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemes.Internal);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.AccountingClaimType);
                });
                opt.AddPolicy(Policies.AccountingAdministratorPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemes.Internal);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.AccountingClaimType);
                    policy.RequireClaim(ClaimHelper.AccountingAdministratorClaimType);
                });
                opt.AddPolicy(Policies.AccountingCreatorPolicy, policy =>
                {
	                policy.AddAuthenticationSchemes(Schemes.Internal);
	                policy.RequireAuthenticatedUser();
	                policy.RequireClaim(ClaimHelper.AccountingClaimType);
	                policy.RequireClaim(ClaimHelper.AccountingCreatorClaimType);
                });
                opt.AddPolicy(Policies.AccountingModifierPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemes.Internal);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.AccountingClaimType);
                    policy.RequireClaim(ClaimHelper.AccountingModifierClaimType);
                });
                opt.AddPolicy(Policies.AccountingViewerPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemes.Internal);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.AccountingClaimType);
                    policy.RequireClaim(ClaimHelper.AccountingViewerClaimType);
                });
                opt.AddPolicy(Policies.MediaLibraryPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemes.Internal);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.MediaLibraryClaimType);
                });
                opt.AddPolicy(Policies.MediaLibraryModifierPolicy, policy =>
                {
	                policy.AddAuthenticationSchemes(Schemes.Internal);
	                policy.RequireAuthenticatedUser();
	                policy.RequireClaim(ClaimHelper.MediaLibraryClaimType);
	                policy.RequireClaim(ClaimHelper.MediaLibraryModifierClaimType);
                });
                opt.AddPolicy(Policies.MediaLibraryLenderPolicy, policy =>
                {
	                policy.AddAuthenticationSchemes(Schemes.Internal);
	                policy.RequireAuthenticatedUser();
	                policy.RequireClaim(ClaimHelper.MediaLibraryClaimType);
	                policy.RequireClaim(ClaimHelper.MediaLibraryLenderClaimType);
                });
                opt.AddPolicy(Policies.CommonDataPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemes.Internal);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.CommonDataClaimType);
                });
                opt.AddPolicy(Policies.SecurityAdminPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(Schemes.Internal);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimHelper.SecurityAdminClaimType);
                });
            });

            services.AddHealthChecks()
                .AddSecurityHealthChecks(opt =>
                {
                    opt.WithOpenIdConnect(Configuration);
                    opt.WithMicrosoftValidation(Configuration, true);
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
            services.AddTransient<ITokenHelperFactory, TokenHelperFactory>();
            services.AddTransient<ITokenHelper, MicrosoftGraphTokenHelper>();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
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
            app.MapStaticAssets();

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

            app.MapRazorPages().WithStaticAssets();
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();
            app.MapHealthChecks("/health");
        }

        private static string GetLoginPath() => $"/Account/Login&scheme={OpenIdConnectDefaults.AuthenticationScheme}";

        private static string GetLogoffPath() => "/Account/Logoff";

        private static string GetAccessDeniedPath() => "/Account/AccessDenied";

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