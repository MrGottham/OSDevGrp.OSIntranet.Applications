using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class AccountTextsBuilder : DynamicTextsBuilderBase<AccountModel, IAccountTexts>, IAccountTextsBuilder
{
    #region Constructor

    public AccountTextsBuilder(IStaticTextProvider staticTextProvider) 
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public override Task<IAccountTexts> BuildAsync(AccountModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return Task.Run<IAccountTexts>(() => new AccountTexts(model, formatProvider), cancellationToken);
    }

    #endregion
}