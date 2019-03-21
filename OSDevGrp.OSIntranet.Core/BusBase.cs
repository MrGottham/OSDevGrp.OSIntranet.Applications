using System;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Core
{
    public abstract class BusBase
    {
        protected Exception Handle(AggregateException aggregateException, Func<Exception, IIntranetExceptionBuilder> intranetExceptionBuilder)
        {
            NullGuard.NotNull(aggregateException, nameof(aggregateException))
                .NotNull(intranetExceptionBuilder, nameof(intranetExceptionBuilder));

            Exception innerException = null;
            aggregateException.Handle(exception =>
            {
                innerException = exception;
                return true;
            });

            IntranetExceptionBase intranetException = innerException as IntranetExceptionBase;
            return intranetException ?? intranetExceptionBuilder(innerException).Build();
        }
    }
}