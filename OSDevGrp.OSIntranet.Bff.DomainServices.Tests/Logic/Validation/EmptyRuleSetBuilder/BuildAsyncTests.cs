using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.EmptyRuleSetBuilder;

[TestFixture]
public class BuildAsyncTests
{
    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsEmptyValidationRuleSet()
    {
        IEmptyRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.Empty);
    }

    private IEmptyRuleSetBuilder CreateSut()
    {

        return new DomainServices.Logic.Validation.EmptyRuleSetBuilder();
    }
}