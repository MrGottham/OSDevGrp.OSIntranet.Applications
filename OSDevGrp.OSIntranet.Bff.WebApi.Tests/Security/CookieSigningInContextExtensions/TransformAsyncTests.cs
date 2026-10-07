using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using System.Globalization;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.CookieSigningInContextExtensionsTests;

[TestFixture]
public class TransformAsyncTests
{
    #region Private variables

    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task TransformAsync_WhenTokenExpiresBeforeAuthenticationTicket_DoesNotShortenCookieExpiry()
    {
        DateTimeOffset authenticationExpires = DateTimeOffset.UtcNow.AddMinutes(_random!.Next(30, 61));
        DateTimeOffset tokenExpires = authenticationExpires.AddMinutes(-_random.Next(1, 20));
        AuthenticationProperties authenticationProperties = new()
        {
            ExpiresUtc = authenticationExpires
        };
        authenticationProperties.Items[".Token.expires_at"] = tokenExpires.ToString("O", CultureInfo.InvariantCulture);
        ClaimsPrincipal principal = new(new ClaimsIdentity([new Claim(ClaimTypes.Name, _fixture!.Create<string>())], "Test"));
        CookieOptions cookieOptions = new();
        CookieSigningInContext context = new(
            new DefaultHttpContext(),
            new AuthenticationScheme("TestScheme", "Test scheme", typeof(CookieAuthenticationHandler)),
            new CookieAuthenticationOptions(),
            principal,
            authenticationProperties,
            cookieOptions);

        await context.TransformAsync();

        Assert.That((cookieOptions.Expires!.Value - authenticationExpires.UtcDateTime).Duration(), Is.LessThan(TimeSpan.FromSeconds(1)));
        Assert.That(authenticationProperties.Items, Is.Empty);
    }
}