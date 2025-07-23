using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class ValidationRuleSetDto
{
    [Required]
    public required IReadOnlyCollection<RequiredValueRuleDto> RequiredValueRules { get; init; } = [];

    [Required]
    public required IReadOnlyCollection<MinLengthRuleDto> MinLengthRules { get; init; } = [];

    [Required]
    public required IReadOnlyCollection<MaxLengthRuleDto> MaxLengthRules { get; init; } = [];

    [Required]
    public required IReadOnlyCollection<ShouldBeIntegerRuleDto> ShouldBeIntegerRules { get; init; } = [];

    [Required]
    public required IReadOnlyCollection<MinValueRuleDto> MinValueRules { get; init; } = [];

    [Required]
    public required IReadOnlyCollection<MaxValueRuleDto> MaxValueRules { get; init; } = [];

    [Required]
    public required IReadOnlyCollection<PatternRuleDto> PatternRules { get; init; } = [];

    [Required]
    public required IReadOnlyCollection<OneOfRuleDto> OneOfRules { get; init; } = [];

    internal static ValidationRuleSetDto Map(IReadOnlyCollection<IValidationRule> validationRuleSet)
    {
        return new ValidationRuleSetDto
        {
            RequiredValueRules = validationRuleSet.OfType<IRequiredValueRule>().Select(RequiredValueRuleDto.Map).ToArray(),
            MinLengthRules = validationRuleSet.OfType<IMinLengthRule>().Select(MinLengthRuleDto.Map).ToArray(),
            MaxLengthRules = validationRuleSet.OfType<IMaxLengthRule>().Select(MaxLengthRuleDto.Map).ToArray(),
            ShouldBeIntegerRules = validationRuleSet.OfType<IShouldBeIntegerRule>().Select(ShouldBeIntegerRuleDto.Map).ToArray(),
            MinValueRules = Map<MinValueRuleDto>(validationRuleSet, typeof(IMinValueRule<>)).ToArray(),
            MaxValueRules = Map<MaxValueRuleDto>(validationRuleSet, typeof(IMaxValueRule<>)).ToArray(),
            PatternRules = validationRuleSet.OfType<IPatternRule>().Select(PatternRuleDto.Map).ToArray(),
            OneOfRules = Map<OneOfRuleDto>(validationRuleSet, typeof(IOneOfRule<>)).ToArray()
        };
    }

    private static IReadOnlyCollection<TTarget> Map<TTarget>(IReadOnlyCollection<IValidationRule> validationRuleSet, Type genericValidationRuleInterface) where TTarget : class
    {
        IList<TTarget> result = new List<TTarget>();
        foreach (IValidationRule validationRule in validationRuleSet)
        {
            Type? interfaceType = validationRule.GetType().GetInterfaces().SingleOrDefault(type => type.IsGenericType && type.GetGenericTypeDefinition() == genericValidationRuleInterface);
            if (interfaceType == null)
            {
                continue;
            }

            MethodInfo? genericMapper = typeof(TTarget).GetMethods(BindingFlags.NonPublic | BindingFlags.Static).SingleOrDefault(method => method.Name == "Map" && method.IsGenericMethod && method.ReturnType == typeof(TTarget));
            if (genericMapper == null)
            {
                continue;
            }

            MethodInfo? mapper = genericMapper.MakeGenericMethod(interfaceType.GetGenericArguments().Single());
            if (mapper == null)
            {
                continue;
            }

            TTarget? mappedResult = mapper.Invoke(null, [validationRule]) as TTarget;
            if (mappedResult == null)
            {
                continue;
            }

            result.Add(mappedResult);
        }
        return result.AsReadOnly();
    }
}