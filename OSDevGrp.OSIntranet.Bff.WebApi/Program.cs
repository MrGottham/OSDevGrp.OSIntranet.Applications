using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OSDevGrp.OSIntranet.Bff.DomainServices;
using OSDevGrp.OSIntranet.Bff.ServiceGateways;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.SchemaValidation;
using OSDevGrp.OSIntranet.Bff.WebApi.Options;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using System.Security.Claims;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);
applicationBuilder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

applicationBuilder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

applicationBuilder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = _ => false;
    options.ConsentCookie.Name = ProgramHelper.GetConsentCookieName();
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.Secure = CookieSecurePolicy.Always;
});

applicationBuilder.Services.ConfigureApplicationCookie(options => 
{
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Name = ProgramHelper.GetApplicationCookieName();
    options.DataProtectionProvider = DataProtectionProvider.Create(ProgramHelper.GetNamespace());
});

applicationBuilder.Services.AddAntiforgery(options => 
{
    options.FormFieldName = "__CSRF";
    options.HeaderName = $"X-{ProgramHelper.GetNamespace()}-CSRF-TOKEN";
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Name = ProgramHelper.GetAntiforgeryCookieName();
});

applicationBuilder.Services.AddDataProtection()
    .SetApplicationName(ProgramHelper.GetNamespace())
    .UseEphemeralDataProtectionProvider()
    .SetDefaultKeyLifetime(new TimeSpan(30, 0, 0, 0));

applicationBuilder.Services.AddCors();

applicationBuilder.Services.AddRazorPages();
applicationBuilder.Services.AddControllers(options => 
{
    options.Filters.Add<ErrorHandlerFilter>();
    options.Filters.Add<SchemaValidationFilter>();
});

applicationBuilder.Services.AddProblemDetails();

AuthenticationBuilder authenticationBuilder = applicationBuilder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = Schemes.Internal;
    options.DefaultSignInScheme = Schemes.Internal;
    options.DefaultSignOutScheme = Schemes.Internal;
    options.DefaultAuthenticateScheme = Schemes.Internal;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
});
authenticationBuilder.AddCookie(Schemes.Internal, options =>
{
    options.ReturnUrlParameter = ProgramHelper.GetReturnUrlParameter();
    options.LoginPath = ProgramHelper.GetLoginPath();
    options.LogoutPath = ProgramHelper.GetLogoutPath();
    options.AccessDeniedPath = ProgramHelper.GetAccessDeniedPath();
    options.ExpireTimeSpan = new TimeSpan(0, 60, 0);
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Name = ProgramHelper.GetInternalAuthenticationCookieName();
    options.Events.OnSigningOut += async context =>
    {
        ITokenStorage tokenStorage = context.HttpContext.RequestServices.GetRequiredService<ITokenStorage>();
        await tokenStorage.DeleteTokenAsync(context.HttpContext.User, context.HttpContext.RequestAborted);

        context.Response.Cookies.Delete(ProgramHelper.GetApplicationCookieName());
        context.Response.Cookies.Delete(ProgramHelper.GetAntiforgeryCookieName());
        context.Response.Cookies.Delete(ProgramHelper.GetInternalAuthenticationCookieName());
        context.Response.Cookies.Delete(ProgramHelper.GetOpenIdConnectAuthenticationCookieName());
    };
    options.DataProtectionProvider = DataProtectionProvider.Create(ProgramHelper.GetNamespace());
});
authenticationBuilder.AddOpenIdConnect(options =>
{
    OSDevGrp.OSIntranet.Bff.WebApi.Options.OpenIdConnectOptions openIdConnectOptions = applicationBuilder.Configuration.GetOpenIdConnectOptions() ?? throw new InvalidOperationException($"Configuration was not provided for {nameof(OSDevGrp.OSIntranet.Bff.WebApi.Options.OpenIdConnectOptions)}.");
    options.Authority = openIdConnectOptions.Authority ?? throw new InvalidOperationException($"{nameof(openIdConnectOptions.Authority)} was not given in the configuration for {nameof(openIdConnectOptions)}.");
    options.ClientId = openIdConnectOptions.ClientId ?? throw new InvalidOperationException($"{nameof(openIdConnectOptions.ClientId)} was not given in the configuration for {nameof(openIdConnectOptions)}.");
    options.ClientSecret = openIdConnectOptions.ClientSecret ?? throw new InvalidOperationException($"{nameof(openIdConnectOptions.ClientSecret)} was not given in the configuration for {nameof(openIdConnectOptions)}.");
    options.SignInScheme = Schemes.Internal;
    options.SignOutScheme = Schemes.Internal;
    options.AccessDeniedPath = ProgramHelper.GetAccessDeniedPath();
    options.CorrelationCookie.SameSite = SameSiteMode.Lax;
    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
    options.CorrelationCookie.Name = ProgramHelper.GetOpenApiDocumentName();
    options.SaveTokens = true;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.UsePkce = true;
    options.UseTokenLifetime = true;
    options.GetClaimsFromUserInfoEndpoint = true; 
    options.MapInboundClaims = true;
    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("email");
    options.Scope.Add("profile");
    options.Scope.Add("webapi");
    options.ClaimActions.Add(new DeleteClaimAction("name"));
    options.ClaimActions.Add(new DeleteClaimAction("auth_time"));
    options.Events.OnTokenValidated += async context =>
    {
        TimeProvider timeProvider = context.HttpContext.RequestServices.GetRequiredService<TimeProvider>();

        string tokenType = context.TokenEndpointResponse!.TokenType;
        string accessToken = context.TokenEndpointResponse!.AccessToken;
        DateTimeOffset expires = new DateTimeOffset(context.SecurityToken.ValidTo, TimeSpan.Zero);
        IToken token = new LocalToken(tokenType, accessToken, expires, timeProvider);

        ITokenStorage tokenStorage = context.HttpContext.RequestServices.GetRequiredService<ITokenStorage>();
        await tokenStorage.StoreTokenAsync(context.Principal!, token, context.HttpContext.RequestAborted);
    };
    options.Events.OnRedirectToIdentityProviderForSignOut = context =>
    {
        context.Response.Redirect(context.Properties.RedirectUri!);
        context.HandleResponse();

        return Task.CompletedTask;
    };
    options.DataProtectionProvider = DataProtectionProvider.Create(ProgramHelper.GetNamespace());
});

