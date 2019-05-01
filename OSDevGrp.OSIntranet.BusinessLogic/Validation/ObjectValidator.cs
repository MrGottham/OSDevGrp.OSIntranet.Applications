using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Validation
{
    public class ObjectValidator : Validator, IObjectValidator
    {
        #region Methods
        public IValidator ShouldBeKnownValue<T>(T value, Func<T, Task<bool>> isKnownValueGetter, Type validatingType, string validatingField, bool allowNull = false)
        {
            NullGuard.NotNull(isKnownValueGetter, nameof(isKnownValueGetter))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (Equals(value, null) && allowNull)
            {
                return this;
            }

            if (Equals(value, null))
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNull, validatingField)
                    .WithValidatingType(validatingType)
                    .WithValidatingField(validatingField)
                    .Build();
            }

            Exception innerException = null;
            try
            {
                Task<bool> isKnownValueGetterTask = isKnownValueGetter(value);
                isKnownValueGetterTask.Wait();

                if (isKnownValueGetterTask.Result)
                {
                    return this;
                }
            }
            catch (AggregateException aggregateException)
            {
                aggregateException.Handle(exception =>
                {
                    innerException = exception;
                    return true;
                });
            }

            IIntranetExceptionBuilder intranetExceptionBuilder = new IntranetExceptionBuilder(ErrorCode.ValueShouldBeKnown, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField);

            throw (innerException == null ? intranetExceptionBuilder : intranetExceptionBuilder.WithInnerException(innerException)).Build();
        }

        public IValidator ShouldBeUnknownValue<T>(T value, Func<T, Task<bool>> isUnknownValueGetter, Type validatingType, string validatingField, bool allowNull = false)
        {
            NullGuard.NotNull(isUnknownValueGetter, nameof(isUnknownValueGetter))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (Equals(value, null) && allowNull)
            {
                return this;
            }

            if (Equals(value, null))
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNull, validatingField)
                    .WithValidatingType(validatingType)
                    .WithValidatingField(validatingField)
                    .Build();
            }

            Exception innerException = null;
            try
            {
                Task<bool> isUnknownValueGetterTask = isUnknownValueGetter(value);
                isUnknownValueGetterTask.Wait();

                if (isUnknownValueGetterTask.Result)
                {
                    return this;
                }
            }
            catch (AggregateException aggregateException)
            {
                aggregateException.Handle(exception =>
                {
                    innerException = exception;
                    return true;
                });
            }

            IIntranetExceptionBuilder intranetExceptionBuilder = new IntranetExceptionBuilder(ErrorCode.ValueShouldBeUnknown, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField);

            throw (innerException == null ? intranetExceptionBuilder : intranetExceptionBuilder.WithInnerException(innerException)).Build();
        }

        #endregion
    }
}
