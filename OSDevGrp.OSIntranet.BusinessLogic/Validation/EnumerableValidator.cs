using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Validation
{
    internal class EnumerableValidator : Validator, IEnumerableValidator
    {
        #region Methods

        public IValidator ShouldContainItems<T>(IEnumerable<T> value, Type validatingType, string validatingField, bool allowNull = false)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
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

            if (value.Any())
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldContainSomeItems, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldHaveMinItems<T>(IEnumerable<T> value, int minItems, Type validatingType, string validatingField, bool allowNull = false)
        {
	        NullGuard.NotNull(validatingType, nameof(validatingType))
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

	        if (value.Count() >= minItems)
	        {
		        return this;
	        }

	        throw new IntranetExceptionBuilder(ErrorCode.ValueShouldContainMinItems, validatingField, minItems)
		        .WithValidatingType(validatingType)
		        .WithValidatingField(validatingField)
		        .Build();
        }

		public IValidator ShouldHaveMaxItems<T>(IEnumerable<T> value, int maxItems, Type validatingType, string validatingField, bool allowNull = false)
        {
	        NullGuard.NotNull(validatingType, nameof(validatingType))
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

	        if (value.Count() <= maxItems)
	        {
		        return this;
	        }

	        throw new IntranetExceptionBuilder(ErrorCode.ValueShouldContainMaxItems, validatingField, maxItems)
		        .WithValidatingType(validatingType)
		        .WithValidatingField(validatingField)
		        .Build();
        }

		#endregion
	}
}