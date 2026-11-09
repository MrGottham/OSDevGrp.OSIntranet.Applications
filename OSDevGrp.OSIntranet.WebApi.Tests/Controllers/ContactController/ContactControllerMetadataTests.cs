using System.Linq;
using System.Reflection;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.Security;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.ContactController
{
    [TestFixture]
    public class ContactControllerMetadataTests
    {
        [Test]
        [Category("UnitTest")]
        public void ContactController_WhenInspected_HasRouteTemplateApiController()
        {
            RouteAttribute result = typeof(Controller).GetCustomAttributes(typeof(RouteAttribute), false).Cast<RouteAttribute>().SingleOrDefault();

            Assert.That(result?.Template, Is.EqualTo("api/[controller]"));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactController_WhenInspected_DeclaresApiVersionOne()
        {
            ApiVersionAttribute result = typeof(Controller).GetCustomAttributes(typeof(ApiVersionAttribute), false).Cast<ApiVersionAttribute>().SingleOrDefault();

            Assert.That(result?.Versions.Select(version => version.ToString()), Is.EqualTo(new[] { "1.0" }));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactController_WhenInspected_RequiresContactPolicy()
        {
            AuthorizeAttribute result = typeof(Controller).GetCustomAttributes(typeof(AuthorizeAttribute), false).Cast<AuthorizeAttribute>().SingleOrDefault();

            Assert.That(result?.Policy, Is.EqualTo(Policies.ContactPolicy));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactController_WhenInspected_HasApiControllerAttribute()
        {
            ApiControllerAttribute result = typeof(Controller).GetCustomAttributes(typeof(ApiControllerAttribute), false).Cast<ApiControllerAttribute>().SingleOrDefault();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ContactController_WhenInspected_DerivesFromControllerBase()
        {
            bool result = typeof(ControllerBase).IsAssignableFrom(typeof(Controller));

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ContactController_WhenInspected_DeclaresNoPublicInstanceMethods()
        {
            int result = typeof(Controller).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length;

            Assert.That(result, Is.EqualTo(0));
        }
    }
}