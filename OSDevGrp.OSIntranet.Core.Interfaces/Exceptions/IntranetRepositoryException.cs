using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using System;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Exceptions
{
	[Serializable]
    public class IntranetRepositoryException : IntranetExceptionBase
    {
        #region Private variables

        private MethodBase _methodBase;

        #endregion

        #region Constructors

        public IntranetRepositoryException(ErrorCode errorCode, string message)
            : base(errorCode, message)
        {
        }

        public IntranetRepositoryException(ErrorCode errorCode, string message, Exception innerException)
            : base(errorCode, message, innerException)
        {
        }

        #endregion

        #region Properties

        public MethodBase MethodBase
        {
            get => _methodBase;
            set => _methodBase = value ?? throw new ArgumentNullException(nameof(value));
        }

        #endregion
    }
}