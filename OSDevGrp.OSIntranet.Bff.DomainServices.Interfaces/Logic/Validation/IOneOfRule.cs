namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IOneOfRule<TValue> : IValidationRule where TValue : struct, IComparable<TValue>
{
    IReadOnlyCollection<TValue> ValidValues { get; }
}