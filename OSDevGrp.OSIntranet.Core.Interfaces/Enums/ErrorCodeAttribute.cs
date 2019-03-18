using System;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Enums
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ErrorCodeAttribute : Attribute
    {
        #region Constructor

        public ErrorCodeAttribute(string message, Type exceptionType)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (exceptionType == null)
            {
                throw new ArgumentNullException(nameof(exceptionType));
            }

            if (exceptionType.IsSubclassOf(typeof(IntranetExceptionBase)) == false)
            {
                throw new ArgumentException($"The argument named '{nameof(exceptionType)}' has an invalid exception type which are not based on '{typeof(IntranetExceptionBase).Name}'.", nameof(exceptionType));
            }

            Message = message;
            ExceptionType = exceptionType;
        }

        #endregion

        #region Properties

        public string Message { get; }

        public Type ExceptionType { get; }

        #endregion
    }
}
