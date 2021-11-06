using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IPostingWarningCollection : IEnumerable<IPostingWarning>
    {
        void Add(IPostingWarning postingWarning);

        void Add(IEnumerable<IPostingWarning> postingWarningCollection);

        IPostingWarningCollection Ordered();
    }
}