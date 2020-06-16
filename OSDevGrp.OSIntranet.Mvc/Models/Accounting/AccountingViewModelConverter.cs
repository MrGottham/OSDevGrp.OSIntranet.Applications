using System.Collections.Generic;
using AutoMapper;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    internal class AccountingViewModelConverter : ConverterBase
    {
        #region Private variables

        private readonly IConverter _commonViewModelConverter = new CommonViewModelConverter();

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IAccounting, AccountingIdentificationViewModel>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IAccounting, AccountingViewModel>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.LetterHead, opt => opt.MapFrom(src => _commonViewModelConverter.Convert<ILetterHead, LetterHeadViewModel>(src.LetterHead)))
                .ForMember(dest => dest.LetterHeads, opt => opt.MapFrom(src => new List<LetterHeadViewModel>(0)))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

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

            mapperConfiguration.CreateMap<Domain.Interfaces.Accounting.Enums.BalanceBelowZeroType, BalanceBelowZeroType>();
            mapperConfiguration.CreateMap<BalanceBelowZeroType, Domain.Interfaces.Accounting.Enums.BalanceBelowZeroType>();

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