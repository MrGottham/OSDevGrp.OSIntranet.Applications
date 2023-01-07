using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Validation
{
    internal class PermissionValidator : Validator, IPermissionValidator
    {
        #region Methods

        public IValidator HasNecessaryPermission(bool necessaryPermissionGranted)
        {
            if (necessaryPermissionGranted)
            {
                return this;
            }

            throw new IntranetExceptionBuilder(ErrorCode.MissingNecessaryPermission).Build();
        }

        #endregion
    }
}