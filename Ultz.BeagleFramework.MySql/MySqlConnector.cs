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
        

        public override DbParameter CreateParameter(string name)
        {
            return new MySqlParameter("@"+name,MySqlDbType.String,-1,name);
        }

        public override DbDataAdapter CreateAdapter(string cmd,DbConnection conn)
        {
            return new MySqlDataAdapter(cmd,(MySqlConnection)conn);
        }

        public override DbParameter CreateIntParameter(string name)
        {
            return new MySqlParameter("@"+name,MySqlDbType.Int32,-1,name);
        }

        public override string VarCharMax => "LONGTEXT";
        public override string Int => "int";

        public override string ProcessMessage(string s)
        {
            return s;
        }
    }
}