using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands
{
    public interface IPushKeyValueEntryCommand : IKeyValueEntryIdentificationCommand
    {
        object Value { get; set; }

        IKeyValueEntry ToDomain();
    }
}