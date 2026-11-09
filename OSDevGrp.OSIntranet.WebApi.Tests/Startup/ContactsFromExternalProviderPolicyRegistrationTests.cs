using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.WebApi.Security;
using WebApiStartup = OSDevGrp.OSIntranet.WebApi.Startup;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Startup
{
    [TestFixture]
    public class ContactsFromExternalProviderPolicyRegistrationTests
    {
        [Test]
        [Category("UnitTest")]
        public void ContactsFromExternalProviderPolicy_WhenRegistered_UsesBearerAuthenticationScheme()
        {
            AuthorizationPolicy result = GetContactsFromExternalProviderPolicy();

            Assert.That(result.AuthenticationSchemes, Does.Contain(JwtBearerDefaults.AuthenticationScheme));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactsFromExternalProviderPolicy_WhenRegistered_RequiresAuthenticatedUser()
        {
            AuthorizationPolicy result = GetContactsFromExternalProviderPolicy();

            Assert.That(result.Requirements.OfType<DenyAnonymousAuthorizationRequirement>().Any(), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ContactsFromExternalProviderPolicy_WhenRegistered_RequiresContactsClaim()
        {
            AuthorizationPolicy result = GetContactsFromExternalProviderPolicy();

            Assert.That(GetRequiredClaimTypes(result), Does.Contain(ClaimHelper.ContactsClaimType));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactsFromExternalProviderPolicy_WhenRegistered_RequiresMicrosoftTokenClaim()
        {
            AuthorizationPolicy result = GetContactsFromExternalProviderPolicy();

            Assert.That(GetRequiredClaimTypes(result), Does.Contain(ClaimHelper.MicrosoftTokenClaimType));
        }

        private static AuthorizationPolicy GetContactsFromExternalProviderPolicy()
        {
            IConfiguration configuration = new ConfigurationBuilder().Build();
            WebApiStartup sut = new WebApiStartup(configuration);
            ServiceCollection services = new ServiceCollection();

            sut.ConfigureServices(services);

            using ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<IOptions<AuthorizationOptions>>().Value.GetPolicy(Policies.ContactsFromExternalProviderPolicy);
        }

        private static string[] GetRequiredClaimTypes(AuthorizationPolicy policy)
        {
            return policy.Requirements.OfType<ClaimsAuthorizationRequirement>().Select(requirement => requirement.ClaimType).ToArray();
        }
    }
}