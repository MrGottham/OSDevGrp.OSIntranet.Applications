using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands
{
    public interface ILetterHeadCommand : ILetterHeadIdentificationCommand
    {
        string Name { get; set; }

        string Line1 { get; set; }

        string Line2 { get; set; }

        string Line3 { get; set; }

        string Line4 { get; set; }

        string Line5 { get; set; }

        string Line6 { get; set; }

        string Line7 { get; set; }

        string CompanyIdentificationNumber { get ; set; }

        ILetterHead ToDomain();
    }
}