using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.DynamicTextsBuilderBase;

internal class MyValueDisplayerText : IDynamicTexts
{
    #region Constructor

    internal MyValueDisplayerText(IValueDisplayer valueDisplayer)
    {
        ValueDisplayer = valueDisplayer;
    }

    #endregion

    #region Properties

    internal IValueDisplayer ValueDisplayer { get; }

    #endregion
}