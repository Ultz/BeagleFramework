#region

using System.Data.Common;
using Ultz.BeagleFramework.Core;

#endregion

namespace Ultz.BeagleFramework.Sql
{
    public abstract class SqlEngineBase : IStorageEngine
    {
        protected SqlEngineBase(string connectionString)
        {
        }
        protected SqlEngineBase(DbConnection connection)
        {
            Connection = connection;
        }
        public DbConnection Connection { get; protected set; }
        public IQuery Execute(Query query)
        {
            return new SqlQuery(this, query);
        }
        public abstract DbCommand Translate(Query query);

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
