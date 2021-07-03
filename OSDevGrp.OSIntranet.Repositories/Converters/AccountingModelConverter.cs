using System.Collections.Concurrent;
using System.Collections.Generic;
using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;
using OSDevGrp.OSIntranet.Repositories.Models.Common;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
    internal class AccountingModelConverter : ConverterBase
    {
        #region Private variables

        private readonly IConverter _commonModelConverter = CommonModelConverter.Create();
        private readonly IDictionary<int, IAccounting> _accountingDictionary = new ConcurrentDictionary<int, IAccounting>();
        private readonly object _syncRoot = new object();

        #endregion

        #region Properties

        protected override IDictionary<string, object> StateDictionary 
        {
            get
            {
                IDictionary<string, object> stateDictionary = new ConcurrentDictionary<string, object>();
                stateDictionary.Add("AccountingDictionary", _accountingDictionary);
                stateDictionary.Add("AccountingModelConverter", this);
                stateDictionary.Add("SyncRoot", _syncRoot);
                return stateDictionary;
            }
        }

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<AccountingModel, IAccounting>()
                .ConvertUsing((accountingModel, accounting, context) => accountingModel.ToDomain(ResolveAccountingDictionaryFromContext(context.Items), ResolveAccountingModelConverterFromContext(context.Items), _commonModelConverter, ResolveSyncRootFromContext(context.Items)));

            mapperConfiguration.CreateMap<IAccounting, AccountingModel>()
                .ForMember(dest => dest.AccountingIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.LetterHeadIdentifier, opt => opt.MapFrom(src => src.LetterHead.Number))
                .ForMember(dest => dest.LetterHead, opt => opt.MapFrom(src => _commonModelConverter.Convert<ILetterHead, LetterHeadModel>(src.LetterHead)))
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.AccountCollection))
                .ForMember(dest => dest.BudgetAccounts, opt => opt.MapFrom(src => src.BudgetAccountCollection))
                .ForMember(dest => dest.ContactAccounts, opt => opt.MapFrom(src => src.ContactAccountCollection))
                .ForMember(dest => dest.PostingLines, opt => opt.MapFrom(src => new List<PostingLineModel>(0)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<AccountModel, IAccount>()
                .ConvertUsing((accountModel, account, context) => accountModel.ToDomain(ResolveAccountingModelConverterFromContext(context.Items), ResolveSyncRootFromContext(context.Items)));

            mapperConfiguration.CreateMap<IAccount, AccountModel>()
                .ForMember(dest => dest.AccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.AccountingIdentifier, opt => opt.MapFrom(src => src.Accounting.Number))
                .ForMember(dest => dest.BasicAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.BasicAccount, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.AccountGroupIdentifier, opt => opt.MapFrom(src => src.AccountGroup.Number))
                .ForMember(dest => dest.CreditInfos, opt => opt.MapFrom(src => new List<CreditInfoModel>(0)))
                .ForMember(dest => dest.PostingLines, opt => opt.MapFrom(src => new List<PostingLineModel>(0)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<IAccount, BasicAccountModel>()
                .ForMember(dest => dest.BasicAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.Accounts, opt => opt.Ignore())
                .ForMember(dest => dest.BudgetAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.ContactAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<BudgetAccountModel, IBudgetAccount>()
                .ConvertUsing((budgetAccountModel, budgetAccount, context) => budgetAccountModel.ToDomain(ResolveAccountingModelConverterFromContext(context.Items), ResolveSyncRootFromContext(context.Items)));

            mapperConfiguration.CreateMap<IBudgetAccount, BudgetAccountModel>()
                .ForMember(dest => dest.BudgetAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.AccountingIdentifier, opt => opt.MapFrom(src => src.Accounting.Number))
                .ForMember(dest => dest.BasicAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.BasicAccount, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.BudgetAccountGroupIdentifier, opt => opt.MapFrom(src => src.BudgetAccountGroup.Number))
                .ForMember(dest => dest.BudgetInfos, opt => opt.MapFrom(src => new List<BudgetInfoModel>(0)))
                .ForMember(dest => dest.PostingLines, opt => opt.MapFrom(src => new List<PostingLineModel>(0)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<IBudgetAccount, BasicAccountModel>()
                .ForMember(dest => dest.BasicAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.Accounts, opt => opt.Ignore())
                .ForMember(dest => dest.BudgetAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.ContactAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<ContactAccountModel, IContactAccount>()
                .ConvertUsing((contactAccountModel, contactAccount, context) => contactAccountModel.ToDomain(ResolveAccountingModelConverterFromContext(context.Items), ResolveSyncRootFromContext(context.Items)));

            mapperConfiguration.CreateMap<IContactAccount, ContactAccountModel>()
                .ForMember(dest => dest.ContactAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.AccountingIdentifier, opt => opt.MapFrom(src => src.Accounting.Number))
                .ForMember(dest => dest.BasicAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.BasicAccount, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.PaymentTermIdentifier, opt => opt.MapFrom(src => src.PaymentTerm.Number))
                .ForMember(dest => dest.PostingLines, opt => opt.MapFrom(src => new List<PostingLineModel>(0)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<IContactAccount, BasicAccountModel>()
                .ForMember(dest => dest.BasicAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.Accounts, opt => opt.Ignore())
                .ForMember(dest => dest.BudgetAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.ContactAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<CreditInfoModel, ICreditInfo>()
                .ConvertUsing((creditInfoModel, creditInfo, context) => creditInfoModel.ToDomain(ResolveAccountingModelConverterFromContext(context.Items)));

            mapperConfiguration.CreateMap<ICreditInfo, CreditInfoModel>()
                .ForMember(dest => dest.CreditInfoIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.AccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.YearMonthIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.YearMonth, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<ICreditInfo, YearMonthModel>()
                .ForMember(dest => dest.YearMonthIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.CreditInfos, opt => opt.Ignore())
                .ForMember(dest => dest.BudgetInfos, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<BudgetInfoModel, IBudgetInfo>()
                .ConvertUsing((budgetInfoModel, budgetInfo, context) => budgetInfoModel.ToDomain(ResolveAccountingModelConverterFromContext(context.Items)));

            mapperConfiguration.CreateMap<IBudgetInfo, BudgetInfoModel>()
                .ForMember(dest => dest.BudgetInfoIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.BudgetAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.YearMonthIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.YearMonth, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<IBudgetInfo, YearMonthModel>()
                .ForMember(dest => dest.YearMonthIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.CreditInfos, opt => opt.Ignore())
                .ForMember(dest => dest.BudgetInfos, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<PostingLineModel, IPostingLine>()
                .ConvertUsing((postingLineModel, postingLine, context) => postingLineModel.ToDomain(ResolveAccountingModelConverterFromContext(context.Items), ResolveSyncRootFromContext(context.Items)));

            mapperConfiguration.CreateMap<IPostingLine, PostingLineModel>()
                .ForMember(dest => dest.PostingLineIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.PostingLineIdentification, opt => opt.MapFrom(src => src.Identifier.ToString("D").ToUpper()))
                .ForMember(dest => dest.AccountingIdentifier, opt => opt.MapFrom(src => src.Accounting.Number))
                .ForMember(dest => dest.PostingDate, opt => opt.MapFrom(src => src.PostingDate.Date))
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Reference) ? null : src.Reference))
                .ForMember(dest => dest.AccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.BudgetAccountIdentifier, opt => opt.MapFrom(src => src.BudgetAccount == null ? null : (int?) default(int)))
                .ForMember(dest => dest.Debit, opt => opt.MapFrom(src => src.Debit == 0M ? null : (decimal?) src.Debit))
                .ForMember(dest => dest.Credit, opt => opt.MapFrom(src => src.Credit == 0M ? null : (decimal?) src.Credit))
                .ForMember(dest => dest.ContactAccountIdentifier, opt => opt.MapFrom(src => src.ContactAccount == null ? null : (int?) default(int)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<AccountGroupModel, IAccountGroup>()
                .ConvertUsing(accountGroupModel => accountGroupModel.ToDomain());

            mapperConfiguration.CreateMap<IAccountGroup, AccountGroupModel>()
                .ForMember(dest => dest.AccountGroupIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Accounts, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<BudgetAccountGroupModel, IBudgetAccountGroup>()
                .ConvertUsing(budgetAccountGroupModel => budgetAccountGroupModel.ToDomain());

            mapperConfiguration.CreateMap<IBudgetAccountGroup, BudgetAccountGroupModel>()
                .ForMember(dest => dest.BudgetAccountGroupIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.BudgetAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<PaymentTermModel, IPaymentTerm>()
                .ConvertUsing(paymentTermModel => paymentTermModel.ToDomain());

            mapperConfiguration.CreateMap<IPaymentTerm, PaymentTermModel>()
                .ForMember(dest => dest.PaymentTermIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.ContactAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.ContactSupplements, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));
        }

        internal static IConverter Create()
        {
            return new AccountingModelConverter();
        }

        private static IDictionary<int, IAccounting> ResolveAccountingDictionaryFromContext(IDictionary<string, object> stateDictionary)
        {
            NullGuard.NotNull(stateDictionary, nameof(stateDictionary));

            return stateDictionary["AccountingDictionary"] as IDictionary<int, IAccounting>;
        }

        private static IConverter ResolveAccountingModelConverterFromContext(IDictionary<string, object> stateDictionary)
        {
            NullGuard.NotNull(stateDictionary, nameof(stateDictionary));

            return stateDictionary["AccountingModelConverter"] as IConverter;
        }

        private static object ResolveSyncRootFromContext(IDictionary<string, object> stateDictionary)
        {
            NullGuard.NotNull(stateDictionary, nameof(stateDictionary));

            return stateDictionary["SyncRoot"];
        }

        #endregion
    }
}