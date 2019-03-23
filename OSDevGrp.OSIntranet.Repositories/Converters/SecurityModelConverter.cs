using System.Security.Claims;
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
                .ConvertUsing(userIdentityModel => userIdentityModel.ToDomain());

            mapperConfiguration.CreateMap<ClientSecretIdentityModel, IClientSecretIdentity>()
                .ConvertUsing(clientSecretIdentityModel => clientSecretIdentityModel.ToDomain());

            mapperConfiguration.CreateMap<ClaimModel, Claim>()
                .ConvertUsing(claimModel => claimModel.ToDomain());
        }

        #endregion
    }
}
