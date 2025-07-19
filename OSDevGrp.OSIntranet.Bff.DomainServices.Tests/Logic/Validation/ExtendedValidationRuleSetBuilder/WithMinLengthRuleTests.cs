using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ExtendedValidationRuleSetBuilder;

[TestFixture]
public class WithMinLengthRuleTests : ExtendedValidationRuleSetBuilderTestBase
{
    #region Private variables

    private Mock<IRequiredValueRuleFactory>? _requiredValueRuleFactoryMock;
    private Mock<IMinLengthRuleFactory>? _minLengthRuleFactoryMock;
    private Mock<IMaxLengthRuleFactory>? _maxLengthRuleFactoryMock;
    private Mock<IMinValueRuleFactory>? _minValueRuleFactoryMock;
    private Mock<IMaxValueRuleFactory>? _maxValueRuleFactoryMock;
    private Mock<IPatternRuleFactory>? _patternRuleFactoryMock;
    private Mock<IOneOfRuleFactory>? _oneOfRuleFactoryMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _requiredValueRuleFactoryMock = new Mock<IRequiredValueRuleFactory>();
        _minLengthRuleFactoryMock = new Mock<IMinLengthRuleFactory>();
        _maxLengthRuleFactoryMock = new Mock<IMaxLengthRuleFactory>();
        _minValueRuleFactoryMock = new Mock<IMinValueRuleFactory>();
        _maxValueRuleFactoryMock = new Mock<IMaxValueRuleFactory>();
        _patternRuleFactoryMock = new Mock<IPatternRuleFactory>();
        _oneOfRuleFactoryMock = new Mock<IOneOfRuleFactory>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public void WithMinLengthRule_WhenCalled_AssertCreateAsyncWasNotCalledOnRequiredValueRuleFactory(StaticTextKey staticTextKey)
    {
        IExtendedValidationRuleSetBuilder sut = CreateSut();

        sut.WithMinLengthRule(staticTextKey, _fixture!.Create<int>());

        _requiredValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public void WithMinLengthRule_WhenCalled_AssertCreateAsyncWasNotCalledOnMinLengthRuleFactory(StaticTextKey staticTextKey)
    {
        IExtendedValidationRuleSetBuilder sut = CreateSut();

        sut.WithMinLengthRule(staticTextKey, _fixture!.Create<int>());

        _minLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public void WithMinLengthRule_WhenCalled_AssertCreateAsyncWasNotCalledOnMaxLengthRuleFactory(StaticTextKey staticTextKey)
    {
        IExtendedValidationRuleSetBuilder sut = CreateSut();

        sut.WithMinLengthRule(staticTextKey, _fixture!.Create<int>());

        _maxLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public void WithMinLengthRule_WhenCalled_AssertCreateAsyncWasNotCalledOnMinValueRuleFactory(StaticTextKey staticTextKey)
    {
        IExtendedValidationRuleSetBuilder sut = CreateSut();

        sut.WithMinLengthRule(staticTextKey, _fixture!.Create<int>());

        _minValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public void WithMinLengthRule_WhenCalled_AssertCreateAsyncWasNotCalledOnMaxValueRuleFactoryMock(StaticTextKey staticTextKey)
    {
        IExtendedValidationRuleSetBuilder sut = CreateSut();

        sut.WithMinLengthRule(staticTextKey, _fixture!.Create<int>());

        _maxValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public void WithMinLengthRule_WhenCalled_AssertCreateAsyncWasNotCalledOnPatternRuleFactory(StaticTextKey staticTextKey)
    {
        IExtendedValidationRuleSetBuilder sut = CreateSut();

        sut.WithMinLengthRule(staticTextKey, _fixture!.Create<int>());

        _patternRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<Regex>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public void WithMinLengthRule_WhenCalled_AssertCreateAsyncWasNotCalledOnOneOfRuleFactory(StaticTextKey staticTextKey)
    {
        IExtendedValidationRuleSetBuilder sut = CreateSut();

        sut.WithMinLengthRule(staticTextKey, _fixture!.Create<int>());

        _oneOfRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<IReadOnlyCollection<IValueSpecification<int>>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public void WithMinLengthRule_WhenCalled_ReturnsSameExtendedValidationRuleSetBuilder(StaticTextKey staticTextKey)
    {
        IExtendedValidationRuleSetBuilder sut = CreateSut();

        IExtendedValidationRuleSetBuilder result = sut.WithMinLengthRule(staticTextKey, _fixture!.Create<int>());

        Assert.That(result, Is.SameAs(sut));
    }

    private IExtendedValidationRuleSetBuilder CreateSut()
    {
        return CreateSut(_fixture!, _requiredValueRuleFactoryMock!, _minLengthRuleFactoryMock!, _maxLengthRuleFactoryMock!, _minValueRuleFactoryMock!, _maxValueRuleFactoryMock!, _patternRuleFactoryMock!, _oneOfRuleFactoryMock!);
    }
}