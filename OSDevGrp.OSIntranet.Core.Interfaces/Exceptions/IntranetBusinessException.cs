using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using System;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Exceptions
{
	[Serializable]
    public class IntranetBusinessException : IntranetExceptionBase
    {
        #region Constructors

        public IntranetBusinessException(ErrorCode errorCode, string message)
            : base(errorCode, message)
        {
        }

        public IntranetBusinessException(ErrorCode errorCode, string message, Exception innerException)
            : base(errorCode, message, innerException)
        {
        }

        #endregion
    }
}