using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Exceptions
{
    [Serializable]
    public class IntranetQueryBusException : IntranetSystemException
    {
        #region Constructors

        public IntranetQueryBusException(ErrorCode errorCode, string message)
            : base(errorCode, message)
        {
        }

        public IntranetQueryBusException(ErrorCode errorCode, string message, Exception innerException)
            : base(errorCode, message, innerException)
        {
        }

        protected IntranetQueryBusException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
