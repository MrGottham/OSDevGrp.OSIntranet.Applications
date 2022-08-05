using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly ITypeConverter<IAccountCollection, AccountDictionaryViewModel> _accountCollectionToAccountDictionaryViewModelTypeConverter = new AccountCollectionToAccountDictionaryViewModelTypeConverter();
        private readonly ITypeConverter<IBudgetAccountCollection, BudgetAccountDictionaryViewModel> _budgetAccountCollectionToBudgetAccountDictionaryViewModelTypeConverter = new BudgetAccountCollectionToBudgetAccountDictionaryViewModelTypeConverter();
        private readonly ITypeConverter<ICreditInfoCollection, CreditInfoDictionaryViewModel> _creditInfoCollectionToCreditInfoDictionaryViewModelTypeConverter = new CreditInfoCollectionToCreditInfoDictionaryViewModelTypeConverter();
        private readonly ITypeConverter<IBudgetInfoCollection, BudgetInfoDictionaryViewModel> _budgetInfoCollectionToBudgetInfoDictionaryViewModelTypeConverter = new BudgetInfoCollectionToBudgetInfoDictionaryViewModelTypeConverter();
        private readonly ITypeConverter<IContactInfoCollection, BalanceInfoDictionaryViewModel> _contactInfoCollectionTBalanceInfoCollectionViewModelTypeConverter = new ContactInfoCollectionTBalanceInfoCollectionViewModelTypeConverter();
        private readonly IValueConverter<CreditInfoDictionaryViewModel, IEnumerable<ICreditInfoCommand>> _creditInfoDictionaryViewModelToCreditInfoCommandCollectionValueConverter = new CreditInfoDictionaryViewModelToCreditInfoCommandCollectionValueConverter();
        private readonly IValueConverter<BudgetInfoDictionaryViewModel, IEnumerable<IBudgetInfoCommand>> _budgetInfoDictionaryViewModelToBudgetInfoCommandCollectionValueConverter = new BudgetInfoDictionaryViewModelToBudgetInfoCommandCollectionValueConverter();
        private readonly ITypeConverter<IPostingLineCollection, PostingLineCollectionViewModel> _postingLineCollectionToPostingLineCollectionViewModelTypeConverter = new PostingLineCollectionToPostingLineCollectionViewModelTypeConverter();
        private readonly ITypeConverter<IPostingWarningCollection, PostingWarningCollectionViewModel> _postingWarningCollectionToPostingWarningCollectionViewModelTypeConverter = new PostingWarningCollectionToPostingWarningCollectionViewModelTypeConverter();
        private readonly IValueConverter<ApplyPostingLineCollectionViewModel, IEnumerable<IApplyPostingLineCommand>> _applyPostingLineCollectionViewModelToApplyPostingLineCommandCollectionValueConverter = new ApplyPostingLineCollectionViewModelToApplyPostingLineCommandCollectionValueConverter();

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
                .ForMember(dest => dest.PostingJournalKey, opt => opt.Ignore())
                .ForMember(dest => dest.PostingJournal, opt => opt.Ignore())
                .ForMember(dest => dest.PostingJournalResultKey, opt => opt.Ignore())
                .ForMember(dest => dest.PostingJournalResult, opt => opt.Ignore())
                .ForMember(dest => dest.LetterHeads, opt => opt.MapFrom(src => new List<LetterHeadViewModel>(0)))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<AccountingViewModel, CreateAccountingCommand>()
                .ForMember(dest => dest.LetterHeadNumber, opt => opt.MapFrom(src => src.LetterHead.Number));

            mapperConfiguration.CreateMap<AccountingViewModel, UpdateAccountingCommand>()
                .ForMember(dest => dest.LetterHeadNumber, opt => opt.MapFrom(src => src.LetterHead.Number));

            mapperConfiguration.CreateMap<IAccountBase, AccountIdentificationViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

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
                    opt.ConvertUsing(_creditInfoDictionaryViewModelToCreditInfoCommandCollectionValueConverter, src => src.CreditInfos);
                });

            mapperConfiguration.CreateMap<AccountViewModel, UpdateAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber))
                .ForMember(dest => dest.AccountGroupNumber, opt => opt.MapFrom(src => src.AccountGroup.Number))
                .ForMember(dest => dest.CreditInfoCollection, opt =>
                {
                    opt.Condition(src => src.CreditInfos != null);
                    opt.ConvertUsing(_creditInfoDictionaryViewModelToCreditInfoCommandCollectionValueConverter, src => src.CreditInfos);
                });

            mapperConfiguration.CreateMap<AccountViewModel, DeleteAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber));

            mapperConfiguration.CreateMap<IAccountCollection, AccountCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(account => account.AccountNumber).ToArray()));

            mapperConfiguration.CreateMap<IAccountCollection, AccountDictionaryViewModel>()
                .ConvertUsing(_accountCollectionToAccountDictionaryViewModelTypeConverter);

            mapperConfiguration.CreateMap<ICreditInfo, CreditInfoViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<CreditInfoViewModel, CreditInfoCommand>();

            mapperConfiguration.CreateMap<ICreditInfoValues, CreditInfoValuesViewModel>();

            mapperConfiguration.CreateMap<IAccountCollectionValues, AccountCollectionValuesViewModel>();

            mapperConfiguration.CreateMap<ICreditInfoCollection, CreditInfoCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderByDescending(creditInfo => creditInfo.Year * 100 + creditInfo.Month).ToArray()));

            mapperConfiguration.CreateMap<ICreditInfoCollection, CreditInfoDictionaryViewModel>()
                .ConvertUsing(_creditInfoCollectionToCreditInfoDictionaryViewModelTypeConverter);

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
                    opt.ConvertUsing(_budgetInfoDictionaryViewModelToBudgetInfoCommandCollectionValueConverter, src => src.BudgetInfos);
                });

            mapperConfiguration.CreateMap<BudgetAccountViewModel, UpdateBudgetAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber))
                .ForMember(dest => dest.BudgetAccountGroupNumber, opt => opt.MapFrom(src => src.BudgetAccountGroup.Number))
                .ForMember(dest => dest.BudgetInfoCollection, opt =>
                {
                    opt.Condition(src => src.BudgetInfos != null);
                    opt.ConvertUsing(_budgetInfoDictionaryViewModelToBudgetInfoCommandCollectionValueConverter, src => src.BudgetInfos);
                });

            mapperConfiguration.CreateMap<BudgetAccountViewModel, DeleteBudgetAccountCommand>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Accounting.AccountingNumber));

            mapperConfiguration.CreateMap<IBudgetAccountCollection, BudgetAccountCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderBy(budgetAccount => budgetAccount.AccountNumber).ToArray()));

            mapperConfiguration.CreateMap<IBudgetAccountCollection, BudgetAccountDictionaryViewModel>()
                .ConvertUsing(_budgetAccountCollectionToBudgetAccountDictionaryViewModelTypeConverter);

            mapperConfiguration.CreateMap<IBudgetInfo, BudgetInfoViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<BudgetInfoViewModel, BudgetInfoCommand>();

            mapperConfiguration.CreateMap<IBudgetInfoValues, BudgetInfoValuesViewModel>();

            mapperConfiguration.CreateMap<IBudgetInfoCollection, BudgetInfoCollectionViewModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderByDescending(budgetInfo => budgetInfo.Year * 100 + budgetInfo.Month).ToArray()));

            mapperConfiguration.CreateMap<IBudgetInfoCollection, BudgetInfoDictionaryViewModel>()
                .ConvertUsing(_budgetInfoCollectionToBudgetInfoDictionaryViewModelTypeConverter);

            mapperConfiguration.CreateMap<IContactAccount, AccountIdentificationViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContactAccount, AccountCoreDataViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContactAccount, ContactAccountViewModel>()
                .ForMember(dest => dest.BalanceInfos, opt => opt.MapFrom(src => src.ContactInfoCollection))
                .ForMember(dest => dest.PostingLines, opt => opt.MapFrom(src => src.PostingLineCollection.Between(DateTime.MinValue, src.StatusDate).Top(25)))
                .ForMember(dest => dest.PaymentTerms, opt => opt.MapFrom(src => new List<PaymentTermViewModel>(0)))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None))
                .AfterMap((src, dest) =>
                {
                    if (dest.PostingLines != null)
                    {
                        dest.PostingLines.ViewMode = PostingLineCollectionViewMode.WithBalanceForContactAccount;
                    }
                });

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
                .ConvertUsing(_contactInfoCollectionTBalanceInfoCollectionViewModelTypeConverter);

            mapperConfiguration.CreateMap<IPostingLine, PostingLineViewModel>()
                .ForMember(dest => dest.AccountValuesAtPostingDate, opt =>
                {
                    opt.Condition(src => src.Account != null && src.AccountValuesAtPostingDate != null);
                    opt.MapFrom(src => src.AccountValuesAtPostingDate);
                })
                .ForMember(dest => dest.BudgetAccountValuesAtPostingDate, opt =>
                {
                    opt.Condition(src => src.BudgetAccount != null && src.BudgetAccountValuesAtPostingDate != null);
                    opt.MapFrom(src => src.BudgetAccountValuesAtPostingDate);
                })
                .ForMember(dest => dest.Debit, opt => opt.MapFrom(src => src.Debit != 0M ? src.Debit : (decimal?)null))
                .ForMember(dest => dest.Credit, opt => opt.MapFrom(src => src.Credit != 0M ? src.Credit : (decimal?)null))
                .ForMember(dest => dest.ContactAccountValuesAtPostingDate, opt =>
                {
                    opt.Condition(src => src.ContactAccount != null && src.ContactAccountValuesAtPostingDate != null);
                    opt.MapFrom(src => src.ContactAccountValuesAtPostingDate);
                })
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IPostingLineCollection, PostingLineCollectionViewModel>()
                .ConvertUsing(_postingLineCollectionToPostingLineCollectionViewModelTypeConverter);

            mapperConfiguration.CreateMap<IPostingWarning, PostingWarningViewModel>()
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => Guid.NewGuid()));

            mapperConfiguration.CreateMap<IPostingWarningCollection, PostingWarningCollectionViewModel>()
                .ConvertUsing(_postingWarningCollectionToPostingWarningCollectionViewModelTypeConverter);

            mapperConfiguration.CreateMap<IPostingJournalResult, ApplyPostingJournalResultViewModel>()
                .ForMember(dest => dest.PostingLines, opt => opt.MapFrom(src => src.PostingLineCollection))
                .ForMember(dest => dest.PostingWarnings, opt => opt.MapFrom(src => src.PostingWarningCollection));

            mapperConfiguration.CreateMap<ApplyPostingLineViewModel, ApplyPostingLineCommand>()
                .ForMember(dest => dest.PostingDate, opt => opt.MapFrom(src => src.PostingDate.LocalDateTime.Date))
                .ForMember(dest => dest.Reference, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.Reference) == false);
                    opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Reference) ? null : src.Reference);
                })
                .ForMember(dest => dest.BudgetAccountNumber, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.BudgetAccountNumber) == false);
                    opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BudgetAccountNumber) ? null : src.BudgetAccountNumber);
                })
                .ForMember(dest => dest.ContactAccountNumber, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.ContactAccountNumber) == false);
                    opt.MapFrom(src => string.IsNullOrWhiteSpace(src.ContactAccountNumber) ? null : src.ContactAccountNumber);
                })
                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder ?? 0));

            mapperConfiguration.CreateMap<ApplyPostingJournalViewModel, ApplyPostingJournalCommand>()
                .ForMember(dest => dest.PostingLineCollection, opt => opt.ConvertUsing(_applyPostingLineCollectionViewModelToApplyPostingLineCommandCollectionValueConverter, src => src.ApplyPostingLines));

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

            mapperConfiguration.CreateMap<Domain.Interfaces.Accounting.Enums.PostingWarningReason, PostingWarningReason>();

            mapperConfiguration.CreateMap<IPaymentTerm, PaymentTermViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<PaymentTermViewModel, CreatePaymentTermCommand>();
            mapperConfiguration.CreateMap<PaymentTermViewModel, UpdatePaymentTermCommand>();
            mapperConfiguration.CreateMap<PaymentTermViewModel, DeletePaymentTermCommand>();
        }

        private abstract class AccountCollectionToAccountDictionaryViewModelTypeConverterBase<TAccountGroup, TAccountCollection, TAccount, TAccountDictionaryViewModel, TAccountGroupViewModel, TAccountCollectionViewModel, TAccountViewModel> : ITypeConverter<TAccountCollection, TAccountDictionaryViewModel> where TAccountGroup : IAccountGroupBase where TAccountCollection : IAccountCollectionBase<TAccount> where TAccount : IAccountBase<TAccount> where TAccountDictionaryViewModel : AccountDictionaryViewModelBase<TAccountGroupViewModel, TAccountCollectionViewModel, TAccountViewModel>, new() where TAccountGroupViewModel : AccountGroupViewModelBase where TAccountCollectionViewModel : AccountCollectionViewModelBase<TAccountViewModel> where TAccountViewModel : AccountIdentificationViewModel
        {
            #region Private variables

            private readonly Func<TAccountCollection, IReadOnlyDictionary<TAccountGroup, TAccountCollection>> _dictionaryGetter;

            #endregion

            #region Constructor

            protected AccountCollectionToAccountDictionaryViewModelTypeConverterBase(Func<TAccountCollection, IReadOnlyDictionary<TAccountGroup, TAccountCollection>> dictionaryGetter)
            {
                NullGuard.NotNull(dictionaryGetter, nameof(dictionaryGetter));

                _dictionaryGetter = dictionaryGetter;
            }

            #endregion

            #region Methods

            public TAccountDictionaryViewModel Convert(TAccountCollection accountCollection, TAccountDictionaryViewModel accountDictionaryViewModel, ResolutionContext context)
            {
                NullGuard.NotNull(accountCollection, nameof(accountCollection))
                    .NotNull(context, nameof(context));

                IDictionary<TAccountGroupViewModel, TAccountCollectionViewModel> dictionary = _dictionaryGetter(accountCollection)
                    .ToDictionary(
                        item => context.Mapper.Map<TAccountGroup, TAccountGroupViewModel>(item.Key),
                        item => context.Mapper.Map<TAccountCollection, TAccountCollectionViewModel>(item.Value));

                TAccountDictionaryViewModel result = new TAccountDictionaryViewModel
                {
                    Items = new ReadOnlyDictionary<TAccountGroupViewModel, TAccountCollectionViewModel>(dictionary)
                };

                return OnConvert(accountCollection, result, context);
            }

            protected abstract TAccountDictionaryViewModel OnConvert(TAccountCollection accountCollection, TAccountDictionaryViewModel accountDictionaryViewModel, ResolutionContext context);

            #endregion
        }

        private class AccountCollectionToAccountDictionaryViewModelTypeConverter : AccountCollectionToAccountDictionaryViewModelTypeConverterBase<IAccountGroup, IAccountCollection, IAccount, AccountDictionaryViewModel, AccountGroupViewModel,  AccountCollectionViewModel, AccountViewModel>
        {
            #region Constructor

            public AccountCollectionToAccountDictionaryViewModelTypeConverter()
                : base(accountCollection => accountCollection.GroupByAccountGroupAsync().GetAwaiter().GetResult())
            {
            }

            #endregion

            #region Methods

            protected override AccountDictionaryViewModel OnConvert(IAccountCollection accountCollection, AccountDictionaryViewModel accountDictionaryViewModel, ResolutionContext context)
            {
                NullGuard.NotNull(accountCollection, nameof(accountCollection))
                    .NotNull(accountDictionaryViewModel, nameof(accountDictionaryViewModel))
                    .NotNull(context, nameof(context));

                accountDictionaryViewModel.ValuesAtStatusDate = context.Mapper.Map<IAccountCollectionValues, AccountCollectionValuesViewModel>(accountCollection.ValuesAtStatusDate);
                accountDictionaryViewModel.ValuesAtEndOfLastMonthFromStatusDate = context.Mapper.Map<IAccountCollectionValues, AccountCollectionValuesViewModel>(accountCollection.ValuesAtEndOfLastMonthFromStatusDate);
                accountDictionaryViewModel.ValuesAtEndOfLastYearFromStatusDate = context.Mapper.Map<IAccountCollectionValues, AccountCollectionValuesViewModel>(accountCollection.ValuesAtEndOfLastYearFromStatusDate);

                return accountDictionaryViewModel;
            }

            #endregion
        }

        private class BudgetAccountCollectionToBudgetAccountDictionaryViewModelTypeConverter : AccountCollectionToAccountDictionaryViewModelTypeConverterBase<IBudgetAccountGroup, IBudgetAccountCollection, IBudgetAccount, BudgetAccountDictionaryViewModel, BudgetAccountGroupViewModel, BudgetAccountCollectionViewModel, BudgetAccountViewModel>
        {
            #region Constructor

            public BudgetAccountCollectionToBudgetAccountDictionaryViewModelTypeConverter() 
                : base(budgetAccountCollection => budgetAccountCollection.GroupByBudgetAccountGroupAsync().GetAwaiter().GetResult())
            {
            }

            #endregion

            #region Methods

            protected override BudgetAccountDictionaryViewModel OnConvert(IBudgetAccountCollection budgetAccountCollection, BudgetAccountDictionaryViewModel budgetAccountDictionaryViewModel, ResolutionContext context)
            {
                NullGuard.NotNull(budgetAccountCollection, nameof(budgetAccountCollection))
                    .NotNull(budgetAccountDictionaryViewModel, nameof(budgetAccountDictionaryViewModel))
                    .NotNull(context, nameof(context));

                budgetAccountDictionaryViewModel.ValuesForMonthOfStatusDate = context.Mapper.Map<IBudgetInfoValues, BudgetInfoValuesViewModel>(budgetAccountCollection.ValuesForMonthOfStatusDate);
                budgetAccountDictionaryViewModel.ValuesForLastMonthOfStatusDate = context.Mapper.Map<IBudgetInfoValues, BudgetInfoValuesViewModel>(budgetAccountCollection.ValuesForLastMonthOfStatusDate);
                budgetAccountDictionaryViewModel.ValuesForYearToDateOfStatusDate = context.Mapper.Map<IBudgetInfoValues, BudgetInfoValuesViewModel>(budgetAccountCollection.ValuesForYearToDateOfStatusDate);
                budgetAccountDictionaryViewModel.ValuesForLastYearOfStatusDate = context.Mapper.Map<IBudgetInfoValues, BudgetInfoValuesViewModel>(budgetAccountCollection.ValuesForLastYearOfStatusDate);

                return budgetAccountDictionaryViewModel;
            }

            #endregion
        }

        private abstract class InfoCollectionToInfoDictionaryViewModelTypeConverterBase<TInfoCollection, TInfo, TInfoDictionaryViewModel, TInfoCollectionViewModel, TInfoViewModel> : ITypeConverter<TInfoCollection, TInfoDictionaryViewModel> where TInfoCollection : IInfoCollection<TInfo> where TInfo : IInfo<TInfo> where TInfoDictionaryViewModel : InfoDictionaryViewModelBase<TInfoCollectionViewModel, TInfoViewModel>, new() where TInfoCollectionViewModel : InfoCollectionViewModelBase<TInfoViewModel>, new() where TInfoViewModel : InfoViewModelBase
        {
            #region Methods

            public TInfoDictionaryViewModel Convert(TInfoCollection infoCollection, TInfoDictionaryViewModel infoDictionaryViewModel, ResolutionContext context)
            {
                NullGuard.NotNull(infoCollection, nameof(infoCollection))
                    .NotNull(context, nameof(context));

                IDictionary<short, TInfoCollectionViewModel> dictionary = infoCollection.GroupBy(info => info.Year)
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

                return new TInfoDictionaryViewModel
                {
                    Items = new ConcurrentDictionary<short, TInfoCollectionViewModel>(dictionary)
                };
            }

            #endregion
        }

        private class CreditInfoCollectionToCreditInfoDictionaryViewModelTypeConverter : InfoCollectionToInfoDictionaryViewModelTypeConverterBase<ICreditInfoCollection, ICreditInfo, CreditInfoDictionaryViewModel, CreditInfoCollectionViewModel, CreditInfoViewModel>
        {
        }

        private class BudgetInfoCollectionToBudgetInfoDictionaryViewModelTypeConverter : InfoCollectionToInfoDictionaryViewModelTypeConverterBase<IBudgetInfoCollection, IBudgetInfo, BudgetInfoDictionaryViewModel, BudgetInfoCollectionViewModel, BudgetInfoViewModel>
        {
        }

        private class ContactInfoCollectionTBalanceInfoCollectionViewModelTypeConverter : InfoCollectionToInfoDictionaryViewModelTypeConverterBase<IContactInfoCollection, IContactInfo, BalanceInfoDictionaryViewModel, BalanceInfoCollectionViewModel, BalanceInfoViewModel>
        {
        }

        private abstract class InfoDictionaryViewModelToInfoCommandCollectionValueConverterBase<TInfoCollectionViewModel, TInfoViewModel, TInfoCommand> : IValueConverter<IReadOnlyDictionary<short, TInfoCollectionViewModel>, IEnumerable<TInfoCommand>> where TInfoCollectionViewModel : InfoCollectionViewModelBase<TInfoViewModel> where TInfoViewModel : InfoViewModelBase where TInfoCommand : IInfoCommand
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

        private class CreditInfoDictionaryViewModelToCreditInfoCommandCollectionValueConverter : InfoDictionaryViewModelToInfoCommandCollectionValueConverterBase<CreditInfoCollectionViewModel, CreditInfoViewModel, ICreditInfoCommand>
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

        private class BudgetInfoDictionaryViewModelToBudgetInfoCommandCollectionValueConverter : InfoDictionaryViewModelToInfoCommandCollectionValueConverterBase<BudgetInfoCollectionViewModel, BudgetInfoViewModel, IBudgetInfoCommand>
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

        private class PostingLineCollectionToPostingLineCollectionViewModelTypeConverter : ITypeConverter<IPostingLineCollection, PostingLineCollectionViewModel>
        {
            #region Methods

            public PostingLineCollectionViewModel Convert(IPostingLineCollection postingLineCollection, PostingLineCollectionViewModel postingLineCollectionViewModel, ResolutionContext context)
            {
                NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection))
                    .NotNull(context, nameof(context));

                postingLineCollectionViewModel ??= new PostingLineCollectionViewModel();

                postingLineCollectionViewModel.AddRange(postingLineCollection.Ordered().Select(postingLine => context.Mapper.Map<IPostingLine, PostingLineViewModel>(postingLine)).ToArray());

                return postingLineCollectionViewModel;
            }

            #endregion
        }

        private class PostingWarningCollectionToPostingWarningCollectionViewModelTypeConverter : ITypeConverter<IPostingWarningCollection, PostingWarningCollectionViewModel>
        {
            #region Methods

            public PostingWarningCollectionViewModel Convert(IPostingWarningCollection postingWarningCollection, PostingWarningCollectionViewModel postingWarningCollectionViewModel, ResolutionContext context)
            {
                NullGuard.NotNull(postingWarningCollection, nameof(postingWarningCollection))
                    .NotNull(context, nameof(context));

                postingWarningCollectionViewModel ??= new PostingWarningCollectionViewModel();

                postingWarningCollectionViewModel.AddRange(postingWarningCollection.Ordered().Select(postingWarning => context.Mapper.Map<IPostingWarning, PostingWarningViewModel>(postingWarning)).ToArray());

                return postingWarningCollectionViewModel;
            }

            #endregion
        }

        private class ApplyPostingLineCollectionViewModelToApplyPostingLineCommandCollectionValueConverter : IValueConverter<ApplyPostingLineCollectionViewModel, IEnumerable<IApplyPostingLineCommand>>
        {
            #region Methods

            public IEnumerable<IApplyPostingLineCommand> Convert(ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel, ResolutionContext context)
            {
                NullGuard.NotNull(applyPostingLineCollectionViewModel, nameof(applyPostingLineCollectionViewModel))
                    .NotNull(context, nameof(context));

                return applyPostingLineCollectionViewModel
                    .OrderBy(applyPostingLineViewModel => applyPostingLineViewModel.PostingDate.LocalDateTime.Date)
                    .ThenBy(applyPostingLineViewModel => applyPostingLineViewModel.SortOrder ?? 0)
                    .Select(applyPostingLineViewModel => context.Mapper.Map<ApplyPostingLineViewModel, ApplyPostingLineCommand>(applyPostingLineViewModel))
                    .ToArray();
            }

            #endregion
        }

        #endregion
    }
}