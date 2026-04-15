namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IMaxLengthRule : IValidationRule
{
    int MaxLength { get; }
}