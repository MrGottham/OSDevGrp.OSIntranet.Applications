using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IInfoCommand : ICommand
    {
        short Year { get; set; }

        short Month { get; set; }

        IValidator Validate(IValidator validator);
    }
}