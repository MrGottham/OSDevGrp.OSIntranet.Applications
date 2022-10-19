using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class AccountGroup : AccountGroupBase, IAccountGroup
    {
        #region Constructor

        public AccountGroup(int number, string name, AccountGroupType accountGroupType, bool deletable = false)
            : base(number, name, deletable)
        {
            AccountGroupType = accountGroupType;
        }

        #endregion

        #region Properties

        public AccountGroupType AccountGroupType { get; }

        #endregion

        #region Methods

        public virtual Task<IAccountGroupStatus> CalculateAsync(DateTime statusDate, IAccountCollection accountCollection)
        {
            NullGuard.NotNull(accountCollection, nameof(accountCollection));

            return new AccountGroupStatus(this, accountCollection).CalculateAsync(statusDate.Date);
        }

        #endregion
    }
}