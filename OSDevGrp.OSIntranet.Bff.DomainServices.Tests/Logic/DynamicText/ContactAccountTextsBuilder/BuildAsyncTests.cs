using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.ContactAccountTextsBuilder;

[TestFixture]
public class BuildAsyncTests
{
    #region Private variables

    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsContactAccountTexts()
    {
        IContactAccountTextsBuilder sut = CreateSut();

        ContactAccountModel contactAccountModel = _fixture!.CreateContactAccountModel(_random!);
        IContactAccountTexts result = await sut.BuildAsync(contactAccountModel, CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<ContactAccountTexts>());
    }

    private IContactAccountTextsBuilder CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.DynamicText.ContactAccountTextsBuilder(_staticTextProviderMock!.Object);
    }
}