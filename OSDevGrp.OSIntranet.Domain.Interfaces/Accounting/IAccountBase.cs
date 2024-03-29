using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccountBase : IAuditable, ICalculable, IProtectable
    {
        IAccounting Accounting { get; }

        string AccountNumber { get; }

        string AccountName { get; }

        string Description { get; set; }

        string Note { get; set; }

        IPostingLineCollection PostingLineCollection { get; }
    }

    public interface IAccountBase<T> : IAccountBase, ICalculable<T> where T : IAccountBase
    {
    }
}