using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal abstract class DynamicTextsBase<TModel> : IDynamicTexts where TModel : class
{
    #region Constructor

    protected DynamicTextsBase(TModel model, IFormatProvider formatProvider)
    {
        Model = model;
        FormatProvider = formatProvider;
    }

    #endregion

    #region Properties

    protected TModel Model { get; }

    protected IFormatProvider FormatProvider { get; }

    #endregion
}