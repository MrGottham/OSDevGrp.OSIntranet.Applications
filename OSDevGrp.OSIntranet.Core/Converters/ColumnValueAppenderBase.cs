using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;

namespace OSDevGrp.OSIntranet.Core.Converters
{
    public abstract class ColumnValueAppenderBase : IColumnValueAppender
    {
        #region Private variables

        private readonly IFormatProvider _formatProvider;
        private readonly IList<string> _columnValues = new List<string>();

        #endregion

        #region Constructor

        protected ColumnValueAppenderBase(IFormatProvider formatProvider)
        {
            NullGuard.NotNull(formatProvider, nameof(formatProvider));

            _formatProvider = formatProvider;
        }

        #endregion

        #region Properties

        public IEnumerable<string> ColumnValues => _columnValues;

        protected IFormatProvider FormatProvider => _formatProvider;

        #endregion

        #region Methods

        public IColumnValueAppender AddColumnValue(string value)
        {
            _columnValues.Add(ResolveValueFor(value));

            return this;
        }

        public IColumnValueAppender AddColumnValue(short? value)
        {
            _columnValues.Add(ResolveValueFor(value.HasValue ? Convert.ToString(value.Value, _formatProvider) : null));

            return this;
        }

        public IColumnValueAppender AddColumnValue(int? value)
        {
            _columnValues.Add(ResolveValueFor(value.HasValue ? Convert.ToString(value.Value, _formatProvider) : null));

            return this;
        }

        public IColumnValueAppender AddColumnValue(long? value)
        {
            _columnValues.Add(ResolveValueFor(value.HasValue ? Convert.ToString(value.Value, _formatProvider) : null));

            return this;
        }

        public IColumnValueAppender AddColumnValue(decimal? value)
        {
            _columnValues.Add(ResolveValueFor(value.HasValue ? Convert.ToString(value.Value, _formatProvider) : null));

            return this;
        }

        public IColumnValueAppender AddColumnValue(DateTime? value)
        {
            string columnValue = null;
            if (value.HasValue)
            {
                columnValue = $"{value.Value.ToString("yyyy-MM-dd", _formatProvider)}T{value.Value.ToString("HH:mm:ss", _formatProvider)}{value.Value.ToString("zzz", _formatProvider)}";
            }

            _columnValues.Add(ResolveValueFor(columnValue));

            return this;
        }

        public IColumnValueAppender AddColumnValue(DateTime? value, string format)
        {
            NullGuard.NotNullOrWhiteSpace(format, nameof(format));

            _columnValues.Add(ResolveValueFor(value?.ToString(format, _formatProvider)));

            return this;
        }

        public IColumnValueAppender AddEmptyColumn()
        {
            _columnValues.Add(ResolveValueFor(null));

            return this;
        }

        public IColumnValueAppender ResetColumns()
        {
            _columnValues.Clear();

            return this;
        }

        private static string ResolveValueFor(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }

        #endregion
    }
}