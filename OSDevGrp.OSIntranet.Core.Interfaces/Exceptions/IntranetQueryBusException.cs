using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using System;

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

        #endregion
    }
}