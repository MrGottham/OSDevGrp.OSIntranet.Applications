using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Converters
{
    public interface IColumnValueAppender
    {
        IEnumerable<string> ColumnValues { get; }

        IColumnValueAppender AddColumnValue(string value);

        IColumnValueAppender AddColumnValue(short? value);

        IColumnValueAppender AddColumnValue(int? value);

        IColumnValueAppender AddColumnValue(long? value);

        IColumnValueAppender AddColumnValue(decimal? value);

        IColumnValueAppender AddColumnValue(DateTime? value);

        IColumnValueAppender AddColumnValue(DateTime? value, string format);

        IColumnValueAppender AddEmptyColumn();

        IColumnValueAppender ResetColumns();
    }
}