using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal class StatusDateProvider : IStatusDateProvider, IStatusDateSetter
    {
        #region Private variables

        private DateTime? _statusDate;

        #endregion

        #region Methods

        public DateTime GetStatusDate()
        {
            if (_statusDate.HasValue == false)
            {
                throw new IntranetExceptionBuilder(ErrorCode.NamedValueNotSetOnObject, "StatusDate", GetType().Name).Build();
            }

            return _statusDate.Value;
        }

        public void SetStatusDate(DateTime statusDate)
        {
            _statusDate = statusDate.Date;
        }

        #endregion
    }
}