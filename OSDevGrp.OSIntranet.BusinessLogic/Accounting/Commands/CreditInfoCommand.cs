using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public class CreditInfoCommand : InfoCommandBase, ICreditInfoCommand
    {
        #region Properties

        public decimal Credit { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return base.Validate(validator)
                .Decimal.ShouldBeGreaterThanOrEqualToZero(Credit, GetType(), nameof(Credit));
        }

        public ICreditInfo ToDomain(IAccount account)
        {
            NullGuard.NotNull(account, nameof(account));

            return new CreditInfo(account, Year, Month, Credit);
        }

        #endregion
    }
}