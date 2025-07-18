using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IPatternRule : IValidationRule
{
    Regex Pattern { get; }
}