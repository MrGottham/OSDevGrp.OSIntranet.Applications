using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public abstract class AccountGroupCommandBase : AccountGroupIdentificationCommandBase, IAccountGroupCommand
    {
        #region Properties

        public string Name { get; set; }

        public AccountGroupType AccountGroupType { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return base.Validate(validator, claimResolver, accountingRepository)
                .String.ShouldNotBeNullOrWhiteSpace(Name, GetType(), nameof(Name))
                .String.ShouldHaveMinLength(Name, 1, GetType(), nameof(Name))
                .String.ShouldHaveMaxLength(Name, 256, GetType(), nameof(Name))
                .Object.ShouldBeKnownValue(AccountGroupType, IsKnownAccountGroupTypeAsync, GetType(), nameof(AccountGroupType));
        }

        public IAccountGroup ToDomain()
        {
            return new AccountGroup(Number, Name, AccountGroupType);
        }

        private static Task<bool> IsKnownAccountGroupTypeAsync(AccountGroupType accountGroupType)
        {
            return Task.Run(() => Enum.GetValues(typeof(AccountGroupType)).Cast<AccountGroupType>().Contains(accountGroupType));
        }

        #endregion
    }
}