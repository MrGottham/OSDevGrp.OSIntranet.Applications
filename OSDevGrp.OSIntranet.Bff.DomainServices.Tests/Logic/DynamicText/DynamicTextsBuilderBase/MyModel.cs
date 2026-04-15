namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.DynamicTextsBuilderBase;

internal  class MyModel<TValue>
{
    #region Constructor

    internal MyModel(TValue value)
    {
        Value = value;
    }

    #endregion

    #region Properties

    internal TValue Value { get; }

    #endregion
}