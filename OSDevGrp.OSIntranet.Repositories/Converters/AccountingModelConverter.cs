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

            mapperConfiguration.CreateMap<IAccountGroup, AccountGroupModel>()
                .ForMember(dest => dest.AccountGroupIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<BudgetAccountGroupModel, IBudgetAccountGroup>()
                .ConvertUsing(budgetAccountGroupModel => budgetAccountGroupModel.ToDomain());

            mapperConfiguration.CreateMap<IBudgetAccountGroup, BudgetAccountGroupModel>()
                .ForMember(dest => dest.BudgetAccountGroupIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));
        }

        #endregion
    }
}
