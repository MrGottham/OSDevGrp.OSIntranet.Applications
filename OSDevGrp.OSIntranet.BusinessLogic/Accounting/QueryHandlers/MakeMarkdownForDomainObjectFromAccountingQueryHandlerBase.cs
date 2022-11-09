﻿using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;
using OSDevGrp.OSIntranet.Core.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal abstract class MakeMarkdownForDomainObjectFromAccountingQueryHandlerBase<TExportFromAccountingQuery, TDomainObject, TDomainObjectToMarkdownConverter> : ExportToMarkdownQueryHandlerBase<TExportFromAccountingQuery, TDomainObject, TDomainObjectToMarkdownConverter> where TExportFromAccountingQuery : IExportFromAccountingQuery where TDomainObject : class where TDomainObjectToMarkdownConverter : IDomainObjectToMarkdownConverter<TDomainObject>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IStatusDateSetter _statusDateSetter;

        #endregion

        #region Constructor

        protected MakeMarkdownForDomainObjectFromAccountingQueryHandlerBase(IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, TDomainObjectToMarkdownConverter domainObjectToMarkdownConverter, bool encoderShouldEmitUtf8Identifier = true) 
            : base(domainObjectToMarkdownConverter, encoderShouldEmitUtf8Identifier)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(statusDateSetter, nameof(statusDateSetter));

            _validator = validator;
            _statusDateSetter = statusDateSetter;

            AccountingRepository = accountingRepository;
        }

        #endregion

        #region Properties

        protected IAccountingRepository AccountingRepository { get; }

        #endregion

        #region Methods

        protected override async Task ValidateQueryAsync(TExportFromAccountingQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            await base.ValidateQueryAsync(query);

            query.Validate(_validator, AccountingRepository);
        }

        protected override async Task BeforeExportAsync(TExportFromAccountingQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            await base.BeforeExportAsync(query);

            _statusDateSetter.SetStatusDate(query.StatusDate);
        }

        #endregion
    }
}