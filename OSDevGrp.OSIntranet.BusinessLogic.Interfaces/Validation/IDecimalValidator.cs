using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation
{
    public interface IDecimalValidator
    {
        IValidator ShouldBeGreaterThanZero(decimal value, Type validatingType, string validatingField);

        IValidator ShouldBeGreaterThanOrEqualToZero(decimal value, Type validatingType, string validatingField);

        IValidator ShouldBeBetween(decimal value, decimal minValue, decimal maxValue, Type validatingType, string validatingField);
    }
}