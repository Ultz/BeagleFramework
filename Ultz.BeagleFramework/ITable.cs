using System;
using System.Collections.Generic;

namespace Ultz.BeagleFramework
{
    public interface ITable : IEnumerable<Row>, IDisposable
    {
        Row this[int index] { get; }
        string Name { get; }
        string[] Columns { get; }
        IEnumerable<Row> Rows { get; }
        bool TryAddColumn(string column);
        bool TryRemoveColumn(string column);
        Row Put(IEnumerable<string> row);
        void Remove(Row row);
        void Remove(int row);
    }
}