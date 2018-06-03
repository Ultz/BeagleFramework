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
        public abstract DbParameter CreateParameter(string name);
        public abstract DbDataAdapter CreateAdapter(string cmd,DbConnection conn);

        public abstract DbParameter CreateIntParameter(string name);

        public abstract string VarCharMax { get; }
        public abstract string Int { get; }

        public abstract string ProcessMessage(string s);
        
        public void Dispose()
        {
        }
    }
}