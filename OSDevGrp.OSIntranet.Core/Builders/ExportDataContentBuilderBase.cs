using System.IO;
using System.Text;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Interfaces.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;

namespace OSDevGrp.OSIntranet.Core.Builders
{
    public abstract class ExportDataContentBuilderBase : IExportDataContentBuilder
    {
        #region Private variables

        private bool _disposed;
        private readonly MemoryStream _memoryStream;

        #endregion

        #region Constructor

        protected ExportDataContentBuilderBase(bool encoderShouldEmitUtf8Identifier = true)
        {
            _disposed = false;
            _memoryStream = new MemoryStream();

            Writer = new StreamWriter(_memoryStream, new UTF8Encoding(encoderShouldEmitUtf8Identifier));
        }

        #endregion

        #region Properties

        protected StreamWriter Writer { get; }

        #endregion

        #region Methods

        public virtual Task WithHeaderAsync<TExportQuery>(TExportQuery query) where TExportQuery : IExportQuery
        {
            NullGuard.NotNull(query, nameof(query));

            return Task.CompletedTask;
        }

        public virtual Task WithContentAsync<TExportQuery, TExportData>(TExportQuery query, TExportData data) where TExportQuery : IExportQuery
        {
            NullGuard.NotNull(query, nameof(query))
                .NotNull(data, nameof(data));

            return Task.CompletedTask;
        }

        public virtual Task WithFooterAsync<TExportQuery>(TExportQuery query) where TExportQuery : IExportQuery
        {
            NullGuard.NotNull(query, nameof(query));

            return Task.CompletedTask;
        }

        public async Task<byte[]> BuildAsync()
        {
            await Writer.FlushAsync();

            _memoryStream.Seek(0, SeekOrigin.Begin);

            return _memoryStream.ToArray();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            Writer.Close(); 
            Writer.Dispose();

            _memoryStream.Close();
            _memoryStream.Dispose();

            _disposed = true;
        }

        #endregion
    }
}