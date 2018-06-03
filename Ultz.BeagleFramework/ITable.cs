using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultz.BeagleFramework
{
    public interface ITable : IDisposable, IEnumerable
    {
        Row this[int index] { get; }
        string Name { get; }
        string[] Columns { get; }
        IEnumerable<Row> Rows { get; }
        bool TryAddColumn(string column);
        bool TryRemoveColumn(string column);
        [Obsolete("To be consistent with naming, Put is being renamed to Add. Put will be removed in a future release.")]
        Row Put(IEnumerable<string> row);
        Row Add(IEnumerable<string> row);
        void Remove(Row row);
        void Remove(int row);
        /// <summary>
        /// Forces the table to refresh its data, overriding any cache
        /// </summary>
        void Refresh();
    }
}