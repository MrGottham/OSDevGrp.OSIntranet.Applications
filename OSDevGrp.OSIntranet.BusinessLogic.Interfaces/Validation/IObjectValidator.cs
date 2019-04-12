using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation
{
    public interface IObjectValidator
    {
        IValidator ShouldBeKnownValue<T>(T value, Func<T, Task<bool>> isKnownValueGetter, Type validatingType, string validatingField, bool allowNull = false);

        IValidator ShouldBeUnknownValue<T>(T value, Func<T, Task<bool>> isUnknownValueGetter, Type validatingType, string validatingField, bool allowNull = false);
    }
}
