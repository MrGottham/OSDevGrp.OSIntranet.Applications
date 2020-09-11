using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Builders
{
    public interface IExportDataContentBuilder : IDisposable
    {
        Task WithHeaderAsync<TExportQuery>(TExportQuery query) where TExportQuery : IExportQuery;

        Task WithContentAsync<TExportQuery, TExportData>(TExportQuery query, TExportData data) where TExportQuery : IExportQuery;

        Task WithFooterAsync<TExportQuery>(TExportQuery query) where TExportQuery : IExportQuery;

        Task<byte[]> BuildAsync();
    }
}