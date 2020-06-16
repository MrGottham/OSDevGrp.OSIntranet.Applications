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
                .ForMember(dest => dest.LetterHead, opt => opt.MapFrom(src => _commonModelConverter.Convert<ILetterHead, LetterHeadIdentificationModel>(src.LetterHead)));

            mapperConfiguration.CreateMap<IAccountGroup, AccountGroupModel>();

            mapperConfiguration.CreateMap<IBudgetAccountGroup, BudgetAccountGroupModel>();

            mapperConfiguration.CreateMap<IPaymentTerm, PaymentTermModel>();

            mapperConfiguration.CreateMap<Domain.Interfaces.Accounting.Enums.BalanceBelowZeroType, BalanceBelowZeroType>();

            mapperConfiguration.CreateMap<Domain.Interfaces.Accounting.Enums.AccountGroupType, AccountGroupType>();
        }

        #endregion
    }
}