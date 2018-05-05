using System;
using System.Collections.Generic;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Ultz.BeagleFramework.Sql;

namespace Ultz.BeagleFramework.MySql
{
    public class MySqlConnector : SqlConnector
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
            return new MySqlConnection(_conn);
        }

        public override DbCommand CreateCommand(string query,DbConnection connection)
        {
            return new MySqlCommand(query,(MySqlConnection)connection);
        }

        public override DbDataAdapter CreateAdapter(string query,DbConnection connection)
        {
            return new MySqlDataAdapter(query,(MySqlConnection)connection);
        }

        public override DbParameter CreateParameter(string name)
        {
            return new MySqlParameter("@"+name,MySqlDbType.String,-1,name);
        }

        public override DbParameter CreateIntParameter(string name)
        {
            return new MySqlParameter("@"+name,MySqlDbType.Int32,-1,name);
        }

        public override IEnumerable<string> GetTables(DbConnection connection)
        {
            using (var reader = CreateCommand("SHOW TABLES", connection).ExecuteReader())
            {
                while (reader.Read())
                {
                    for (var i = 0; i < reader.FieldCount; i++)
                        yield return reader.GetValue(i).ToString();
                }
            }
        }

        public override string VarCharMax => "LONGTEXT";
        public override string ProcessMessage(string s)
        {
            return s;
        }
    }
}