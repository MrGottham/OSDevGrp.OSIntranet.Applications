using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
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
                .ForMember(dest => dest.PostingLines, opt => opt.MapFrom(src => src.GetPostingLinesAsync(src.StatusDate).GetAwaiter().GetResult().Between(DateTime.MinValue, src.StatusDate).Top(25)))
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
                .ForMember(dest => dest.PostingLines, opt => opt.MapFrom(src => src.PostingLineCollection.Between(DateTime.MinValue, src.StatusDate).Top(25)))
                .ForMember(dest => dest.AccountGroups, opt => opt.MapFrom(src => new List<AccountGroupViewModel>(0)))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<AccountViewModel, CreateAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber))
                .ForMember(dest => dest.AccountGroupNumber, opt => opt.MapFrom(src => src.AccountGroup.Number))
                .ForMember(dest => dest.CreditInfoCollection, opt =>
                {
                    opt.Condition(src => src.CreditInfos != null);
                    opt.ConvertUsing(new CreditInfoDictionaryViewModelToCreditInfoCommandCollectionValueConverter(), "CreditInfos");
                });

            mapperConfiguration.CreateMap<AccountViewModel, UpdateAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber))
                .ForMember(dest => dest.AccountGroupNumber, opt => opt.MapFrom(src => src.AccountGroup.Number))
                .ForMember(dest => dest.CreditInfoCollection, opt =>
                {
                    opt.Condition(src => src.CreditInfos != null);
                    opt.ConvertUsing(new CreditInfoDictionaryViewModelToCreditInfoCommandCollectionValueConverter(), "CreditInfos");
                });

            mapperConfiguration.CreateMap<AccountViewModel, DeleteAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber));

            mapperConfiguration.CreateMap<IAccountCollection, AccountCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(account => account.AccountNumber).ToArray()));

            mapperConfiguration.CreateMap<IAccountCollection, AccountDictionaryViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.GroupByAccountGroupAsync().GetAwaiter().GetResult()))
                .ForMember(dest => dest.Keys, opt => opt.Ignore())
                .ForMember(dest => dest.Values, opt => opt.Ignore());

            mapperConfiguration.CreateMap<ICreditInfo, CreditInfoViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<CreditInfoViewModel, CreditInfoCommand>();

            mapperConfiguration.CreateMap<ICreditInfoValues, CreditInfoValuesViewModel>();

            mapperConfiguration.CreateMap<IAccountCollectionValues, AccountCollectionValuesViewModel>();

            mapperConfiguration.CreateMap<ICreditInfoCollection, CreditInfoCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderByDescending(creditInfo => creditInfo.Year * 100 + creditInfo.Month).ToArray()));

            mapperConfiguration.CreateMap<ICreditInfoCollection, CreditInfoDictionaryViewModel>()
                .ForMember(dest => dest.Items, opt => opt.ConvertUsing(new InfoCollectionToDictionaryValueConverter<ICreditInfoCollection, ICreditInfo, CreditInfoCollectionViewModel, CreditInfoViewModel>(), src => src))
                .ForMember(dest => dest.Keys, opt => opt.Ignore())
                .ForMember(dest => dest.Values, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IBudgetAccount, AccountIdentificationViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IBudgetAccount, AccountCoreDataViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IBudgetAccount, BudgetAccountViewModel>()
                .ForMember(dest => dest.BudgetInfos, opt => opt.MapFrom(src => src.BudgetInfoCollection))
                .ForMember(dest => dest.PostingLines, opt => opt.MapFrom(src => src.PostingLineCollection.Between(src.StatusDate.Year > DateTime.MinValue.Year ? new DateTime(src.StatusDate.Year - 1, 1, 1) : DateTime.MinValue, src.StatusDate).Top(25)))
                .ForMember(dest => dest.BudgetAccountGroups, opt => opt.MapFrom(src => new List<BudgetAccountGroupViewModel>(0)))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<BudgetAccountViewModel, CreateBudgetAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber))
                .ForMember(dest => dest.BudgetAccountGroupNumber, opt => opt.MapFrom(src => src.BudgetAccountGroup.Number))
                .ForMember(dest => dest.BudgetInfoCollection, opt =>
                {
                    opt.Condition(src => src.BudgetInfos != null);
                    opt.ConvertUsing(new BudgetInfoDictionaryViewModelToBudgetInfoCommandCollectionValueConverter(), "BudgetInfos");
                });

            mapperConfiguration.CreateMap<BudgetAccountViewModel, UpdateBudgetAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber))
                .ForMember(dest => dest.BudgetAccountGroupNumber, opt => opt.MapFrom(src => src.BudgetAccountGroup.Number))
                .ForMember(dest => dest.BudgetInfoCollection, opt =>
                {
                    opt.Condition(src => src.BudgetInfos != null);
                    opt.ConvertUsing(new BudgetInfoDictionaryViewModelToBudgetInfoCommandCollectionValueConverter(), "BudgetInfos");
                });

            mapperConfiguration.CreateMap<BudgetAccountViewModel, DeleteBudgetAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber));

            mapperConfiguration.CreateMap<IBudgetAccountCollection, BudgetAccountCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(budgetAccount => budgetAccount.AccountNumber).ToArray()));

            mapperConfiguration.CreateMap<IBudgetAccountCollection, BudgetAccountDictionaryViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.GroupByBudgetAccountGroupAsync().GetAwaiter().GetResult()))
                .ForMember(dest => dest.Keys, opt => opt.Ignore())
                .ForMember(dest => dest.Values, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IBudgetInfo, BudgetInfoViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<BudgetInfoViewModel, BudgetInfoCommand>();

            mapperConfiguration.CreateMap<IBudgetInfoValues, BudgetInfoValuesViewModel>();

            mapperConfiguration.CreateMap<IBudgetInfoCollection, BudgetInfoCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderByDescending(budgetInfo => budgetInfo.Year * 100 + budgetInfo.Month).ToArray()));

            mapperConfiguration.CreateMap<IBudgetInfoCollection, BudgetInfoDictionaryViewModel>()
                .ForMember(dest => dest.Items, opt => opt.ConvertUsing(new InfoCollectionToDictionaryValueConverter<IBudgetInfoCollection, IBudgetInfo, BudgetInfoCollectionViewModel, BudgetInfoViewModel>(), src => src))
                .ForMember(dest => dest.Keys, opt => opt.Ignore())
                .ForMember(dest => dest.Values, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IContactAccount, AccountIdentificationViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContactAccount, AccountCoreDataViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContactAccount, ContactAccountViewModel>()
                .ForMember(dest => dest.BalanceInfos, opt => opt.MapFrom(src => src.ContactInfoCollection))
                .ForMember(dest => dest.PostingLines, opt => opt.MapFrom(src => src.PostingLineCollection.Between(DateTime.MinValue, src.StatusDate).Top(25)))
                .ForMember(dest => dest.PaymentTerms, opt => opt.MapFrom(src => new List<PaymentTermViewModel>(0)))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<ContactAccountViewModel, CreateContactAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber))
                .ForMember(dest => dest.PaymentTermNumber, opt => opt.MapFrom(src => src.PaymentTerm.Number));

            mapperConfiguration.CreateMap<ContactAccountViewModel, UpdateContactAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber))
                .ForMember(dest => dest.PaymentTermNumber, opt => opt.MapFrom(src => src.PaymentTerm.Number));

            mapperConfiguration.CreateMap<ContactAccountViewModel, DeleteContactAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber));

            mapperConfiguration.CreateMap<IContactAccountCollection, ContactAccountCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(contactAccount => contactAccount.AccountName).ToArray()));

            mapperConfiguration.CreateMap<IContactInfo, BalanceInfoViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContactInfoValues, BalanceInfoValuesViewModel>();

            mapperConfiguration.CreateMap<IContactAccountCollectionValues, ContactAccountCollectionValuesViewModel>();

            mapperConfiguration.CreateMap<IContactInfoCollection, BalanceInfoCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderByDescending(contactInfo => contactInfo.Year * 100 + contactInfo.Month).ToArray()));

            mapperConfiguration.CreateMap<IContactInfoCollection, BalanceInfoDictionaryViewModel>()
                .ForMember(dest => dest.Items, opt => opt.ConvertUsing(new InfoCollectionToDictionaryValueConverter<IContactInfoCollection, IContactInfo, BalanceInfoCollectionViewModel, BalanceInfoViewModel>(), src => src))
                .ForMember(dest => dest.Keys, opt => opt.Ignore())
                .ForMember(dest => dest.Values, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IPostingLine, PostingLineViewModel>()
                .ForMember(dest => dest.Debit, opt => opt.MapFrom(src => src.Debit != 0M ? src.Debit : (decimal?) null))
                .ForMember(dest => dest.Credit, opt => opt.MapFrom(src => src.Credit != 0M ? src.Credit : (decimal?) null))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IPostingLineCollection, PostingLineCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Ordered().ToArray()));

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

        private class InfoCollectionToDictionaryValueConverter<TInfoCollection, TInfo, TInfoCollectionViewModel, TInfoViewModel> : IValueConverter<TInfoCollection, IDictionary<short, TInfoCollectionViewModel>> where TInfoCollection : IInfoCollection<TInfo> where TInfo : IInfo<TInfo> where TInfoCollectionViewModel : InfoCollectionViewModelBase<TInfoViewModel>, new() where TInfoViewModel : InfoViewModelBase
        {
            #region Methods

            public IDictionary<short, TInfoCollectionViewModel> Convert(TInfoCollection sourceMember, ResolutionContext context)
            {
                NullGuard.NotNull(sourceMember, nameof(sourceMember))
                    .NotNull(context, nameof(context));

                IDictionary<short, TInfoCollectionViewModel> dictionary = sourceMember.GroupBy(info => info.Year)
                    .OrderBy(group => group.Key)
                    .ToDictionary(group => group.Key, group =>
                    {
                        return new TInfoCollectionViewModel
                        {
                            Items = group.Select(info => context.Mapper.Map<TInfo, TInfoViewModel>(info))
                                .OrderBy(infoViewModel => infoViewModel.Month)
                                .ToArray()
                        };
                    });

                return new ConcurrentDictionary<short, TInfoCollectionViewModel>(dictionary);
            }

            #endregion
        }

        private class CreditInfoDictionaryViewModelToCreditInfoCommandCollectionValueConverter : InfoDictionaryViewModelToInfoCommandCollectionValueConverter<CreditInfoCollectionViewModel, CreditInfoViewModel, ICreditInfoCommand>
        {
            #region Methods

            protected override ICreditInfoCommand Convert(CreditInfoViewModel creditInfoViewModel, ResolutionContext context)
            {
                NullGuard.NotNull(creditInfoViewModel, nameof(creditInfoViewModel))
                    .NotNull(context, nameof(context));

                return context.Mapper.Map<CreditInfoViewModel, CreditInfoCommand>(creditInfoViewModel);
            }

            #endregion
        }

        private class BudgetInfoDictionaryViewModelToBudgetInfoCommandCollectionValueConverter : InfoDictionaryViewModelToInfoCommandCollectionValueConverter<BudgetInfoCollectionViewModel, BudgetInfoViewModel, IBudgetInfoCommand>
        {
            #region Methods

            protected override IBudgetInfoCommand Convert(BudgetInfoViewModel budgetInfoViewModel, ResolutionContext context)
            {
                NullGuard.NotNull(budgetInfoViewModel, nameof(budgetInfoViewModel))
                    .NotNull(context, nameof(context));

                return context.Mapper.Map<BudgetInfoViewModel, BudgetInfoCommand>(budgetInfoViewModel);
            }

            #endregion
        }

        private abstract class InfoDictionaryViewModelToInfoCommandCollectionValueConverter<TInfoCollectionViewModel, TInfoViewModel, TInfoCommand> : IValueConverter<IReadOnlyDictionary<short, TInfoCollectionViewModel>, IEnumerable<TInfoCommand>> where TInfoCollectionViewModel : InfoCollectionViewModelBase<TInfoViewModel> where TInfoViewModel : InfoViewModelBase where TInfoCommand : IInfoCommand
        {
            #region Methods

            public IEnumerable<TInfoCommand> Convert(IReadOnlyDictionary<short, TInfoCollectionViewModel> sourceMember, ResolutionContext context)
            {
                NullGuard.NotNull(sourceMember, nameof(sourceMember))
                    .NotNull(context, nameof(context));

                return sourceMember.SelectMany(item => item.Value)
                    .AsParallel()
                    .Where(infoViewModel => infoViewModel.Editable || infoViewModel.IsCurrentMonth)
                    .Select(infoViewModel => Convert(infoViewModel, context))
                    .ToArray();
            }

            protected abstract TInfoCommand Convert(TInfoViewModel infoViewModel, ResolutionContext context);

            #endregion
        }

        #endregion
    }
}