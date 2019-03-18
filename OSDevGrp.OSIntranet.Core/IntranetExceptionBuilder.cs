using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Core
{
    public class IntranetExceptionBuilder : IIntranetExceptionBuilder
    {
        #region Private variables

        private readonly ErrorCode _errorCode;
        private readonly IEnumerable<object> _argumentCollection;
        private Exception _innerException;
        private MethodBase _methodBase;

        #endregion
        
        #region Constructor

        public IntranetExceptionBuilder(ErrorCode errorCode, params object[] argumentCollection)
        {
            _errorCode = errorCode;
            _argumentCollection = argumentCollection;
        }

        #endregion

        #region Methods

        public IIntranetExceptionBuilder WithInnerException(Exception innerException)
        {
            NullGuard.NotNull(innerException, nameof(innerException));

            _innerException = innerException;

            return this;
        }

        public IIntranetExceptionBuilder WithMethodBase(MethodBase methodBase)
        {
            NullGuard.NotNull(methodBase, nameof(methodBase));

            _methodBase = methodBase;

            return this;
        }

        public IntranetExceptionBase Build()
        {
            ErrorCodeAttribute errorCodeAttribute = GetErrorCodeAttribute(_errorCode);

            string message = _argumentCollection == null
                ? errorCodeAttribute.Message
                : string.Format(errorCodeAttribute.Message, _argumentCollection.ToArray());

            IntranetExceptionBase intranetException = _innerException == null
                ? Build(errorCodeAttribute.ExceptionType, _errorCode, message)
                : Build(errorCodeAttribute.ExceptionType, _errorCode, message, _innerException);

            if (_methodBase == null)
            {
                return intranetException;
            }

            AddMethodBae(intranetException, _methodBase);

            return intranetException;
        }

        private static IntranetExceptionBase Build(Type exceptionType, ErrorCode errorCode, string message)
        {
            NullGuard.NotNull(exceptionType, nameof(exceptionType))
                .NotNullOrWhiteSpace(message, nameof(message));

            return Build(exceptionType, type => type.GetConstructor(new[] {typeof(ErrorCode), typeof(string)}), errorCode, message);
        }

        private static IntranetExceptionBase Build(Type exceptionType, ErrorCode errorCode, string message, Exception innerException)
        {
            NullGuard.NotNull(exceptionType, nameof(exceptionType))
                .NotNullOrWhiteSpace(message, nameof(message))
                .NotNull(innerException, nameof(innerException));

            return Build(exceptionType, type => type.GetConstructor(new[] {typeof(ErrorCode), typeof(string), typeof(Exception)}), errorCode, message, innerException);
        }

        private static IntranetExceptionBase Build(Type exceptionType, Func<Type, ConstructorInfo> constructorGetter, params object[] argumentCollection)
        {
            NullGuard.NotNull(exceptionType, nameof(exceptionType))
                .NotNull(constructorGetter, nameof(constructorGetter))
                .NotNull(argumentCollection, nameof(argumentCollection));

            ConstructorInfo constructorInfo = constructorGetter(exceptionType);
            if (constructorInfo == null)
            {
                throw new MissingMethodException($"Unable to find a suitable constructor on the type named '{exceptionType.Name}'.", exceptionType.Name);
            }

            try
            {
                return (IntranetExceptionBase) constructorInfo.Invoke(argumentCollection.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        private static void AddMethodBae(IntranetExceptionBase intranetException, MethodBase methodBase)
        {
            NullGuard.NotNull(intranetException, nameof(intranetException))
                .NotNull(methodBase, nameof(methodBase));

            FieldInfo[] fieldInfoArray = intranetException.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(fieldInfo => fieldInfo.FieldType == typeof(MethodBase))
                .ToArray();

            foreach (FieldInfo fieldInfo in fieldInfoArray)
            {
                try
                {
                    fieldInfo.SetValue(intranetException, methodBase);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException ?? ex;
                }
            }
        }

        private static ErrorCodeAttribute GetErrorCodeAttribute(ErrorCode errorCode)
        {
            FieldInfo fieldInfo = errorCode.GetType().GetField(Convert.ToString(errorCode), BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            if (fieldInfo == null)
            {
                throw new MissingFieldException($"Unable to find a field named '{errorCode}' in '{errorCode.GetType().Name}'", Convert.ToString(errorCode));
            }

            ErrorCodeAttribute errorCodeAttribute = fieldInfo.GetCustomAttributes<ErrorCodeAttribute>().SingleOrDefault();
            if (errorCodeAttribute == null)
            {
                throw new InvalidOperationException($"The error code named '{errorCode}' has no '{typeof(ErrorCodeAttribute).Name}'.");
            }

            return errorCodeAttribute;
        }

        #endregion
    }
}
