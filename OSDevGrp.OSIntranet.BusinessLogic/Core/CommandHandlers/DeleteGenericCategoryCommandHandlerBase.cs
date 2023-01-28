using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers
{
    internal abstract class DeleteGenericCategoryCommandHandlerBase<TDeleteGenericCategoryCommand, TGenericCategory> : GenericCategoryIdentificationCommandHandlerBase<TDeleteGenericCategoryCommand, TGenericCategory> where TDeleteGenericCategoryCommand : IDeleteGenericCategoryCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
        #region Constructor

        protected DeleteGenericCategoryCommandHandlerBase(IValidator validator) 
            : base(validator)
        {
        }

        #endregion

        #region Methods

        protected sealed override async Task ManageRepositoryAsync(TDeleteGenericCategoryCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            await ManageRepositoryAsync(command.Number);
        }

        protected abstract Task ManageRepositoryAsync(int number);

        #endregion
    }
}