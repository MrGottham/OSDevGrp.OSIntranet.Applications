using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Interfaces.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.Core.QueryHandlers
{
    public abstract class ExportQueryHandlerBase<TExportQuery, TExportData> : QueryHandlerBase, IQueryHandler<TExportQuery, byte[]> where TExportQuery : IExportQuery
    {
        #region Methods

        public async Task<byte[]> QueryAsync(TExportQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            TExportData exportData = await GetExportDataAsync(query);
            if (exportData == null)
            {
                return new byte[0];
            }

            using IExportDataContentBuilder exportDataContentBuilder = await CreateExportDataContentBuilderAsync();
            await exportDataContentBuilder.WithHeaderAsync(query);
            await exportDataContentBuilder.WithContentAsync(query, exportData);
            await exportDataContentBuilder.WithFooterAsync(query);

            return await exportDataContentBuilder.BuildAsync();
        }

        protected abstract Task<TExportData> GetExportDataAsync(TExportQuery query);

        protected abstract Task<IExportDataContentBuilder> CreateExportDataContentBuilderAsync();

        #endregion
    }
}