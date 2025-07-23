using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Shared.Dtos;

internal static class FixtureExtensions
{
    #region Methods

    internal static IUserInfoModel CreateUserInfoModel(this Fixture fixture, Random? random = null, bool hasNameIdentifier = true, bool hasName = true, bool hasMailAddress = true, bool hasAccountingAccess = true, bool hasDefaultAccountingNumber = true, bool isAccountingAdministrator = true, bool isAccountingCreator = true, bool isAccountingModifier = true, bool isAccountingViewer = true, bool hasCommonDataAccess = true)
    {
        return fixture.CreateUserInfoModelMock(random, hasNameIdentifier, hasName, hasMailAddress, hasAccountingAccess, hasDefaultAccountingNumber, isAccountingAdministrator, isAccountingCreator, isAccountingModifier, isAccountingViewer, hasCommonDataAccess).Object;
    }

    internal static Mock<IUserInfoModel> CreateUserInfoModelMock(this Fixture fixture, Random? random = null, bool hasNameIdentifier = true, bool hasName = true, bool hasMailAddress = true, bool hasAccountingAccess = true, bool hasDefaultAccountingNumber = true, bool isAccountingAdministrator = true, bool isAccountingCreator = true, bool isAccountingModifier = true, bool isAccountingViewer = true, bool hasCommonDataAccess = true)
    {
        Mock<IUserInfoModel> userInfoModelMock = new Mock<IUserInfoModel>();
        userInfoModelMock.Setup(m => m.NameIdentifier)
            .Returns(hasNameIdentifier ? fixture.Create<string>() : null);
        userInfoModelMock.Setup(m => m.Name)
            .Returns(hasName ? fixture.Create<string>() : null);
        userInfoModelMock.Setup(m => m.MailAddress)
            .Returns(hasMailAddress ? $"{fixture.Create<string>()}@{fixture.Create<string>()}.local" : null);
        userInfoModelMock.Setup(m => m.HasAccountingAccess)
            .Returns(hasAccountingAccess);
        userInfoModelMock.Setup(m => m.DefaultAccountingNumber)
            .Returns(hasAccountingAccess ? hasDefaultAccountingNumber ? fixture.Create<int>() : null : null);
        userInfoModelMock.Setup(m => m.Accountings)
            .Returns(hasAccountingAccess ? fixture.CreateAccontingDictionary(random) : new Dictionary<int, string>());
        userInfoModelMock.Setup(m => m.IsAccountingAdministrator)
            .Returns(hasAccountingAccess && isAccountingAdministrator);
        userInfoModelMock.Setup(m => m.IsAccountingCreator)
            .Returns(hasAccountingAccess && isAccountingCreator);
        userInfoModelMock.Setup(m => m.IsAccountingModifier)
            .Returns(hasAccountingAccess && isAccountingModifier);
        userInfoModelMock.Setup(m => m.ModifiableAccountings)
            .Returns(hasAccountingAccess && isAccountingModifier ? fixture.CreateAccontingDictionary(random) : new Dictionary<int, string>());
        userInfoModelMock.Setup(m => m.IsAccountingViewer)
            .Returns(hasAccountingAccess && isAccountingViewer);
        userInfoModelMock.Setup(m => m.ViewableAccountings)
            .Returns(hasAccountingAccess && isAccountingModifier ? fixture.CreateAccontingDictionary(random) : new Dictionary<int, string>());
        return userInfoModelMock;
    }

    internal static IReadOnlyDictionary<int, string> CreateAccontingDictionary(this Fixture fixture, Random? random = null)
    {
        return fixture.CreateMany<int>(random?.Next(1, 5) ?? 3)
            .Distinct()
            .ToDictionary(accountingNumber => accountingNumber, accountingNumber => fixture.Create<string>());
    }

    internal static IReadOnlyDictionary<StaticTextKey, string> CreateStaticTexts(this Fixture fixture, Random? random = null)
    {
        return fixture.CreateMany<StaticTextKey>(random?.Next(1, 5) ?? 3)
            .Distinct()
            .ToDictionary(staticTextKey => staticTextKey, staticTextKey => fixture.Create<string>());
    }

    internal static IReadOnlyCollection<IValidationRule> CreateValidationRuleSet(this Fixture fixture)
    {
        return
        [
            fixture.CreateRequiredValueRuleMock().As<IValidationRule>().Object,
            fixture.CreateMinLengthRuleMock().As<IValidationRule>().Object,
            fixture.CreateMaxLengthRuleMock().As<IValidationRule>().Object,
            fixture.CreateShouldBeIntegerRuleMock().As<IValidationRule>().Object,
            fixture.CreateMinValueRuleMock<int>().As<IValidationRule>().Object,
            fixture.CreateMaxValueRuleMock<int>().As<IValidationRule>().Object,
            fixture.CreatePatternRuleMock().As<IValidationRule>().Object,
            fixture.CreateOneOfRuleMock<string>().As<IValidationRule>().Object
        ];
    }

