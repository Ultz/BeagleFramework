using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Ultz.BeagleFramework.Sql
{
    public abstract class SqlConnector : IDisposable
    {
        public abstract void Init(string connectionString);

        public abstract void Deinit();

        public abstract DbConnection CreateConnection();
        public abstract DbCommand CreateCommand(string query,DbConnection connection);
        public abstract DbDataAdapter CreateAdapter(string query,DbConnection connection);
        public abstract DbParameter CreateParameter(string name);
        public abstract DbParameter CreateIntParameter(string name);

        public abstract IEnumerable<string> GetTables(DbConnection connection);
        public abstract string VarCharMax { get; }

        public abstract string ProcessMessage(string s);
        
        public void Dispose()
        {
        }
    }
}