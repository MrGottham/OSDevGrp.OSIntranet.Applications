using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class PostingDateRuleSetBuilder : ValidationRuleSetBuilderBase, IPostingDateRuleSetBuilder
{
    #region Private variables

    private readonly TimeProvider _timeProvider;

    #endregion

    #region Constructor

    public PostingDateRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder, TimeProvider timeProvider)
        : base(extendedValidationRuleSetBuilder)
    {
        _timeProvider = timeProvider;
    }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        DateTimeOffset utcNow = _timeProvider.GetUtcNow();
        DateTimeOffset utcDate = new DateTimeOffset(utcNow.UtcDateTime.Date, TimeSpan.Zero);

        return await ExtendedValidationRuleSetBuilder.WithRequiredValueRule(StaticTextKey.PostingDate)
            .WithMinValueRule(StaticTextKey.PostingDate, utcDate.AddDays(-AccountingRuleSetSpecifications.BackDatingMaxValue))
            .WithMaxValueRule(StaticTextKey.PostingDate, utcDate)
            .BuildAsync(formatProvider, cancellationToken);
    }

    #endregion
}