﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Ultz.BeagleFramework.Sql;

namespace Ultz.BeagleFramework.SqlServer
{
    public class SqlServerConnector : SqlConnector
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
            return new SqlConnection(_conn);
        }

        public override DbCommand CreateCommand(string query,DbConnection connection)
        {
            return new SqlCommand(query,(SqlConnection)connection);
        }

        public override DbParameter CreateParameter(string name)
        {
            return new SqlParameter("@"+name,SqlDbType.VarChar,-1,name);
        }

        public override DbDataAdapter CreateAdapter(string cmd, DbConnection conn)
        {
            return new SqlDataAdapter(cmd,(SqlConnection)conn);
        }

        public override DbParameter CreateIntParameter(string name)
        {
            return new SqlParameter("@"+name,SqlDbType.Int,-1,name);
        }

        public override string VarCharMax => "varchar(max)";
        public override string Int => "int";

        public override string ProcessMessage(string s)
        {
            return s;
        }
    }
}