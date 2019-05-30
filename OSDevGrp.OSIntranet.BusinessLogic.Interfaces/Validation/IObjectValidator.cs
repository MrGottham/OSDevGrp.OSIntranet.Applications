using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation
{
    public interface IObjectValidator
    {
        IValidator ShouldBeKnownValue<T>(T value, Func<T, Task<bool>> isKnownValueGetter, Type validatingType, string validatingField, bool allowNull = false);

        IValidator ShouldBeUnknownValue<T>(T value, Func<T, Task<bool>> isUnknownValueGetter, Type validatingType, string validatingField, bool allowNull = false);

        IValidator ShouldBeDeletable<TValue, TDeletable>(TValue value, Func<TValue, Task<TDeletable>> deletableGetter, Type validatingType, string validatingField, bool allowNull = false) where TDeletable : IDeletable;

        IValidator ShouldNotBeNull<T>(T value, Type validatingType, string validatingField);
    }
}
