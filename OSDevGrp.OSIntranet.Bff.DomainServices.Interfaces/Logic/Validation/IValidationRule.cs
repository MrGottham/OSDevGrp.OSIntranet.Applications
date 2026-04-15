namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IValidationRule
{
    string Name { get; }

    ValidationRuleType RuleType { get; }

    string ValidationError { get; }
}