using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.AccountCollection))
                .ForMember(dest => dest.BudgetAccounts, opt => opt.MapFrom(src => src.BudgetAccountCollection))
                .ForMember(dest => dest.ContactAccounts, opt => opt.MapFrom(src => src.ContactAccountCollection))
                .ForMember(dest => dest.LetterHeads, opt => opt.MapFrom(src => new List<LetterHeadViewModel>(0)))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<AccountingViewModel, CreateAccountingCommand>()
                .ForMember(dest => dest.LetterHeadNumber, opt => opt.MapFrom(src => src.LetterHead.Number));

            mapperConfiguration.CreateMap<AccountingViewModel, UpdateAccountingCommand>()
                .ForMember(dest => dest.LetterHeadNumber, opt => opt.MapFrom(src => src.LetterHead.Number));

            mapperConfiguration.CreateMap<IAccount, AccountIdentificationViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IAccount, AccountCoreDataViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IAccount, AccountViewModel>()
                .ForMember(dest => dest.CreditInfos, opt => opt.MapFrom(src => src.CreditInfoCollection))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IAccountCollection, AccountCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(account => account.AccountNumber).ToArray()));

            mapperConfiguration.CreateMap<IAccountCollection, AccountDictionaryViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.GroupByAccountGroupAsync().GetAwaiter().GetResult()))
                .ForMember(dest => dest.Keys, opt => opt.Ignore())
                .ForMember(dest => dest.Values, opt => opt.Ignore());

            mapperConfiguration.CreateMap<ICreditInfo, CreditInfoViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<ICreditInfoValues, CreditInfoValuesViewModel>();

            mapperConfiguration.CreateMap<IAccountCollectionValues, AccountCollectionValuesViewModel>();

            mapperConfiguration.CreateMap<ICreditInfoCollection, CreditInfoCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderByDescending(creditInfo => creditInfo.Year * 100 + creditInfo.Month).ToArray()));

            mapperConfiguration.CreateMap<ICreditInfoCollection, CreditInfoDictionaryViewModel>()
                .ForMember(dest => dest.Items, opt => opt.ConvertUsing(new InfoCollectionToDictionaryValueConverter<ICreditInfoCollection, ICreditInfo, CreditInfoDictionaryViewModel, CreditInfoCollectionViewModel, CreditInfoViewModel>(), src => src))
                .ForMember(dest => dest.Keys, opt => opt.Ignore())
                .ForMember(dest => dest.Values, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IBudgetAccount, AccountIdentificationViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IBudgetAccount, AccountCoreDataViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IBudgetAccount, BudgetAccountViewModel>()
                .ForMember(dest => dest.BudgetInfos, opt => opt.MapFrom(src => src.BudgetInfoCollection))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IBudgetAccountCollection, BudgetAccountCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(budgetAccount => budgetAccount.AccountNumber).ToArray()));

            mapperConfiguration.CreateMap<IBudgetAccountCollection, BudgetAccountDictionaryViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.GroupByBudgetAccountGroupAsync().GetAwaiter().GetResult()))
                .ForMember(dest => dest.Keys, opt => opt.Ignore())
                .ForMember(dest => dest.Values, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IBudgetInfo, BudgetInfoViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IBudgetInfoValues, BudgetInfoValuesViewModel>();

            mapperConfiguration.CreateMap<IBudgetInfoCollection, BudgetInfoCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderByDescending(budgetInfo => budgetInfo.Year * 100 + budgetInfo.Month).ToArray()));

            mapperConfiguration.CreateMap<IBudgetInfoCollection, BudgetInfoDictionaryViewModel>()
                .ForMember(dest => dest.Items, opt => opt.ConvertUsing(new InfoCollectionToDictionaryValueConverter<IBudgetInfoCollection, IBudgetInfo, BudgetInfoDictionaryViewModel, BudgetInfoCollectionViewModel, BudgetInfoViewModel>(), src => src))
                .ForMember(dest => dest.Keys, opt => opt.Ignore())
                .ForMember(dest => dest.Values, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IContactAccount, AccountIdentificationViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContactAccount, AccountCoreDataViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContactAccount, ContactAccountViewModel>()
                .ForMember(dest => dest.BalanceInfos, opt => opt.MapFrom(src => src.ContactInfoCollection))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContactAccountCollection, ContactAccountCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(contactAccount => contactAccount.AccountName).ToArray()));

            mapperConfiguration.CreateMap<IContactInfo, BalanceInfoViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContactInfoValues, BalanceInfoValuesViewModel>();

            mapperConfiguration.CreateMap<IContactAccountCollectionValues, ContactAccountCollectionValuesViewModel>();

            mapperConfiguration.CreateMap<IContactInfoCollection, BalanceInfoCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderByDescending(contactInfo => contactInfo.Year * 100 + contactInfo.Month).ToArray()));

            mapperConfiguration.CreateMap<IContactInfoCollection, BalanceInfoDictionaryViewModel>()
                .ForMember(dest => dest.Items, opt => opt.ConvertUsing(new InfoCollectionToDictionaryValueConverter<IContactInfoCollection, IContactInfo, BalanceInfoDictionaryViewModel, BalanceInfoCollectionViewModel, BalanceInfoViewModel>(), src => src))
                .ForMember(dest => dest.Keys, opt => opt.Ignore())
                .ForMember(dest => dest.Values, opt => opt.Ignore());

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

        private class InfoCollectionToDictionaryValueConverter<TInfoCollection, TInfo, TInfoDictionaryViewModel, TInfoCollectionViewModel, TInfoViewModel> : IValueConverter<TInfoCollection, TInfoDictionaryViewModel> where TInfoCollection : IInfoCollection<TInfo> where TInfo : IInfo<TInfo> where TInfoDictionaryViewModel : InfoDictionaryViewModelBase<TInfoCollectionViewModel, TInfoViewModel>, new() where TInfoCollectionViewModel : InfoCollectionViewModelBase<TInfoViewModel>, new() where TInfoViewModel : InfoViewModelBase
        {
            #region Methods

            public TInfoDictionaryViewModel Convert(TInfoCollection sourceMember, ResolutionContext context)
            {
                NullGuard.NotNull(sourceMember, nameof(sourceMember))
                    .NotNull(context, nameof(context));

                IDictionary<short, TInfoCollectionViewModel> dictionary = sourceMember.GroupBy(info => info.Year)
                    .OrderByDescending(group => group.Key)
                    .ToDictionary(group => group.Key, group =>
                    {
                        return new TInfoCollectionViewModel
                        {
                            Items = group.Select(info => context.Mapper.Map<TInfo, TInfoViewModel>(info))
                                .OrderByDescending(infoViewModel => infoViewModel.Month)
                                .ToArray()
                        };
                    });

                return new TInfoDictionaryViewModel
                {
                    Items = new ReadOnlyDictionary<short, TInfoCollectionViewModel>(dictionary)
                };
            }

            #endregion
        }

        #endregion
    }
}