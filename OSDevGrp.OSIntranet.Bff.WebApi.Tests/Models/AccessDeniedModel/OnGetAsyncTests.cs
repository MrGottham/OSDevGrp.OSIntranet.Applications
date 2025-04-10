using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Models.AccessDeniedModel;

[TestFixture]
public class OnGetAsyncTests
{
    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenCalled_ReturnsNotNull()
    {
        WebApi.Models.AccessDeniedModel sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenCalled_ReturnsPageResult()
    {
        WebApi.Models.AccessDeniedModel sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<PageResult>());
    }

    private WebApi.Models.AccessDeniedModel CreateSut()
    {
        return new WebApi.Models.AccessDeniedModel();
    }
}