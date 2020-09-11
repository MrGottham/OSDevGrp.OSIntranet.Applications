using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;

namespace OSDevGrp.OSIntranet.Core.Builders
{
    public sealed class CsvContentBuilder<TDomainObject, TDomainObjectToCsvConverter> : ExportDataContentBuilderBase where TDomainObject : class where TDomainObjectToCsvConverter : IDomainObjectToCsvConverter<TDomainObject>
    {
        #region Private variables

        private readonly TDomainObjectToCsvConverter _domainObjectToCsvConverter;

        #endregion

        #region Constructor

        public CsvContentBuilder(TDomainObjectToCsvConverter domainObjectToCsvConverter, bool encoderShouldEmitUtf8Identifier = true)
            : base(encoderShouldEmitUtf8Identifier)
        {
            NullGuard.NotNull(domainObjectToCsvConverter, nameof(domainObjectToCsvConverter));

            _domainObjectToCsvConverter = domainObjectToCsvConverter;
        }

        #endregion

        #region Methods

        public override async Task WithHeaderAsync<TExportQuery>(TExportQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            string[] columnNames = (await _domainObjectToCsvConverter.GetColumnNamesAsync())?
                .Where(columnName => string.IsNullOrWhiteSpace(columnName) == false)
                .ToArray();

            if (columnNames == null || columnNames.Length == 0)
            {
                return;
            }

            await WriteLine(columnNames);
        }

        public override async Task WithContentAsync<TExportQuery, TExportData>(TExportQuery query, TExportData data)
        {
            NullGuard.NotNull(query, nameof(query))
                .NotNull(data, nameof(data));

            IEnumerable<TDomainObject> exportDataCollection = data as IEnumerable<TDomainObject>;
            if (exportDataCollection == null)
            {
                throw new ArgumentException($"Value cannot be cast to IEnumerable<{typeof(TDomainObject).Name}>.", nameof(data));
            }

            foreach (TDomainObject domainObject in exportDataCollection.Where(m => m != null))
            {
                string[] columnValues = (await _domainObjectToCsvConverter.ConvertAsync(domainObject))?
                    .Select(columnValue => string.IsNullOrWhiteSpace(columnValue) ? string.Empty : columnValue)
                    .ToArray();

                if (columnValues == null || columnValues.Length == 0)
                {
                    continue;
                }

                await WriteLine(columnValues);
            }
        }

        private async Task WriteLine(IEnumerable<string> values)
        {
            NullGuard.NotNull(values, nameof(values));

            await Writer.WriteLineAsync(string.Join(';', values.Where(value => value != null).Select(value => $"\"{value.Replace("\"", string.Empty)}\"")));
        }

        #endregion
    }
}