using AutoFixture;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationState
{
    public abstract class AuthorizationStateTestBase
    {
        #region Private variables

        private static readonly Regex Base64Regex = new("^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        #endregion

        #region Methods

        protected static IAuthorizationState CreateSut(Fixture fixture, Random random, string responseType = null, string clientId = null, bool hasClientSecret = false, string clientSecret = null, Uri redirectUri = null, string[] scopes = null, bool hasExternalState = true, string externalState = null, bool hasAuthorizationCode = false, IAuthorizationCode authorizationCode = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(random, nameof(random));

            return new Domain.Security.AuthorizationState(
                responseType ?? fixture.Create<string>(),
                clientId ?? fixture.Create<string>(),
                hasClientSecret ? clientSecret ?? fixture.Create<string>() : null,
                redirectUri ?? CreateRedirectUri(fixture),
                scopes ?? CreateScopes(fixture, random),
                hasExternalState ? externalState ?? fixture.Create<string>() : null,
                hasAuthorizationCode ? authorizationCode ?? fixture.BuildAuthorizationCodeMock().Object : null);
        }

        protected static Uri CreateRedirectUri(Fixture fixture, IReadOnlyDictionary<string, string> queryParameters = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            string path = $"https://{fixture.Create<string>().Replace("/", string.Empty)}.local/{fixture.Create<string>().Replace("/", string.Empty)}";

            return queryParameters == null || queryParameters.Count == 0
                ? new Uri(path, UriKind.Absolute)
                : new Uri($"{path}?{string.Join("&", queryParameters.Select(queryParameter => $"{HttpUtility.UrlEncode(queryParameter.Key)}={HttpUtility.UrlEncode(queryParameter.Value)}").ToArray())}", UriKind.Absolute);
        }

        protected IReadOnlyDictionary<string, string> CreateQueryParameters(Fixture fixture, Random random)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(random, nameof(random));

            KeyValuePair<string, string>[] queryParameters = fixture.CreateMany<string>(random.Next(5, 10))
                .Select(name => CreateQueryParameter(fixture, name))
                .ToArray();

            return CreateQueryParameters(queryParameters);
        }

        protected IReadOnlyDictionary<string, string> CreateQueryParameters(params KeyValuePair<string, string>[] queryParameters)
        {
            NullGuard.NotNull(queryParameters, nameof(queryParameters));

            return queryParameters.ToDictionary().AsReadOnly();
        }

        protected KeyValuePair<string, string> CreateQueryParameter(Fixture fixture, string name = null, bool hasValue = true, string value = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return new KeyValuePair<string, string>(name ?? fixture.Create<string>(), hasValue ? value ?? fixture.Create<string>() : null);
        }

        protected static string[] CreateScopes(Fixture fixture, Random random)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(random, nameof(random));

            return fixture.CreateMany<string>(random.Next(5, 10)).ToArray();
        }

        protected static bool IsBase64String(string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return Base64Regex.IsMatch(value);
        }

        protected static bool HasMatchingResponseType(string base64, string expectedResponseType)
        {
            NullGuard.NotNullOrWhiteSpace(base64, nameof(base64))
                .NotNullOrWhiteSpace(expectedResponseType, nameof(expectedResponseType));

            return HasMatchingJsonProperty(Base64ToJson(base64), "ResponseType", expectedResponseType);
        }

        protected static bool HasMatchingClientId(string base64, string expectedClientId)
        {
            NullGuard.NotNullOrWhiteSpace(base64, nameof(base64))
                .NotNullOrWhiteSpace(expectedClientId, nameof(expectedClientId));

            return HasMatchingJsonProperty(Base64ToJson(base64), "ClientId", expectedClientId);
        }

        protected static bool HasMatchingClientSecret(string base64, string clientSecret)
        {
            NullGuard.NotNullOrWhiteSpace(base64, nameof(base64))
                .NotNullOrWhiteSpace(clientSecret, nameof(clientSecret));

            return HasMatchingJsonProperty(Base64ToJson(base64), "ClientSecret", clientSecret);
        }

        protected static bool HasClientSecretWithoutValue(string base64)
        {
            NullGuard.NotNullOrWhiteSpace(base64, nameof(base64));

            return HasJsonPropertyWithoutValue(Base64ToJson(base64), "ClientSecret");
        }

        protected static bool HasMatchingRedirectUri(string base64, Uri redirectUri)
        {
            NullGuard.NotNullOrWhiteSpace(base64, nameof(base64))
                .NotNull(redirectUri, nameof(redirectUri));

            return HasMatchingJsonProperty(Base64ToJson(base64), "RedirectUri", redirectUri.AbsoluteUri);
        }

        protected static bool HasMatchingScopes(string base64, string[] scopes)
        {
            NullGuard.NotNullOrWhiteSpace(base64, nameof(base64))
                .NotNull(scopes, nameof(scopes));

            return HasMatchingJsonProperty(Base64ToJson(base64), "Scopes", scopes);
        }

        protected static bool HasMatchingExternalState(string base64, string expectedExternalState)
        {
            NullGuard.NotNullOrWhiteSpace(base64, nameof(base64))
                .NotNullOrWhiteSpace(expectedExternalState, nameof(expectedExternalState));

            return HasMatchingJsonProperty(Base64ToJson(base64), "ExternalState", expectedExternalState);
        }

        protected static bool HasExternalStateWithoutValue(string base64)
        {
            NullGuard.NotNullOrWhiteSpace(base64, nameof(base64));

            return HasJsonPropertyWithoutValue(Base64ToJson(base64), "ExternalState");
        }

        protected static bool HasMatchingValueForAuthorizationCode(string base64, string value)
        {
            NullGuard.NotNullOrWhiteSpace(base64, nameof(base64))
                .NotNullOrWhiteSpace(value, nameof(value));

            return HasMatchingJsonProperty(ExtractJsonForProperty(Base64ToJson(base64), "AuthorizationCode"), "Value", value);
        }

        protected static bool HasMatchingExpiresForAuthorizationCode(string base64, DateTime expires)
        {
            NullGuard.NotNullOrWhiteSpace(base64, nameof(base64));

            return HasMatchingJsonProperty(ExtractJsonForProperty(Base64ToJson(base64), "AuthorizationCode"), "Expires", expires.ToUniversalTime());
        }

        protected static bool HasAuthorizationCodeWithoutValue(string base64)
        {
            NullGuard.NotNullOrWhiteSpace(base64, nameof(base64));

            return HasJsonPropertyWithoutValue(Base64ToJson(base64), "AuthorizationCode");
        }

        private static string Base64ToJson(string base64)
        {
            NullGuard.NotNullOrWhiteSpace(base64, nameof(base64));

            using MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(base64));
            using StreamReader streamReader = new StreamReader(memoryStream);

            return streamReader.ReadToEnd();
        }

        private static string ExtractJsonForProperty(string json, string propertyName)
        {
            NullGuard.NotNullOrWhiteSpace(json, nameof(json))
                .NotNullOrWhiteSpace(propertyName, nameof(propertyName));

            int pos = json.IndexOf($"\"{propertyName}\":{{", StringComparison.Ordinal);
            if (pos < 0)
            {
                return string.Empty;
            }

            string result = json[pos..];
            pos = result.IndexOf('}');

            return pos < 0 ? string.Empty : result[..(pos + 1)];
        }

        private static bool HasMatchingJsonProperty(string json, string propertyName, string propertyValue)
        {
            NullGuard.NotNullOrWhiteSpace(json, nameof(json))
                .NotNullOrWhiteSpace(propertyName, nameof(propertyName))
                .NotNullOrWhiteSpace(propertyValue, nameof(propertyValue));

            Regex propertyRegex = new Regex($"(\"{propertyName}\"\\s*:\\s*\"{propertyValue}\"){{1}}", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
            return propertyRegex.IsMatch(json);
        }

        private static bool HasMatchingJsonProperty(string json, string propertyName, DateTimeOffset propertyValue)
        {
            NullGuard.NotNullOrWhiteSpace(json, nameof(json))
                .NotNullOrWhiteSpace(propertyName, nameof(propertyName));

            Regex propertyRegex = new Regex($"(\"{propertyName}\"\\s*:\\s*\"([0-9]{{4}}-[0-9]{{2}}-[0-9]{{2}}T[0-9]{{2}}:[0-9]{{2}}:[0-9]{{2}}.[0-9]*\\+[0-9]{{2}}:[0-9]{{2}})\"){{1}}", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
            Match match = propertyRegex.Match(json);
            if (match.Success == false)
            {
                return false;
            }

            DateTimeOffset fromJson = DateTimeOffset.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture, DateTimeStyles.None);
            return fromJson >= propertyValue.AddMilliseconds(-10) && fromJson <= propertyValue.AddMilliseconds(10);
        }

        private static bool HasMatchingJsonProperty(string json, string propertyName, string[] propertyValues)
        {
            NullGuard.NotNullOrWhiteSpace(json, nameof(json))
                .NotNullOrWhiteSpace(propertyName, nameof(propertyName))
                .NotNull(propertyValues, nameof(propertyValues));

            string jsonArray = "\\[" + string.Join(',', propertyValues.Select(propertyValue => $"\\s*\"{propertyValue.Replace("-", "\\-")}\"\\s*").ToArray()) + "\\]";
            Regex propertyRegex = new Regex($"(\"{propertyName}\"\\s*:\\s*{jsonArray}){{1}}", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
            return propertyRegex.IsMatch(json);
        }

        private static bool HasJsonPropertyWithoutValue(string json, string propertyName)
        {
            NullGuard.NotNullOrWhiteSpace(json, nameof(json))
                .NotNullOrWhiteSpace(propertyName, nameof(propertyName));

            Regex propertyRegex = new Regex($"(\"{propertyName}\"\\s*:\\s*null){{1}}", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
            return propertyRegex.IsMatch(json);
        }

        #endregion
    }
}