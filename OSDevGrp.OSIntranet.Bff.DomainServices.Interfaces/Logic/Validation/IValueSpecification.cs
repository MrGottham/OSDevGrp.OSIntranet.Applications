namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IValueSpecification<TValue> where TValue : IComparable<TValue>
{
    TValue Value { get; }

    string Description { get; }
}