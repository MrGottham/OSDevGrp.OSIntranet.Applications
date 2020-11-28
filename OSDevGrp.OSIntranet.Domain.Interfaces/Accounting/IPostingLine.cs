using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IPostingLine : IAuditable, ICalculable<IPostingLine>
    {
        Guid Identifier { get; }

        DateTime PostingDate { get; }

        decimal PostingValue { get; }
    }
}