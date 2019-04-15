#region

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Ultz.BeagleFramework.Core;

#endregion

namespace Ultz.BeagleFramework.Sql
{
    public class SqlTable : IRowCollection, IColumnCollection
    {
        private readonly DbCommand _cmd;
        private IEnumerable<Column> _columns;
        private IEnumerable<Row> _rows;

        public SqlTable(DbCommand cmd)
        {
            _cmd = cmd;
            Read();
        }

        public string Name { get; private set; }

        IEnumerator<Column> IEnumerable<Column>.GetEnumerator()
        {
            Read();
            return _columns.GetEnumerator();
        }

        int IReadOnlyCollection<Column>.Count => (_columns ?? Read().Item2).Count();

        Column IReadOnlyList<Column>.this[int index] => (_columns ?? Read().Item2).ElementAtOrDefault(index);

        public int IndexOf(string col)
        {
            return (_columns ?? Read().Item2).TakeWhile(x => x.Name != col).Count();
        }

        public IEnumerator<Row> GetEnumerator()
        {
            Read();
            return _rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            Read();
            return _rows.GetEnumerator();
        }

        int IReadOnlyCollection<Row>.Count => (_rows ?? Read().Item1).Count();

        Row IReadOnlyList<Row>.this[int index] => (_rows ?? Read().Item1).ElementAtOrDefault(index);

        public (IEnumerable<Row>, IEnumerable<Column>) Read()
        {
            using (var reader = _cmd.ExecuteReader(CommandBehavior.KeyInfo))
            {
                _columns = reader.GetColumnSchema()
                    .Select
                    (
                        (x, y) =>
                        {
                            Name = x.BaseTableName;
                            return new Column
                            {
                                Name = x.ColumnName, Type = SqlUtils.ConvertToDataType(x),
                                Constraints = SqlUtils.GetConstraints(x).ToList()
                            };
                        }
                    )
                    .ToList();
                _rows = reader.Enumerate()
                    .Select(x => new Row(_columns.Select((y, z) => new Field {Value = x[z]})))
                    .ToList();
            }

            return (_rows, _columns);
        }
    }
}
