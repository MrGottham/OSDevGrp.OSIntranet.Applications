using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Validation
{
	internal class DateTimeValidator : Validator, IDateTimeValidator
    {
        #region Methods

        public IValidator ShouldBePastDate(DateTime value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (UtcDate(value) < UtcDate(System.DateTime.Today))
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBePastDate, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBePastDateOrToday(DateTime value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (UtcDate(value) <= UtcDate(System.DateTime.Today))
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBePastDateOrToday, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBePastDateTime(DateTime value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (UtcDateTime(value) < System.DateTime.UtcNow)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBePastDateTime, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeToday(DateTime value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (UtcDate(value) == UtcDate(System.DateTime.Today))
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeToday, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeFutureDate(DateTime value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (UtcDate(value) > UtcDate(System.DateTime.Today))
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeFutureDate, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeFutureDateOrToday(DateTime value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (UtcDate(value) >= UtcDate(System.DateTime.Today))
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeFutureDateOrToday, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeFutureDateTime(DateTime value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (UtcDateTime(value) > System.DateTime.UtcNow)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeFutureDateTime, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBePastDateWithinDaysFromOffsetDate(DateTime value, int days, DateTime offsetDate, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            DateTime minUtcDate = UtcDate(offsetDate).AddDays(Math.Abs(days) * -1);
            if (UtcDate(value) >= minUtcDate && UtcDate(value) <= UtcDate(offsetDate))
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBePastDateWithinDaysFromOffsetDate, validatingField, days, days == 1 ? "day" : "days", offsetDate.Date == System.DateTime.Today ? "today" : offsetDate.ToLongDateString())
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeFutureDateWithinDaysFromOffsetDate(DateTime value, int days, DateTime offsetDate, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            DateTime maxDate = UtcDate(offsetDate).AddDays(Math.Abs(days));
            if (UtcDate(value) >= UtcDate(offsetDate) && UtcDate(value) <= maxDate)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeFutureDateWithinDaysFromOffsetDate, validatingField, days, days == 1 ? "day" : "days", offsetDate.Date == System.DateTime.Today ? "today" : offsetDate.ToLongDateString())
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeEarlierThanOffsetDate(DateTime value, DateTime offsetDate, Type validatingType, string validatingField)
        {
	        NullGuard.NotNull(validatingType, nameof(validatingType))
		        .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

	        if (UtcDate(value) < UtcDate(offsetDate))
	        {
		        return this;
	        }

	        throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeEarlierThanOffsetDate, validatingField, offsetDate.Date == System.DateTime.Today ? "today" : offsetDate.ToLongDateString())
		        .WithValidatingType(validatingType)
		        .WithValidatingField(validatingField)
		        .Build();
        }

		public IValidator ShouldBeEarlierThanOrEqualToOffsetDate(DateTime value, DateTime offsetDate, Type validatingType, string validatingField)
        {
	        NullGuard.NotNull(validatingType, nameof(validatingType))
		        .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

	        if (UtcDate(value) <= UtcDate(offsetDate))
	        {
		        return this;
	        }

	        throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeEarlierThanOrEqualToOffsetDate, validatingField, offsetDate.Date == System.DateTime.Today ? "today" : offsetDate.ToLongDateString())
		        .WithValidatingType(validatingType)
		        .WithValidatingField(validatingField)
		        .Build();
        }

		public IValidator ShouldBeLaterThanOffsetDate(DateTime value, DateTime offsetDate, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (UtcDate(value) > UtcDate(offsetDate))
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeLaterThanOffsetDate, validatingField, offsetDate.Date == System.DateTime.Today ? "today" : offsetDate.ToLongDateString())
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeLaterThanOrEqualToOffsetDate(DateTime value, DateTime offsetDate, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (UtcDate(value) >= UtcDate(offsetDate))
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeLaterThanOrEqualToOffsetDate, validatingField, offsetDate.Date == System.DateTime.Today ? "today" : offsetDate.ToLongDateString())
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        private DateTime UtcDate(DateTime value)
        {
            return UtcDateTime(value).Date;
        }

        private DateTime UtcDateTime(DateTime value)
        {
            return value.Date.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        }

        #endregion
    }
}