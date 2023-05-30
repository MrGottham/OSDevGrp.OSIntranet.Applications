using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands
{
	public interface IGenericCategoryIdentificationCommand<TGenericCategory> : ICommand where TGenericCategory : IGenericCategory
    {
        int Number { get; }

        IValidator Validate(IValidator validator, Func<bool> hasNecessaryPermissionGetter, Func<int, Task<TGenericCategory>> genericCategoryGetter);
    }
}