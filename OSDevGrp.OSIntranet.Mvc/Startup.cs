using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OSDevGrp.OSIntranet.BusinessLogic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Resolvers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Filters;
using OSDevGrp.OSIntranet.Repositories;

namespace OSDevGrp.OSIntranet.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            NullGuard.NotNull(services, nameof(services));

            services.Configure<CookiePolicyOptions>(opt =>
            {
                opt.CheckConsentNeeded = context => true;
                opt.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews(opt => opt.Filters.Add(typeof(AcquireTokenActionFilter)));
            services.AddRazorPages();

            services.AddDataProtection()
                .SetApplicationName("OSDevGrp.OSIntranet.Mvc")
                .SetDefaultKeyLifetime(new TimeSpan(30, 0, 0, 0));

            services.AddAuthentication(opt => 
            {
                opt.DefaultScheme = "OSDevGrp.OSIntranet.Internal";
                opt.DefaultSignInScheme = "OSDevGrp.OSIntranet.External";
            })
            .AddCookie("OSDevGrp.OSIntranet.Internal", opt => 
            {
                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logoff";
                opt.ExpireTimeSpan = new TimeSpan(0, 60, 0);
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            })
            .AddCookie("OSDevGrp.OSIntranet.External", opt =>
            {
                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logoff";
                opt.ExpireTimeSpan = new TimeSpan(0, 0, 10);
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            })
            .AddMicrosoftAccount(opt => 
            {
                opt.ClientId = Configuration["Security:Microsoft:ClientId"];
                opt.ClientSecret = Configuration["Security:Microsoft:ClientSecret"];
                opt.SignInScheme = "OSDevGrp.OSIntranet.External";
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
                opt.ClientId = Configuration["Security:Google:ClientId"];
                opt.ClientSecret = Configuration["Security:Google:ClientSecret"];
                opt.SignInScheme = "OSDevGrp.OSIntranet.External";
                opt.DataProtectionProvider = DataProtectionProvider.Create("OSDevGrp.OSIntranet.Mvc");
            });
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("SecurityAdmin", policy => policy.RequireClaim(ClaimHelper.SecurityAdminClaimType));
                opt.AddPolicy("Accounting", policy => policy.RequireClaim(ClaimHelper.AccountingClaimType));
                opt.AddPolicy("CommonData", policy => policy.RequireClaim(ClaimHelper.CommonDataClaimType));
                opt.AddPolicy("Contacts", policy => policy.RequireClaim(ClaimHelper.ContactsClaimType));
            });

            services.AddHealthChecks();

            services.AddCommandBus().AddCommandHandlers(typeof(AuthenticateCommandHandlerBase<,>).Assembly);
            services.AddQueryBus().AddQueryHandlers(typeof(AuthenticateCommandHandlerBase<,>).Assembly);
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRequestLocalization(options => 
            {
                options.AddSupportedCultures("da-DK", "da");
                options.AddSupportedUICultures("da-DK", "da");
                options.SetDefaultCulture("da-DK");
            });

            app.UseCookiePolicy();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

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
    }
}