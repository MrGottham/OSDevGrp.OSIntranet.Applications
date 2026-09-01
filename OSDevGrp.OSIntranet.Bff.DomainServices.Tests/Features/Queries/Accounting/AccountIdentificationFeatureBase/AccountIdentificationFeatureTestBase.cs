using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ValidationRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountIdentificationFeatureBase;

public abstract class AccountIdentificationFeatureTestBase : AccountingPageFeatureTestBase
{
    #region Methods

    protected static IPermissionVerifiable<MyAccountIdentificationRequest> CreateSut(Fixture fixture, Mock<IPermissionChecker> permissionCheckerMock, Mock<IAccountingGateway> accountingGatewayMock, Mock<IStaticTextProvider> staticTextProviderMock, Mock<IDynamicTextsBuilder<object, IDynamicTexts>> dynamicTextsBuilderMock, Mock<IValidationRuleSetBuilder> validationRuleSetBuilderMock, bool isAuthenticated = true, bool hasAccountingAccess = true, bool isAccountingViewer = true, Func<MyAccountIdentificationRequest, CancellationToken, Task<object>>? modelGetter = null, Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, IReadOnlyCollection<IValidationRule>, CancellationToken, Task<MyAccountIdentificationResponse>>? responseBuilder = null, Func<MyAccountIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>>? staticTextSpecificationsGetter = null, IDynamicTexts? dynamicTexts = null, IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        permissionCheckerMock.Setup(fixture, isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer);
        staticTextProviderMock.Setup(fixture);
        dynamicTextsBuilderMock.Setup(dynamicTexts: dynamicTexts);
        validationRuleSetBuilderMock.Setup(fixture, validationRuleSet: validationRuleSet);

        modelGetter ??= (_, _) => Task.FromResult(new object());
        responseBuilder ??= (m, st, dt, vrs, _) => Task.FromResult(new MyAccountIdentificationResponse(m, dt, st, vrs));
        staticTextSpecificationsGetter ??= (_, _) => new Dictionary<StaticTextKey, IEnumerable<object>>();

        return new MyAccountIdentificationFeature(modelGetter, responseBuilder, staticTextSpecificationsGetter, permissionCheckerMock.Object, accountingGatewayMock.Object, staticTextProviderMock.Object, dynamicTextsBuilderMock.Object, validationRuleSetBuilderMock.Object);
    }

    protected static IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> CreateSutAsQueryFeature(Fixture fixture, Mock<IPermissionChecker> permissionCheckerMock, Mock<IAccountingGateway> accountingGatewayMock, Mock<IStaticTextProvider> staticTextProviderMock, Mock<IDynamicTextsBuilder<object, IDynamicTexts>> dynamicTextsBuilderMock, Mock<IValidationRuleSetBuilder> validationRuleSetBuilderMock, bool isAuthenticated = true, bool hasAccountingAccess = true, bool isAccountingViewer = true, Func<MyAccountIdentificationRequest, CancellationToken, Task<object>>? modelGetter = null, Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, IReadOnlyCollection<IValidationRule>, CancellationToken, Task<MyAccountIdentificationResponse>>? responseBuilder = null, Func<MyAccountIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>>? staticTextSpecificationsGetter = null, IDynamicTexts? dynamicTexts = null, IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        return (IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse>)CreateSut(fixture, permissionCheckerMock, accountingGatewayMock, staticTextProviderMock, dynamicTextsBuilderMock, validationRuleSetBuilderMock, isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer, modelGetter: modelGetter, responseBuilder: responseBuilder, staticTextSpecificationsGetter: staticTextSpecificationsGetter, dynamicTexts: dynamicTexts, validationRuleSet: validationRuleSet);
    }

    protected static MyAccountIdentificationRequest CreateAccountIdentificationRequest(Fixture fixture, int? accountingNumber = null, string? accountNumber = null, DateTimeOffset? statusDate = null, IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null)
    {
        return new MyAccountIdentificationRequest(Guid.NewGuid(), accountingNumber ?? fixture.Create<int>(), accountNumber ?? fixture.Create<string>(), statusDate ?? fixture.Create<DateTimeOffset>(), formatProvider ?? CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext(fixture));
    }

    #endregion

    #region Private classes

    private class MyAccountIdentificationFeature : AccountIdentificationFeatureBase<MyAccountIdentificationRequest, MyAccountIdentificationResponse, object, IDynamicTexts, IDynamicTextsBuilder<object, IDynamicTexts>, IValidationRuleSetBuilder>
    {
        #region Private variables

        private readonly Func<MyAccountIdentificationRequest, CancellationToken, Task<object>> _modelGetter;
        private readonly Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, IReadOnlyCollection<IValidationRule>, CancellationToken, Task<MyAccountIdentificationResponse>> _responseBuilder;
        private readonly Func<MyAccountIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> _staticTextSpecificationsGetter;

        #endregion

        #region Constructor

        public MyAccountIdentificationFeature(Func<MyAccountIdentificationRequest, CancellationToken, Task<object>> modelGetter, Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, IReadOnlyCollection<IValidationRule>, CancellationToken, Task<MyAccountIdentificationResponse>> responseBuilder, Func<MyAccountIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter, IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, IStaticTextProvider staticTextProvider, IDynamicTextsBuilder<object, IDynamicTexts> dynamicTextsBuilder, IValidationRuleSetBuilder validationRuleSetBuilder)
            : base(permissionChecker, accountingGateway, staticTextProvider, dynamicTextsBuilder, validationRuleSetBuilder)
        {
            _modelGetter = modelGetter;
            _responseBuilder = responseBuilder;
            _staticTextSpecificationsGetter = staticTextSpecificationsGetter;
        }

        #endregion

        #region Methods

        protected override Task<object> GetModelAsync(MyAccountIdentificationRequest request, CancellationToken cancellationToken) => _modelGetter(request, cancellationToken);

        protected override Task<MyAccountIdentificationResponse> BuildResponseAsync(object model, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IDynamicTexts dynamicTexts, IReadOnlyCollection<IValidationRule> validationRuleSet, CancellationToken cancellationToken) => _responseBuilder(model, staticTexts, dynamicTexts, validationRuleSet, cancellationToken);

        protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(MyAccountIdentificationRequest request, object model) => _staticTextSpecificationsGetter(request, model);

        #endregion
    }

    #endregion
}