applicationBuilder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.AuthenticatedUser, policy =>
    {
        policy.AddAuthenticationSchemes(Schemes.Internal);
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.NameIdentifier);
        policy.RequireClaim(ClaimTypes.Name);
        policy.RequireClaim(ClaimTypes.Email);
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
applicationBuilder.Services.AddOpenApi(ProgramHelper.GetOpenApiDocumentName(), options =>
{
    options.AddDocumentTransformer((document, _, _) => 
    {
        document.Info = new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = ProgramHelper.GetTitle(),
            Version = "v1",
            Description = ProgramHelper.GetDescription()
        };
        return Task.CompletedTask;
    });
});

applicationBuilder.Services.AddHealthChecks()
    .AddServiceGatewayHealthCheck()
    .AddCheck<OpenIdConnectOptionsHealthCheck>(nameof(OpenIdConnectOptionsHealthCheck))
    .AddCheck<TrustedDomainOptionsHealthCheck>(nameof(TrustedDomainOptionsHealthCheck));

applicationBuilder.Services.AddHttpContextAccessor()
    .AddMemoryCache();

applicationBuilder.Services.Configure<OSDevGrp.OSIntranet.Bff.WebApi.Options.OpenIdConnectOptions>(applicationBuilder.Configuration.GetOpenIdConnectSection())
    .Configure<TrustedDomainOptions>(applicationBuilder.Configuration.GetTrustedDomainSection())
    .AddScoped<IProblemDetailsFactory, ProblemDetailsFactory>()
    .AddScoped<ISchemaValidator, SchemaValidator>()
    .AddSingleton(TimeProvider.System)
    .AddSingleton<ITokenKeyGenerator, TokenKeyGenerator>()
    .Configure<TokenKeyProviderOptions>(_ => { })
    .AddSingleton<ITokenKeyProvider, TokenKeyProvider>()
    .AddSingleton<ITokenProvider, TokenProvider>()
    .AddSingleton<ITokenStorage, TokenStorage>()
    .AddTransient<ITrustedDomainResolver, TrustedDomainResolver>()
    .AddServiceGateways<SecurityContextProvider>(applicationBuilder.Configuration)
    .AddDomainServices();

WebApplication webApplication = applicationBuilder.Build();

webApplication.UseForwardedHeaders();

webApplication.UseExceptionHandler();
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseDeveloperExceptionPage();
}
else
{
    webApplication.UseHsts();
}

Uri? openApiDocumentUrl = @ProgramHelper.GetOpenApiDocumentUrl(webApplication.Environment);
if (openApiDocumentUrl != null)
{
    webApplication.MapOpenApi();
}

if (ProgramHelper.RunningInDocker() == false)
{
    webApplication.UseHttpsRedirection();
}

webApplication.UseCookiePolicy();

webApplication.UseCors("default");

webApplication.UseAuthentication();
webApplication.UseAuthorization();

webApplication.MapDefaultControllerRoute().WithStaticAssets();
webApplication.MapRazorPages().WithStaticAssets();

webApplication.MapHealthChecks("/health");
webApplication.MapHealthChecks("/api/health");

webApplication.Run();
