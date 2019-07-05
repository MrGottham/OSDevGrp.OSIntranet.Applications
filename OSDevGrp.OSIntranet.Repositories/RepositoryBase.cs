using System;
using System.Reflection;
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

        protected void Execute(Action action, MethodBase methodBase)
        {
            NullGuard.NotNull(action, nameof(action))
                .NotNull(methodBase, nameof(methodBase));

            try
            {
                action();
            }
            catch (AggregateException aggregateException)
            {
                throw HandleAndCreateException(aggregateException, methodBase);
            }
        }

        protected T Execute<T>(Func<T> resultGetter, MethodBase methodBase)
        {
            NullGuard.NotNull(resultGetter, nameof(resultGetter))
                .NotNull(methodBase, nameof(methodBase));

            try
            {
                return resultGetter();
            }
            catch (AggregateException aggregateException)
            {
                throw HandleAndCreateException(aggregateException, methodBase);
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

        #endregion
    }
}
