using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;

namespace OSDevGrp.OSIntranet.Repositories
{
    public abstract class WebRepositoryBase : RepositoryBase
    {
        #region Private variables

        private readonly ILogger _logger;

        #endregion

        #region Constructor

        protected WebRepositoryBase(IConfiguration configuration, IPrincipalResolver principalResolver, ILogger logger)
            : base(configuration, principalResolver)
        {
            NullGuard.NotNull(logger, nameof(logger));

            _logger = logger;
        }

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

        private Task<TResult> ExecuteAsync<TResult>(Uri requestUri, Func<Uri, HttpRequestMessage> httpRequestMessageCreator, MethodBase methodBase, Action<HttpRequestMessage> httpRequestMessageCallback = null, DataContractJsonSerializerSettings serializerSettings = null) where TResult : class
        {
            NullGuard.NotNull(requestUri, nameof(requestUri))
                .NotNull(httpRequestMessageCreator, nameof(httpRequestMessageCreator))
                .NotNull(methodBase, nameof(methodBase));

            return Execute(async () =>
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        using (HttpRequestMessage httpRequestMessage = httpRequestMessageCreator(requestUri))
                        {
                            try
                            {
                                httpRequestMessageCallback?.Invoke(httpRequestMessage);
                            }
                            catch (TargetInvocationException ex)
                            {
                                throw ex.InnerException ?? ex;
                            }

                            using (HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage))
                            {
                                if (httpResponseMessage.IsSuccessStatusCode)
                                {
                                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TResult), serializerSettings);
                                    using (Stream responseReader = await httpResponseMessage.Content.ReadAsStreamAsync())
                                    {
                                        return (TResult) serializer.ReadObject(responseReader);
                                    }
                                }

                                switch (httpResponseMessage.StatusCode)
                                {
                                    case HttpStatusCode.Unauthorized:
                                        throw new UnauthorizedAccessException();

                                    default:
                                        string errorResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                                        _logger.LogError($"{methodBase}: {Convert.ToString(httpResponseMessage.StatusCode)}, Response={errorResponse}");

                                        throw new IntranetExceptionBuilder(ErrorCode.RepositoryError, methodBase.Name, httpResponseMessage.ReasonPhrase)
                                            .WithMethodBase(methodBase)
                                            .Build();
                                }
                            }

                        }
                    }
                },
                methodBase);
        }

        #endregion
    }
}
