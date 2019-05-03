using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Models.MicrosoftGraph;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
    internal class MicrosoftGraphModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<TokenModel, IRefreshableToken>()
                .ConvertUsing(tokenModel=> tokenModel.ToDomain());
        }

        #endregion
    }
}
