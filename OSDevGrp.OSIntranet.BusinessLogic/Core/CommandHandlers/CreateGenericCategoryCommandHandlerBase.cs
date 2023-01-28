using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers
{
    internal abstract class CreateGenericCategoryCommandHandlerBase<TCreateGenericCategoryCommand, TGenericCategory> : GenericCategoryDataCommandHandlerBase<TCreateGenericCategoryCommand, TGenericCategory> where TCreateGenericCategoryCommand : ICreateGenericCategoryCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
        #region Constructor

        protected CreateGenericCategoryCommandHandlerBase(IValidator validator) 
            : base(validator)
        {
        }

        #endregion
    }
}