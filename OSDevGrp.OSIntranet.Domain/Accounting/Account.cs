using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class Account : AccountBase<IAccount>, IAccount
    {
        #region Private variables

        private IAccountGroup _accountGroup;

        #endregion

        #region Constructor

        public Account(IAccounting accounting, string accountNumber, string accountName, IAccountGroup accountGroup)
            : base(accounting, accountNumber, accountName)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup));

            AccountGroup = accountGroup;
        }

        #endregion

        #region Properties

        public IAccountGroup AccountGroup
        { 
            get => _accountGroup;
            set
            {
                NullGuard.NotNull(value, nameof(value));

                _accountGroup = value;
            } 
        }

        #endregion

        #region Methods

        protected override IAccount Calculate(DateTime statusDate)
        {
            return this;
        }

        #endregion
    }
}