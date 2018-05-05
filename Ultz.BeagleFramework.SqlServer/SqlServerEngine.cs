using System.Collections.Generic;
using Ultz.BeagleFramework.Sql;

namespace Ultz.BeagleFramework.SqlServer
{
    public class SqlServerEngine : SqlEngine
    {
        public override string Id => "sql_server";
        public override SqlConnector Connector { get; } = new SqlServerConnector();

        public SqlServerEngine(string connectionString) : base(connectionString)
        {
        }
    }
}