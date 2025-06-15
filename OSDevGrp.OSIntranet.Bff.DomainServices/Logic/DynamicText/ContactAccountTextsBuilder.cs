using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ContactAccountTextsBuilder : DynamicTextsBuilderBase<ContactAccountModel, IContactAccountTexts>, IContactAccountTextsBuilder
{
    #region Constructor

    public ContactAccountTextsBuilder(IStaticTextProvider staticTextProvider) 
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public override Task<IContactAccountTexts> BuildAsync(ContactAccountModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return Task.Run<IContactAccountTexts>(() => new ContactAccountTexts(model, formatProvider), cancellationToken);
    }

    #endregion
}