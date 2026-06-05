using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ExtendedValidationRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PatternRuleFactory;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ContactAccountNumberRuleSetBuilder;

[TestFixture]
public class BuildAsyncTests
{
    #region Private variables

    private Mock<IExtendedValidationRuleSetBuilder>? _extendedValidationRuleSetBuilderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _extendedValidationRuleSetBuilderMock = new Mock<IExtendedValidationRuleSetBuilder>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithRequiredValueRuleWasNotCalledOnExtendedValidationRuleSetBuilder()
    {
        IContactAccountNumberRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithRequiredValueRule(It.IsAny<StaticTextKey>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithMinLengthRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndMinLengthForContactAccount()
    {
        IContactAccountNumberRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithMinLengthRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.ContactAccount),
                It.Is<int>(value => value == AccountingRuleSetSpecifications.AccountNumberMinLength)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithMaxLengthRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndMaxLengthForContactAccount()
    {
        IContactAccountNumberRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithMaxLengthRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.ContactAccount),
                It.Is<int>(value => value == AccountingRuleSetSpecifications.AccountNumberMaxLength)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithPatternRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndPatternForContactAccount()
    {
        IContactAccountNumberRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithPatternRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.ContactAccount),
                It.Is<string>(value => value == AccountingRuleSetSpecifications.AccountNumberRegexPattern)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnExtendedValidationRuleSetBuilderWithGivenFormatProvider()
    {
        IContactAccountNumberRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnExtendedValidationRuleSetBuilderWithGivenCancellationToken()
    {
        IContactAccountNumberRuleSetBuilder sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsValidationRuleSetBuildedByExtendedValidationRuleSetBuilder()
    {
        IValidationRule[] validationRuleSet =
        [
            _fixture!.CreateMinLengthRule(),
            _fixture!.CreateMaxLengthRule(),
            _fixture!.CreatePatternRule(),
        ];
        IContactAccountNumberRuleSetBuilder sut = CreateSut(validationRuleSet: validationRuleSet);

        IReadOnlyCollection<IValidationRule> result = await sut.BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.EqualTo(validationRuleSet));
    }

    private IContactAccountNumberRuleSetBuilder CreateSut(IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        _extendedValidationRuleSetBuilderMock!.Setup(_fixture!, validationRuleSet: validationRuleSet);

        return new DomainServices.Logic.Validation.ContactAccountNumberRuleSetBuilder(_extendedValidationRuleSetBuilderMock!.Object);
    }
}