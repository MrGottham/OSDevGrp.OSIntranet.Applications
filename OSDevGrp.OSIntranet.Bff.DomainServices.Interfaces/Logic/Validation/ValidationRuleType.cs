namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public enum ValidationRuleType
{
    RequiredValueRule,
    MinLengthRule,
    MaxLengthRule,
    ShouldBeIntegerRule,
    MinValueRule,
    MaxValueRule,
    PatternRule,
    OneOfRule,
}