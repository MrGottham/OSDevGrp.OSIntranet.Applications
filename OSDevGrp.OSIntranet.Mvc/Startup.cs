using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.BusinessLogic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Mvc.Controllers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Resolvers;
using OSDevGrp.OSIntranet.Repositories;

namespace OSDevGrp.OSIntranet.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(opt =>
            {
                opt.CheckConsentNeeded = context => true;
                opt.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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
                })
                .AddCookie("OSDevGrp.OSIntranet.External", opt =>
                {
                    opt.LoginPath = "/Account/Login";
                    opt.LogoutPath = "/Account/Logoff";
                    opt.ExpireTimeSpan = new TimeSpan(0, 0, 10);
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
                        int seconds = o.ExpiresIn?.Seconds ?? 0;
                        IRefreshableToken refreshableToken = new RefreshableToken(o.TokenType, o.AccessToken, o.RefreshToken, DateTime.UtcNow.AddSeconds(seconds));
                        AccountController.StoreMicrosoftGraphToken(o.HttpContext, refreshableToken);
                        return Task.CompletedTask;
                    };
                })
                .AddGoogle(opt =>
                {
                    opt.ClientId = Configuration["Security:Google:ClientId"];
                    opt.ClientSecret = Configuration["Security:Google:ClientSecret"];
                    opt.SignInScheme = "OSDevGrp.OSIntranet.External";
                });
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("SecurityAdmin", policy => policy.RequireClaim(ClaimHelper.SecurityAdminClaimType));
                opt.AddPolicy("Accounting", policy => policy.RequireClaim(ClaimHelper.AccountingClaimType));
                opt.AddPolicy("CommonData", policy => policy.RequireClaim(ClaimHelper.CommonDataClaimType));
                opt.AddPolicy("Contacts", policy => policy.RequireClaim(ClaimHelper.ContactsClaimType));
            });

            services.AddCommandBus().AddCommandHandlers(typeof(AuthenticateCommandHandlerBase<,>).Assembly);
            services.AddQueryBus().AddQueryHandlers(typeof(AuthenticateCommandHandlerBase<,>).Assembly);
            services.AddRepositories();
            services.AddBusinessLogicValidators();
            services.AddBusinessLogicHelpers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipalResolver, PrincipalResolver>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRequestLocalization(options => 
            {
                options.AddSupportedCultures("da-DK", "da");
                options.AddSupportedUICultures("da-DK", "da");
                options.SetDefaultCulture("da-DK");
            });
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
