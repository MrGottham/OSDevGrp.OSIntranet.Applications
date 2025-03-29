using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using OSDevGrp.OSIntranet.Bff.DomainServices;
using OSDevGrp.OSIntranet.Bff.ServiceGateways;
using OSDevGrp.OSIntranet.Bff.WebApi;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.SchemaValidation;
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
    options.ConsentCookie.Name = $"{ProgramHelper.GetNamespace()}.Consent";
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.Secure = CookieSecurePolicy.Always;
});

applicationBuilder.Services.ConfigureApplicationCookie(options => 
{
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Name = $"{ProgramHelper.GetNamespace()}.Application";
    options.DataProtectionProvider = DataProtectionProvider.Create(ProgramHelper.GetNamespace());
});

applicationBuilder.Services.AddAntiforgery(options => 
{
    options.FormFieldName = "__CSRF";
    options.HeaderName = $"X-{ProgramHelper.GetNamespace()}-CSRF-TOKEN";
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Name = $"{ProgramHelper.GetNamespace()}.Antiforgery";
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
    options.DefaultChallengeScheme = Schemes.Internal;
});
// TODO: Implement the correct authentication handlers.
authenticationBuilder.AddCookie(Schemes.Internal, options =>
{
    options.ExpireTimeSpan = new TimeSpan(0, 0, 10);
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Name = $"{ProgramHelper.GetNamespace()}.Authentication.{Schemes.Internal}";
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
    .AddServiceGatewayHealthCheck();

applicationBuilder.Services.AddHttpContextAccessor()
    .AddMemoryCache();

applicationBuilder.Services.AddScoped<IProblemDetailsFactory, ProblemDetailsFactory>()
    .AddScoped<ISchemaValidator, SchemaValidator>()
    .AddSingleton(TimeProvider.System)
    .AddSingleton<ITokenKeyGenerator, TokenKeyGenerator>()
    .Configure<TokenKeyProviderOptions>(_ => { })
    .AddSingleton<ITokenKeyProvider, TokenKeyProvider>()
    .AddSingleton<ITokenProvider, TokenProvider>()
    .AddSingleton<ITokenStorage, TokenStorage>()
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
