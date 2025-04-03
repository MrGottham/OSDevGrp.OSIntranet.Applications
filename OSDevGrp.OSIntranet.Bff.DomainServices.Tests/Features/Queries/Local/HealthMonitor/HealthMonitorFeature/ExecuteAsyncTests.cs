using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Local.HealthMonitor;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DependencyHealth;
using OSDevGrp.OSIntranet.Bff.DomainServices.Models.Local;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DependencyHealth.DependencyHealthMonitor;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Local.HealthMonitor.HealthMonitorFeature;

[TestFixture]
public class ExecuteAsyncTests
{
    #region Private variables

    private Mock<IDependencyHealthMonitor>? _dependencyHealthMonitorMock;
    private Fixture? _fixture;
    private Random? random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _dependencyHealthMonitorMock = new Mock<IDependencyHealthMonitor>();
        _fixture = new Fixture();
        random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertCheckHealthAsyncWasCalledOnDependencyHealthMonitorWithEachDependencyHealthModelFromGivenHealtMonitorRequest()
    {
        IQueryFeature<HealthMonitorRequest, HealthMonitorResponse> sut = CreateSut();

        DependencyHealthModel[] dependencies = CreateDependencyHealthModelCollection();
        HealthMonitorRequest healthMonitorRequest = CreateHealthMonitorRequest(dependencies);
        await sut.ExecuteAsync(healthMonitorRequest);

        foreach (DependencyHealthModel dependencyHealthModel in dependencies)
        {
            _dependencyHealthMonitorMock!.Verify(m => m.CheckHealthAsync(It.Is<DependencyHealthModel>(value => value == dependencyHealthModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertCheckHealthAsyncWasCalledOnDependencyHealthMonitorWithGivenCancellationTokenForEachDependencyHealthModelFromGivenHealtMonitorRequest()
    {
        IQueryFeature<HealthMonitorRequest, HealthMonitorResponse> sut = CreateSut();

        DependencyHealthModel[] dependencies = CreateDependencyHealthModelCollection();
        HealthMonitorRequest healthMonitorRequest = CreateHealthMonitorRequest(dependencies);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(healthMonitorRequest, cancellationToken);

        _dependencyHealthMonitorMock!.Verify(m => m.CheckHealthAsync(It.IsAny<DependencyHealthModel>(), It.Is<CancellationToken>(value => value == cancellationToken)), Times.Exactly(dependencies.Length));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsHealthMonitorResponseWhereXContainsXForEachDependencyHealthModelFromGivenHealtMonitorRequest()
    {
        IQueryFeature<HealthMonitorRequest, HealthMonitorResponse> sut = CreateSut();

        DependencyHealthModel[] dependencies = CreateDependencyHealthModelCollection();
        HealthMonitorRequest healthMonitorRequest = CreateHealthMonitorRequest(dependencies);
        HealthMonitorResponse result = await sut.ExecuteAsync(healthMonitorRequest);

        Assert.That(dependencies.All(dependencyHealthModel => result.Dependencies.SingleOrDefault(dependencyHealthResultModel => dependencyHealthResultModel.Description == dependencyHealthModel.Description && dependencyHealthResultModel.HealthEndpoint == dependencyHealthModel.HealthEndpoint) != null), Is.True);
    }

    private IQueryFeature<HealthMonitorRequest, HealthMonitorResponse> CreateSut()
    {
        _dependencyHealthMonitorMock!.Setup(_fixture!);

        return new DomainServices.Features.Queries.Local.HealthMonitor.HealthMonitorFeature(_dependencyHealthMonitorMock!.Object);
    }

    private HealthMonitorRequest CreateHealthMonitorRequest(IEnumerable<DependencyHealthModel> dependencies)
    {

        return new HealthMonitorRequest(dependencies ?? CreateDependencyHealthModelCollection(), Guid.NewGuid(), _fixture!.CreateSecurityContext());
    }

    private DependencyHealthModel[] CreateDependencyHealthModelCollection()
    {
        return
        [
            CreateDependencyHealthModel(),
            CreateDependencyHealthModel(),
            CreateDependencyHealthModel(),
        ];
    }

    private DependencyHealthModel CreateDependencyHealthModel()
    {
        return new DependencyHealthModel(_fixture!.Create<string>(), new Uri($"https://{_fixture!.Create<string>()}.local/{_fixture!.Create<string>()}"));
    }
}