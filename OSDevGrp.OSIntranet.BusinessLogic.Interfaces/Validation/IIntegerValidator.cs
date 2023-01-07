using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation
{
    public interface IIntegerValidator
    {
        IValidator ShouldBeGreaterThanZero(int value, Type validatingType, string validatingField);

        IValidator ShouldBeGreaterThanOrEqualToZero(int value, Type validatingType, string validatingField);

        IValidator ShouldBeBetween(int value, int minValue, int maxValue, Type validatingType, string validatingField);
    }
}