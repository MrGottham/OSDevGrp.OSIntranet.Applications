using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    internal class AccountingModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IAccountGroup, AccountGroupModel>();

            mapperConfiguration.CreateMap<IBudgetAccountGroup, BudgetAccountGroupModel>();

            mapperConfiguration.CreateMap<Domain.Interfaces.Accounting.Enums.AccountGroupType, AccountGroupType>();
        }

        #endregion
    }
}
