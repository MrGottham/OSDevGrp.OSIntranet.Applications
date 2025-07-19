using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextKeyExtensions;

[TestFixture]
public class DefaultArgumentsTests
{
    #region Private variables

    private readonly static IEnumerable<StaticTextKey> StaticTextKeyWithDefaultArguments =
    [
        StaticTextKey.BalanceBelowZero
    ];

    #endregion

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.BalanceBelowZero)]
    public void DefaultArguments_WhenStaticTextKeyHasDefaultArguments_ReturnsNonEmptyArgumentCollection(StaticTextKey staticTextKey)
    {
        IEnumerable<object> result = DomainServices.Logic.StaticText.StaticTextKeyExtensions.DefaultArguments(staticTextKey);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.BalanceBelowZero, 1)]
    public void DefaultArguments_WhenStaticTextKeyHasDefaultArguments_ReturnsNonEmptyArgumentCollectionWithSpecifiedNumberOfArguments(StaticTextKey staticTextKey, int expectedArguemnts)
    {
        IEnumerable<object> result = DomainServices.Logic.StaticText.StaticTextKeyExtensions.DefaultArguments(staticTextKey);

        Assert.That(result.Count(), Is.EqualTo(expectedArguemnts));
    }

    [Test]
    [Category("UnitTest")]
    public void DefaultArguments_WhenCalledWithBalanceBelowZero_ReturnsNonEmptyArgumentCollectionWithSpecifieArguments()
    {
        IEnumerable<object> result = DomainServices.Logic.StaticText.StaticTextKeyExtensions.DefaultArguments(StaticTextKey.BalanceBelowZero);

        Assert.That(result.ElementAt(0), Is.EqualTo(0));
    }

    [Test]
    [Category("UnitTest")]
    public void DefaultArguments_WhenStaticTextKeyDoesNotHaveAnyDefaultArguments_ReturnsEmptyArgumentCollection()
    {
        foreach (StaticTextKey staticTextKey in Enum.GetValues<StaticTextKey>().OfType<StaticTextKey>().Except(StaticTextKeyWithDefaultArguments))
        {
            IEnumerable<object> result = DomainServices.Logic.StaticText.StaticTextKeyExtensions.DefaultArguments(staticTextKey);

            Assert.That(result, Is.Empty);
        }
    }
}