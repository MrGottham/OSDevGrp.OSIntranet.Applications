using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation
{
    public interface IEnumerableValidator
    {
        IValidator ShouldContainItems<T>(IEnumerable<T> value, Type validatingType, string validatingField, bool allowNull = false);

        IValidator ShouldHaveMinItems<T>(IEnumerable<T> value, int minItems, Type validatingType, string validatingField, bool allowNull = false);

        IValidator ShouldHaveMaxItems<T>(IEnumerable<T> value, int maxItems, Type validatingType, string validatingField, bool allowNull = false);
    }
}