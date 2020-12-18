using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public abstract class InfoCommandBase : IInfoCommand
    {
        #region Properties

        public short Year { get; set; }

        public short Month { get; set; }

        #endregion

        #region Methods

        public virtual IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            DateTime today = DateTime.Today;

            return validator.Integer.ShouldBeBetween(Year, today.Year, InfoBase<IInfo>.MaxYear, GetType(), nameof(Year))
                .Integer.ShouldBeBetween(Month, InfoBase<IInfo>.MinMonth, InfoBase<IInfo>.MaxMonth, GetType(), nameof(Month))
                .Integer.ShouldBeBetween(Year * 100 + Month, today.Year * 100 + today.Month, InfoBase<IInfo>.MaxYear * 100 + InfoBase<IInfo>.MaxMonth, GetType(), $"{nameof(Year)},{nameof(Month)}");
        }

        #endregion
    }
}