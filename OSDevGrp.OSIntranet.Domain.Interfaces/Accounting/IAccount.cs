using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccount : IAccountBase<IAccount>
    {
        IAccountGroup AccountGroup { get; set; }

        AccountGroupType AccountGroupType { get; }

        ICreditInfoValues ValuesAtStatusDate { get; }

        ICreditInfoValues ValuesAtEndOfLastMonthFromStatusDate { get; }

        ICreditInfoValues ValuesAtEndOfLastYearFromStatusDate { get; }

        ICreditInfoCollection CreditInfoCollection { get; }
    }
}