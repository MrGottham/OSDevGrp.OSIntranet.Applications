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

        private readonly IConverter _commonModelConverter = new CommonModelConverter();

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<AccountingModel, IAccounting>()
                .ConvertUsing(accountingModel => accountingModel.ToDomain(this, _commonModelConverter));

            mapperConfiguration.CreateMap<IAccounting, AccountingModel>()
                .ForMember(dest => dest.AccountingIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.LetterHeadIdentifier, opt => opt.MapFrom(src => src.LetterHead.Number))
                .ForMember(dest => dest.LetterHead, opt => opt.MapFrom(src => _commonModelConverter.Convert<ILetterHead, LetterHeadModel>(src.LetterHead)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.AccountCollection))
                .ForMember(dest => dest.BudgetAccounts, opt => opt.MapFrom(src => src.BudgetAccountCollection))
                .ForMember(dest => dest.ContactAccounts, opt => opt.MapFrom(src => src.ContactAccountCollection));

            mapperConfiguration.CreateMap<AccountModel, IAccount>()
                .ConvertUsing(accountModel => accountModel.ToDomain(this));

            mapperConfiguration.CreateMap<IAccount, AccountModel>()
                .ForMember(dest => dest.AccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.AccountingIdentifier, opt => opt.MapFrom(src => src.Accounting.Number))
                .ForMember(dest => dest.BasicAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.BasicAccount, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.AccountGroupIdentifier, opt => opt.MapFrom(src => src.AccountGroup.Number))
                .ForMember(dest => dest.CreditInfos, opt => opt.MapFrom(src => new List<CreditInfoModel>(0)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<IAccount, BasicAccountModel>()
                .ForMember(dest => dest.BasicAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.Accounts, opt => opt.Ignore())
                .ForMember(dest => dest.BudgetAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.ContactAccounts, opt => opt.Ignore());

            mapperConfiguration.CreateMap<BudgetAccountModel, IBudgetAccount>()
                .ConvertUsing(budgetAccountModel => budgetAccountModel.ToDomain(this));

            mapperConfiguration.CreateMap<IBudgetAccount, BudgetAccountModel>()
                .ForMember(dest => dest.BudgetAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.AccountingIdentifier, opt => opt.MapFrom(src => src.Accounting.Number))
                .ForMember(dest => dest.BasicAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.BasicAccount, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.BudgetAccountGroupIdentifier, opt => opt.MapFrom(src => src.BudgetAccountGroup.Number))
                .ForMember(dest => dest.BudgetInfos, opt => opt.MapFrom(src => new List<BudgetInfoModel>(0)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<IBudgetAccount, BasicAccountModel>()
                .ForMember(dest => dest.BasicAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.Accounts, opt => opt.Ignore())
                .ForMember(dest => dest.BudgetAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.ContactAccounts, opt => opt.Ignore());

            mapperConfiguration.CreateMap<ContactAccountModel, IContactAccount>()
                .ConvertUsing(contactAccountModel => contactAccountModel.ToDomain(this));

            mapperConfiguration.CreateMap<IContactAccount, ContactAccountModel>()
                .ForMember(dest => dest.ContactAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.AccountingIdentifier, opt => opt.MapFrom(src => src.Accounting.Number))
                .ForMember(dest => dest.BasicAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.BasicAccount, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.PaymentTermIdentifier, opt => opt.MapFrom(src => src.PaymentTerm.Number))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<IContactAccount, BasicAccountModel>()
                .ForMember(dest => dest.BasicAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.Accounts, opt => opt.Ignore())
                .ForMember(dest => dest.BudgetAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.ContactAccounts, opt => opt.Ignore());

            mapperConfiguration.CreateMap<CreditInfoModel, ICreditInfo>()
                .ConvertUsing(creditInfoModel => creditInfoModel.ToDomain(this));

            mapperConfiguration.CreateMap<ICreditInfo, CreditInfoModel>()
                .ForMember(dest => dest.CreditInfoIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.AccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.YearMonthIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.YearMonth, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<ICreditInfo, YearMonthModel>()
                .ForMember(dest => dest.YearMonthIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.CreditInfos, opt => opt.Ignore())
                .ForMember(dest => dest.BudgetInfos, opt => opt.Ignore());

            mapperConfiguration.CreateMap<BudgetInfoModel, IBudgetInfo>()
                .ConvertUsing(budgetInfoModel => budgetInfoModel.ToDomain(this));

            mapperConfiguration.CreateMap<IBudgetInfo, BudgetInfoModel>()
                .ForMember(dest => dest.BudgetInfoIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.BudgetAccountIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.YearMonthIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.YearMonth, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<IBudgetInfo, YearMonthModel>()
                .ForMember(dest => dest.YearMonthIdentifier, opt => opt.MapFrom(src => default(int)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.CreditInfos, opt => opt.Ignore())
                .ForMember(dest => dest.BudgetInfos, opt => opt.Ignore());

            mapperConfiguration.CreateMap<AccountGroupModel, IAccountGroup>()
                .ConvertUsing(accountGroupModel => accountGroupModel.ToDomain());

            mapperConfiguration.CreateMap<IAccountGroup, AccountGroupModel>()
                .ForMember(dest => dest.AccountGroupIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.Accounts, opt => opt.Ignore());

            mapperConfiguration.CreateMap<BudgetAccountGroupModel, IBudgetAccountGroup>()
                .ConvertUsing(budgetAccountGroupModel => budgetAccountGroupModel.ToDomain());

            mapperConfiguration.CreateMap<IBudgetAccountGroup, BudgetAccountGroupModel>()
                .ForMember(dest => dest.BudgetAccountGroupIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.BudgetAccounts, opt => opt.Ignore());

            mapperConfiguration.CreateMap<PaymentTermModel, IPaymentTerm>()
                .ConvertUsing(paymentTermModel => paymentTermModel.ToDomain());

            mapperConfiguration.CreateMap<IPaymentTerm, PaymentTermModel>()
                .ForMember(dest => dest.PaymentTermIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ContactAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.ContactSupplements, opt => opt.Ignore());
        }

        #endregion
    }
}