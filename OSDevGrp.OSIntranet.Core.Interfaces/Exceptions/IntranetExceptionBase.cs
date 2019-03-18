using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Exceptions
{
    [Serializable]
    public abstract class IntranetExceptionBase : Exception
    {
        #region Constructors

        protected IntranetExceptionBase(ErrorCode errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        protected IntranetExceptionBase(ErrorCode errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        protected IntranetExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Properties

        public ErrorCode ErrorCode { get; }

        #endregion
    }
}
