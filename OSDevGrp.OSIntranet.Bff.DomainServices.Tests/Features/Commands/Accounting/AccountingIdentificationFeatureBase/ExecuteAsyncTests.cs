using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.AccountingIdentificationFeatureBase;

[TestFixture]
public class ExecuteAsyncTests : AccountingIdentificationFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Fixture? _fixture;

    #endregion

    #region Setup

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _fixture = new Fixture();
    }

    #endregion

    #region Tests

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertExecuteAsyncWasCalledWithGivenRequest()
    {
        TestAccountingIdentificationRequest? capturedRequest = null;
        Func<TestAccountingIdentificationRequest, CancellationToken, Task> executeAsyncDelegate = (req, _) =>
        {
            capturedRequest = req;
            return Task.CompletedTask;
        };

        ICommandFeature<TestAccountingIdentificationRequest> sut = CreateSut<TestAccountingIdentificationRequest>(
            _fixture!,
            _permissionCheckerMock!,
            _accountingGatewayMock!,
            executeAsyncDelegate: executeAsyncDelegate);

        TestAccountingIdentificationRequest request = CreateAccountingIdentificationRequest<TestAccountingIdentificationRequest>(_fixture!);
        await sut.ExecuteAsync(request);

        Assert.That(capturedRequest, Is.EqualTo(request));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertExecuteAsyncWasCalledWithGivenCancellationToken()
    {
        CancellationToken? capturedToken = null;
        Func<TestAccountingIdentificationRequest, CancellationToken, Task> executeAsyncDelegate = (_, token) =>
        {
            capturedToken = token;
            return Task.CompletedTask;
        };

        ICommandFeature<TestAccountingIdentificationRequest> sut = CreateSut<TestAccountingIdentificationRequest>(
            _fixture!,
            _permissionCheckerMock!,
            _accountingGatewayMock!,
            executeAsyncDelegate: executeAsyncDelegate);

        CancellationTokenSource cts = new();
        CancellationToken token = cts.Token;

        TestAccountingIdentificationRequest request = CreateAccountingIdentificationRequest<TestAccountingIdentificationRequest>(_fixture!);
        await sut.ExecuteAsync(request, token);

        Assert.That(capturedToken, Is.EqualTo(token));
    }

    #endregion

    #region Nested classes

    private class TestAccountingIdentificationRequest : OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting.AccountingIdentificationRequestBase
    {
        public TestAccountingIdentificationRequest(Guid requestId, int accountingNumber, ISecurityContext securityContext)
            : base(requestId, accountingNumber, securityContext)
        {
        }
    }

    #endregion
}