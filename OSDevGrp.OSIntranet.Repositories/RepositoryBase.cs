using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;

namespace OSDevGrp.OSIntranet.Repositories
{
    public abstract class RepositoryBase
    {
        #region Constructor

        protected RepositoryBase(IConfiguration configuration, IPrincipalResolver principalResolver, ILoggerFactory loggerFactory)
        {
            NullGuard.NotNull(configuration, nameof(configuration))
                .NotNull(principalResolver, nameof(principalResolver))
                .NotNull(loggerFactory, nameof(loggerFactory));

            Configuration = configuration;
            PrincipalResolver = principalResolver;
            LoggerFactory = loggerFactory;
        }

        #endregion

        #region Properties

        protected IConfiguration Configuration { get; }

        protected IPrincipalResolver PrincipalResolver { get; }

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

            return new IntranetExceptionBuilder(ErrorCode.RepositoryError, methodBase.Name, handledException.Message)
                .WithInnerException(handledException)
                .WithMethodBase(methodBase)
                .Build();
        }

        private Exception HandleAndCreateException(Exception exception, MethodBase methodBase)
        {
            NullGuard.NotNull(exception, nameof(exception))
                .NotNull(methodBase, nameof(methodBase));

            return new IntranetExceptionBuilder(ErrorCode.RepositoryError, methodBase.Name, exception.Message)
                .WithInnerException(exception)
                .WithMethodBase(methodBase)
                .Build();
        }

        #endregion
    }
}