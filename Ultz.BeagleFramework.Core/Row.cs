#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ultz.BeagleFramework.Core.Structure;

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

        public Table Table { get; private set; }

        public bool IsReadOnly => !(Table != null && Table.CanPush && Table.Columns.Any
                                        (x => x.Constraints.Any(y => y is Constraint.PrimaryKey)));

        public Field this[int index] => _fields[index];

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
                                   _fields.Where((x, y) => Table.Columns[y].Name != _primaryKey)
                                       .Select((x, y) => (Table.Columns[y].Name, x.Value))
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
