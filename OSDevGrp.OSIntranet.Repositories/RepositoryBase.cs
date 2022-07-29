using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Repositories
{
    internal abstract class RepositoryBase
    {
        #region Constructor

        protected RepositoryBase(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            NullGuard.NotNull(configuration, nameof(configuration))
                .NotNull(loggerFactory, nameof(loggerFactory));

            Configuration = configuration;
            LoggerFactory = loggerFactory;
        }

        #endregion

        #region Properties

        protected IConfiguration Configuration { get; }

        protected ILoggerFactory LoggerFactory { get; }

        protected ILogger Logger => LoggerFactory.CreateLogger(GetType());

        #endregion

        #region Methods

        protected async Task ExecuteAsync(Func<Task> action, MethodBase methodBase)
        {
            NullGuard.NotNull(action, nameof(action))
                .NotNull(methodBase, nameof(methodBase));

            try
            {
                await action();
            }
            catch (AggregateException aggregateException)
            {
                throw HandleAndCreateException(aggregateException, methodBase);
            }
            catch (Exception exception)
            {
                throw HandleAndCreateException(exception, methodBase);
            }
        }

        protected async Task<T> ExecuteAsync<T>(Func<Task<T>> resultGetter, MethodBase methodBase)
        {
            NullGuard.NotNull(resultGetter, nameof(resultGetter))
                .NotNull(methodBase, nameof(methodBase));

            try
            {
                return await resultGetter();
            }
            catch (AggregateException aggregateException)
            {
                throw HandleAndCreateException(aggregateException, methodBase);
            }
            catch (Exception exception)
            {
                throw HandleAndCreateException(exception, methodBase);
            }
        }

        private Exception HandleAndCreateException(AggregateException aggregateException, MethodBase methodBase)
        {
            NullGuard.NotNull(aggregateException, nameof(aggregateException))
                .NotNull(methodBase, nameof(methodBase));

            Exception handledException = null;
            aggregateException.Handle(exception =>
            {
                handledException = exception;
                return true;
            });

            return new IntranetExceptionBuilder(ErrorCode.RepositoryError, methodBase.Name, BuildErrorMessage(handledException))
                .WithInnerException(handledException)
                .WithMethodBase(methodBase)
                .Build();
        }

        private Exception HandleAndCreateException(Exception exception, MethodBase methodBase)
        {
            NullGuard.NotNull(exception, nameof(exception))
                .NotNull(methodBase, nameof(methodBase));

            return new IntranetExceptionBuilder(ErrorCode.RepositoryError, methodBase.Name, BuildErrorMessage(exception))
                .WithInnerException(exception)
                .WithMethodBase(methodBase)
                .Build();
        }

        private static string BuildErrorMessage(Exception exception)
        {
            NullGuard.NotNull(exception, nameof(exception));

            StringBuilder errorMessageBuilder = new StringBuilder(exception.Message);
            if (exception.InnerException == null)
            {
                return errorMessageBuilder.ToString();
            }

            Exception innerException = exception.InnerException;
            while (innerException.InnerException != null)
            {
                innerException = innerException.InnerException;
            }

            errorMessageBuilder.Append($" (Original reason: {innerException.Message})");

            return errorMessageBuilder.ToString();
        }

        #endregion
    }
}