namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IMaxValueRule<TValue> : IValidationRule where TValue : struct, IComparable<TValue>
{
    TValue MaxValue { get; }
}