using System.Collections.Generic;
using System.Data.Common;
using Npgsql;
using NpgsqlTypes;
using Ultz.BeagleFramework.Sql;

namespace Ultz.BeagleFramework.PostgreSql
{
    public class NpgsqlConnector : SqlConnector
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
            return new NpgsqlConnection(_conn);
        }

        public override DbCommand CreateCommand(string query,DbConnection connection)
        {
            return new NpgsqlCommand(query,(NpgsqlConnection)connection);
        }
        

        public override DbParameter CreateParameter(string name)
        {
            return new NpgsqlParameter("@" + name, NpgsqlDbType.Varchar, -1, name);
        }

        public override DbDataAdapter CreateAdapter(string cmd, DbConnection conn)
        {
            return new NpgsqlDataAdapter(cmd,(NpgsqlConnection)conn);
        }

        public override DbParameter CreateIntParameter(string name)
        {
            return new NpgsqlParameter("@" + name, NpgsqlDbType.Integer, -1, name);
        }

        public override string Int => "int";

        public override string ProcessMessage(string s)
        {
            return s.Replace("'", "''").Replace('`', '\"');
        }

        public override string VarCharMax => "text";
    }
}