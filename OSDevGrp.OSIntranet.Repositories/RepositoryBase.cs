using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Repositories
{
    public abstract class RepositoryBase
    {
        #region Constructor

        protected RepositoryBase(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            Configuration = configuration;
        }

        #endregion

        #region Properties

        protected IConfiguration Configuration { get; }

        #endregion

        #region Methods

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
                Exception handledException = null;
                aggregateException.Handle(exception =>
                {
                    handledException = exception;
                    return true;
                });

                throw new IntranetExceptionBuilder(ErrorCode.RepositoryError, methodBase.Name, handledException.Message)
                    .WithInnerException(handledException)
                    .WithMethodBase(methodBase)
                    .Build();
            }
        }

        #endregion
    }
}
