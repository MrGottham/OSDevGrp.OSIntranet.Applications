using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Mvc.Models.Security
{
    internal class SecurityViewModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IUserIdentity, UserIdentityViewModel>();

            mapperConfiguration.CreateMap<IClientSecretIdentity, ClientSecretIdentityViewModel>();
        }

        #endregion
    }
}