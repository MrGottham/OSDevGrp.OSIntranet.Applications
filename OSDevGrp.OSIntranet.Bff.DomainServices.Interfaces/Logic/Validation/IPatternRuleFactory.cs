using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IPatternRuleFactory : IValidationRuleFactory
{
    Task<IValidationRule> CreateAsync(string name, StaticTextKey field, Regex pattern, IFormatProvider formatProvider, CancellationToken cancellationToken = default);
}