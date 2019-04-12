using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Validation
{
    public class DateTimeValidator : Validator, IDateTimeValidator
    {
        #region Methods

        public IValidator ShouldBePastDate(DateTime value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (value.Date < System.DateTime.Today)
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

            if (value.Date <= System.DateTime.Today)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBePastDateOrToday, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeToday(DateTime value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (value.Date == System.DateTime.Today)
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

            if (value.Date > System.DateTime.Today)
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

            if (value.Date >= System.DateTime.Today)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeFutureDateOrToday, validatingField)
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBePastDateWithinDaysFromOffsetDate(DateTime value, int days, DateTime offsetDate, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            DateTime minDate = offsetDate.Date.AddDays(Math.Abs(days) * -1);
            if (value.Date >= minDate.Date && value.Date <= offsetDate.Date)
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

            DateTime maxDate = offsetDate.Date.AddDays(Math.Abs(days));
            if (value.Date >= offsetDate.Date && value.Date <= maxDate.Date)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeFutureDateWithinDaysFromOffsetDate, validatingField, days, days == 1 ? "day" : "days", offsetDate.Date == System.DateTime.Today ? "today" : offsetDate.ToLongDateString())
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        public IValidator ShouldBeLaterThanOffsetDate(DateTime value, DateTime offsetDate, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (value.Date > offsetDate.Date)
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

            if (value.Date >= offsetDate.Date)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeLaterThanOrEqualToOffsetDate, validatingField, offsetDate.Date == System.DateTime.Today ? "today" : offsetDate.ToLongDateString())
                .WithValidatingType(validatingType)
                .WithValidatingField(validatingField)
                .Build();
        }

        #endregion
    }
}
