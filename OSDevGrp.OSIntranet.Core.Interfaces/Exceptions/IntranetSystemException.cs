using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using System;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Exceptions
{
	[Serializable]
    public class IntranetSystemException : IntranetExceptionBase
    {
        #region Constructors

        public IntranetSystemException(ErrorCode errorCode, string message)
            : base(errorCode, message)
        {
        }

        public IntranetSystemException(ErrorCode errorCode, string message, Exception innerException)
            : base(errorCode, message, innerException)
        {
        }

        #endregion
    }
}