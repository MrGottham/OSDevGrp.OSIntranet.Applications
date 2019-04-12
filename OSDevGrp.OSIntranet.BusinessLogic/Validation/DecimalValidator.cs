using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Validation
{
    public class DecimalValidator : Validator, IDecimalValidator
    {
        #region Methods

        public IValidator ShouldBeGreaterThanZero(decimal value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (value > 0M)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueNotGreaterThanZero, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeGreaterThanOrEqualToZero(decimal value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (value >= 0M)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueNotGreaterThanOrEqualToZero, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeBetween(decimal value, decimal minValue, decimal maxValue, Type validatingType, string validatingField)
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
