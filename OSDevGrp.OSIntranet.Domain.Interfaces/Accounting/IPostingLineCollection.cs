using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IPostingLineCollection : IEnumerable<IPostingLine>, ICalculable<IPostingLineCollection>
    {
        void Add(IPostingLine postingLine);

        void Add(IEnumerable<IPostingLine> postingLineCollection);
    }
}