#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Ultz.BeagleFramework.Core
{
    public class Row : IEnumerable<Field>
    {
        private readonly Field[] _fields;
        private string _primaryKey;
        private object _primaryKeyValue;

        public Row(IEnumerable<Field> fields)
        {
            _fields = fields.ToArray();
        }
        public Row(params Field[] fields)
        {
            _fields = fields;
        }
        public Row(params object[] fields)
        {
            _fields = fields.Select(x => new Field(){Value = x}).ToArray();
        }

        public Table Table { get; private set; }

        public bool IsReadOnly => !(Table != null && Table.CanPush && Table.Columns.Any
                                        (x => x.Constraints.Any(y => y is Constraint.PrimaryKey)));

        public Field this[int index] => _fields[index];

        public Field this[string index] =>
            _fields[Table?.Columns.IndexOf(index)
                    ?? throw new NotSupportedException("This row isn't associated with a table.")];

        public IEnumerator<Field> GetEnumerator()
        {
            return _fields.Cast<Field>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Associate(Table table)
        {
            _primaryKey = table.Columns.FirstOrDefault(x => x.Constraints.Any(y => y is Constraint.PrimaryKey))?.Name;
            _primaryKeyValue = _fields[table.Columns.Where(x => x.Name == _primaryKey).Select((x, y) => y).Min()].Value;
            Table = table;
        }

        public bool SaveChanges()
        {
            if (IsReadOnly)
                throw new NotSupportedException();
            return Table.Engine.Execute
                   (
                       new QueryBuilder().Update()
                           .TableName(Table.Name)
                           .Set
                           (
                               _fields.Select((x, y) => (Table.Columns[y].Name, x.Value))
                                   .Where((x, y) => x.Name != _primaryKey)
                                   .ToArray()
                           )
                           .Where()
                           .Column(_primaryKey)
                           .Equal()
                           .Value(_primaryKeyValue)
                           .Build()
                   )
                   .AsNonQuery() > 0;
        }
    }
}
