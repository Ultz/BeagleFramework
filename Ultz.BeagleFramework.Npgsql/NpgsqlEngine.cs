using Ultz.BeagleFramework.Sql;

namespace Ultz.BeagleFramework.PostgreSql
{
    public class NpgsqlEngine : SqlEngine
    {
        public override string Id => "npgsql";
        public override SqlConnector Connector { get; } = new NpgsqlConnector();

        public NpgsqlEngine(string connectionString) : base(connectionString)
        {
        }
    }
}