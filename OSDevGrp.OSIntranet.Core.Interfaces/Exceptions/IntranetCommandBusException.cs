using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Exceptions
{
    [Serializable]
    public class IntranetCommandBusException : IntranetSystemException
    {
        #region Constructors

        public IntranetCommandBusException(ErrorCode errorCode, string message)
            : base(errorCode, message)
        {
        }

        public IntranetCommandBusException(ErrorCode errorCode, string message, Exception innerException)
            : base(errorCode, message, innerException)
        {
        }

        protected IntranetCommandBusException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}