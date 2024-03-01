using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories
{
    internal abstract class WebRepositoryBase(ILoggerFactory loggerFactory) : RepositoryBase(loggerFactory)
    {
        #region Properties

        protected virtual SslProtocols EnforceSslProtocol => SslProtocols.Tls12;

        #endregion

        #region Methods

        protected virtual Task<TResult> GetAsync<TResult>(Uri requestUri, Action<HttpRequestMessage> httpRequestMessageCallback = null, DataContractJsonSerializerSettings serializerSettings = null) where TResult : class
        {
            NullGuard.NotNull(requestUri, nameof(requestUri));

            return ExecuteAsync<TResult>(requestUri, uri => new HttpRequestMessage(HttpMethod.Get, uri), MethodBase.GetCurrentMethod(), httpRequestMessageCallback, serializerSettings);
        }

        protected virtual Task<TResult> PostAsync<TResult>(Uri requestUri, Action<HttpRequestMessage> httpRequestMessageCallback = null, DataContractJsonSerializerSettings serializerSettings = null) where TResult : class
        {
            NullGuard.NotNull(requestUri, nameof(requestUri));

            return ExecuteAsync<TResult>(requestUri, uri => new HttpRequestMessage(HttpMethod.Post, uri), MethodBase.GetCurrentMethod(), httpRequestMessageCallback, serializerSettings);
        }

        protected virtual Task<TResult> PatchAsync<TResult>(Uri requestUri, Action<HttpRequestMessage> httpRequestMessageCallback = null, DataContractJsonSerializerSettings serializerSettings = null) where TResult : class
        {
            NullGuard.NotNull(requestUri, nameof(requestUri));

            return ExecuteAsync<TResult>(requestUri, uri => new HttpRequestMessage(new HttpMethod("PATCH"), uri), MethodBase.GetCurrentMethod(), httpRequestMessageCallback, serializerSettings);
        }

        protected virtual Task DeleteAsync(Uri requestUri, Action<HttpRequestMessage> httpRequestMessageCallback = null)
        {
            NullGuard.NotNull(requestUri, nameof(requestUri));

            return ExecuteAsync(requestUri, uri => new HttpRequestMessage(HttpMethod.Delete, uri), MethodBase.GetCurrentMethod(), httpRequestMessageCallback);
        }

        private Task ExecuteAsync(Uri requestUri, Func<Uri, HttpRequestMessage> httpRequestMessageCreator, MethodBase methodBase, Action<HttpRequestMessage> httpRequestMessageCallback = null)
        {
            NullGuard.NotNull(requestUri, nameof(requestUri))
                .NotNull(httpRequestMessageCreator, nameof(httpRequestMessageCreator))
                .NotNull(methodBase, nameof(methodBase));

            return ExecuteAsync(async () =>
	            {
		            using HttpClientHandler httpClientHandler = CreateHttpClientHandler(EnforceSslProtocol);
                    using HttpClient httpClient = new HttpClient(httpClientHandler);
                    using HttpRequestMessage httpRequestMessage = CreateHttpRequestMessage(requestUri, httpRequestMessageCreator, httpRequestMessageCallback);
                    using HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        return;
                    }

                    throw await HandleUnsuccessfulResponseAndCreateExceptionAsync(methodBase, httpRequestMessage, httpResponseMessage);
                },
                methodBase);
        }

        private Task<TResult> ExecuteAsync<TResult>(Uri requestUri, Func<Uri, HttpRequestMessage> httpRequestMessageCreator, MethodBase methodBase, Action<HttpRequestMessage> httpRequestMessageCallback = null, DataContractJsonSerializerSettings serializerSettings = null) where TResult : class
        {
            NullGuard.NotNull(requestUri, nameof(requestUri))
                .NotNull(httpRequestMessageCreator, nameof(httpRequestMessageCreator))
                .NotNull(methodBase, nameof(methodBase));

            return ExecuteAsync(async () =>
                {
	                using HttpClientHandler httpClientHandler = CreateHttpClientHandler(EnforceSslProtocol);
                    using HttpClient httpClient = new HttpClient(httpClientHandler);
                    using HttpRequestMessage httpRequestMessage = CreateHttpRequestMessage(requestUri, httpRequestMessageCreator, httpRequestMessageCallback);
                    using HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TResult), serializerSettings);
                        await using Stream responseReader = await httpResponseMessage.Content.ReadAsStreamAsync();

                        return (TResult) serializer.ReadObject(responseReader);
                    }

                    throw await HandleUnsuccessfulResponseAndCreateExceptionAsync(methodBase, httpRequestMessage, httpResponseMessage);
                },
                methodBase);
        }

        private async Task<Exception> HandleUnsuccessfulResponseAndCreateExceptionAsync(MethodBase methodBase, HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage)
        {
            NullGuard.NotNull(methodBase, nameof(methodBase))
                .NotNull(httpRequestMessage, nameof(httpRequestMessage))
                .NotNull(httpResponseMessage, nameof(httpResponseMessage));

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException();

                default:
                    Uri requestUri = httpRequestMessage.RequestUri;
                    string request = await ReadContentAsString(httpRequestMessage.Content);
                    string response = await ReadContentAsString(httpResponseMessage.Content);
                    Logger.LogError($"{methodBase}: {Convert.ToString(httpResponseMessage.StatusCode)}, Url={requestUri}, Request={request}, Response={response}");

                    return new IntranetExceptionBuilder(ErrorCode.RepositoryError, methodBase.Name, httpResponseMessage.ReasonPhrase)
                        .WithMethodBase(methodBase)
                        .Build();
            }
        }

        private static async Task<string> ReadContentAsString(HttpContent httpContent)
        {
            if (httpContent == null)
            {
                return "{null}";
            }

            return await httpContent.ReadAsStringAsync();
        }

        private static HttpClientHandler CreateHttpClientHandler(SslProtocols enforceSslProtocol)
        {
	        SecurityProtocolType enforceSecurityProtocolType = ConvertSslProtocolsToSecurityProtocolType(enforceSslProtocol);

	        SecurityProtocolType existingSecurityProtocolType = ServicePointManager.SecurityProtocol;
	        if (existingSecurityProtocolType.HasFlag(enforceSecurityProtocolType) == false)
	        {
		        ServicePointManager.SecurityProtocol = existingSecurityProtocolType | enforceSecurityProtocolType;
	        }

	        return new HttpClientHandler
	        {
		        SslProtocols = enforceSslProtocol
	        };
        }

        private static SecurityProtocolType ConvertSslProtocolsToSecurityProtocolType(SslProtocols sslProtocol)
        {
	        switch (sslProtocol)
	        {
		        case SslProtocols.Tls12:
			        return SecurityProtocolType.Tls12;

		        case SslProtocols.Tls13:
			        return SecurityProtocolType.Tls13;

		        default:
			        throw new NotSupportedException($"Unsupported SSL protocol: {sslProtocol}");
	        }
        }

        private static HttpRequestMessage CreateHttpRequestMessage(Uri requestUri, Func<Uri, HttpRequestMessage> httpRequestMessageCreator, Action<HttpRequestMessage> httpRequestMessageCallback = null)
        {
	        NullGuard.NotNull(requestUri, nameof(requestUri))
		        .NotNull(httpRequestMessageCreator, nameof(httpRequestMessageCreator));

	        HttpRequestMessage httpRequestMessage = httpRequestMessageCreator(requestUri);
	        try
	        {
		        httpRequestMessageCallback?.Invoke(httpRequestMessage);
	        }
	        catch (TargetInvocationException ex)
	        {
		        httpRequestMessage.Dispose();

		        throw ex.InnerException ?? ex;
	        }
	        return httpRequestMessage;
        }

		#endregion
	}
}