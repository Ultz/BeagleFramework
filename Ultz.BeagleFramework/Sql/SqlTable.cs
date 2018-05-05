using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Ultz.BeagleFramework.Sql
{
    public partial class SqlTable : ITable
    {
        internal DataTable _table;
        internal DbDataAdapter _adapter;
        internal DbConnection _connection;
        private IStorageEngine _engine;
        public SqlTable(string name,SqlConnector connector, IStorageEngine engine)
        {
            _table = new DataTable {TableName = name.ToUpper()};
            _connection = connector.CreateConnection();
            _adapter = connector.CreateAdapter(connector.ProcessMessage("SELECT * FROM `" + name.ToUpper()+"`"),_connection);
            _adapter.Fill(_table);
            _table.PrimaryKey = new[]{_table.Columns["onultz_id"]};
            _engine = engine;
        }

        public Row this[int index] => Rows.ToList().ElementAt(index);

        public string Name => _table.TableName;
        public string[] Columns => _table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).Where(x => x != "onultz_id").Select(x => x.ToUpper()).ToArray();

        public IEnumerable<Row> Rows
        {
            get
            {
                //_table.Rows.Cast<DataRow>().Select(x => new SqlRow(this, x));
                foreach (DataRow row in _table.Rows)
                {
                    yield return new SqlRow(this,row,_engine);
                }
            }
        }

        public bool TryAddColumn(string column)
        {
            CreateCommands();
            try
            {
                _table.Columns.Add(column.ToUpper(), typeof(string));
                _adapter.Update(_table);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool TryRemoveColumn(string column)
        {
            CreateCommands();
            if (column == "onultz_id")
                return false;
            try
            {
                _table.Columns.Remove(column.ToUpper());
                _adapter.Update(_table);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public Row Put(IEnumerable<string> row)
        {
            var enumerable = row.ToList();
            if (enumerable.Count() != Columns.Length)
                throw new ArgumentException("The amount of fields in the row doesn't match the amount provided in the schema. Got: "+enumerable.Count()+", Expected: "+Columns.Count());
            var workRow = _table.NewRow();
            workRow[0] = GetId();
            for (var i = 0; i < Columns.Length; i++)   
            {  
                workRow[i + 1] = enumerable.ElementAt(i);  
            }  
            _table.Rows.Add(workRow);
            CreateCommands();
            _adapter.Update(_table);
            return this[_table.Rows.IndexOf(workRow)];
        }

        public void Remove(Row row)
        {
            ((SqlRow)row).Remove();
        }

        public void Remove(int row)
        {
            Remove(this[row]);
        }
        
        public IEnumerator<Row> GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            _table?.Dispose();
            _adapter?.Dispose();
            _connection?.Dispose();
        }
    }
}