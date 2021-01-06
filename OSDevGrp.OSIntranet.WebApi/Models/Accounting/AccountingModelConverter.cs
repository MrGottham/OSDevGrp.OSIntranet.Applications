using System.Linq;
using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.WebApi.Models.Common;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    internal class AccountingModelConverter : ConverterBase
    {
        #region Private variables

        private readonly IConverter _commonModelConverter = new CommonModelConverter();

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IAccounting, AccountingIdentificationModel>();

            mapperConfiguration.CreateMap<IAccounting, AccountingModel>()
                .ForMember(dest => dest.LetterHead, opt => opt.MapFrom(src => _commonModelConverter.Convert<ILetterHead, LetterHeadIdentificationModel>(src.LetterHead)))
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.AccountCollection))
                .ForMember(dest => dest.BudgetAccounts, opt => opt.MapFrom(src => src.BudgetAccountCollection))
                .ForMember(dest => dest.ContactAccounts, opt => opt.MapFrom(src => src.ContactAccountCollection));

            mapperConfiguration.CreateMap<IAccount, AccountIdentificationModel>();

            mapperConfiguration.CreateMap<IAccount, AccountCoreDataModel>();

            mapperConfiguration.CreateMap<IAccount, AccountModel>();

            mapperConfiguration.CreateMap<IAccountCollection, AccountCollectionModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(account => account.AccountNumber).ToArray()));

            mapperConfiguration.CreateMap<ICreditInfoValues, CreditInfoValuesModel>();

            mapperConfiguration.CreateMap<IBudgetAccount, AccountIdentificationModel>();

            mapperConfiguration.CreateMap<IBudgetAccount, AccountCoreDataModel>();

            mapperConfiguration.CreateMap<IBudgetAccount, BudgetAccountModel>();

            mapperConfiguration.CreateMap<IBudgetAccountCollection, BudgetAccountCollectionModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(budgetAccount => budgetAccount.AccountNumber).ToArray()));

            mapperConfiguration.CreateMap<IContactAccount, AccountIdentificationModel>();

            mapperConfiguration.CreateMap<IContactAccount, AccountCoreDataModel>();

            mapperConfiguration.CreateMap<IContactAccount, ContactAccountModel>();

            mapperConfiguration.CreateMap<IContactAccountCollection, ContactAccountCollectionModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(contactAccount => contactAccount.AccountName).ToArray()));

            mapperConfiguration.CreateMap<IAccountGroup, AccountGroupModel>();

            mapperConfiguration.CreateMap<IBudgetAccountGroup, BudgetAccountGroupModel>();

            mapperConfiguration.CreateMap<IPaymentTerm, PaymentTermModel>();

            mapperConfiguration.CreateMap<Domain.Interfaces.Accounting.Enums.BalanceBelowZeroType, BalanceBelowZeroType>();

            mapperConfiguration.CreateMap<Domain.Interfaces.Accounting.Enums.AccountGroupType, AccountGroupType>();
        }

        #endregion
    }
}