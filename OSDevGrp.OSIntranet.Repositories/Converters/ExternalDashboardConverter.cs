using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Options;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;
using OSDevGrp.OSIntranet.Repositories.Models.ExternalDashboard;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
    internal class ExternalDashboardConverter : ConverterBase
    {
        #region Constructor

        private ExternalDashboardConverter(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
            : base(licensesOptions, loggerFactory)
        {
        }

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<ExternalDashboardModel, IEnumerable<INews>>()
                .ConvertUsing(src => src.ToDomain(this));

            mapperConfiguration.CreateMap<ExternalDashboardItemModel, INews>()
                .ConvertUsing(src => src.ToDomain(this));
        }

        internal static IConverter Create(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
        {
            NullGuard.NotNull(licensesOptions, nameof(licensesOptions))
                .NotNull(loggerFactory, nameof(loggerFactory));

            return new ExternalDashboardConverter(licensesOptions, loggerFactory);
        }

        #endregion
    }
}