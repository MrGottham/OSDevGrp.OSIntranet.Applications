namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IOneOfRule<TValue> : IValidationRule where TValue : IComparable<TValue>
{
    IReadOnlyCollection<IValueSpecification<TValue>> ValidValues { get; }
}