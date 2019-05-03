using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    internal class SecurityModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IToken, AccessTokenModel>()
                .ForMember(dest => dest.AccessToken, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.AccessToken) == false);
                });
        }

        #endregion
    }
}
