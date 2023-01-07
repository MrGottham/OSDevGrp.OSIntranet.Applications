using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Validation
{
    internal class ObjectValidator : Validator, IObjectValidator
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
                if (isKnownValueGetter(value).GetAwaiter().GetResult())
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
            catch (Exception exception)
            {
                innerException = exception;
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
                if (isUnknownValueGetter(value).GetAwaiter().GetResult())
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
            catch (Exception exception)
            {
                innerException = exception;
            }

            IIntranetExceptionBuilder intranetExceptionBuilder = new IntranetExceptionBuilder(ErrorCode.ValueShouldBeUnknown, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField);

            throw (innerException == null ? intranetExceptionBuilder : intranetExceptionBuilder.WithInnerException(innerException)).Build();
        }

        public IValidator ShouldBeDeletable<TValue, TDeletable>(TValue value, Func<TValue, Task<TDeletable>> deletableGetter, Type validatingType, string validatingField, bool allowNull = false) where TDeletable : IDeletable
        {
            NullGuard.NotNull(deletableGetter, nameof(deletableGetter))
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
                IDeletable deletable = deletableGetter(value).GetAwaiter().GetResult();
                if (deletable is { Deletable: true })
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
            catch (Exception exception)
            {
                innerException = exception;
            }

            IIntranetExceptionBuilder intranetExceptionBuilder = new IntranetExceptionBuilder(ErrorCode.ValueShouldReferToDeletableEntity, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField);

            throw (innerException == null ? intranetExceptionBuilder : intranetExceptionBuilder.WithInnerException(innerException)).Build();
        }

        public IValidator ShouldNotBeNull<T>(T value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (Equals(value, null) == false)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNull, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        #endregion
    }
}