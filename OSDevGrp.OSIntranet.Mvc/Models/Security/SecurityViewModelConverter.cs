using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Options;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

namespace OSDevGrp.OSIntranet.Mvc.Models.Security
{
    internal class SecurityViewModelConverter : ConverterBase
    {
        #region Constructor

        private SecurityViewModelConverter(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
            : base(licensesOptions, loggerFactory)
        {
        }

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IUserIdentity, UserIdentityViewModel>()
                .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => new List<ClaimViewModel>(0)))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<UserIdentityViewModel, CreateUserIdentityCommand>()
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => int.MaxValue))
                .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.Claims.Where(claimViewModel => claimViewModel.IsSelected).ToList()));

            mapperConfiguration.CreateMap<UserIdentityViewModel, UpdateUserIdentityCommand>()
                .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.Claims.Where(claimViewModel => claimViewModel.IsSelected).ToList()));

            mapperConfiguration.CreateMap<UserIdentityViewModel, DeleteUserIdentityCommand>();

            mapperConfiguration.CreateMap<IClientSecretIdentity, ClientSecretIdentityViewModel>()
                .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => new List<ClaimViewModel>(0)))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<ClientSecretIdentityViewModel, CreateClientSecretIdentityCommand>()
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => int.MaxValue))
                .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.Claims.Where(claimViewModel => claimViewModel.IsSelected).ToList()));

            mapperConfiguration.CreateMap<ClientSecretIdentityViewModel, UpdateClientSecretIdentityCommand>()
                .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.Claims.Where(claimViewModel => claimViewModel.IsSelected).ToList()));

            mapperConfiguration.CreateMap<ClientSecretIdentityViewModel, DeleteClientSecretIdentityCommand>();

            mapperConfiguration.CreateMap<Claim, ClaimViewModel>()
                .ForMember(dest => dest.ClaimType, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.ActualValue, opt => 
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.Value) == false);
                    opt.MapFrom(src => src.Value);
                })
                .ForMember(dest => dest.DefaultValue, opt => 
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.Value) == false);
                    opt.MapFrom(src => src.Value);
                })
                .ForMember(dest => dest.IsSelected, opt => opt.MapFrom(src => false));

            mapperConfiguration.CreateMap<ClaimViewModel, Claim>()
                .ConvertUsing(src => new Claim(src.ClaimType, string.IsNullOrWhiteSpace(src.ActualValue) ? string.Empty : src.ActualValue));
        }

        internal static IConverter Create(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
        {
            NullGuard.NotNull(licensesOptions, nameof(licensesOptions))
                .NotNull(loggerFactory, nameof(loggerFactory));

            return new SecurityViewModelConverter(licensesOptions, loggerFactory);
        }

        #endregion
    }
}