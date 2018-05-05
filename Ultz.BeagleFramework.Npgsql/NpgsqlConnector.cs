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

        public override DbDataAdapter CreateAdapter(string query,DbConnection connection)
        {
            return new NpgsqlDataAdapter(query,(NpgsqlConnection)connection);
        }

        public override DbParameter CreateParameter(string name)
        {
            return new NpgsqlParameter("@" + name, NpgsqlDbType.Varchar, -1, name);
        }

        public override DbParameter CreateIntParameter(string name)
        {
            return new NpgsqlParameter("@" + name, NpgsqlDbType.Integer, -1, name);
        }

        public override IEnumerable<string> GetTables(DbConnection connection)
        {
            using (var reader = CreateCommand("SELECT table_name FROM information_schema.tables WHERE table_schema='public' AND table_type='BASE TABLE';", connection).ExecuteReader())
            {
                while (reader.Read())
                {
                    for (var i = 0; i < reader.FieldCount; i++)
                        yield return reader.GetValue(i).ToString();
                }
            }
        }

        public override string ProcessMessage(string s)
        {
            return s.Replace("'", "''").Replace('`', '\"');
        }

        public override string VarCharMax => "text";
    }
}