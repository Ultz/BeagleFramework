#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Ultz.BeagleFramework.Core
{
    public class Table : ICollection<Row>, IDisposable
    {
        public Table
        (
            IStorageEngine engine,
            string name,
            IRowCollection rows,
            IColumnCollection cols,
            Query sql,
            bool isReadOnly = true
        )
        {
            Columns = cols;
            foreach (var row in rows) row.Associate(this);

            Rows = rows;
            Query = sql;
            Name = name;
            Engine = engine;
            IsReadOnly = isReadOnly;
        }

        public bool CanPush => !IsReadOnly && CanPull;
        public Query Query { get; }
        public IColumnCollection Columns { get; }
        public IRowCollection Rows { get; }
        public IStorageEngine Engine { get; }
        public string Name { get; }
        public bool CanPull => Query != null && Engine != null;

        public Row this[int index] => Rows[index];

        public IEnumerator<Row> GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Row item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.Associate(this);
            Engine.Execute
            (
                new QueryBuilder().Insert()
                    .Into()
                    .TableName(Name)
                    .Columns(true, item.Where(x => x.Value != null).Select((_, x) => Columns[x].Name).ToArray())
                    .Values(item.Where(x => x.Value != null).Select(x => x.Value).ToArray())
                    .Build()
            )
            .AsNonQuery();
        }

        public void Clear()
        {
            Engine.Execute(new QueryBuilder().Delete().From().TableName(Name).Build()).AsNonQuery();
        }

        public bool Contains(Row item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            return Rows.Any(x => x.Select(y => y.Value).SequenceEqual(item.Select(y => y.Value)));
        }

        public void CopyTo(Row[] array, int arrayIndex)
        {
            for (var i = arrayIndex; i < array.Length; i++) array[i] = Rows[i - arrayIndex];
        }

        public bool Remove(Row item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            return Engine.Execute
            (
                new Query
                (
                    new QueryBuilder().Delete()
                        .From()
                        .TableName(Name)
                        .Where()
                        .Build()
                        .Clauses.Concat
                        (
                            item.SelectMany
                            (
                                (x, y) => new Clause[]
                                {
                                    new Clause.ColumnName {Column = Columns[y].Name}, new Clause.Equal(),
                                    new Clause.Value {Item = x.Value}
                                }
                            )
                        )
                        .ToArray()
                )
            )
            .AsNonQuery() > 0;
        }

        public int Count => Rows.Count;
        public bool IsReadOnly { get; }

        public void Dispose()
        {
        }
    }
}
