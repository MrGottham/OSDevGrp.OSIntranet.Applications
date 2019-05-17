using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands
{
    public interface ILetterHeadIdentificationCommand : ICommand
    {
        int Number { get; set; }

        IValidator Validate(IValidator validator, ICommonRepository commonRepository);
    }
}