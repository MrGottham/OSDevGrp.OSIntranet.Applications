using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Common
{
    public interface ILetterHead : IAuditable, IDeletable
    {
        int Number { get; }

        string Name { get; }

        string Line1 { get; set; }

        string Line2 { get; set; }

        string Line3 { get; set; }

        string Line4 { get; set; }

        string Line5 { get; set; }

        string Line6 { get; set; }

        string Line7 { get; set; }

        string CompanyIdentificationNumber { get ; set; }
    }
}