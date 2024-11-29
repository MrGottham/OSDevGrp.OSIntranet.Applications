using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
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
                opt.DefaultScheme = Schemes.Internal;
                opt.DefaultSignInScheme = Schemes.Internal;
                opt.DefaultSignOutScheme = Schemes.Internal;
                opt.DefaultAuthenticateScheme = Schemes.Internal;
                opt.DefaultChallengeScheme = MicrosoftAccountDefaults.AuthenticationScheme;
            })
            .AddCookie(Schemes.Internal, opt =>
            {
                opt.LoginPath = $"/Account/Login&scheme={MicrosoftAccountDefaults.AuthenticationScheme}";
                opt.LogoutPath = "/Account/Logoff";

                opt.ExpireTimeSpan = new TimeSpan(0, 60, 0);
                opt.Cookie.SameSite = SameSiteMode.Lax;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
                
                // TODO: Initialize values
                //opt.AccessDeniedPath = "/Account/AccessDenied";
            })
            .AddCookie(Schemes.External, opt =>
            {
                opt.LoginPath = $"/Account/Login&scheme={MicrosoftAccountDefaults.AuthenticationScheme}";
                opt.LogoutPath = "/Account/Logoff";

                opt.ExpireTimeSpan = new TimeSpan(0, 0, 10);
                opt.Cookie.SameSite = SameSiteMode.Lax;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");

                // TODO: Initialize values
                //opt.AccessDeniedPath = "/Account/AccessDenied";
            })
            .AddMicrosoftAccount(opt =>
            {
                MicrosoftSecurityOptions microsoftSecurityOptions = Configuration.GetMicrosoftSecurityOptions();
                opt.ClientId = microsoftSecurityOptions.ClientId ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.MicrosoftClientId).Build();
                opt.ClientSecret = microsoftSecurityOptions.ClientSecret ?? throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, SecurityConfigurationKeys.MicrosoftClientSecret).Build();
                opt.SignInScheme = Schemes.External;
                opt.CorrelationCookie.SameSite = SameSiteMode.Lax;
                opt.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
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
                opt.CorrelationCookie.SameSite = SameSiteMode.Lax;
                opt.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
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