using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
	internal class ApplyPostingJournalCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<IApplyPostingJournalCommand, IPostingJournalResult>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IClaimResolver _claimResolver;
        private readonly IAccountingRepository _accountingRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly IPostingWarningCalculator _postingWarningCalculator;

        #endregion

        #region Constructor

        public ApplyPostingJournalCommandHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, ICommonRepository commonRepository, IPostingWarningCalculator postingWarningCalculator)
        {
	        NullGuard.NotNull(validator, nameof(validator))
		        .NotNull(claimResolver, nameof(claimResolver))
		        .NotNull(accountingRepository, nameof(accountingRepository))
		        .NotNull(commonRepository, nameof(commonRepository))
		        .NotNull(postingWarningCalculator, nameof(postingWarningCalculator));

            _validator = validator;
            _claimResolver = claimResolver;
            _accountingRepository = accountingRepository;
            _commonRepository = commonRepository;
            _postingWarningCalculator = postingWarningCalculator;
        }

        #endregion

        #region Methods

        public async Task<IPostingJournalResult> ExecuteAsync(IApplyPostingJournalCommand applyPostingJournalCommand)
        {
            NullGuard.NotNull(applyPostingJournalCommand, nameof(applyPostingJournalCommand));

            applyPostingJournalCommand.Validate(_validator, _claimResolver, _accountingRepository, _commonRepository);

            IPostingJournal postingJournal = applyPostingJournalCommand.ToDomain(_accountingRepository);
            IPostingJournalResult postingJournalResult = await _accountingRepository.ApplyPostingJournalAsync(postingJournal, _postingWarningCalculator);
            if (postingJournalResult == null)
            {
                return await new PostingJournalResult(new PostingLineCollection(), _postingWarningCalculator).CalculateAsync(DateTime.Today);
            }

            return await postingJournalResult.CalculateAsync(DateTime.Today);
        }

        #endregion
    }
}