using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic
{
    public interface IContactAccountStatementToMarkdownConverter : IAccountToMarkdownConverter<IContactAccount>
    {
    }
}