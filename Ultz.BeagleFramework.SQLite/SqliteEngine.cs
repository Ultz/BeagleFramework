using System;
using System.Data;
using System.Diagnostics;
using Ultz.BeagleFramework.Sql;

namespace Ultz.BeagleFramework.SQLite
{
    public class SqliteEngine : SqlEngine, IStorageEngine
    {
        public override string Id => "sqlite";
        public override SqlConnector Connector { get; } = new SqliteConnector();

        public SqliteEngine(string connectionString) : base(connectionString)
        {
        }

        public new ITable GetTable(string name)
        {
            try
            {
                var tbl = new SqliteTable(name, Connector, this);
                if (tbl.GetDataTable().Rows.Count != 0) return tbl;
                foreach (DataColumn tableColumn in tbl.GetDataTable().Columns)
                {
                    tableColumn.DataType = tableColumn.ColumnName == "onultz_id" ? typeof(int) : typeof(string);
                }
                return tbl;
            }
            catch
            {
                return null;
            }
        }
    }
}