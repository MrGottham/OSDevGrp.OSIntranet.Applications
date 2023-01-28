using OSDevGrp.OSIntranet.BusinessLogic.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    internal class DeleteLanguageCommand : DeleteGenericCategoryCommandBase<ILanguage>, IDeleteLanguageCommand
    {
        #region Constructor

        public DeleteLanguageCommand(int number) 
            : base(number)
        {
        }

        #endregion
    }
}