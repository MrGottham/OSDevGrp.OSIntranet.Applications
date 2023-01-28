using OSDevGrp.OSIntranet.BusinessLogic.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    internal class DeleteNationalityCommand : DeleteGenericCategoryCommandBase<INationality>, IDeleteNationalityCommand
    {
        #region Constructor

        public DeleteNationalityCommand(int number) 
            : base(number)
        {
        }

        #endregion
    }
}