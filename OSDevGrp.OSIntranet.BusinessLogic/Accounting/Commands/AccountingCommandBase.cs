using OSDevGrp.OSIntranet.BusinessLogic.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public abstract class AccountingCommandBase : AccountingIdentificationCommandBase, IAccountingCommand
    {
        #region Private variables

        private ILetterHead _letterHead;

        #endregion

        #region Properties

        public string Name { get; set; }

        public int LetterHeadNumber { get; set; }

        public BalanceBelowZeroType BalanceBelowZero { get; set; }

        public int BackDating { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
	            .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, claimResolver, accountingRepository, commonRepository)
                .String.ShouldNotBeNullOrWhiteSpace(Name, GetType(), nameof(Name))
                .String.ShouldHaveMinLength(Name, 1, GetType(), nameof(Name))
                .String.ShouldHaveMaxLength(Name, 256, GetType(), nameof(Name))
                .ValidateLetterHeadIdentifier(LetterHeadNumber, GetType(), nameof(LetterHeadNumber))
                .Object.ShouldBeKnownValue(LetterHeadNumber, letterHeadNumber => Task.Run(async () => await GetLetterHead(commonRepository) != null), GetType(), nameof(LetterHeadNumber))
                .Object.ShouldBeKnownValue(BalanceBelowZero, IsKnownBalanceBelowZeroType, GetType(), nameof(BalanceBelowZero))
                .Integer.ShouldBeGreaterThanOrEqualToZero(BackDating, GetType(), nameof(BackDating));
        }

        public IAccounting ToDomain(ICommonRepository commonRepository)
        {
            NullGuard.NotNull(commonRepository, nameof(commonRepository));

            ILetterHead letterHead = GetLetterHead(commonRepository).GetAwaiter().GetResult();

            return new Domain.Accounting.Accounting(AccountingNumber, Name, letterHead, BalanceBelowZero, BackDating);
        }

        protected Task<ILetterHead> GetLetterHead(ICommonRepository commonRepository)
        {
            NullGuard.NotNull(commonRepository, nameof(commonRepository));

            return Task.Run(() => LetterHeadNumber.GetLetterHead(commonRepository, ref _letterHead));
        }

        private static Task<bool> IsKnownBalanceBelowZeroType(BalanceBelowZeroType balanceBelowZero)
        {
            return Task.Run(() => Enum.GetValues(typeof(BalanceBelowZeroType)).Cast<BalanceBelowZeroType>().Contains(balanceBelowZero));
        }

        #endregion
    }
}