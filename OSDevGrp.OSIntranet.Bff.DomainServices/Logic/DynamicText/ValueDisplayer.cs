using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ValueDisplayer<TValue> : IValueDisplayer
{
    #region Constructor

    public ValueDisplayer(string label, TValue value, IFormatProvider formatProvider, Func<TValue, IFormatProvider, string?> valueFormatter)
    {
        Label = label;
        Value = Format(value, formatProvider, valueFormatter);
    }

    #endregion

    #region Properties

    public string Label { get; }

    public string? Value { get; }

    #endregion

    #region Methods

    private static string? Format(TValue value, IFormatProvider formatProvider, Func<TValue, IFormatProvider, string?> valueFormatter)
    {
        string? result = valueFormatter(value, formatProvider);
        return string.IsNullOrWhiteSpace(result) ? null : result;
    }

    #endregion
}