using System.Collections.Generic;
using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;
using OSDevGrp.OSIntranet.Repositories.Models.ExternalDashboard;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
    internal class ExternalDashboardConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<ExternalDashboardModel, IEnumerable<INews>>()
                .ConvertUsing(src => src.ToDomain(this));

            mapperConfiguration.CreateMap<ExternalDashboardItemModel, INews>()
                .ConvertUsing(src => src.ToDomain(this));
        }

        internal static IConverter Create()
        {
            return new ExternalDashboardConverter();
        }

        #endregion
    }
}