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
    public class ContactPolicyRegistrationTests
    {
        [Test]
        [Category("UnitTest")]
        public void ContactPolicy_WhenRegistered_UsesBearerAuthenticationScheme()
        {
            AuthorizationPolicy result = GetContactPolicy();

            Assert.That(result.AuthenticationSchemes, Does.Contain(JwtBearerDefaults.AuthenticationScheme));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactPolicy_WhenRegistered_RequiresAuthenticatedUser()
        {
            AuthorizationPolicy result = GetContactPolicy();

            Assert.That(result.Requirements.OfType<DenyAnonymousAuthorizationRequirement>().Any(), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ContactPolicy_WhenRegistered_RequiresContactsClaim()
        {
            AuthorizationPolicy result = GetContactPolicy();

            Assert.That(GetRequiredClaimTypes(result), Does.Contain(ClaimHelper.ContactsClaimType));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactPolicy_WhenRegistered_DoesNotRequireMicrosoftTokenClaim()
        {
            AuthorizationPolicy result = GetContactPolicy();

            Assert.That(GetRequiredClaimTypes(result), Does.Not.Contain(ClaimHelper.MicrosoftTokenClaimType));
        }

        private static AuthorizationPolicy GetContactPolicy()
        {
            IConfiguration configuration = new ConfigurationBuilder().Build();
            WebApiStartup sut = new WebApiStartup(configuration);
            ServiceCollection services = new ServiceCollection();

            sut.ConfigureServices(services);

            using ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<IOptions<AuthorizationOptions>>().Value.GetPolicy(Policies.ContactPolicy);
        }

        private static string[] GetRequiredClaimTypes(AuthorizationPolicy policy)
        {
            return policy.Requirements.OfType<ClaimsAuthorizationRequirement>().Select(requirement => requirement.ClaimType).ToArray();
        }
    }
}