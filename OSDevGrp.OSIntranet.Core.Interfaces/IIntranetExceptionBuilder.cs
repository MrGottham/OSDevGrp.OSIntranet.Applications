using System;
using System.Reflection;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Core.Interfaces
{
    public interface IIntranetExceptionBuilder
    {
        IIntranetExceptionBuilder WithInnerException(Exception innException);

        IIntranetExceptionBuilder WithMethodBase(MethodBase methodInfo);

        IIntranetExceptionBuilder WithValidatingType(Type validatingType);

        IIntranetExceptionBuilder WithValidatingField(string validatingField);

        IntranetExceptionBase Build();
    }
}
