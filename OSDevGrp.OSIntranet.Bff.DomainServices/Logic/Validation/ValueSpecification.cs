using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class ValueSpecification<TValue> : IValueSpecification<TValue> where TValue : IComparable<TValue>
{
    #region Constructor

    private ValueSpecification(TValue value, string description)
    {
        Value = value;
        Description = description;
    }

    #endregion

    #region Properties

    public TValue Value { get; }

    public string Description { get; }

    #endregion

    #region Metods

    internal static IValueSpecification<TValue> Create(TValue value, string description)
    {
        return new ValueSpecification<TValue>(value, description);
    }

    #endregion
}