using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers
{
    internal abstract class UpdateGenericCategoryCommandHandlerBase<TUpdateGenericCategoryCommand, TGenericCategory> : GenericCategoryDataCommandHandlerBase<TUpdateGenericCategoryCommand, TGenericCategory> where TUpdateGenericCategoryCommand : IUpdateGenericCategoryCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
        #region Constructor

        protected UpdateGenericCategoryCommandHandlerBase(IValidator validator)
            : base(validator)
        {
        }

        #endregion
    }
}