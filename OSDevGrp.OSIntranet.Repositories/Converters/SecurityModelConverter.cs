using System.Collections.Generic;
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

            mapperConfiguration.CreateMap<IUserIdentity, UserIdentityModel>()
                .ForMember(dest => dest.UserIdentityIdentifier, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(dest => dest.UserIdentityClaims, opt => opt.MapFrom(src => new List<UserIdentityClaimModel>(0)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<UserIdentityClaimModel, Claim>()
                .ConvertUsing(userIdentityClaimModel => userIdentityClaimModel.ToDomain());

            mapperConfiguration.CreateMap<Claim, UserIdentityClaimModel>()
                .ForMember(dest => dest.UserIdentityClaimIdentifier, opt => opt.Ignore())
                .ForMember(dest => dest.UserIdentityIdentifier, opt => opt.Ignore())
                .ForMember(dest => dest.UserIdentity, opt => opt.Ignore())
                .ForMember(dest => dest.ClaimIdentifier, opt => opt.Ignore())
                .ForMember(dest => dest.Claim, opt => opt.Ignore())
                .ForMember(dest => dest.ClaimValue, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByIdentifier, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedByIdentifier, opt => opt.Ignore());

            mapperConfiguration.CreateMap<ClientSecretIdentityModel, IClientSecretIdentity>()
                .ConvertUsing(clientSecretIdentityModel => clientSecretIdentityModel.ToDomain());

            mapperConfiguration.CreateMap<IClientSecretIdentity, ClientSecretIdentityModel>()
                .ForMember(dest => dest.ClientSecretIdentityIdentifier, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(dest => dest.ClientSecretIdentityClaims, opt => opt.MapFrom(src => new List<ClientSecretIdentityClaimModel>(0)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<ClientSecretIdentityClaimModel, Claim>()
                .ConvertUsing(clientSecretIdentityClaimModel => clientSecretIdentityClaimModel.ToDomain());

            mapperConfiguration.CreateMap<Claim, ClientSecretIdentityClaimModel>()
                .ForMember(dest => dest.ClientSecretIdentityClaimIdentifier, opt => opt.Ignore())
                .ForMember(dest => dest.ClientSecretIdentityIdentifier, opt => opt.Ignore())
                .ForMember(dest => dest.ClientSecretIdentity, opt => opt.Ignore())
                .ForMember(dest => dest.ClaimIdentifier, opt => opt.Ignore())
                .ForMember(dest => dest.Claim, opt => opt.Ignore())
                .ForMember(dest => dest.ClaimValue, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByIdentifier, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedByIdentifier, opt => opt.Ignore());

            mapperConfiguration.CreateMap<ClaimModel, Claim>()
                .ConvertUsing(claimModel => claimModel.ToDomain());
        }

        #endregion
    }
}
