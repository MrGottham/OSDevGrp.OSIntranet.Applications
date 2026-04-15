namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IMinLengthRule : IValidationRule
{
    int MinLength { get; }
}