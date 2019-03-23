using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Models.Security;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
    internal class SecurityModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<UserIdentityModel, IUserIdentity>()
                .ConstructUsing(userIdentityModel => userIdentityModel.ToDomain());

            mapperConfiguration.CreateMap<ClientSecretIdentityModel, IClientSecretIdentity>()
                .ConstructUsing(clientSecretIdentityModel => clientSecretIdentityModel.ToDomain());
        }

        #endregion
    }
}
