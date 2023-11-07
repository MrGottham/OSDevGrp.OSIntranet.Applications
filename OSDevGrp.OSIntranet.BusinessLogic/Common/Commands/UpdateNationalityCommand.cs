using OSDevGrp.OSIntranet.BusinessLogic.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    internal class UpdateNationalityCommand : UpdateGenericCategoryCommandBase<INationality>, IUpdateNationalityCommand
    {
        #region Constructor

        public UpdateNationalityCommand(int number, string name) 
            : base(number, name)
        {
        }

        #endregion

        #region Methods

        public override INationality ToDomain()
        {
            return new Nationality(Number, Name);
        }

        #endregion
    }
}