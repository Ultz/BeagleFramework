using Ultz.BeagleFramework.Sql;

namespace Ultz.BeagleFramework.MySql
{
    public class MySqlEngine : SqlEngine
    {
        public override string Id => "mysql";
        public override SqlConnector Connector { get; } = new MySqlConnector();

        public MySqlEngine(string connectionString) : base(connectionString)
        {
        }
    }
}