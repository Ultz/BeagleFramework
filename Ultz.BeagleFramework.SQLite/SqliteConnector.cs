using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Ultz.BeagleFramework.Sql;

namespace Ultz.BeagleFramework.SQLite
{
    public class SqliteConnector : SqlConnector
    {
        private string _conn;
        public override void Init(string connectionString)
        {
            //string connectionString =
            //    "Data Source=(local);Initial Catalog=Northwind;"
            //    + "Integrated Security=true";

            _conn = connectionString;
        }

        public override void Deinit()
        {
        }

        public override DbConnection CreateConnection()
        {
            return new SqliteConnection(_conn);
        }

        public override DbCommand CreateCommand(string query,DbConnection connection)
        {
            return new SqliteCommand(query,(SqliteConnection)connection);
        }
        

        public override DbParameter CreateParameter(string name)
        {
            return new SqliteParameter("@" + name, SqliteType.Text, -1, name);
        }

        public override DbDataAdapter CreateAdapter(string cmd, DbConnection conn)
        {
            return new SqliteDataAdapter(cmd,(SqliteConnection)conn);
        }

        public override DbParameter CreateIntParameter(string name)
        {
            return new SqliteParameter("@" + name, SqliteType.Integer, -1, name);
        }

        public override string Int => "INTEGER";

        public override string ProcessMessage(string s)
        {
            return s;
        }

        public override string VarCharMax => "TEXT";
    }
}