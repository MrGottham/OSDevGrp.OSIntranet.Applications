namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IMinValueRule<TValue> : IValidationRule where TValue : struct, IComparable<TValue>
{
    TValue MinValue { get; }
}