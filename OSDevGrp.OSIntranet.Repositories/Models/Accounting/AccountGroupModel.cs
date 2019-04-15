using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class AccountGroupModel : AccountGroupModelBase
    {
        public virtual int AccountGroupIdentifier { get; set; }

        public virtual AccountGroupType AccountGroupType { get; set; }
    }

    internal static class AccountGroupModelExtensions
    {
        internal static IAccountGroup ToDomain(this AccountGroupModel accountGroupModel)
        {
            NullGuard.NotNull(accountGroupModel, nameof(accountGroupModel));

            IAccountGroup accountGroup = new AccountGroup(accountGroupModel.AccountGroupIdentifier, accountGroupModel.Name, accountGroupModel.AccountGroupType);
            accountGroup.AddAuditInformations(accountGroupModel.CreatedUtcDateTime, accountGroupModel.CreatedByIdentifier, accountGroupModel.ModifiedUtcDateTime, accountGroupModel.ModifiedByIdentifier);

            return accountGroup;
        }
    }
}
