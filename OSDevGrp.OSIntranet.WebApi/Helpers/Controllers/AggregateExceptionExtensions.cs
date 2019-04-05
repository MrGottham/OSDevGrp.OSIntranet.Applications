using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.WebApi.Models.Core;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Controllers
{
    internal static class AggregateExceptionExtensions
    {
        public static ErrorModel ToErrorModel(this AggregateException aggregateException, IConverter coreModelConverter)
        {
            NullGuard.NotNull(aggregateException, nameof(aggregateException))
                .NotNull(coreModelConverter, nameof(coreModelConverter));

            ErrorModel errorModel = null;
            aggregateException.Handle(exception =>
            {
                IntranetExceptionBase intranetException = exception as IntranetExceptionBase;
                if (exception == null)
                {
                    return true;
                }

                errorModel = coreModelConverter.Convert<IntranetExceptionBase, ErrorModel>(intranetException);
                return true;
            });

            if (errorModel != null)
            {
                return errorModel;
            }

            throw aggregateException;
        }
    }
}
