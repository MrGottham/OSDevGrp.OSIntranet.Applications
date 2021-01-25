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

            mapperConfiguration.CreateMap<IAccount, AccountModel>()
                .ForMember(dest => dest.CreditInfos, opt => opt.MapFrom(src => src.CreditInfoCollection));

            mapperConfiguration.CreateMap<IAccountCollection, AccountCollectionModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(account => account.AccountNumber).ToArray()));

            mapperConfiguration.CreateMap<ICreditInfo, CreditInfoModel>();

            mapperConfiguration.CreateMap<ICreditInfoValues, CreditInfoValuesModel>();

            mapperConfiguration.CreateMap<ICreditInfoCollection, CreditInfoCollectionModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderByDescending(creditInfo => creditInfo.Year * 100 + creditInfo.Month).ToArray()));

            mapperConfiguration.CreateMap<IBudgetAccount, AccountIdentificationModel>();

            mapperConfiguration.CreateMap<IBudgetAccount, AccountCoreDataModel>();

            mapperConfiguration.CreateMap<IBudgetAccount, BudgetAccountModel>()
                .ForMember(dest => dest.BudgetInfos, opt => opt.MapFrom(src => src.BudgetInfoCollection));

            mapperConfiguration.CreateMap<IBudgetAccountCollection, BudgetAccountCollectionModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(budgetAccount => budgetAccount.AccountNumber).ToArray()));

            mapperConfiguration.CreateMap<IBudgetInfo, BudgetInfoModel>();

            mapperConfiguration.CreateMap<IBudgetInfoValues, BudgetInfoValuesModel>();

            mapperConfiguration.CreateMap<IBudgetInfoCollection, BudgetInfoCollectionModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderByDescending(budgetInfo => budgetInfo.Year * 100 + budgetInfo.Month).ToArray()));

            mapperConfiguration.CreateMap<IContactAccount, AccountIdentificationModel>();

            mapperConfiguration.CreateMap<IContactAccount, AccountCoreDataModel>();

            mapperConfiguration.CreateMap<IContactAccount, ContactAccountModel>()
                .ForMember(dest => dest.BalanceInfos, opt => opt.MapFrom(src => src.ContactInfoCollection));

            mapperConfiguration.CreateMap<IContactAccountCollection, ContactAccountCollectionModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(contactAccount => contactAccount.AccountName).ToArray()));

            mapperConfiguration.CreateMap<IContactInfo, BalanceInfoModel>();

            mapperConfiguration.CreateMap<IContactInfoValues, BalanceInfoValuesModel>();

            mapperConfiguration.CreateMap<IContactInfoCollection, BalanceInfoCollectionModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderByDescending(contactInfo => contactInfo.Year * 100 + contactInfo.Month).ToArray()));

            mapperConfiguration.CreateMap<IAccountGroup, AccountGroupModel>();

            mapperConfiguration.CreateMap<IBudgetAccountGroup, BudgetAccountGroupModel>();

            mapperConfiguration.CreateMap<IPaymentTerm, PaymentTermModel>();

            mapperConfiguration.CreateMap<Domain.Interfaces.Accounting.Enums.BalanceBelowZeroType, BalanceBelowZeroType>();

            mapperConfiguration.CreateMap<Domain.Interfaces.Accounting.Enums.AccountGroupType, AccountGroupType>();
        }

        #endregion
    }
}