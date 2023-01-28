using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers
{
    internal abstract class GenericCategoryDataCommandHandlerBase<TGenericCategoryDataCommand, TGenericCategory> : GenericCategoryIdentificationCommandHandlerBase<TGenericCategoryDataCommand, TGenericCategory> where TGenericCategoryDataCommand : IGenericCategoryDataCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
        #region Constructor

        protected GenericCategoryDataCommandHandlerBase(IValidator validator) 
            : base(validator)
        {
        }

        #endregion

        #region Methods

        protected sealed override async Task ManageRepositoryAsync(TGenericCategoryDataCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            TGenericCategory genericCategory = command.ToDomain();

            await ManageRepositoryAsync(genericCategory);
        }

        protected abstract Task ManageRepositoryAsync(TGenericCategory genericCategory);

        #endregion
    }
}