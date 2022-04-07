using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.ExternalDashboard;

namespace OSDevGrp.OSIntranet.Repositories
{
    public class ExternalDashboardRepository : WebRepositoryBase, IExternalDashboardRepository
    {
        #region Constructor

        public ExternalDashboardRepository(IConfiguration configuration, ILoggerFactory loggerFactory)
            : base(configuration,  loggerFactory)
        {
        }

        #endregion

        #region Properties

        private string ExternalDashboardUrl => Configuration["ExternalData:Dashboard:EndpointAddress"];

        private string GetNewsUrl => $"{ExternalDashboardUrl}/api/export?numberOfNews={{numberOfNews}}";

        #endregion

        #region Methods

        public async Task<IEnumerable<INews>> GetNewsAsync(int numberOfNews)
        {
            try
            {
                DataContractJsonSerializerSettings serializerSettings = new DataContractJsonSerializerSettings
                {
                    DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture)
                };
                ExternalDashboardModel externalDashboardModel = await GetAsync<ExternalDashboardModel>(new Uri(GetNewsUrl.Replace("{numberOfNews}", numberOfNews.ToString(CultureInfo.InvariantCulture))), null, serializerSettings);

                IConverter converter = ExternalDashboardConverter.Create();

                return converter.Convert<ExternalDashboardModel, IEnumerable<INews>>(externalDashboardModel);
            }
            catch (IntranetRepositoryException ex)
            {
                Logger.LogError(ex, ex.Message);

                return Array.Empty<INews>();
            }
        }

        #endregion
    }
}