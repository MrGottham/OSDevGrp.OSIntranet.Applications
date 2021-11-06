using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands
{
    public interface IKeyValueEntryIdentificationCommand : ICommand
    {
        string Key { get; set; }

        IValidator Validate(IValidator validator, ICommonRepository commonRepository);
    }
}