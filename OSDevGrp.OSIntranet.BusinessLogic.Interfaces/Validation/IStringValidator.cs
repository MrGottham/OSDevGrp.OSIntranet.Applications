using System;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation
{
    public interface IStringValidator
    {
        IValidator ShouldNotBeNull(string value, Type validatingType, string validatingField);

        IValidator ShouldNotBeNullOrEmpty(string value, Type validatingType, string validatingField);

        IValidator ShouldNotBeNullOrWhiteSpace(string value, Type validatingType, string validatingField);

        IValidator ShouldHaveMinLength(string value, int minLength, Type validatingType, string validatingField, bool allowNull = false);

        IValidator ShouldHaveMaxLength(string value, int maxLength, Type validatingType, string validatingField, bool allowNull = false);

        IValidator ShouldMatchPattern(string value, Regex pattern, Type validatingType, string validatingField, bool allowNull = false);
    }
}
