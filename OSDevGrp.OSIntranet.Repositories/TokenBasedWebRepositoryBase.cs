using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Repositories
{
    public abstract class TokenBasedWebRepositoryBase<TToken> : WebRepositoryBase where TToken : IToken
    {
        #region Constructor

        protected TokenBasedWebRepositoryBase(IConfiguration configuration, IPrincipalResolver principalResolver, ILoggerFactory loggerFactory)
            : base(configuration, principalResolver, loggerFactory)
        {
        }

        #endregion

        #region Properties

        protected abstract string AuthorizeUrl { get; }

        protected abstract string TokenUrl { get; }

        protected virtual string RefreshTokenUrl => TokenUrl;

        protected abstract TToken Token { get; set; }

        #endregion

        #region Methods

        protected virtual IDictionary<string, string> GetParametersToAuthorize(Uri redirectUri, string scope, string state)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNullOrWhiteSpace(scope, nameof(scope))
                .NotNullOrWhiteSpace(state, nameof(state));

            return new Dictionary<string, string>();
        }

        protected virtual Task<Uri> GetAuthorizeUriAsync(Uri redirectUri, string scope, string state)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNullOrWhiteSpace(scope, nameof(scope))
                .NotNullOrWhiteSpace(state, nameof(state));

            return Task.Run(() =>
            {
                IDictionary<string, string> parametersToAuthorize = GetParametersToAuthorize(redirectUri, scope, state);
                if (parametersToAuthorize.Count == 0)
                {
                    return new Uri($"{AuthorizeUrl}");
                }

                string[] parameterArray = parametersToAuthorize.Select(keyValuePair => $"{keyValuePair.Key}={HttpUtility.UrlEncode(keyValuePair.Value)}").ToArray();
                return new Uri($"{AuthorizeUrl}?{string.Join("&", parameterArray)}");
            });
        }

        protected abstract void SetupRequestToAcquireToken(HttpRequestMessage httpRequestMessage, Uri redirectUri, string scope, string code);

        protected virtual Task<TTokenModel> AcquireTokenAsync<TTokenModel>(Uri redirectUri, string scope, string code) where TTokenModel : class
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNullOrWhiteSpace(scope, nameof(scope))
                .NotNullOrWhiteSpace(code, nameof(code));

            return base.PostAsync<TTokenModel>(new Uri(TokenUrl), httpRequestMessage => SetupRequestToAcquireToken(httpRequestMessage, redirectUri, scope, code));
        }

        protected abstract void SetupRequestToRefreshToken(HttpRequestMessage httpRequestMessage, Uri redirectUri, string scope, TToken token);

        protected virtual Task<TTokenModel> RefreshTokenAsync<TTokenModel>(Uri redirectUri, string scope, TToken token) where TTokenModel : class
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNullOrWhiteSpace(scope, nameof(scope))
                .NotNull(token, nameof(token));

            return base.PostAsync<TTokenModel>(new Uri(RefreshTokenUrl), httpRequestMessage => SetupRequestToRefreshToken(httpRequestMessage, redirectUri, scope, token));
        }

        protected abstract void SetupAuthenticationForRequest(HttpRequestMessage httpRequestMessage, TToken token);

        protected override async Task<TResult> GetAsync<TResult>(Uri requestUri, Action<HttpRequestMessage> httpRequestMessageCallback = null, DataContractJsonSerializerSettings serializerSettings = null)
        {
            NullGuard.NotNull(requestUri, nameof(requestUri));

            ValidatorToken();

            return await base.GetAsync<TResult>(
                requestUri,
                httpRequestMessage =>
                {
                    NullGuard.NotNull(httpRequestMessage, nameof(httpRequestMessage));

                    SetupAuthenticationForRequest(httpRequestMessage, Token);

                    if (httpRequestMessageCallback == null)
                    {
                        return;
                    }

                    httpRequestMessageCallback(httpRequestMessage);
                },
                serializerSettings);
        }

        protected override async Task<TResult> PostAsync<TResult>(Uri requestUri, Action<HttpRequestMessage> httpRequestMessageCallback = null, DataContractJsonSerializerSettings serializerSettings = null)
        {
            NullGuard.NotNull(requestUri, nameof(requestUri));

            ValidatorToken();

            return await base.PostAsync<TResult>(
                requestUri,
                httpRequestMessage =>
                {
                    NullGuard.NotNull(httpRequestMessage, nameof(httpRequestMessage));

                    SetupAuthenticationForRequest(httpRequestMessage, Token);

                    if (httpRequestMessageCallback == null)
                    {
                        return;
                    }

                    httpRequestMessageCallback(httpRequestMessage);
                },
                serializerSettings);
        }

        protected override async Task<TResult> PatchAsync<TResult>(Uri requestUri, Action<HttpRequestMessage> httpRequestMessageCallback = null, DataContractJsonSerializerSettings serializerSettings = null)
        {
            NullGuard.NotNull(requestUri, nameof(requestUri));

            ValidatorToken();

            return await base.PatchAsync<TResult>(
                requestUri,
                httpRequestMessage =>
                {
                    NullGuard.NotNull(httpRequestMessage, nameof(httpRequestMessage));

                    SetupAuthenticationForRequest(httpRequestMessage, Token);

                    if (httpRequestMessageCallback == null)
                    {
                        return;
                    }

                    httpRequestMessageCallback(httpRequestMessage);
                },
                serializerSettings);
        }

        protected override async Task DeleteAsync(Uri requestUri, Action<HttpRequestMessage> httpRequestMessageCallback = null)
        {
            NullGuard.NotNull(requestUri, nameof(requestUri));

            ValidatorToken();

            await base.DeleteAsync(
                requestUri,
                httpRequestMessage =>
                {
                    NullGuard.NotNull(httpRequestMessage, nameof(httpRequestMessage));

                    SetupAuthenticationForRequest(httpRequestMessage, Token);

                    if (httpRequestMessageCallback == null)
                    {
                        return;
                    }

                    httpRequestMessageCallback(httpRequestMessage);
                });
        }

        private void ValidatorToken()
        {
            if (Token == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.NoTokenHasBeenAcquired, GetType().Name)
                    .WithMethodBase(MethodBase.GetCurrentMethod())
                    .Build();
            }

            if (Token.Expires < DateTime.UtcNow)
            {
                throw new IntranetExceptionBuilder(ErrorCode.TokenHasExpired, GetType().Name)
                    .WithMethodBase(MethodBase.GetCurrentMethod())
                    .Build();
            }
        }

        #endregion
    }
}
