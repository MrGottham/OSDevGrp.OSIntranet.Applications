using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.Security;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.ContactController;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

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
        public void ContactController_WhenInspected_DeclaresContactLookupActions()
        {
            string[] result = typeof(Controller).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Select(method => method.Name)
                .Where(methodName => methodName.EndsWith("Async"))
                .OrderBy(methodName => methodName)
                .ToArray();

            Assert.That(result, Is.EqualTo(new[] { "ContactAsync", "ContactGroupsAsync", "ContactsAsync", "ContactsWithBirthdayAsync", "CountriesAsync", "PaymentTermsAsync", "PostalCodesAsync" }));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactGroupsAsync_WhenInspected_HasRouteTemplate()
        {
            HttpGetAttribute result = typeof(Controller).GetMethod(nameof(Controller.ContactGroupsAsync))
                .GetCustomAttributes(typeof(HttpGetAttribute), false).Cast<HttpGetAttribute>().SingleOrDefault();

            Assert.That(result?.Template, Is.EqualTo("contactgroups"));
        }

        [Test]
        [Category("UnitTest")]
        public void CountriesAsync_WhenInspected_HasRouteTemplate()
        {
            HttpGetAttribute result = typeof(Controller).GetMethod(nameof(Controller.CountriesAsync))
                .GetCustomAttributes(typeof(HttpGetAttribute), false).Cast<HttpGetAttribute>().SingleOrDefault();

            Assert.That(result?.Template, Is.EqualTo("countries"));
        }

        [Test]
        [Category("UnitTest")]
        public void PaymentTermsAsync_WhenInspected_HasRouteTemplate()
        {
            HttpGetAttribute result = typeof(Controller).GetMethod(nameof(Controller.PaymentTermsAsync))
                .GetCustomAttributes(typeof(HttpGetAttribute), false).Cast<HttpGetAttribute>().SingleOrDefault();

            Assert.That(result?.Template, Is.EqualTo("paymentterms"));
        }

        [Test]
        [Category("UnitTest")]
        public void PostalCodesAsync_WhenInspected_HasRouteTemplate()
        {
            HttpGetAttribute result = typeof(Controller).GetMethod(nameof(Controller.PostalCodesAsync))
                .GetCustomAttributes(typeof(HttpGetAttribute), false).Cast<HttpGetAttribute>().SingleOrDefault();

            Assert.That(result?.Template, Is.EqualTo("countries/{countryCode}/postalcodes"));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactsAsync_WhenInspected_RequiresContactsFromExternalProviderPolicy()
        {
            AuthorizeAttribute result = typeof(Controller).GetMethod(nameof(Controller.ContactsAsync))
                .GetCustomAttributes(typeof(AuthorizeAttribute), false).Cast<AuthorizeAttribute>().SingleOrDefault();

            Assert.That(result?.Policy, Is.EqualTo(Policies.ContactsFromExternalProviderPolicy));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAsync_WhenInspected_HasRouteTemplate()
        {
            HttpGetAttribute result = typeof(Controller).GetMethod(nameof(Controller.ContactAsync))
                .GetCustomAttributes(typeof(HttpGetAttribute), false).Cast<HttpGetAttribute>().SingleOrDefault();

            Assert.That(result?.Template, Is.EqualTo("{externalIdentifier}"));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAsync_WhenInspected_ExternalIdentifierIsRequired()
        {
            RequiredAttribute result = GetParameter(nameof(Controller.ContactAsync), "externalIdentifier")
                .GetCustomAttributes(typeof(RequiredAttribute), false).Cast<RequiredAttribute>().SingleOrDefault();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAsync_WhenInspected_ExternalIdentifierHasMaximumLengthOf256()
        {
            StringLengthAttribute result = GetParameter(nameof(Controller.ContactAsync), "externalIdentifier")
                .GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>().SingleOrDefault();

            Assert.That(result?.MaximumLength, Is.EqualTo(256));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAsync_WhenInspected_ExternalIdentifierHasMinimumLengthOf1()
        {
            StringLengthAttribute result = GetParameter(nameof(Controller.ContactAsync), "externalIdentifier")
                .GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>().SingleOrDefault();

            Assert.That(result?.MinimumLength, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactsWithBirthdayAsync_WhenInspected_HasRouteTemplate()
        {
            HttpGetAttribute result = typeof(Controller).GetMethod(nameof(Controller.ContactsWithBirthdayAsync))
                .GetCustomAttributes(typeof(HttpGetAttribute), false).Cast<HttpGetAttribute>().SingleOrDefault();

            Assert.That(result?.Template, Is.EqualTo("withbirthdays"));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactsWithBirthdayAsync_WhenInspected_WithinDaysIsRequired()
        {
            RequiredAttribute result = GetParameter(nameof(Controller.ContactsWithBirthdayAsync), "withinDays")
                .GetCustomAttributes(typeof(RequiredAttribute), false).Cast<RequiredAttribute>().SingleOrDefault();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ContactsWithBirthdayAsync_WhenInspected_WithinDaysHasMinimumOfZero()
        {
            RangeAttribute result = GetParameter(nameof(Controller.ContactsWithBirthdayAsync), "withinDays")
                .GetCustomAttributes(typeof(RangeAttribute), false).Cast<RangeAttribute>().SingleOrDefault();

            Assert.That(result?.Minimum, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactsWithBirthdayAsync_WhenInspected_WithinDaysHasMaximumOf365()
        {
            RangeAttribute result = GetParameter(nameof(Controller.ContactsWithBirthdayAsync), "withinDays")
                .GetCustomAttributes(typeof(RangeAttribute), false).Cast<RangeAttribute>().SingleOrDefault();

            Assert.That(result?.Maximum, Is.EqualTo(365));
        }

        private static ParameterInfo GetParameter(string methodName, string parameterName)
        {
            return typeof(Controller).GetMethod(methodName).GetParameters().Single(parameter => parameter.Name == parameterName);
        }
    }
}