using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation
{
    public interface IDateTimeValidator
    {
        IValidator ShouldBePastDate(DateTime value, Type validatingType, string validatingField);

        IValidator ShouldBePastDateOrToday(DateTime value, Type validatingType, string validatingField);

        IValidator ShouldBeToday(DateTime value, Type validatingType, string validatingField);

        IValidator ShouldBeFutureDate(DateTime value, Type validatingType, string validatingField);

        IValidator ShouldBeFutureDateOrToday(DateTime value, Type validatingType, string validatingField);

        IValidator ShouldBePastDateWithinDaysFromOffsetDate(DateTime value, int days, DateTime offsetDate, Type validatingType, string validatingField);

        IValidator ShouldBeFutureDateWithinDaysFromOffsetDate(DateTime value, int days, DateTime offsetDate, Type validatingType, string validatingField);

        IValidator ShouldBeLaterThanOffsetDate(DateTime value, DateTime offsetDate, Type validatingType, string validatingField);

        IValidator ShouldBeLaterThanOrEqualToOffsetDate(DateTime value, DateTime offsetDate, Type validatingType, string validatingField);
    }
}
