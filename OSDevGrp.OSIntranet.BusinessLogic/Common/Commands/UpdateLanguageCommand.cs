using OSDevGrp.OSIntranet.BusinessLogic.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    internal class UpdateLanguageCommand : UpdateGenericCategoryCommandBase<ILanguage>, IUpdateLanguageCommand
    {
        #region Constructor

        public UpdateLanguageCommand(int number, string name) 
            : base(number, name)
        {
        }

        #endregion

        #region Methods

        public override ILanguage ToDomain()
        {
            return new Language(Number, Name);
        }

        #endregion
    }
}