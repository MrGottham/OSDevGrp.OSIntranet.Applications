using System;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Validation
{
    public class StringValidator : Validator, IStringValidator
    {
        #region Methods

        public IValidator ShouldNotBeNull(string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (value != null)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNull, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldNotBeNullOrEmpty(string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (string.IsNullOrEmpty(value) == false)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNullOrEmpty, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldNotBeNullOrWhiteSpace(string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (string.IsNullOrWhiteSpace(value) == false)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNullOrWhiteSpace, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldHaveMinLength(string value, int minLength, Type validatingType, string validatingField, bool allowNull = false)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (value == null && allowNull)
            {
                return this;
            }

            if (value == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNull, validatingField)
                    .WithValidatingType(validatingType)
                    .WithValidatingField(validatingField)
                    .Build();
            }

            if (value.Length >= minLength)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldHaveMinLength, validatingField, minLength)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldHaveMaxLength(string value, int maxLength, Type validatingType, string validatingField, bool allowNull = false)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (value == null && allowNull)
            {
                return this;
            }

            if (value == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNull, validatingField)
                    .WithValidatingType(validatingType)
                    .WithValidatingField(validatingField)
                    .Build();
            }

            if (value.Length <= maxLength)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldHaveMaxLength, validatingField, maxLength)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldMatchPattern(string value, Regex pattern, Type validatingType, string validatingField, bool allowNull = false)
        {
            NullGuard.NotNull(pattern, nameof(pattern))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (value == null && allowNull)
            {
                return this;
            }

            if (value == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNull, validatingField)
                    .WithValidatingType(validatingType)
                    .WithValidatingField(validatingField)
                    .Build();
            }

            if (pattern.IsMatch(value))
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldMatchPattern, validatingField, pattern.ToString())
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        #endregion
    }
}
