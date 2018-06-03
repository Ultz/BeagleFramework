using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Ultz.BeagleFramework.Sql;

namespace Ultz.BeagleFramework.SQLite
{
    public class SqliteTable : SqlTable, ITable
    {
        public SqliteTable(string name, SqlConnector connector, IStorageEngine engine) : base(name, connector, engine)
        {
        }

        public new void Refresh()
        {
            base.Refresh();
            if (GetDataTable().Rows.Count != 0) return;
            foreach (DataColumn tableColumn in GetDataTable().Columns)
            {
                tableColumn.DataType = tableColumn.ColumnName == "onultz_id" ? typeof(int) : typeof(string);
            }
        }
    }
}