    internal static Mock<IRequiredValueRule> CreateRequiredValueRuleMock(this Fixture fixture)
    {
        string name = fixture.Create<string>();
        string validationError = fixture.Create<string>();

        Mock<IRequiredValueRule> requiredValueRuleMock = new Mock<IRequiredValueRule>();
        requiredValueRuleMock.Setup(m => m.Name)
            .Returns(name);
        requiredValueRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.RequiredValueRule);
        requiredValueRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = requiredValueRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.RequiredValueRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return requiredValueRuleMock;
    }

    internal static Mock<IMinLengthRule> CreateMinLengthRuleMock(this Fixture fixture)
    {
        string name = fixture.Create<string>();
        string validationError = fixture.Create<string>();

        Mock<IMinLengthRule> minLengthRuleMock = new Mock<IMinLengthRule>();
        minLengthRuleMock.Setup(m => m.Name)
            .Returns(name);
        minLengthRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MinLengthRule);
        minLengthRuleMock.Setup(m => m.MinLength)
            .Returns(fixture.Create<int>());
        minLengthRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = minLengthRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MinLengthRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return minLengthRuleMock;
    }

    internal static Mock<IMaxLengthRule> CreateMaxLengthRuleMock(this Fixture fixture)
    {
        string name = fixture.Create<string>();
        string validationError = fixture.Create<string>();

        Mock<IMaxLengthRule> maxLengthRuleMock = new Mock<IMaxLengthRule>();
        maxLengthRuleMock.Setup(m => m.Name)
            .Returns(name);
        maxLengthRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MaxLengthRule);
        maxLengthRuleMock.Setup(m => m.MaxLength)
            .Returns(fixture.Create<int>());
        maxLengthRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = maxLengthRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MaxLengthRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return maxLengthRuleMock;
    }

    internal static Mock<IShouldBeIntegerRule> CreateShouldBeIntegerRuleMock(this Fixture fixture)
    {
        string name = fixture.Create<string>();
        string validationError = fixture.Create<string>();

        Mock<IShouldBeIntegerRule> shouldBeIntegerRuleMock = new Mock<IShouldBeIntegerRule>();
        shouldBeIntegerRuleMock.Setup(m => m.Name)
            .Returns(name);
        shouldBeIntegerRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.ShouldBeIntegerRule);
        shouldBeIntegerRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = shouldBeIntegerRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.ShouldBeIntegerRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return shouldBeIntegerRuleMock;
    }

    internal static Mock<IMinValueRule<TValue>> CreateMinValueRuleMock<TValue>(this Fixture fixture) where TValue : struct, IComparable<TValue>
    {
        string name = fixture.Create<string>();
        string validationError = fixture.Create<string>();

        Mock<IMinValueRule<TValue>> minValueRuleMock = new Mock<IMinValueRule<TValue>>();
        minValueRuleMock.Setup(m => m.Name)
            .Returns(name);
        minValueRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MinValueRule);
        minValueRuleMock.Setup(m => m.MinValue)
            .Returns(fixture.Create<TValue>());
        minValueRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = minValueRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MinValueRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return minValueRuleMock;
    }

    internal static Mock<IMaxValueRule<TValue>> CreateMaxValueRuleMock<TValue>(this Fixture fixture) where TValue : struct, IComparable<TValue>
    {
        string name = fixture.Create<string>();
        string validationError = fixture.Create<string>();

        Mock<IMaxValueRule<TValue>> maxValueRuleMock = new Mock<IMaxValueRule<TValue>>();
        maxValueRuleMock.Setup(m => m.Name)
            .Returns(name);
        maxValueRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MaxValueRule);
        maxValueRuleMock.Setup(m => m.MaxValue)
            .Returns(fixture.Create<TValue>());
        maxValueRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = maxValueRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MaxValueRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return maxValueRuleMock;
    }

    internal static Mock<IPatternRule> CreatePatternRuleMock(this Fixture fixture)
    {
        string name = fixture.Create<string>();
        string validationError = fixture.Create<string>();

        Mock<IPatternRule> patternRuleMock = new Mock<IPatternRule>();
        patternRuleMock.Setup(m => m.Name)
            .Returns(name);
        patternRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.PatternRule);
        patternRuleMock.Setup(m => m.Pattern)
            .Returns(new Regex("^[0-9A-Za-z]{32}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(32)));
        patternRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = patternRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.PatternRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return patternRuleMock;
    }

    internal static Mock<IOneOfRule<TValue>> CreateOneOfRuleMock<TValue>(this Fixture fixture) where TValue : IComparable<TValue>
    {
        string name = fixture.Create<string>();
        string validationError = fixture.Create<string>();

        Mock<IOneOfRule<TValue>> oneOfRuleMock = new Mock<IOneOfRule<TValue>>();
        oneOfRuleMock.Setup(m => m.Name)
            .Returns(name);
        oneOfRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.OneOfRule);
        oneOfRuleMock.Setup(m => m.ValidValues)
            .Returns([fixture.CreateValueSpecification<TValue>(), fixture.CreateValueSpecification<TValue>(), fixture.CreateValueSpecification<TValue>(), fixture.CreateValueSpecification<TValue>(), fixture.CreateValueSpecification<TValue>()]);
        oneOfRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = oneOfRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.OneOfRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return oneOfRuleMock;
    }

    internal static IValueSpecification<TValue> CreateValueSpecification<TValue>(this Fixture fixture) where TValue : IComparable<TValue>
    {
        return fixture.CreateValueSpecificationMock<TValue>().Object;
    }

    internal static Mock<IValueSpecification<TValue>> CreateValueSpecificationMock<TValue>(this Fixture fixture) where TValue : IComparable<TValue>
    {
        Mock<IValueSpecification<TValue>> valueSpecificationMock = new Mock<IValueSpecification<TValue>>();
        valueSpecificationMock.Setup(m => m.Value)
            .Returns(fixture.Create<TValue>());
        valueSpecificationMock.Setup(m => m.Description)
            .Returns(fixture.Create<string>());
        return valueSpecificationMock;
    }

    #endregion
}