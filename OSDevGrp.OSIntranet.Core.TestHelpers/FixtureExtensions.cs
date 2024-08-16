using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace OSDevGrp.OSIntranet.Core.TestHelpers
{
    public static class FixtureExtensions
    {
        #region Methods

        public static string CreateDomainName(this Fixture fixture, string domainName = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return WebUtility.UrlEncode(domainName ?? $"{fixture.Create<string>()}.local");
        }

        public static KeyValuePair<string, string> CreateQueryParameter(this Fixture fixture, string name = null, bool hasValue = true, string value = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return new KeyValuePair<string, string>(name ?? fixture.Create<string>(), hasValue ? value ?? fixture.Create<string>() : null);
        }

        public static KeyValuePair<string, string>[] CreateQueryParameters(this Fixture fixture, Random random)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(random, nameof(random));

            return fixture.CreateMany<string>(random.Next(5, 10))
                .Select(name => fixture.CreateQueryParameter(name: name))
                .ToArray();
        }

        public static string CreateEndpointPathAndQuery(this Fixture fixture, string path = null, params KeyValuePair<string, string>[] queryParameters)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(queryParameters, nameof(queryParameters));

            StringBuilder pathAndQueryBuilder = new StringBuilder(EncodePath(path ?? fixture.Create<string>()));
            if (queryParameters.Length > 0)
            {
                pathAndQueryBuilder.Append($"?{string.Join('&', queryParameters.Select(queryParameter => $"{WebUtility.UrlEncode(queryParameter.Key)}={WebUtility.UrlEncode(queryParameter.Value)}"))}");
            }

            return pathAndQueryBuilder.ToString();
        }

        public static string CreateEndpointString(this Fixture fixture, string domainName = null, bool withoutPathAndQuery = false, string path = null, params KeyValuePair<string, string>[] queryParameters)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(queryParameters, nameof(queryParameters));

            StringBuilder endpointStringBuilder = new StringBuilder($"https://{fixture.CreateDomainName(domainName)}");
            if (withoutPathAndQuery == false)
            {
                endpointStringBuilder.Append($"/{fixture.CreateEndpointPathAndQuery(path, queryParameters)}");
            }

            return ValidateEndpointString(endpointStringBuilder.ToString(), UriKind.Absolute);
        }

        public static string CreateRelativeEndpointString(this Fixture fixture, string path = null, params KeyValuePair<string, string>[] queryParameters)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(queryParameters, nameof(queryParameters));

            return ValidateEndpointString($"/{fixture.CreateEndpointPathAndQuery(path, queryParameters)}", UriKind.Relative);
        }

        public static Uri CreateEndpoint(this Fixture fixture, string domainName = null, bool withoutPathAndQuery = false, string path = null, params KeyValuePair<string, string>[] queryParameters)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(queryParameters, nameof(queryParameters));

            return new Uri(fixture.CreateEndpointString(domainName, withoutPathAndQuery, path, queryParameters), UriKind.Absolute);
        }

        public static Uri CreateRelativeEndpoint(this Fixture fixture, string path = null, params KeyValuePair<string, string>[] queryParameters)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(queryParameters, nameof(queryParameters));

            return new Uri(fixture.CreateRelativeEndpointString(path, queryParameters), UriKind.Relative);
        }

        private static string EncodePath(string path)
        {
            NullGuard.NotNullOrWhiteSpace(path, nameof(path));

            return string.Join('/', path.Split('/').Select(WebUtility.UrlEncode));
        }

        private static string ValidateEndpointString(string endpointString, UriKind uriKind)
        {
            NullGuard.NotNullOrWhiteSpace(endpointString, nameof(endpointString));

            if (Uri.TryCreate(endpointString, uriKind, out Uri _) == false)
            {
                throw new NotSupportedException($"Unable to create an uri ({uriKind}) for the value: {endpointString}");
            }

            return endpointString;
        }

        #endregion
    }
}