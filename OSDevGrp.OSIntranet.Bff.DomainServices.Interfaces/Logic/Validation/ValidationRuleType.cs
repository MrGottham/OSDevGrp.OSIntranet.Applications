namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public enum ValidationRuleType
{
    RequiredValueRule,
    MinLengthRule,
    MaxLengthRule,
    MinValueRule,
    MaxValueRule,
    PatternRule,
    OneOfRule,
}