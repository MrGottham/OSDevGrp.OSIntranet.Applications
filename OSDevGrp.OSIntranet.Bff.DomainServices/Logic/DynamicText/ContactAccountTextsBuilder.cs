using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ContactAccountTextsBuilder : DynamicTextsBuilderBase<ContactAccountModel, IContactAccountTexts>, IContactAccountTextsBuilder
{
    #region Constructor

    public ContactAccountTextsBuilder(IStaticTextProvider staticTextProvider) 
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public override async Task<IContactAccountTexts> BuildAsync(ContactAccountModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(formatProvider);

        IValueDisplayer? statusDate = null;
        IContactAccountValuesDisplayer? valuesAtStatusDate = null;
        IContactAccountValuesDisplayer? valuesAtEndOfLastMonthFromStatusDate = null;
        IContactAccountValuesDisplayer? valuesAtEndOfLastYearFromStatusDate = null;

        Task buildStatusDateTask = GetStatusDateAsync(model.StatusDate, "d", formatProvider, cancellationToken).ContinueWith(task => statusDate = task.Result, cancellationToken);
        Task buildValuesAtStatusDateTask = ContactAccountValuesDisplayer.CreateAsync(StaticTextKey.ContactAccountValuesAtStatusDate, model.ValuesAtStatusDate, StaticTextProvider, formatProvider, cancellationToken).ContinueWith(task => valuesAtStatusDate = task.Result, cancellationToken);
        Task buildValuesAtEndOfLastMonthFromStatusDateTask = ContactAccountValuesDisplayer.CreateAsync(StaticTextKey.ContactAccountValuesAtEndOfLastMonthFromStatusDate, model.ValuesAtEndOfLastMonthFromStatusDate, StaticTextProvider, formatProvider, cancellationToken).ContinueWith(task => valuesAtEndOfLastMonthFromStatusDate = task.Result, cancellationToken);
        Task buildValuesAtEndOfLastYearFromStatusDateTask = ContactAccountValuesDisplayer.CreateAsync(StaticTextKey.ContactAccountValuesAtEndOfLastYearFromStatusDate, model.ValuesAtEndOfLastYearFromStatusDate, StaticTextProvider, formatProvider, cancellationToken).ContinueWith(task => valuesAtEndOfLastYearFromStatusDate = task.Result, cancellationToken);

        await Task.WhenAll(buildStatusDateTask, buildValuesAtStatusDateTask, buildValuesAtEndOfLastMonthFromStatusDateTask, buildValuesAtEndOfLastYearFromStatusDateTask);

        return new ContactAccountTexts(model, statusDate!, valuesAtStatusDate!, valuesAtEndOfLastMonthFromStatusDate!, valuesAtEndOfLastYearFromStatusDate!, formatProvider);
    }

    #endregion
}