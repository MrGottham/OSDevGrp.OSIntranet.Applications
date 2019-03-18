using System;
using System.Reflection;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

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

        protected IntranetRepositoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
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
