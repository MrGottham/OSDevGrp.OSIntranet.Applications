using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
    internal class AccountingModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<AccountGroupModel, IAccountGroup>()
                .ConvertUsing(accountGroupModel => accountGroupModel.ToDomain());

            mapperConfiguration.CreateMap<BudgetAccountGroupModel, IBudgetAccountGroup>()
                .ConvertUsing(budgetAccountGroupModel => budgetAccountGroupModel.ToDomain());
        }

        #endregion
    }
}
