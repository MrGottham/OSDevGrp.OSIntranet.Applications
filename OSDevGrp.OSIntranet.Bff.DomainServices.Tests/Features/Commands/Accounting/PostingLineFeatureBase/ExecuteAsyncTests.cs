using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.PostingLineFeatureBase;

[TestFixture]
public class ExecuteAsyncTests
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_CallsGetPostingJournalAsyncWithCorrectAccountingNumber()
    {
        // Arrange
        TestPostingLineFeature sut = CreateSut();
        int accountingNumber = _fixture!.Create<int>();
        TestPostingLineFeatureRequest request = CreateTestPostingLineFeatureRequest(accountingNumber: accountingNumber);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        _accountingGatewayMock!.Verify(
            m => m.GetPostingJournalAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_CallsGetPostingJournalAsyncWithGivenCancellationToken()
    {
        // Arrange
        TestPostingLineFeature sut = CreateSut();
        TestPostingLineFeatureRequest request = CreateTestPostingLineFeatureRequest();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        // Act
        await sut.ExecuteAsync(request, cancellationToken);

        // Assert
        _accountingGatewayMock!.Verify(
            m => m.GetPostingJournalAsync(
                It.IsAny<int>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_CallsProcessPostingJournalAsyncWithExactPostingJournalInstance()
    {
        // Arrange
        ApplyPostingJournalModel postingJournalModel = _fixture!.Create<ApplyPostingJournalModel>();
        TestPostingLineFeature sut = CreateSut(postingJournalModel: postingJournalModel);
        TestPostingLineFeatureRequest request = CreateTestPostingLineFeatureRequest();

        ApplyPostingJournalModel? capturedPostingJournal = null;

        sut.SetupProcessPostingJournalAsync((journal, req, token) =>
        {
            capturedPostingJournal = journal;
            return Task.FromResult(journal);
        });

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(ReferenceEquals(capturedPostingJournal, postingJournalModel), Is.True,
            "ProcessPostingJournalAsync should receive the exact same ApplyPostingJournalModel instance returned from GetPostingJournalAsync");
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_CallsProcessPostingJournalAsyncWithRequest()
    {
        // Arrange
        TestPostingLineFeature sut = CreateSut();
        TestPostingLineFeatureRequest request = CreateTestPostingLineFeatureRequest();

        TestPostingLineFeatureRequest? capturedRequest = null;

        sut.SetupProcessPostingJournalAsync((journal, req, token) =>
        {
            capturedRequest = req;
            return Task.FromResult(journal);
        });

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(capturedRequest, Is.EqualTo(request));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_CallsProcessPostingJournalAsyncWithCancellationToken()
    {
        // Arrange
        TestPostingLineFeature sut = CreateSut();
        TestPostingLineFeatureRequest request = CreateTestPostingLineFeatureRequest();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        CancellationToken? capturedCancellationToken = null;

        sut.SetupProcessPostingJournalAsync((journal, req, token) =>
        {
            capturedCancellationToken = token;
            return Task.FromResult(journal);
        });

        // Act
        await sut.ExecuteAsync(request, cancellationToken);

        // Assert
        Assert.That(capturedCancellationToken, Is.EqualTo(cancellationToken));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_CallsSavePostingJournalAsyncWithCorrectAccountingNumber()
    {
        // Arrange
        TestPostingLineFeature sut = CreateSut();
        int accountingNumber = _fixture!.Create<int>();
        TestPostingLineFeatureRequest request = CreateTestPostingLineFeatureRequest(accountingNumber: accountingNumber);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        _accountingGatewayMock!.Verify(
            m => m.SavePostingJournalAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<ApplyPostingJournalModel>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_CallsSavePostingJournalAsyncWithExactPostingJournalInstance()
    {
        // Arrange
        ApplyPostingJournalModel fetchedPostingJournalModel = _fixture!.Create<ApplyPostingJournalModel>();
        ApplyPostingJournalModel modifiedPostingJournalModel = _fixture!.Create<ApplyPostingJournalModel>();
        
        TestPostingLineFeature sut = CreateSut(postingJournalModel: fetchedPostingJournalModel);
        TestPostingLineFeatureRequest request = CreateTestPostingLineFeatureRequest();

        ApplyPostingJournalModel? capturedSavedPostingJournal = null;

        sut.SetupProcessPostingJournalAsync((journal, req, token) =>
        {
            // Return a different model instance (simulating modification)
            return Task.FromResult(modifiedPostingJournalModel);
        });

        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel journal, CancellationToken _) => capturedSavedPostingJournal = journal)
            .ReturnsAsync(modifiedPostingJournalModel);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(ReferenceEquals(capturedSavedPostingJournal, modifiedPostingJournalModel), Is.True,
            "SavePostingJournalAsync should receive the exact model instance returned from ProcessPostingJournalAsync, not the fetched one");
        Assert.That(ReferenceEquals(capturedSavedPostingJournal, fetchedPostingJournalModel), Is.False,
            "SavePostingJournalAsync should NOT receive the original fetched model");
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_CallsSavePostingJournalAsyncWithGivenCancellationToken()
    {
        // Arrange
        TestPostingLineFeature sut = CreateSut();
        TestPostingLineFeatureRequest request = CreateTestPostingLineFeatureRequest();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        // Act
        await sut.ExecuteAsync(request, cancellationToken);

        // Assert
        _accountingGatewayMock!.Verify(
            m => m.SavePostingJournalAsync(
                It.IsAny<int>(),
                It.IsAny<ApplyPostingJournalModel>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenProcessPostingJournalAsyncThrows_PropagatesException()
    {
        // Arrange
        TestPostingLineFeature sut = CreateSut();
        TestPostingLineFeatureRequest request = CreateTestPostingLineFeatureRequest();

        InvalidOperationException testException = new InvalidOperationException("Test exception from ProcessPostingJournalAsync");
        sut.SetupProcessPostingJournalAsync((journal, req, token) =>
        {
            throw testException;
#pragma warning disable CS0162
            return Task.FromResult(journal);
#pragma warning restore CS0162
        });

        // Act & Assert
        InvalidOperationException caughtException = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await sut.ExecuteAsync(request))!;

        Assert.That(caughtException, Is.SameAs(testException));

        // Verify SavePostingJournalAsync was NOT called
        _accountingGatewayMock!.Verify(
            m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenGetPostingJournalAsyncThrows_PropagatesException()
    {
        // Arrange
        TestPostingLineFeature sut = CreateSut();
        TestPostingLineFeatureRequest request = CreateTestPostingLineFeatureRequest();

        InvalidOperationException testException = new InvalidOperationException("Test exception from GetPostingJournalAsync");
        _accountingGatewayMock!
            .Setup(m => m.GetPostingJournalAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(testException);

        // Act & Assert
        InvalidOperationException caughtException = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await sut.ExecuteAsync(request))!;

        Assert.That(caughtException, Is.SameAs(testException));

        // Verify SavePostingJournalAsync was NOT called
        _accountingGatewayMock!.Verify(
            m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenSavePostingJournalAsyncThrows_PropagatesException()
    {
        // Arrange
        TestPostingLineFeature sut = CreateSut();
        TestPostingLineFeatureRequest request = CreateTestPostingLineFeatureRequest();

        InvalidOperationException testException = new InvalidOperationException("Test exception from SavePostingJournalAsync");
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(testException);

        // Act & Assert
        InvalidOperationException caughtException = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await sut.ExecuteAsync(request))!;

        Assert.That(caughtException, Is.SameAs(testException));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenProcessPostingJournalAsyncReturnsModifiedModel_PassesReturnedModelToSavePostingJournalAsync()
    {
        // Arrange
        ApplyPostingJournalModel fetchedPostingJournalModel = _fixture!.Create<ApplyPostingJournalModel>();
        ApplyPostingJournalModel returnedPostingJournalModel = _fixture!.Create<ApplyPostingJournalModel>();

        TestPostingLineFeature sut = CreateSut(postingJournalModel: fetchedPostingJournalModel);
        TestPostingLineFeatureRequest request = CreateTestPostingLineFeatureRequest();

        ApplyPostingJournalModel? capturedSavedModel = null;

        sut.SetupProcessPostingJournalAsync((journal, req, token) =>
        {
            // Return a completely different model instance
            return Task.FromResult(returnedPostingJournalModel);
        });

        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel journal, CancellationToken _) => capturedSavedModel = journal)
            .ReturnsAsync(returnedPostingJournalModel);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(ReferenceEquals(capturedSavedModel, returnedPostingJournalModel), Is.True,
            "SavePostingJournalAsync should receive the exact model instance returned from ProcessPostingJournalAsync");
        Assert.That(ReferenceEquals(capturedSavedModel, fetchedPostingJournalModel), Is.False,
            "SavePostingJournalAsync should NOT receive the fetched model; it should receive what ProcessPostingJournalAsync returned");
    }

    #region Test doubles and helpers

    private sealed class TestPostingLineFeatureRequest : OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting.PostingJournalLineIdentificationRequestBase
    {
        public TestPostingLineFeatureRequest(
            Guid requestId,
            int accountingNumber,
            Guid identifier,
            ISecurityContext securityContext)
            : base(requestId, accountingNumber, identifier, securityContext)
        {
        }
    }

    private sealed class TestPostingLineFeature : OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting.PostingLineFeatureBase<TestPostingLineFeatureRequest>
    {
        private Func<ApplyPostingJournalModel, TestPostingLineFeatureRequest, CancellationToken, Task<ApplyPostingJournalModel>>? _processAction;

        public TestPostingLineFeature(
            IPermissionChecker permissionChecker,
            IAccountingGateway accountingGateway,
            IStaticTextProvider staticTextProvider)
            : base(permissionChecker, accountingGateway, staticTextProvider)
        {
        }

        public void SetupProcessPostingJournalAsync(
            Func<ApplyPostingJournalModel, TestPostingLineFeatureRequest, CancellationToken, Task<ApplyPostingJournalModel>> action)
        {
            _processAction = action;
        }

        protected override async Task<ApplyPostingJournalModel> ProcessPostingJournalAsync(
            ApplyPostingJournalModel postingJournal,
            TestPostingLineFeatureRequest request,
            CancellationToken cancellationToken)
        {
            if (_processAction != null)
            {
                return await _processAction.Invoke(postingJournal, request, cancellationToken);
            }

            return postingJournal;
        }
    }

    private TestPostingLineFeature CreateSut(ApplyPostingJournalModel? postingJournalModel = null)
    {
        ApplyPostingJournalModel modelToUse = postingJournalModel ?? _fixture!.Create<ApplyPostingJournalModel>();

        _accountingGatewayMock!
            .Setup(m => m.GetPostingJournalAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(modelToUse);

        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(modelToUse);

        return new TestPostingLineFeature(
            _permissionCheckerMock!.Object,
            _accountingGatewayMock.Object,
            _staticTextProviderMock!.Object);
    }

    private TestPostingLineFeatureRequest CreateTestPostingLineFeatureRequest(
        Guid? requestId = null,
        int? accountingNumber = null,
        Guid? identifier = null,
        ISecurityContext? securityContext = null)
    {
        return new TestPostingLineFeatureRequest(
            requestId ?? _fixture!.Create<Guid>(),
            accountingNumber ?? _fixture!.Create<int>(),
            identifier ?? _fixture!.Create<Guid>(),
            securityContext ?? CreateSecurityContext());
    }

    private ISecurityContext CreateSecurityContext()
    {
        Mock<ISecurityContext> securityContextMock = new Mock<ISecurityContext>();
        securityContextMock.Setup(m => m.User).Returns(_fixture!.Create<System.Security.Claims.ClaimsPrincipal>());
        return securityContextMock.Object;
    }

    #endregion
}