using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Validation
{
    public class IntegerValidator : Validator, IIntegerValidator
    {
        #region Methods

        public IValidator ShouldBeGreaterThanZero(int value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (value > 0)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueNotGreaterThanZero, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeGreaterThanOrEqualToZero(int value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (value >= 0)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueNotGreaterThanOrEqualToZero, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeBetween(int value, int minValue, int maxValue, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (value >= minValue && value <= maxValue)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueNotBetween, validatingField, minValue, maxValue)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        #endregion
    }
}
