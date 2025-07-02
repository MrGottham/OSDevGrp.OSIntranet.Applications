using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountingIdentificationFeatureBase;

public abstract class AccountingIdentificationFeatureTestBase : AccountingPageFeatureTestBase
{
    #region Methods

    protected static IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> CreateSut(Fixture fixture, Mock<IPermissionChecker> permissionCheckerMock, Mock<IAccountingGateway> accountingGatewayMock, Mock<IStaticTextProvider> staticTextProviderMock, Mock<IDynamicTextsBuilder<object, IDynamicTexts>> dynamicTextsBuilderMock, bool isAuthenticated = true, bool hasAccountingAccess = true, bool isAccountingViewer = true, Func<MyAccountingIdentificationRequest, CancellationToken, Task<object>>? modelGetter = null, Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, CancellationToken, Task<MyAccountingIdentificationResponse>>? responseBuilder = null, Func<MyAccountingIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>>? staticTextSpecificationsGetter = null, IDynamicTexts? dynamicTexts = null)
    {
        permissionCheckerMock.Setup(fixture, isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer);
        staticTextProviderMock.Setup(fixture);
        dynamicTextsBuilderMock.Setup(dynamicTexts: dynamicTexts);

        modelGetter ??= (_, _) => Task.FromResult(new object());
        responseBuilder ??= (model, staticTexts, dynamicTexts, _) => Task.FromResult(new MyAccountingIdentificationResponse(model, dynamicTexts, staticTexts));
        staticTextSpecificationsGetter ??= (_, _) => new Dictionary<StaticTextKey, IEnumerable<object>>();

        return new MyAccountingIdentificationFeature(modelGetter, responseBuilder, staticTextSpecificationsGetter, permissionCheckerMock.Object, accountingGatewayMock.Object, staticTextProviderMock.Object, dynamicTextsBuilderMock.Object);
    }

    protected static MyAccountingIdentificationRequest CreateAccountingIdentificationRequest(Fixture fixture, int? accountingNumber = null, DateTimeOffset? statusDate = null, IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null)
    {
        return new MyAccountingIdentificationRequest(Guid.NewGuid(), accountingNumber ?? fixture.Create<int>(), statusDate ?? fixture.Create<DateTimeOffset>(), formatProvider ?? CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext(fixture));
    }

    #endregion

    #region Private classes

    private class MyAccountingIdentificationFeature : AccountingIdentificationFeatureBase<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse, object, IDynamicTexts, IDynamicTextsBuilder<object, IDynamicTexts>>
    {
        #region Private variables

        private readonly Func<MyAccountingIdentificationRequest, CancellationToken, Task<object>> _modelGetter;
        private readonly Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, CancellationToken, Task<MyAccountingIdentificationResponse>> _responseBuilder;
        private readonly Func<MyAccountingIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> _staticTextSpecificationsGetter;

        #endregion

        #region Methods

        public MyAccountingIdentificationFeature(Func<MyAccountingIdentificationRequest, CancellationToken, Task<object>> modelGetter, Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, CancellationToken, Task<MyAccountingIdentificationResponse>> responseBuilder, Func<MyAccountingIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter, IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, IStaticTextProvider staticTextProvider, IDynamicTextsBuilder<object, IDynamicTexts> dynamicTextsBuilder)
            : base(permissionChecker, accountingGateway, staticTextProvider, dynamicTextsBuilder)
        {
            _modelGetter = modelGetter;
            _responseBuilder = responseBuilder;
            _staticTextSpecificationsGetter = staticTextSpecificationsGetter;
        }

        #endregion

        #region Methods

        protected override Task<object> GetModelAsync(MyAccountingIdentificationRequest request, CancellationToken cancellationToken) => _modelGetter(request, cancellationToken);

        protected override Task<MyAccountingIdentificationResponse> BuildResponseAsync(object model, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IDynamicTexts dynamicTexts, CancellationToken cancellationToken) => _responseBuilder(model, staticTexts, dynamicTexts, cancellationToken);

        protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(MyAccountingIdentificationRequest request, object model) => _staticTextSpecificationsGetter(request, model);

        #endregion
    }

    #endregion
}