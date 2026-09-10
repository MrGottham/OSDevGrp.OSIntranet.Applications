using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;

internal abstract class AccountIdentificationFeatureBase<TAccountIdentificationRequest, TAccountIdentificationResponse, TModel, TDynamicTexts, TDynamicTextsBuilder, TValidationRuleSetBuilder> : AccountingIdentificationFeatureBase<TAccountIdentificationRequest, TAccountIdentificationResponse, TModel, TDynamicTexts, TDynamicTextsBuilder, TValidationRuleSetBuilder> where TAccountIdentificationRequest : AccountIdentificationRequestBase where TAccountIdentificationResponse : AccountIdentificationResponseBase<TModel, TDynamicTexts> where TModel : class where TDynamicTexts : IDynamicTexts where TDynamicTextsBuilder : IDynamicTextsBuilder<TModel, TDynamicTexts> where TValidationRuleSetBuilder : IValidationRuleSetBuilder
{
    #region Constructor

    protected AccountIdentificationFeatureBase(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, IStaticTextProvider staticTextProvider, TDynamicTextsBuilder dynamicTextsBuilder, TValidationRuleSetBuilder validationRuleSetBuilder)
        : base(permissionChecker, accountingGateway, staticTextProvider, dynamicTextsBuilder, validationRuleSetBuilder)
    {
    }

    #endregion

    #region Properties

    #endregion

    #region Methods

    #endregion
}