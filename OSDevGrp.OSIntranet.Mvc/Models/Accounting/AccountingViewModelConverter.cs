using AutoMapper;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    internal class AccountingViewModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IAccountGroup, AccountGroupViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<AccountGroupViewModel, CreateAccountGroupCommand>();
            mapperConfiguration.CreateMap<AccountGroupViewModel, UpdateAccountGroupCommand>();
            mapperConfiguration.CreateMap<AccountGroupViewModel, DeleteAccountGroupCommand>();

            mapperConfiguration.CreateMap<IBudgetAccountGroup, BudgetAccountGroupViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<BudgetAccountGroupViewModel, CreateBudgetAccountGroupCommand>();
            mapperConfiguration.CreateMap<BudgetAccountGroupViewModel, UpdateBudgetAccountGroupCommand>();
            mapperConfiguration.CreateMap<BudgetAccountGroupViewModel, DeleteBudgetAccountGroupCommand>();

            mapperConfiguration.CreateMap<Domain.Interfaces.Accounting.Enums.AccountGroupType, AccountGroupType>();
            mapperConfiguration.CreateMap<AccountGroupType, Domain.Interfaces.Accounting.Enums.AccountGroupType>();

            mapperConfiguration.CreateMap<IPaymentTerm, PaymentTermViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<PaymentTermViewModel, CreatePaymentTermCommand>();
            mapperConfiguration.CreateMap<PaymentTermViewModel, UpdatePaymentTermCommand>();
            mapperConfiguration.CreateMap<PaymentTermViewModel, DeletePaymentTermCommand>();

        }

        #endregion
    }
}