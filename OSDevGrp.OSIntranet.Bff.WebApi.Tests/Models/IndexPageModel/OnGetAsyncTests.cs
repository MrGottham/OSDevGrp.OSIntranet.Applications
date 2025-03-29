using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Models.IndexPageModel;

[TestFixture]
public class OnGetAsyncTests
{
    #region Private variables

    private Mock<IWebHostEnvironment>? _webHostEnvironmentMock;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_AssertEnvironmentNameWasCalledOnWebHostEnvironment(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        _webHostEnvironmentMock!.Verify(m => m.EnvironmentName, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_AssertTitleIsIntiailizedWithSpecificText(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.Title, Is.EqualTo(ProgramHelper.GetTitle()));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_AssertDescriptionIsIntiailizedWithSpecificText(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.Description, Is.EqualTo(ProgramHelper.GetDescription()));
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenEnviromentIsDevelopment_AssertOpenApiDocumentUrlIsIntiailizedWithSpecificOpenApiDocumentUrl()
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: true);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.OpenApiDocumentUrl, Is.EqualTo(ProgramHelper.GetOpenApiDocumentUrl(_webHostEnvironmentMock!.Object)));
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenEnviromentIsDevelopment_AssertOpenApiDocumentNameIsIntiailizedWithSpecificOpenApiDocumentName()
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: true);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.OpenApiDocumentName, Is.EqualTo(ProgramHelper.GetOpenApiDocumentName()));
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenEnviromentIsNotDevelopment_AssertOpenApiDocumentUrlIsNotIntiailized()
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: false);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.OpenApiDocumentUrl, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenEnviromentIsNotDevelopment_AssertOpenApiDocumentNameIsNotIntiailized()
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: false);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.OpenApiDocumentName, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_ReturnsNotNull(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_ReturnsPageResult(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<PageResult>());
    }

    private WebApi.Models.IndexPageModel CreateSut(bool isDevelopment = true)
    {
        _webHostEnvironmentMock!.Setup(m => m.EnvironmentName)
            .Returns(isDevelopment ? "Development" : "Production");

        return new WebApi.Models.IndexPageModel(_webHostEnvironmentMock!.Object);
    }
}